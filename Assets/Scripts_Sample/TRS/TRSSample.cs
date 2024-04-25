using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TRSSample : MonoBehaviour {

    #region Serializable Fields
    [SerializeField] Transform parentObject;
    [SerializeField] Transform childObject;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float scaleSpeed = 1f;

    [SerializeField] PressableElement btn_T_R;
    [SerializeField] PressableElement btn_T_L;
    [SerializeField] PressableElement btn_T_U;
    [SerializeField] PressableElement btn_T_D;
    [SerializeField] PressableElement btn_T_F;
    [SerializeField] PressableElement btn_T_B;

    [SerializeField] PressableElement btn_R_R;
    [SerializeField] PressableElement btn_R_L;
    [SerializeField] PressableElement btn_R_U;
    [SerializeField] PressableElement btn_R_D;
    [SerializeField] PressableElement btn_R_F;
    [SerializeField] PressableElement btn_R_B;

    [SerializeField] PressableElement btn_S_Add;
    [SerializeField] PressableElement btn_S_Sub;
    [SerializeField] PressableElement btn_Reset;
    #endregion

    TRSModel offsetTRS;
    TRSModel originTRS;
    TRSInputEntity inputEntity;

    void Awake() {
        inputEntity = new TRSInputEntity();
        inputEntity.Reset();

        offsetTRS = new TRSModel {
            t = parentObject.InverseTransformPoint(childObject.position),
            r = Quaternion.Inverse(parentObject.rotation) * childObject.rotation,
            s = childObject.localScale.ElementwiseDivide(parentObject.localScale)
        };

        originTRS = new TRSModel {
            t = parentObject.position,
            r = parentObject.rotation,
            s = parentObject.localScale
        };
    }

    void Update() {
        var dt = Time.deltaTime;
        inputEntity.BakeInput(dt, moveSpeed, rotateSpeed, scaleSpeed);
        ApplyCheckUI(dt);
        ApplyParentTRS();
        ApplyChildTRS();
        inputEntity.Reset();
    }

    #region ProcessInput
    void ApplyCheckUI(float dt) {
        if (btn_T_R.IsPressing) {
            inputEntity.input_moveAxis = inputEntity.UI_CalAxis(inputEntity.input_moveAxis, Vector3.right, dt, moveSpeed);
        }
        if (btn_T_L.IsPressing) {
            inputEntity.input_moveAxis = inputEntity.UI_CalAxis(inputEntity.input_moveAxis, Vector3.left, dt, moveSpeed);
        }
        if (btn_T_U.IsPressing) {
            inputEntity.input_moveAxis = inputEntity.UI_CalAxis(inputEntity.input_moveAxis, Vector3.up, dt, moveSpeed);
        }
        if (btn_T_D.IsPressing) {
            inputEntity.input_moveAxis = inputEntity.UI_CalAxis(inputEntity.input_moveAxis, Vector3.down, dt, moveSpeed);
        }
        if (btn_T_F.IsPressing) {
            inputEntity.input_moveAxis = inputEntity.UI_CalAxis(inputEntity.input_moveAxis, Vector3.forward, dt, moveSpeed);
        }
        if (btn_T_B.IsPressing) {
            inputEntity.input_moveAxis = inputEntity.UI_CalAxis(inputEntity.input_moveAxis, Vector3.back, dt, moveSpeed);
        }
        if (btn_R_R.IsPressing) {
            inputEntity.input_rotateAxis = inputEntity.UI_CalAxis(inputEntity.input_rotateAxis, Vector3.right, dt, rotateSpeed);
        }
        if (btn_R_L.IsPressing) {
            inputEntity.input_rotateAxis = inputEntity.UI_CalAxis(inputEntity.input_rotateAxis, Vector3.left, dt, rotateSpeed);
        }
        if (btn_R_U.IsPressing) {
            inputEntity.input_rotateAxis = inputEntity.UI_CalAxis(inputEntity.input_rotateAxis, Vector3.up, dt, rotateSpeed);
        }
        if (btn_R_D.IsPressing) {
            inputEntity.input_rotateAxis = inputEntity.UI_CalAxis(inputEntity.input_rotateAxis, Vector3.down, dt, rotateSpeed);
        }
        if (btn_R_F.IsPressing) {
            inputEntity.input_rotateAxis = inputEntity.UI_CalAxis(inputEntity.input_rotateAxis, Vector3.forward, dt, rotateSpeed);
        }
        if (btn_R_B.IsPressing) {
            inputEntity.input_rotateAxis = inputEntity.UI_CalAxis(inputEntity.input_rotateAxis, Vector3.back, dt, rotateSpeed);
        }
        if (btn_S_Add.IsPressing) {
            inputEntity.input_scaleAxis = inputEntity.UI_CalAxis(inputEntity.input_scaleAxis, Vector3.one, dt, scaleSpeed);
        }
        if (btn_S_Sub.IsPressing) {
            inputEntity.input_scaleAxis = inputEntity.UI_CalAxis(inputEntity.input_scaleAxis, -Vector3.one, dt, scaleSpeed);
        }
        if (btn_Reset.IsPressing) {
            ApplyParentReset();
        }
    }
    #endregion

    #region ApplyTRS
    void ApplyParentTRS() {
        ParentMove(inputEntity.input_moveAxis);
        ParentRotate(inputEntity.input_rotateAxis);
        ParentScale(inputEntity.input_scaleAxis);
    }

    void ApplyParentReset() {
        parentObject.position = originTRS.t;
        parentObject.rotation = originTRS.r;
        parentObject.localScale = originTRS.s;
    }

    void ApplyChildTRS() {
        TRSModel parntTRS = new TRSModel {
            t = parentObject.position,
            r = parentObject.rotation,
            s = parentObject.localScale
        };
        TRSModel childTRS = MatrixUtil.ApplyTRSWithOffset(in parntTRS, in offsetTRS);
        childObject.position = childTRS.t;
        childObject.rotation = childTRS.r;
        childObject.localScale = childTRS.s;
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
    #endregion

    void OnDestroy() {
    }

}