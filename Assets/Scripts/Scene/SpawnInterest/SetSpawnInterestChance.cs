using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpawnInterestChance : MonoBehaviour
{
    public SceneScroller sceneScroller;
    public float spawnInterestChance;
    public bool setOnStart = true;

    void Start()
    {
        this.Set();        
    }

    void Set() {
        if (this.sceneScroller == null) {
            Debug.LogError("SceneScroller not set", this);
            return;
        }
        this.sceneScroller.spawnableInterestChance = this.spawnInterestChance;
    }
}
