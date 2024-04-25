using UnityEngine;

public class CameraModel {
    public Vector3 Position;
    public Quaternion Rotation;
    public float FieldOfView;
    public float AspectRatio;
    public float NearClip;
    public float FarClip;

    public CameraModel(Vector3 position, Quaternion rotation, float fov, float aspectRatio, float nearClip, float farClip) {
        Position = position;
        Rotation = rotation;
        FieldOfView = fov;
        AspectRatio = aspectRatio;
        NearClip = nearClip;
        FarClip = farClip;
    }

    public Matrix4x4 GetProjectionMatrix() {
        return Matrix4x4.Perspective(FieldOfView, AspectRatio, NearClip, FarClip);
    }

    public Matrix4x4 GetViewMatrix() {
        var m = Matrix4x4.TRS(Position, Rotation, Vector3.one);
        m = Matrix4x4.Inverse(m);
        // m.m20 *= -1f;
        // m.m21 *= -1f;
        // m.m22 *= -1f;
        // m.m23 *= -1f;
        return m;
    }

}