using UnityEngine;

public class AddSceneGroupInfos : MonoBehaviour
{
    public SceneScroller sceneScroller;
    public SceneScroller.SceneGroupInfo[] sceneGroupInfos;

    public bool addOnStart = true;

    void Start() {
        if (this.addOnStart) {
            this.Add();
        }
    }

    public void Add() {
        if (this.sceneScroller == null) {
            Debug.LogError("Unable to find SceneScroller", this);
            return;
        }

        // this.sceneScroller.sceneGroupInfos = this.sceneScroller.sceneGroupInfos
        var allSceneGroupInfos = new SceneScroller.SceneGroupInfo[this.sceneScroller.sceneGroupInfos.Length + this.sceneGroupInfos.Length];
        this.sceneScroller.sceneGroupInfos.CopyTo(allSceneGroupInfos, 0);
        this.sceneGroupInfos.CopyTo(allSceneGroupInfos, this.sceneScroller.sceneGroupInfos.Length);

        this.sceneScroller.sceneGroupInfos = allSceneGroupInfos;

    }
}