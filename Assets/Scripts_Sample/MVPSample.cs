using System.Collections.Generic;
using UnityEngine;

public class MVPSample : MonoBehaviour {

    [SerializeField] GameObject objectPrefab;
    [SerializeField] GameObject panelPrefab;
    [SerializeField] Canvas canvas;
    [SerializeField] int groundLayer;
    [SerializeField] Camera mainCamera;

    CameraModel cameraModel;
    List<GameObject> objects;
    List<GameObject> panels;

    void Awake() {
        objects = new List<GameObject>();
        panels = new List<GameObject>();
        cameraModel = new CameraModel(
            position: mainCamera.transform.position,
            rotation: mainCamera.transform.rotation,
            fov: mainCamera.fieldOfView,
            aspectRatio: mainCamera.aspect,
            nearClip: mainCamera.nearClipPlane,
            farClip: mainCamera.farClipPlane
        );
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << groundLayer)) {
                var depth = hit.distance;
                Vector3 worldPos = CameraMathUtil.ScreenToWorldPos(cameraModel, Input.mousePosition, depth);
                var _worldPos = hit.point;
                GameObject obj = Instantiate(objectPrefab, worldPos, Quaternion.identity);
                GameObject panel = Instantiate(panelPrefab, Vector2.zero, Quaternion.identity, canvas.transform);
                objects.Add(obj);
                panels.Add(panel);
            }
        }

        for (int i = 0; i < panels.Count; i++) {
            var panel = panels[i];
            var worldPos = objects[i].transform.position;
            Vector2 screenPos = CameraMathUtil.WorldToScreenPos(cameraModel, worldPos);
            var _screenPos = mainCamera.WorldToScreenPoint(worldPos);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(), screenPos, null, out var canvasPos);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(), _screenPos, null, out var _canvasPos);

            panel.transform.localPosition = canvasPos;
            // panel.transform.localPosition = _canvasPos;

            // Debug.Log($"WorldPos: {worldPos}, ScreenPos: {screenPos} - _{_screenPos}, CanvasPos: {canvasPos} - {_canvasPos}");
        }
    }

    void OnDestroy() {
        foreach (var obj in objects) {
            Destroy(obj);
        }
        foreach (var panel in panels) {
            Destroy(panel);
        }
        objects.Clear();
        panels.Clear();
    }

}