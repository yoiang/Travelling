using UnityEngine;

[System.Serializable]
public class RandomWithoutAdjacentDuplicatesSelection<T>: RandomSelection<T>
{
    [Range(0.0f, 1.0f)]
    public float allowDuplicatesRate;
    private int previousIndex = -1;

    public override int GetNextIndex(T[] from)
    {
        bool allowDuplicate = this.previousIndex == -1 || from.Length == 1 || Random.Range(0.0f, 1.0f) < this.allowDuplicatesRate;

        int index = -1;
        do {
            index = base.GetNextIndex(from);
        } while(!allowDuplicate && index == this.previousIndex);
        return index;
    }
};
