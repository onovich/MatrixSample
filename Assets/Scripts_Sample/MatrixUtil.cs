using UnityEngine;

public static class MatrixUtil {

    // TRS
    public static Matrix4x4 TRS(Vector3 t, Quaternion r, Vector3 s) {
        Matrix4x4 m = Matrix4x4.identity;
        m.SetTRS(t, r, s);
        return m;
    }

}