using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInterestsInside : MonoBehaviour
{
    public BoxCollider boxCollider;
    public int amount;

    public GameObject[] SpawnInstances(SceneScroller.SpawnableInterestInfo[] templates, float spawnChance, int amountMultiplier) {
        var result = new List<GameObject>(templates.Length);

        while (result.Count < this.amount * amountMultiplier) {
            if (UnityEngine.Random.Range(0.0f, 1.0f) > spawnChance) {
                continue;
            }
            var template = WeightedSet<SceneScroller.SpawnableInterestInfo>.SelectRandom(templates);
            result.Add(this.SpawnInstance(template.spawnableInterest));
        }

        return result.ToArray();
    }

    public GameObject SpawnInstance(GameObject template) {
        if (this.boxCollider == null) {
            Debug.LogError("boxCollider is null", this);
            return null;
        }

        var position = new Vector3(
            UnityEngine.Random.Range(this.boxCollider.bounds.min.x, this.boxCollider.bounds.max.x),
            UnityEngine.Random.Range(this.boxCollider.bounds.min.y, this.boxCollider.bounds.max.y),
            UnityEngine.Random.Range(this.boxCollider.bounds.min.z, this.boxCollider.bounds.max.z)
        );

        var result = GameObject.Instantiate(
            template,
            position,
            template.transform.rotation,
            this.gameObject.transform);
        result.SetActive(true);
        return result;
    }
}
