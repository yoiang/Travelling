using UnityEngine;

[System.Serializable]
public class LinearClampSelection<T> : ISetSelection<T>
{
    protected int previousIndex = -1;
    public virtual int GetNextIndex(T[] from)
    {
        var nextIndex = this.previousIndex + 1;
        if (from.Length <= nextIndex) {
            nextIndex = from.Length - 1;
        }
        this.previousIndex = nextIndex;

        return nextIndex;
    }
};

