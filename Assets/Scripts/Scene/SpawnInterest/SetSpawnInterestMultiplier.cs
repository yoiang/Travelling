using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpawnInterestMultiplier : MonoBehaviour
{
    public SceneScroller sceneScroller;
    public int multiplier;
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
        this.sceneScroller.spawnableInterestMultiplier = this.multiplier;
    }
}
