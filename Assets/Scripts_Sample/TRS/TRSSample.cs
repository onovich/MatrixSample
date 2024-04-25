using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TRSSample : MonoBehaviour {

    [SerializeField] Transform parentObject;
    [SerializeField] Transform childObject;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float scaleSpeed = 1f;

    [SerializeField] Button btn_T_R;
    [SerializeField] Button btn_T_L;
    [SerializeField] Button btn_T_U;
    [SerializeField] Button btn_T_D;
    [SerializeField] Button btn_T_F;
    [SerializeField] Button btn_T_B;

    [SerializeField] Button btn_R_R;
    [SerializeField] Button btn_R_L;
    [SerializeField] Button btn_R_U;
    [SerializeField] Button btn_R_D;
    [SerializeField] Button btn_R_F;
    [SerializeField] Button btn_R_B;

    [SerializeField] Button btn_S_Add;
    [SerializeField] Button btn_S_Sub;

    TRSModel originTRS;
    Vector3 moveAxis;
    Vector3 rotateAxis;
    Vector3 scaleAxis;

    void Awake() {
        moveAxis = Vector3.zero;
        rotateAxis = Vector3.zero;
        scaleAxis = Vector3.zero;

        originTRS = new TRSModel {
            t = parentObject.InverseTransformPoint(childObject.position),
            r = Quaternion.Inverse(parentObject.rotation) * childObject.rotation,
            s = childObject.localScale.ElementwiseDivide(parentObject.localScale)
        };

        btn_T_R.onClick.AddListener(() => moveAxis += CalMoveAxis(moveAxis, Vector3.right, Time.deltaTime, moveSpeed));
        btn_T_L.onClick.AddListener(() => moveAxis += CalMoveAxis(moveAxis, Vector3.left, Time.deltaTime, moveSpeed));
        btn_T_U.onClick.AddListener(() => moveAxis += CalMoveAxis(moveAxis, Vector3.up, Time.deltaTime, moveSpeed));
        btn_T_D.onClick.AddListener(() => moveAxis += CalMoveAxis(moveAxis, Vector3.down, Time.deltaTime, moveSpeed));
        btn_T_F.onClick.AddListener(() => moveAxis += CalMoveAxis(moveAxis, Vector3.forward, Time.deltaTime, moveSpeed));
        btn_T_B.onClick.AddListener(() => moveAxis += CalMoveAxis(moveAxis, Vector3.back, Time.deltaTime, moveSpeed));

        btn_R_R.onClick.AddListener(() => rotateAxis += CalRotateAxis(rotateAxis, Vector3.right, Time.deltaTime, rotateSpeed));
        btn_R_L.onClick.AddListener(() => rotateAxis += CalRotateAxis(rotateAxis, Vector3.left, Time.deltaTime, rotateSpeed));
        btn_R_U.onClick.AddListener(() => rotateAxis += CalRotateAxis(rotateAxis, Vector3.up, Time.deltaTime, rotateSpeed));
        btn_R_D.onClick.AddListener(() => rotateAxis += CalRotateAxis(rotateAxis, Vector3.down, Time.deltaTime, rotateSpeed));
        btn_R_F.onClick.AddListener(() => rotateAxis += CalRotateAxis(rotateAxis, Vector3.forward, Time.deltaTime, rotateSpeed));
        btn_R_B.onClick.AddListener(() => rotateAxis += CalRotateAxis(rotateAxis, Vector3.back, Time.deltaTime, rotateSpeed));

        btn_S_Add.onClick.AddListener(() => scaleAxis += CalScaleAxis(scaleAxis, Vector3.one, Time.deltaTime, scaleSpeed));
        btn_S_Sub.onClick.AddListener(() => scaleAxis += CalScaleAxis(scaleAxis, -Vector3.one, Time.deltaTime, scaleSpeed));
    }

    void Update() {
        var dt = Time.deltaTime;
        BakeInput(dt);
        ApplyParentTRS();
        ApplyChildTRS();
        ResetInput();
    }

    void BakeInput(float dt) {
        if (Input.GetKey(KeyCode.A)) {
            moveAxis += CalMoveAxis(moveAxis, Vector3.left, dt, moveSpeed);
            rotateAxis += CalRotateAxis(rotateAxis, Vector3.up, dt, rotateSpeed);
            scaleAxis += CalScaleAxis(scaleAxis, Vector3.one, dt, scaleSpeed);
        }
        if (Input.GetKey(KeyCode.D)) {
            moveAxis += CalMoveAxis(moveAxis, Vector3.right, dt, moveSpeed);
            rotateAxis += CalRotateAxis(rotateAxis, Vector3.down, dt, rotateSpeed);
            scaleAxis += CalScaleAxis(scaleAxis, -Vector3.one, dt, scaleSpeed);
        }
        if (Input.GetKey(KeyCode.W)) {
            moveAxis += CalMoveAxis(moveAxis, Vector3.forward, dt, moveSpeed);
            rotateAxis += CalRotateAxis(rotateAxis, Vector3.right, dt, rotateSpeed);
        }
        if (Input.GetKey(KeyCode.S)) {
            moveAxis += CalMoveAxis(moveAxis, Vector3.back, dt, moveSpeed);
            rotateAxis += CalRotateAxis(rotateAxis, Vector3.left, dt, rotateSpeed);
        }
        if (Input.GetKey(KeyCode.Q)) {
            moveAxis += CalMoveAxis(moveAxis, Vector3.up, dt, moveSpeed);
            rotateAxis += CalRotateAxis(rotateAxis, Vector3.forward, dt, rotateSpeed);
        }
        if (Input.GetKey(KeyCode.E)) {
            moveAxis += CalMoveAxis(moveAxis, Vector3.down, dt, moveSpeed);
            rotateAxis += CalRotateAxis(rotateAxis, Vector3.back, dt, rotateSpeed);
        }
    }

    void ApplyParentTRS() {
        ParentMove(moveAxis);
        ParentRotate(rotateAxis);
        ParentScale(scaleAxis);
    }

    void ApplyChildTRS() {
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

    void ResetInput() {
        moveAxis = Vector3.zero;
        rotateAxis = Vector3.zero;
        scaleAxis = Vector3.zero;
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
        btn_T_R.onClick.RemoveAllListeners();
        btn_T_L.onClick.RemoveAllListeners();
        btn_T_U.onClick.RemoveAllListeners();
        btn_T_D.onClick.RemoveAllListeners();
        btn_T_F.onClick.RemoveAllListeners();
        btn_T_B.onClick.RemoveAllListeners();
        btn_R_R.onClick.RemoveAllListeners();
        btn_R_L.onClick.RemoveAllListeners();
        btn_R_U.onClick.RemoveAllListeners();
        btn_R_D.onClick.RemoveAllListeners();
        btn_R_F.onClick.RemoveAllListeners();
        btn_R_B.onClick.RemoveAllListeners();
        btn_S_Add.onClick.RemoveAllListeners();
        btn_S_Sub.onClick.RemoveAllListeners();
    }

}