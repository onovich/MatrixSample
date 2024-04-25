using UnityEngine;

public static class CameraMathUtil {

    public static float GetModelDepth(Vector2 screenPoint, Vector2 screenSize, float nearClip, float fov) {
        float ndcX = (2.0f * (screenPoint.x / screenSize.x) - 1.0f);
        float ndcY = (2.0f * (screenPoint.y / screenSize.y) - 1.0f);

        float angleX = Mathf.Atan(Mathf.Tan(fov * Mathf.Deg2Rad / 2.0f) * ndcX);
        float angleY = Mathf.Atan(Mathf.Tan(fov * Mathf.Deg2Rad / 2.0f) * ndcY);

        float depth = nearClip / Mathf.Cos(Mathf.Max(Mathf.Abs(angleX), Mathf.Abs(angleY)));
        return depth;
    }

}