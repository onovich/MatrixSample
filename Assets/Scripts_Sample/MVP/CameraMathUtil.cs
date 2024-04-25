using UnityEngine;

public static class CameraMathUtil {
    // 世界坐标到屏幕坐标的转换
    public static Vector2 WorldToScreenPos(CameraModel cam, Vector3 worldPos) {
        // 获取 MVP 矩阵
        Matrix4x4 mvp = cam.GetProjectionMatrix() * cam.GetViewMatrix();

        // 将世界坐标转换为裁剪空间坐标
        Vector4 clipSpacePosition = mvp * new Vector4(worldPos.x, worldPos.y, worldPos.z, 1);

        // 透视除法，从裁剪空间转换到 NDC (Normalized Device Coordinates)
        if (clipSpacePosition.w != 0)
            clipSpacePosition /= clipSpacePosition.w;

        // 将 NDC 映射到屏幕坐标
        Vector3 screenPosition = new Vector3(
            (clipSpacePosition.x + 1) * 0.5f * Screen.width,
            (clipSpacePosition.y + 1) * 0.5f * Screen.height,
            clipSpacePosition.z);

        return screenPosition;
    }

    // 屏幕坐标到世界坐标的转换
    public static Vector3 ScreenToWorldPos(CameraModel cam, Vector2 screenPosition, float depth) {
        // 将屏幕坐标转换回 NDC
        Vector3 ndc = new Vector3(
            (screenPosition.x / Screen.width) * 2 - 1,
            (screenPosition.y / Screen.height) * 2 - 1,
            depth);

        // 逆 MVP 矩阵
        Matrix4x4 inverseMVP = (cam.GetProjectionMatrix() * cam.GetViewMatrix()).inverse;

        // 从 NDC 到世界坐标
        Vector4 worldPosition = inverseMVP * new Vector4(ndc.x, ndc.y, ndc.z, 1);

        if (worldPosition.w != 0)
            worldPosition /= worldPosition.w;

        return new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
    }

}