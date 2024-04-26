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
        mainCamera.depthTextureMode |= DepthTextureMode.Depth;
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
                Vector3 screenPos = Input.mousePosition;
                float z = Vector3.Dot(hit.point - Camera.main.transform.position, Camera.main.transform.forward);
                screenPos.z = z;

                Vector3 _worldPos = TestScreenToWorldPoint(screenPos);
                Vector3 worldPos = hit.point;
                TestWorldToScreenPoint(worldPos);
            }
        }

    }

    Vector3 TestScreenToWorldPoint(Vector3 screenPos) {
        Vector3 worldPos = MatrixUtil.ScreenToWorldPoint(cameraModel, screenPos, new Vector2(Screen.width, Screen.height));
        // var worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        GameObject obj = Instantiate(objectPrefab, worldPos, Quaternion.identity);
        objects.Add(obj);
        return worldPos;
    }

    void TestWorldToScreenPoint(Vector3 worldPos) {
        GameObject panel = Instantiate(panelPrefab, Vector2.zero, Quaternion.identity, canvas.transform);
        Vector2 screenPos = MatrixUtil.WorldToScreenPoint(cameraModel, worldPos, new Vector2(Screen.width, Screen.height));
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(), screenPos, null, out var canvasPos);

        panel.transform.localPosition = canvasPos;
        panels.Add(panel);
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