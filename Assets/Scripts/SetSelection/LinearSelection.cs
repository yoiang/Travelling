using UnityEngine;

[System.Serializable]
public class LinearSelection<T> : ISetSelection<T>
{
    protected int previousIndex = -1;
    public virtual int GetNextIndex(T[] from)
    {
        var nextIndex = this.previousIndex + 1;
        if (from.Length <= nextIndex) {
            nextIndex = 0;
        }
        this.previousIndex = nextIndex;

        return nextIndex;
    }
};

