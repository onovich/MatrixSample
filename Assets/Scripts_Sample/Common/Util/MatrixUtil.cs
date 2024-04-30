using UnityEngine;
using UnityEngine.Animations;

public static class MatrixUtil {

    // TRS
    public static TRSModel ApplyTRSWithOffset(in TRSModel target, in TRSModel offset) {
        Matrix4x4 m = Matrix4x4.TRS(target.t, target.r, target.s);
        TRSModel dst = new TRSModel {
            t = m.MultiplyPoint(offset.t),
            r = target.r * offset.r,
            s = new Vector3(
                target.s.x * offset.s.x,
                target.s.y * offset.s.y,
                target.s.z * offset.s.z
            )
        };
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