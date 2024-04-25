using UnityEngine;

public static class CameraMathUtil {

    // TRS
    public static float GetModelDepth(Vector2 screenPoint, Vector2 screenSize, float nearClip, float fov) {
        float ndcX = (2.0f * (screenPoint.x / screenSize.x) - 1.0f);
        float ndcY = 1.0f - 2.0f * (screenPoint.y / screenSize.y);
        float depth = nearClip / Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        return depth;
    }

}