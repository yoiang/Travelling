using UnityEngine;

[System.Serializable]
public class RandomSelection<T> : ISetSelection<T>
{
    public virtual int GetNextIndex(T[] from)
    {
        return Random.Range(0, from.Length);
    }
};

