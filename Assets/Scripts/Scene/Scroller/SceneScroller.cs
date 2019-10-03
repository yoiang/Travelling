using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

[RequireComponent(typeof(BGCcMath))]
public class SceneScroller : MonoBehaviour
{
    [System.Serializable]
    public struct SceneGroupInfo : IWeighted
    {
        public SceneGroup sceneGroup;

        [IntMinMaxRange(1, 100)]
        public IntMinMaxRange sceneInstanceCount;

        [SerializeField]
        private float weight;
        public float Weight
        {
            get {
                return this.weight;
            }
        }

        public SceneGroupInfo(SceneGroup sceneGroup, IntMinMaxRange sceneInstanceCount, float weight) {
            this.sceneGroup = sceneGroup;
            this.sceneInstanceCount = sceneInstanceCount;
            this.weight = weight;
        }
    };
    public SceneGroupInfo[] sceneGroupInfos;

    public uint sceneInstancesMaxCount;
    public float sceneInstancesDepthOffset;
    private float _sceneInstancesTravellingOffset;
    public BGCurve sceneInstancesTravelCurve;

    private uint cachedSceneInstancesMaxCount = 0;
    private float cachedSceneInstancesDepthOffset;
    private BGCurve cachedSceneInstancesTravelCurve;
    private BGCcMath sceneInstancesTravelCurveSolver;

    [System.Serializable]
    public struct SpawnableInterestInfo : IWeighted
    {
        public GameObject spawnableInterest;

        [SerializeField]
        private float weight;
        public float Weight
        {
            get {
                return this.weight;
            }
        }

        public SpawnableInterestInfo(GameObject spawnableInterest, float weight) {
            this.spawnableInterest = spawnableInterest;
            this.weight = weight;
        }
    };
    public SpawnableInterestInfo[] spawnableInterestInfo;
    public float spawnableInterestChance;
    public float spawnableInterestSubchance;
    public int spawnableInterestMultiplier = 1;

    protected struct SceneGroupInstanceInfo
    {
        public SceneGroupInfo group;
        public int remainingInstanceCount;
    };
    private SceneGroupInstanceInfo _sceneGroupInstanceInfo;
    private List<GameObject> _sceneInstances;

    public SceneGroupInfo overrideSceneGroupInfo;

    public float speed;
    public float setSpeedDistance;
    public float cullAfterZ;
    
    // Start is called before the first frame update
    void Start()
    {
        // TODO: test this.sceneInfos validity
        this._sceneInstances = new List<GameObject>();
        // this._sceneGroupIndices = new List<SceneGroupInstanceInfo>();

        this._sceneInstancesTravellingOffset = this.sceneInstancesDepthOffset;

        this.sceneInstancesTravelCurveSolver = this.GetComponent<BGCcMath>();
        if (this.sceneInstancesTravelCurveSolver == null) {
            Debug.LogError("Could not find curve solver for Travel curve");
        }
    }

    // Update is called once per frame
    void Update()
    {
        int index = 0;
        
        this._sceneInstancesTravellingOffset -= this.speed * Time.deltaTime;
        while (this._sceneInstancesTravellingOffset < 0) {
            this._sceneInstancesTravellingOffset += this.sceneInstancesDepthOffset;
        }

        while (index < this._sceneInstances.Count)
        {
            GameObject sceneInstance = this._sceneInstances[index];

            if (this.RemovePast(sceneInstance))
            {
                this._sceneInstancesTravellingOffset += this.sceneInstancesDepthOffset;

                continue;
            }
            this.UpdatePositions(sceneInstance, index);
            index += 1;
        }
        this.FillRemaining();

        if (this.ShouldApplyGlobalRenderLimits()) {
            this.ApplyGlobalRenderLimits();
        }
    }

    protected bool ShouldApplyGlobalRenderLimits() {
        return this.cachedSceneInstancesMaxCount != this.sceneInstancesMaxCount ||
            this.cachedSceneInstancesDepthOffset != this.sceneInstancesDepthOffset ||
            this.cachedSceneInstancesTravelCurve != this.sceneInstancesTravelCurve;
    }

    // TODO: adjust when appropriate values change
    protected void ApplyGlobalRenderLimits() {
        this.cachedSceneInstancesMaxCount = this.sceneInstancesMaxCount;
        this.cachedSceneInstancesDepthOffset = this.sceneInstancesDepthOffset;
        this.cachedSceneInstancesTravelCurve = this.sceneInstancesTravelCurve;

        var farthestSceneStart = this.cachedSceneInstancesMaxCount * this.cachedSceneInstancesDepthOffset; // + 1 because we want the back edge of the last possible scene
        var farthestSceneEnd = farthestSceneStart + this.cachedSceneInstancesDepthOffset;

        if (this.sceneInstancesTravelCurveSolver != null) {
            Vector3 farthestSceneStartLocation = this.sceneInstancesTravelCurveSolver.CalcPositionByDistance(farthestSceneStart);
            Vector3 farthestSceneEndLocation = this.sceneInstancesTravelCurveSolver.CalcPositionByDistance(farthestSceneEnd);
            
            var camera = this.GetComponent<Camera>();
            if (camera != null) {
                camera.farClipPlane = farthestSceneEndLocation.z; // Should we add a buffer to be safe?
            }

            // var fogStartDistance = (this.cachedSceneInstancesMaxCount - 1) * this.cachedSceneInstancesDepthOffset;
            Vector3 fogStartDistanceLocation = this.sceneInstancesTravelCurveSolver.CalcPositionByDistance(farthestSceneEnd / 2);

            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogStartDistance = fogStartDistanceLocation.z;
            RenderSettings.fogEndDistance = farthestSceneEndLocation.z;
        }
    }

