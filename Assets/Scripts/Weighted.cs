// Weighted.cs
// Ian Grossberg / Adorkable
// Creative Commons 2019

using UnityEngine;

public interface IWeighted {
    float Weight { get; }
}

class WeightedSet<T> where T: IWeighted {
    public static float GetTotalWeight(T[] from) {
        float result = 0;
        foreach (T weighted in from) {
            result += weighted.Weight;
        }
        return result;
    }

    public static int SelectRandomIndex(T[] from, float total) {
        float selected = Random.Range(0.0f, total);

        int resultIndex = -1;
        float traversalAccumulated = 0;
        for (int traverse = 0; traverse < from.Length; traverse ++) {
            traversalAccumulated += from[traverse].Weight;
            if (traversalAccumulated > selected) {
                resultIndex = traverse;
                break;
            }
        }

        // TODO: it shouldn't happen but should we report and/or assign last Weighted if result is null?
        return resultIndex;
    }

    public static int SelectRandomIndex(T[] from) {
        float total = WeightedSet<T>.GetTotalWeight(from);
        return WeightedSet<T>.SelectRandomIndex(from, total);
    }

    public static T SelectRandom(T[] from) {
        float total = WeightedSet<T>.GetTotalWeight(from);
        int index = WeightedSet<T>.SelectRandomIndex(from, total);
        if (index == -1) {
            return default(T);
        }
        return from[index];
    }
};