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

    // MVP
    public static Matrix4x4 GetModelMatrix(in TRSModel trs) {
        return Matrix4x4.TRS(trs.t, trs.r, trs.s);
    }

    public static Matrix4x4 GetViewMatrix(CameraModel camera) {
        return camera.GetViewMatrix();
    }

    public static Matrix4x4 GetProjectionMatrix(CameraModel camera) {
        return camera.GetProjectionMatrix();
    }

    // VP
    public static Vector3 WorldToScreenPoint(CameraModel camera, Vector3 worldSpacePoint, Vector2 screenSize) {

        // World -> View
        Matrix4x4 viewMatrix = camera.GetViewMatrix();
        Vector3 cameraSpacePoint = viewMatrix * new Vector4(worldSpacePoint.x, worldSpacePoint.y, worldSpacePoint.z, 1);

        // View -> Projection
        Matrix4x4 projectionMatrix = camera.GetProjectionMatrix();
        Vector4 clipSpacePoint = projectionMatrix * cameraSpacePoint;

        // Projection -> NDC
        Vector3 ndcPoint = clipSpacePoint / clipSpacePoint.w;

        // NDC -> ViewPort
        Vector3 viewportPoint = new Vector3(
            (-ndcPoint.x + 1) * 0.5f,
            (-ndcPoint.y + 1) * 0.5f,
            cameraSpacePoint.z
        );

        // ViewPort -> Screen
        Vector3 screenPos = new Vector3(
            viewportPoint.x * screenSize.x,
            viewportPoint.y * screenSize.y,
            viewportPoint.z
        );

        return screenPos;
    }

}