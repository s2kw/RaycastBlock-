# RaycastBlock-
Raycastをブロックしているやつを吊し上げるサンプルプロジェクトです。

環境: UnityEditor 2022.3.33f1

![](readme_imgs/img.gif)

- `Scenes/SampleScene.unity` を開く
- Playボタンを押下して適当にクリック
- クリックした位置から飛ばしたRayに触れたオブジェクト全部を順次Consoleに出力される
- Consoleの各セルをクリックすると触れたオブジェクトにフォーカスする

コードは下記の通り

```c#
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIBlockDetector : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Camera raycastCamera;

    private void Start()
    {
        Debug.Log("UIBlockDetector: Start method called");
        
        if (graphicRaycaster == null)
        {
            graphicRaycaster = GetComponent<GraphicRaycaster>();
        }
        Debug.Log($"GraphicRaycaster: {(graphicRaycaster != null ? "Found" : "Not found")}");

        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<EventSystem>();
        }
        Debug.Log($"EventSystem: {(eventSystem != null ? "Found" : "Not found")}");

        if (raycastCamera == null)
        {
            raycastCamera = Camera.main;
        }
        Debug.Log($"Camera: {(raycastCamera != null ? "Found" : "Not found")}");
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("UIBlockDetector: Mouse button released");
            DetectBlockingUI();
        }
    }

    public void DetectBlockingUI()
    {
        Debug.Log("UIBlockDetector: DetectBlockingUI method called");

        if (graphicRaycaster == null || eventSystem == null || raycastCamera == null)
        {
            Debug.LogError("UIBlockDetector: Missing required components");
            return;
        }

        PointerEventData pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);

        Debug.Log($"UIBlockDetector: Raycast results count: {results.Count}");

        if (results.Count > 0)
        {
            Debug.Log("UIBlockDetector: UI objects blocking the click:");
            foreach (RaycastResult result in results)
            {
                Debug.Log($"- {result.gameObject.name}",result.gameObject);
            }
        }
        else
        {
            Debug.Log("UIBlockDetector: No UI objects blocking the click");
        }

        // 3D object detection
        Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log($"UIBlockDetector: 3D object hit: {hit.collider.gameObject.name}");
        }
        else
        {
            Debug.Log("UIBlockDetector: No 3D object hit");
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(UIBlockDetector))]
    public class UIBlockDetectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            UIBlockDetector detector = (UIBlockDetector)target;

            if (GUILayout.Button("Detect Blocking UI"))
            {
                Debug.Log("UIBlockDetector: Detect button clicked in Inspector");
                detector.DetectBlockingUI();
            }
        }
    }
    #endif
}
```