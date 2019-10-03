using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneGroup : MonoBehaviour
{
    [System.Serializable]
    public struct SceneInfo : IWeighted
    {
        public GameObject scene;

        [SerializeField]
        private float weight; // TODO: is there a way to collectively visualize all involved?
        public float Weight
        {
            get {
                return this.weight;
            }
        }

        public abstract class ModifySceneScrollerSpeed: MonoBehaviour {
            protected IModifyValue<float> modifyValue;

            virtual public float Modify(float previous) {
                if (this.modifyValue == null) {
                    return previous;
                }
                return this.modifyValue.Modify(previous);
            }
        };
        public ModifySceneScrollerSpeed modifySceneScrollerSpeed;

        public SceneInfo(GameObject scene, float weight, ModifySceneScrollerSpeed modifySceneScrollerSpeed) {
            this.scene = scene;
            this.weight = weight;

            this.modifySceneScrollerSpeed = modifySceneScrollerSpeed;
        }
    };
    public SceneInfo[] sceneInfos;

    [System.Serializable]
    public abstract class SceneInfoSelection: MonoBehaviour {
        abstract public SceneInfo GetNext(SceneInfo[] from);
    };
    public SceneInfoSelection setSelection;

    // Start is called before the first frame update
    void Start()
    { 
        // TODO: test sceneInfos validity  
    }

    public GameObject GetNextScene() {
        if (this.sceneInfos.Length == 0)
        {
            Debug.LogError("SceneGroup " + this + " contains no scenes", this);
            return null;
        }
        return this.setSelection.GetNext(this.sceneInfos).scene;
    }
}

// [CustomEditor (typeof (SceneGroup))]
// public class SceneGroupInspector : Editor
// {
//    public override void OnInspectorGUI ()
//    {
//         // base.OnInspectorGUI();
//         DrawDefaultInspector ();

//         SceneGroup script = (SceneGroup)target;
//         script.setSelection = EditorGUILayout.ObjectField ((Object)script.setSelection, typeof (SetSelection<SceneGroup.SceneInfo>)) as SetSelection<SceneGroup.SceneInfo>;
//    }
// }