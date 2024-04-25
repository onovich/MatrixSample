using System.Collections.Generic;
using UnityEngine;

public class TRSSample : MonoBehaviour {

    [SerializeField] Transform parentObject;
    [SerializeField] Transform childObject;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float scaleSpeed = 1f;

    TRSModel originTRS;

    void Awake() {
        originTRS = new TRSModel {
            t = parentObject.InverseTransformPoint(childObject.position),
            r = Quaternion.Inverse(parentObject.rotation) * childObject.rotation,
            s = childObject.localScale.ElementwiseDivide(parentObject.localScale)
        };
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

        TRSModel parntTRS = new TRSModel {
            t = parentObject.position,
            r = parentObject.rotation,
            s = parentObject.localScale
        };

        TRSModel childTRS = MatrixUtil.ApplyTRS(in parntTRS, in originTRS);
        childObject.position = childTRS.t;
        childObject.rotation = childTRS.r;
        childObject.localScale = childTRS.s;
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