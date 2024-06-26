using UnityEngine;

public static class Vector3Extensions {
    public static Vector3 ElementwiseMultiply(this Vector3 a, Vector3 b) {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public static Vector3 ElementwiseDivide(this Vector3 a, Vector3 b) {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }
}