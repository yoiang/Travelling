using UnityEngine;

public class SetAbsoluteValue<T>: IModifyValue<T> {
    public T absoluteValue;

    public T Modify(T previous) {
        return this.absoluteValue;
    }
}