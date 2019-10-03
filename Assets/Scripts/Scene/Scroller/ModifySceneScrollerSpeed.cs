using UnityEngine;

public class ModifySceneScrollerSpeed : MonoBehaviour
{
    public SceneScroller sceneScroller;
    public SceneGroup.SceneInfo.ModifySceneScrollerSpeed modifyWith;

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

        this.sceneScroller.ModifySpeed(this.modifyWith);
    }
}