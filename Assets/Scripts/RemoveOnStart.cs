using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveOnStart : MonoBehaviour
{
    public GameObject[] remove;

    void Start() {
        if (this.remove == null) {
            Debug.LogError("Remove not set", this);
            return;
        }
        foreach(GameObject remove in this.remove) {
            Destroy(remove);
        }
    }
}
