using UnityEngine;

public class SetSceneScrollerOverrideSceneGroup : MonoBehaviour
{
    public SceneScroller sceneScroller;
    public SceneScroller.SceneGroupInfo overrideSceneGroupInfo;

    public bool setOnStart = true;

    void Start() {
        if (this.setOnStart) {
            this.Set();
        }
    }

    public void Set() {
        if (this.sceneScroller == null) {
            Debug.LogError("Unable to find SceneScroller", this);
            return;
        }

        this.sceneScroller.overrideSceneGroupInfo = this.overrideSceneGroupInfo;

    }
}