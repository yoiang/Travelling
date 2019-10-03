using UnityEngine;

public interface IModifyValue<T> {
    T Modify(T previous);
}