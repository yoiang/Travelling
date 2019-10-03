using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomWithoutReplacementSelection<T> : ISetSelection<T>
{
    private List<int> unused = null;
    public int GetNextIndex(T[] from)
    {
        if (this.unused == null || this.unused.Count == 0) {
            var index = 0;
            this.unused = (new List<T>(from)).ConvertAll((value) => {
                var mapResult = index;
                index ++;
                return mapResult;
            });
        }

        var unusedIndex = Random.Range(0, this.unused.Count);
        var result = this.unused[unusedIndex];
        this.unused.RemoveAt(unusedIndex);
        return result;
    }
}