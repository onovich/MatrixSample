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
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(Rotation);
        Vector3 inversePosition = -Position;
        Matrix4x4 translationMatrix = Matrix4x4.Translate(inversePosition);
        return rotationMatrix * translationMatrix;
    }

}