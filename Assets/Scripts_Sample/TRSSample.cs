using System.Collections.Generic;
using UnityEngine;

public class TRSSample : MonoBehaviour {

    [SerializeField] Transform parentObject;
    [SerializeField] Transform childObject;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float scaleSpeed = 1f;

    private Vector3 initialRelativePosition;
    private Quaternion initialRelativeRotation;
    private Vector3 initialRelativeScale;

    void Awake() {
        initialRelativePosition = parentObject.InverseTransformPoint(childObject.position);
        initialRelativeRotation = Quaternion.Inverse(parentObject.rotation) * childObject.rotation;
        initialRelativeScale = new Vector3(
            childObject.localScale.x / parentObject.localScale.x,
            childObject.localScale.y / parentObject.localScale.y,
            childObject.localScale.z / parentObject.localScale.z
        );
    }

    void Update() {
        var dt = Time.deltaTime;
        var moveAxis = Vector3.zero;
        var rotateAxis = Vector3.zero;
        var scaleAxis = Vector3.zero;
        if (Input.GetKey(KeyCode.A)) {
            moveAxis = CalMoveAxis(moveAxis, Vector3.left, dt, moveSpeed);
            rotateAxis = CalRotateAxis(rotateAxis, Vector3.up, dt, rotateSpeed);
            scaleAxis = CalScaleAxis(scaleAxis, Vector3.one, dt, scaleSpeed);
        }
        if (Input.GetKey(KeyCode.D)) {
            moveAxis = CalMoveAxis(moveAxis, Vector3.right, dt, moveSpeed);
            rotateAxis = CalRotateAxis(rotateAxis, Vector3.down, dt, rotateSpeed);
            scaleAxis = CalScaleAxis(scaleAxis, -Vector3.one, dt, scaleSpeed);
        }
        if (Input.GetKey(KeyCode.W)) {
            moveAxis = CalMoveAxis(moveAxis, Vector3.forward, dt, moveSpeed);
            rotateAxis = CalRotateAxis(rotateAxis, Vector3.right, dt, rotateSpeed);
        }
        if (Input.GetKey(KeyCode.S)) {
            moveAxis = CalMoveAxis(moveAxis, Vector3.back, dt, moveSpeed);
            rotateAxis = CalRotateAxis(rotateAxis, Vector3.left, dt, rotateSpeed);
        }
        if (Input.GetKey(KeyCode.Q)) {
            moveAxis = CalMoveAxis(moveAxis, Vector3.up, dt, moveSpeed);
            rotateAxis = CalRotateAxis(rotateAxis, Vector3.forward, dt, rotateSpeed);
        }
        if (Input.GetKey(KeyCode.E)) {
            moveAxis = CalMoveAxis(moveAxis, Vector3.down, dt, moveSpeed);
            rotateAxis = CalRotateAxis(rotateAxis, Vector3.back, dt, rotateSpeed);
        }
        if (moveAxis != Vector3.zero) {
            ParentMove(moveAxis);
        }
        if (rotateAxis != Vector3.zero) {
            ParentRotate(rotateAxis);
        }
        if (scaleAxis != Vector3.zero) {
            ParentScale(scaleAxis);
        }

        ApplyTRS(parentObject.position, parentObject.rotation, parentObject.localScale);
    }

    void ApplyTRS(Vector3 t, Quaternion r, Vector3 s) {
        Matrix4x4 m = Matrix4x4.identity;
        m.SetTRS(t, r, s);
        // T
        childObject.position = m.MultiplyPoint(initialRelativePosition);
        // R
        childObject.rotation = r * initialRelativeRotation;
        // S
        childObject.localScale = new Vector3(
            s.x * initialRelativeScale.x,
            s.y * initialRelativeScale.y,
            s.z * initialRelativeScale.z
        );
    }

    Vector3 CalScaleAxis(Vector3 axis, Vector3 dir, float dt, float speed) {
        if (!Input.GetKey(KeyCode.RightShift)) {
            return axis;
        }
        return axis + dir * dt * speed;
    }

    Vector3 CalRotateAxis(Vector3 axis, Vector3 dir, float dt, float speed) {
        if (!Input.GetKey(KeyCode.LeftShift)) {
            return axis;
        }
        return axis + dir * dt * speed;
    }

    Vector3 CalMoveAxis(Vector3 axis, Vector3 dir, float dt, float speed) {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            return axis;
        }
        return axis + dir * dt * speed;
    }

    void ParentMove(Vector3 axis) {
        var parentPos = parentObject.position;
        parentPos += axis;
        parentObject.position = parentPos;
    }

    void ParentRotate(Vector3 axis) {
        var parentRot = parentObject.rotation;
        parentRot *= Quaternion.Euler(axis);
        parentObject.rotation = parentRot;
    }

    void ParentScale(Vector3 axis) {
        var parentScale = parentObject.localScale;
        parentScale += axis;
        parentObject.localScale = parentScale;
    }

    void OnDestroy() {
    }

}