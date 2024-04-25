using UnityEngine;

public static class MatrixUtil {

    // TRS
    public static TRSModel ApplyTRSWithOffset(in TRSModel src, in TRSModel offset) {
        Matrix4x4 m = Matrix4x4.identity;
        m.SetTRS(src.t, src.r, src.s);
        TRSModel dst = new TRSModel();
        // T
        dst.t = m.MultiplyPoint(offset.t);
        // R
        dst.r = src.r * offset.r;
        // S
        dst.s = new Vector3(
           src.s.x * offset.s.x,
           src.s.y * offset.s.y,
           src.s.z * offset.s.z
        );
        return dst;
    }

}