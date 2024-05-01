using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class TRSFollowSample : MonoBehaviour {

    #region Serializable Fields
    [SerializeField] Transform parentObject;
    [SerializeField] Transform childObject;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float scaleSpeed = 1f;
    [SerializeField] float followDampingFactor = .25f;
    [SerializeField] float rotationDampingFactor = .25f;
    [SerializeField] float scaleDampingFactor = .25f;

    [SerializeField] Button resetButton;
    [SerializeField] Dropdown dropdown;
    #endregion

    public enum FollowType {
        FollowXYZ,
        FollowYZ,
        FollowYZAndRound,
    }

    FollowType followType;
    TRSModel offsetTRS;
    TRSModel originTRS;
    Quaternion patrentDirRot;
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

        var followTypes = System.Enum.GetNames(typeof(FollowType));
        dropdown.options.Clear();
        for (int i = 0; i < followTypes.Length; i++) {
            dropdown.options.Add(new Dropdown.OptionData(followTypes[i]));
        }
        dropdown.value = 0;
        dropdown.onValueChanged.AddListener((int index) => {
            followType = (FollowType)index;
        });

        resetButton.onClick.AddListener(() => {
            ApplyParentReset();
        });

        patrentDirRot = Quaternion.identity;
    }

    void Update() {
        var dt = Time.deltaTime;
        inputEntity.BakeInput(dt, moveSpeed, rotateSpeed, scaleSpeed);
        ApplyParentTRS();
        ApplyChildTRS();
        inputEntity.Reset();
    }

    #region ApplyTRS
    void ApplyParentTRS() {
        ParentMove(inputEntity.input_moveAxis);
        ParentRotate(inputEntity.input_rotateAxis);
        ParentScale(inputEntity.input_scaleAxis);
        ParentFace(inputEntity.input_moveAxis);
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
        TRSModel parntDirTRS = new TRSModel {
            t = parentObject.position,
            r = patrentDirRot,
            s = parentObject.localScale
        };
        TRSModel childTRS = MatrixUtil.ApplyTRSWithOffset(in parntTRS, in offsetTRS);
        TRSModel childDirTRS = MatrixUtil.ApplyTRSWithOffset(in parntDirTRS, in offsetTRS);

        if (followType == FollowType.FollowXYZ) {
            childObject.position = Vector3.Lerp(childObject.position, childTRS.t, followDampingFactor);
            childObject.rotation = Quaternion.Lerp(childObject.rotation, childTRS.r, rotationDampingFactor);
            childObject.localScale = Vector3.Lerp(childObject.localScale, childTRS.s, scaleDampingFactor);
        }
        if (followType == FollowType.FollowYZ) {
            var t = new Vector3(childObject.position.x, childTRS.t.y, childTRS.t.z);
            childObject.position = Vector3.Lerp(childObject.position, t, followDampingFactor);
            childObject.rotation = Quaternion.Lerp(childObject.rotation, childTRS.r, rotationDampingFactor);
            childObject.localScale = Vector3.Lerp(childObject.localScale, childTRS.s, scaleDampingFactor);
        }
        if (followType == FollowType.FollowYZAndRound) {
            childObject.position = Vector3.Lerp(childObject.position, childDirTRS.t, followDampingFactor);
            childObject.rotation = Quaternion.Lerp(childObject.rotation, childDirTRS.r, rotationDampingFactor);
            childObject.localScale = Vector3.Lerp(childObject.localScale, childDirTRS.s, scaleDampingFactor);
        }

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

    void ParentFace(Vector3 axis) {
        if (axis == Vector3.zero) return;
        patrentDirRot = Quaternion.LookRotation(axis);
    }
    #endregion

    void OnDestroy() {
    }

}