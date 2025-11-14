using UnityEngine;
using System.Collections.Generic;

namespace MobileGame.Managers
{
    /// <summary>
    /// UI 패널과 팝업 관리를 담당하는 매니저
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("UI 캔버스")]
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private Canvas popupCanvas;

        private Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();
        private Stack<GameObject> popupStack = new Stack<GameObject>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeCanvases();
        }

        /// <summary>
        /// 캔버스 초기화
        /// </summary>
        private void InitializeCanvases()
        {
            if (mainCanvas == null)
            {
                GameObject canvasObj = new GameObject("MainCanvas");
                mainCanvas = canvasObj.AddComponent<Canvas>();
                mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                mainCanvas.sortingOrder = 0;

                canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>().uiScaleMode =
                    UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

                canvasObj.transform.SetParent(transform);
            }

            if (popupCanvas == null)
            {
                GameObject popupObj = new GameObject("PopupCanvas");
                popupCanvas = popupObj.AddComponent<Canvas>();
                popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                popupCanvas.sortingOrder = 100;

                popupObj.AddComponent<UnityEngine.UI.CanvasScaler>().uiScaleMode =
                    UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
                popupObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

                popupObj.transform.SetParent(transform);
            }
        }

        /// <summary>
        /// 패널 등록
        /// </summary>
        public void RegisterPanel(string panelName, GameObject panel)
        {
            if (!panels.ContainsKey(panelName))
            {
                panels.Add(panelName, panel);
                panel.SetActive(false);
                Debug.Log($"[UIManager] 패널 등록: {panelName}");
            }
        }

        /// <summary>
        /// 패널 표시
        /// </summary>
        public void ShowPanel(string panelName)
        {
            if (panels.TryGetValue(panelName, out GameObject panel))
            {
                panel.SetActive(true);
                Debug.Log($"[UIManager] 패널 표시: {panelName}");
            }
            else
            {
                Debug.LogWarning($"[UIManager] 패널을 찾을 수 없습니다: {panelName}");
            }
        }

        /// <summary>
        /// 패널 숨기기
        /// </summary>
        public void HidePanel(string panelName)
        {
            if (panels.TryGetValue(panelName, out GameObject panel))
            {
                panel.SetActive(false);
                Debug.Log($"[UIManager] 패널 숨김: {panelName}");
            }
        }

        /// <summary>
        /// 모든 패널 숨기기
        /// </summary>
        public void HideAllPanels()
        {
            foreach (var panel in panels.Values)
            {
                panel.SetActive(false);
            }
            Debug.Log("[UIManager] 모든 패널 숨김");
        }

        /// <summary>
        /// 팝업 표시
        /// </summary>
        public void ShowPopup(GameObject popup)
        {
            if (popup == null)
            {
                Debug.LogWarning("[UIManager] 팝업이 null입니다.");
                return;
            }

            popup.transform.SetParent(popupCanvas.transform, false);
            popup.SetActive(true);
            popupStack.Push(popup);

            Debug.Log($"[UIManager] 팝업 표시: {popup.name}");
        }

        /// <summary>
        /// 현재 팝업 닫기
        /// </summary>
        public void CloseCurrentPopup()
        {
            if (popupStack.Count > 0)
            {
                GameObject popup = popupStack.Pop();
                popup.SetActive(false);
                Debug.Log($"[UIManager] 팝업 닫기: {popup.name}");
            }
        }

        /// <summary>
        /// 모든 팝업 닫기
        /// </summary>
        public void CloseAllPopups()
        {
            while (popupStack.Count > 0)
            {
                GameObject popup = popupStack.Pop();
                popup.SetActive(false);
            }
            Debug.Log("[UIManager] 모든 팝업 닫기");
        }

        /// <summary>
        /// 뒤로 가기 버튼 처리 (Android)
        /// </summary>
        private void Update()
        {
            // Android 뒤로 가기 버튼
            // if (Input.GetKeyDown(KeyCode.Escape))
            // {
            //     HandleBackButton();
            // }
        }

        private void HandleBackButton()
        {
            if (popupStack.Count > 0)
            {
                CloseCurrentPopup();
            }
            else
            {
                // 뒤로 가기 동작 (예: 일시정지 메뉴 표시)
                Debug.Log("[UIManager] 뒤로 가기 버튼 감지");
            }
        }
    }
}
