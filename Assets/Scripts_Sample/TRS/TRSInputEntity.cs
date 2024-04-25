using UnityEngine;

public class TRSInputEntity {

    public Vector3 input_moveAxis;
    public Vector3 input_rotateAxis;
    public Vector3 input_scaleAxis;

    public void Reset() {
        input_moveAxis = Vector3.zero;
        input_rotateAxis = Vector3.zero;
        input_scaleAxis = Vector3.zero;
    }

    public void Input_SetMoveAxis(Vector3 axis) {
        input_moveAxis = axis;
    }

    public void Input_SetRotateAxis(Vector3 axis) {
        input_rotateAxis = axis;
    }

    public void Input_SetScaleAxis(Vector3 axis) {
        input_scaleAxis = axis;
    }

    public void BakeInput(float dt, float moveSpeed, float rotateSpeed, float scaleSpeed) {
        if (Input.GetKey(KeyCode.A)) {
            input_moveAxis = Input_CalMoveAxis(input_moveAxis, Vector3.left, dt, moveSpeed);
            input_rotateAxis = Input_CalRotateAxis(input_rotateAxis, Vector3.up, dt, rotateSpeed);
            input_scaleAxis = Input_CalScaleAxis(input_scaleAxis, Vector3.one, dt, scaleSpeed);
        }
        if (Input.GetKey(KeyCode.D)) {
            input_moveAxis = Input_CalMoveAxis(input_moveAxis, Vector3.right, dt, moveSpeed);
            input_rotateAxis = Input_CalRotateAxis(input_rotateAxis, Vector3.down, dt, rotateSpeed);
            input_scaleAxis = Input_CalScaleAxis(input_scaleAxis, -Vector3.one, dt, scaleSpeed);
        }
        if (Input.GetKey(KeyCode.W)) {
            input_moveAxis = Input_CalMoveAxis(input_moveAxis, Vector3.forward, dt, moveSpeed);
            input_rotateAxis = Input_CalRotateAxis(input_rotateAxis, Vector3.right, dt, rotateSpeed);
        }
        if (Input.GetKey(KeyCode.S)) {
            input_moveAxis = Input_CalMoveAxis(input_moveAxis, Vector3.back, dt, moveSpeed);
            input_rotateAxis = Input_CalRotateAxis(input_rotateAxis, Vector3.left, dt, rotateSpeed);
        }
        if (Input.GetKey(KeyCode.Q)) {
            input_moveAxis = Input_CalMoveAxis(input_moveAxis, Vector3.up, dt, moveSpeed);
            input_rotateAxis = Input_CalRotateAxis(input_rotateAxis, Vector3.forward, dt, rotateSpeed);
        }
        if (Input.GetKey(KeyCode.E)) {
            input_moveAxis = Input_CalMoveAxis(input_moveAxis, Vector3.down, dt, moveSpeed);
            input_rotateAxis = Input_CalRotateAxis(input_rotateAxis, Vector3.back, dt, rotateSpeed);
        }
    }

    Vector3 Input_CalScaleAxis(Vector3 axis, Vector3 dir, float dt, float speed) {
        if (!Input.GetKey(KeyCode.RightShift)) {
            return axis;
        }
        return axis + dir * dt * speed;
    }

    Vector3 Input_CalRotateAxis(Vector3 axis, Vector3 dir, float dt, float speed) {
        if (!Input.GetKey(KeyCode.LeftShift)) {
            return axis;
        }
        return axis + dir * dt * speed;
    }

    Vector3 Input_CalMoveAxis(Vector3 axis, Vector3 dir, float dt, float speed) {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            return axis;
        }
        return axis + dir * dt * speed;
    }

    public Vector3 UI_CalAxis(Vector3 axis, Vector3 dir, float dt, float speed) {
        return axis + dir * dt * speed;
    }

}