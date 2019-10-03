using UnityEngine;

public class Utility {
   public static Vector3 RandomWithin(Bounds within) {
        return new Vector3(
            Random.Range(within.min.x, within.max.x),
            Random.Range(within.min.y, within.max.y),
            Random.Range(within.min.z, within.max.z)
            );
    }
}

