using UnityEngine;

public interface ISetSelection<T> {
    int GetNextIndex(T[] from);
}

static public class ISetSelectionExtensions {
    static public T GetNext<T>(this ISetSelection<T> self, T[] from)
    {
        var index = self.GetNextIndex(from);
        if (index < from.Length) {
            return from[index];
        } 
        
        return default(T);
    }

}
