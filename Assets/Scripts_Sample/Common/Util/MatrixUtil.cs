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

        Debug.Log("World To Screen:" + " worldSpacePoint = " + worldSpacePoint);
        // World -> View
        Matrix4x4 viewMatrix = camera.GetViewMatrix();
        Vector3 cameraSpacePoint = viewMatrix * new Vector4(worldSpacePoint.x, worldSpacePoint.y, worldSpacePoint.z, 1);
        Debug.Log("World To Screen:" + " cameraSpacePoint = " + cameraSpacePoint);

        // View -> Projection
        Matrix4x4 projectionMatrix = camera.GetProjectionMatrix();
        Vector4 clipSpacePoint = projectionMatrix * cameraSpacePoint;
        Debug.Log("World To Screen:" + " clipSpacePoint = " + clipSpacePoint);

        // Projection -> NDC
        Vector3 ndcPoint = clipSpacePoint / clipSpacePoint.w;
        Debug.Log("World To Screen:" + " ndcPoint = " + ndcPoint);

        // NDC -> ViewPort
        Vector3 viewportPoint = new Vector3(
            (-ndcPoint.x + 1) * 0.5f,
            (-ndcPoint.y + 1) * 0.5f,
            cameraSpacePoint.z
        );
        Debug.Log("World To Screen:" + " viewportPoint = " + viewportPoint);

        // ViewPort -> Screen
        Vector3 screenPos = new Vector3(
            viewportPoint.x * screenSize.x,
            viewportPoint.y * screenSize.y,
            viewportPoint.z
        );
        Debug.Log("World To Screen:" + " screenPos = " + screenPos);

        return screenPos;
    }

    public static Vector3 ScreenToWorldPoint(CameraModel camera, Vector3 screenPoint, Vector2 screenSize) {

        Debug.Log("Screen To World:" + " screenPoint = " + screenPoint);
        // Screen -> ViewPort
        Vector3 viewportPoint = new Vector3(screenPoint.x / screenSize.x, screenPoint.y / screenSize.y, screenPoint.z);
        Debug.Log("Screen To World:" + " viewportPoint = " + viewportPoint);

        // ViewPort -> NDC
        Vector3 ndcPoint = new Vector3(
           1 - 2.0f * viewportPoint.x,
           1 - 2.0f * viewportPoint.y,
           1
        );
        Debug.Log("Screen To World:" + " ndcPoint = " + ndcPoint);

        // NDC -> Projection
        Matrix4x4 projectionMatrix = camera.GetProjectionMatrix();
        Vector4 clipSpacePoint = new Vector4(ndcPoint.x, ndcPoint.y, ndcPoint.z, 1.0f);
        Debug.Log("Screen To World:" + " clipSpacePoint = " + clipSpacePoint);

        // Projection -> View
        Vector4 cameraSpacePoint = projectionMatrix.inverse * clipSpacePoint;
        Debug.Log("Screen To World:" + " cameraSpacePoint = " + cameraSpacePoint);

        // View -> World
        // Vector3 worldSpacePoint = Quaternion.Inverse(camera.Rotation) * cameraSpacePoint;
        Matrix4x4 viewMatrix = camera.GetViewMatrix();
        Vector3 worldSpacePoint = viewMatrix.inverse * new Vector4(cameraSpacePoint.x, cameraSpacePoint.y, cameraSpacePoint.z, 1);

        Debug.Log("Screen To World:" + " worldSpacePoint = " + worldSpacePoint);

        return worldSpacePoint;
    }

}