    protected bool RemovePast(GameObject scene)
    {
        if (scene.transform.position.z < this.cullAfterZ)
        {
            this._sceneInstances.Remove(scene);
            Destroy(scene);
            return true;
        }
        return false;
    }

    protected float DistanceForSceneInstanceIndex(int index) {
        return this._sceneInstancesTravellingOffset + this.sceneInstancesDepthOffset * index;
    }

    protected void UpdatePositions(GameObject scene, int index)
    {
        if (this.sceneInstancesTravelCurveSolver == null) {
            return;
        }

        var wasBeyondSetSpeedDistance = Vector3.Distance(this.gameObject.transform.position, scene.transform.position) > this.setSpeedDistance;

        var updatedPosition = this.sceneInstancesTravelCurveSolver.CalcPositionByDistance(this.DistanceForSceneInstanceIndex(index));//, out tangAtOneMeter);

        scene.transform.SetPositionAndRotation(updatedPosition, scene.transform.rotation);

        var isPastSetSpeedDistance = Vector3.Distance(this.gameObject.transform.position, scene.transform.position) <= this.setSpeedDistance;
        if (wasBeyondSetSpeedDistance && isPastSetSpeedDistance) {
            this.ModifySpeed(scene.GetComponent<SceneGroup.SceneInfo.ModifySceneScrollerSpeed>());
        }
    }

    public void ModifySpeed(SceneGroup.SceneInfo.ModifySceneScrollerSpeed modifyWith) {
        if (modifyWith == null) {
            return;
        }
        var previous = this.speed;
        var next = modifyWith.Modify(previous);
        Debug.Log("Modifying scene scroller speed from " + previous + " to " + next, this);
        this.speed = next;
    }

    protected void FillRemaining()
    {
        while (this._sceneInstances.Count < this.sceneInstancesMaxCount)
        {
            float distance = this.DistanceForSceneInstanceIndex(this._sceneInstances.Count);

            var math = this.GetComponent<BGCcMath>();

            // TODO: remove tangent calc
            var tangAtOneMeter = new Vector3();
            Vector3 nextPosition = math.CalcPositionAndTangentByDistance(distance, out tangAtOneMeter);
            GameObject newInstance = this.InstantiateNext(nextPosition);
            if (newInstance == null)
            {
                Debug.LogError("Instantiate returned null", this);
                return;
            }

            this.SetupNewInstance(newInstance);

            this._sceneInstances.Add(newInstance);
        }
    }

    protected void SetupNewInstance(GameObject newInstance) {
        if (this._sceneInstances.Count > 0) {
            var lastInstance = this._sceneInstances[this._sceneInstances.Count - 1];
            var lastTerrain = lastInstance.GetComponent<Terrain>();
            var newTerrain = newInstance.GetComponent<Terrain>();
            if (lastTerrain != null && newTerrain != null) {
                newTerrain.SetNeighbors(null, null, null, lastTerrain);

                if (this._sceneInstances.Count > 1) {
                    var lastLastInstance = this._sceneInstances[this._sceneInstances.Count - 2];
                    var lastLastTerrain = lastLastInstance.GetComponent<Terrain>();
                    lastTerrain.SetNeighbors(null, newTerrain, null, lastLastTerrain);
                }
            }
        }
    }

    protected SceneGroupInstanceInfo GetNextSceneGroupInstanceInfo()
    {
        SceneGroupInstanceInfo result = new SceneGroupInstanceInfo();
        if (this.overrideSceneGroupInfo.sceneGroup != null) {
            result.group = this.overrideSceneGroupInfo;
        } else {
            result.group = WeightedSet<SceneGroupInfo>.SelectRandom(this.sceneGroupInfos);
        }

        var instanceCount = result.group.sceneInstanceCount;
        result.remainingInstanceCount = instanceCount.GetRandomValue();
        return result;
    }

    protected GameObject InstantiateNext(Vector3 nextPosition)
    {
        var instanceInfo = this._sceneGroupInstanceInfo;
        if (instanceInfo.remainingInstanceCount == 0)
        {
            instanceInfo = this.GetNextSceneGroupInstanceInfo();

            // HACK: for now let's make sure we have a proper SceneGroupInfo, but also not forever
            var count = 0;
            SceneGroupInfo test = instanceInfo.group;
            while(!test.sceneGroup && count < 10) {
                instanceInfo = this.GetNextSceneGroupInstanceInfo();
                count++;
            }
        }

        // Regardless of fail we're going to decrement, especially if there is a failing with this SceneGroup specifically
        instanceInfo.remainingInstanceCount -= 1;

        SceneGroupInfo current = instanceInfo.group;
        if(!current.sceneGroup) {
            Debug.LogWarning("Current SceneGroupInfo " + current + " has no scene group, unable to InstantiateNext", this);
            return null;
        }
        // TODO: test current

        GameObject originalInstance = current.sceneGroup.GetNextScene();
        GameObject result = GameObject.Instantiate(
            originalInstance,
            nextPosition,
            originalInstance.transform.rotation,
            this.transform);

        this.SpawnInterests(result.GetComponent<SpawnInterestsInside>());

        this._sceneGroupInstanceInfo = instanceInfo;

        return result;
    }

    protected void SpawnInterests(SpawnInterestsInside spawnInterests) {
        if (spawnInterests == null) {
            return;
        }
        if (Random.Range(0.0f, 1.0f) > this.spawnableInterestChance) { 
            return;
        }
        if (this.spawnableInterestInfo.Length == 0) {
            return;
        }
        spawnInterests.SpawnInstances(this.spawnableInterestInfo, this.spawnableInterestSubchance, this.spawnableInterestMultiplier);
    } 
}
