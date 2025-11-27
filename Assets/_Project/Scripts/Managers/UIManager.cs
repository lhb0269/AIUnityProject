using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MobileGame.UI;
using MobileGame.Interfaces;

namespace MobileGame.Managers
{
    /// <summary>
    /// UI 패널과 팝업 관리를 담당하는 매니저
    /// 팝업 프리팹 등록, 생성, 스택 관리를 포함합니다.
    /// DI를 통해 주입되어 사용됩니다.
    /// </summary>
    public class UIManager : MonoBehaviour, IUIManager
    {

        [Header("UI 캔버스")]
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private Canvas popupCanvas;

        [Header("팝업 프리팹")]
        [SerializeField] private List<PopupPrefabEntry> initialPopupPrefabs = new List<PopupPrefabEntry>();

        private Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();
        private Stack<GameObject> popupStack = new Stack<GameObject>();

        // 팝업 프리팹 관리용 필드
        private Dictionary<string, GameObject> popupPrefabs = new Dictionary<string, GameObject>();
        private Stack<BasePopup> activePopupStack = new Stack<BasePopup>();
        private int currentSortingOrder;
        private int baseSortingOrder = 100;

        /// <summary>
        /// DI 컨테이너에서 호출하는 초기화 메서드
        /// </summary>
        public void Initialize(Canvas mainCanvas, Canvas popupCanvas, List<PopupPrefabEntry> initialPopupPrefabs)
        {
            this.mainCanvas = mainCanvas;
            this.popupCanvas = popupCanvas;
            this.initialPopupPrefabs = initialPopupPrefabs;

            InitializeCanvases();
            RegisterInitialPrefabs();

            currentSortingOrder = baseSortingOrder;

            Debug.Log("[UIManager] DI 초기화 완료");
        }

        private void Awake()
        {
            // VContainer가 Initialize()를 호출하지 않은 경우를 위한 폴백
            // (테스트 환경 등에서 사용)
            if (mainCanvas == null && popupCanvas == null)
            {
                InitializeCanvases();
                RegisterInitialPrefabs();
                currentSortingOrder = baseSortingOrder;
            }
        }

        /// <summary>
        /// 초기 팝업 프리팹 등록
        /// </summary>
        private void RegisterInitialPrefabs()
        {
            foreach (var entry in initialPopupPrefabs)
            {
                if (!string.IsNullOrEmpty(entry.popupName) && entry.prefab != null)
                {
                    RegisterPopupPrefab(entry.popupName, entry.prefab);
                }
            }
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

        #region 팝업 프리팹 관리 메서드

        /// <summary>
        /// 팝업 프리팹을 등록합니다.
        /// </summary>
        /// <param name="name">팝업 이름</param>
        /// <param name="prefab">팝업 프리팹</param>
        public void RegisterPopupPrefab(string name, GameObject prefab)
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError("[UIManager] 팝업 이름이 비어있습니다.");
                return;
            }

            if (prefab == null)
            {
                Debug.LogError($"[UIManager] 팝업 프리팹이 null입니다: {name}");
                return;
            }

            if (popupPrefabs.ContainsKey(name))
            {
                Debug.LogWarning($"[UIManager] 이미 등록된 팝업 덮어쓰기: {name}");
                popupPrefabs[name] = prefab;
            }
            else
            {
                popupPrefabs.Add(name, prefab);
                Debug.Log($"[UIManager] 팝업 프리팹 등록: {name}");
            }
        }

        /// <summary>
        /// 지정된 이름의 팝업을 표시합니다.
        /// </summary>
        /// <param name="popupName">팝업 이름</param>
        /// <returns>생성된 BasePopup 인스턴스, 실패 시 null</returns>
        public BasePopup ShowPopup(string popupName)
        {
            if (string.IsNullOrEmpty(popupName))
            {
                Debug.LogError("[UIManager] 팝업 이름이 비어있습니다.");
                return null;
            }

            if (!popupPrefabs.TryGetValue(popupName, out GameObject prefab))
            {
                Debug.LogError($"[UIManager] 등록되지 않은 팝업: {popupName}");
                return null;
            }

            if (prefab == null)
            {
                Debug.LogError($"[UIManager] 팝업 프리팹이 null입니다: {popupName}");
                return null;
            }

            // 팝업 인스턴스 생성
            GameObject popupInstance = Instantiate(prefab, popupCanvas.transform);

            if (popupInstance == null)
            {
                Debug.LogError($"[UIManager] 팝업 인스턴스 생성 실패: {popupName}");
                return null;
            }

            // RectTransform 자동 설정 (전체 화면 채우기, 스케일 1로 설정)
            RectTransform rectTransform = popupInstance.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.offsetMin = Vector2.zero;  // Left, Bottom = 0
                rectTransform.offsetMax = Vector2.zero;  // Right, Top = 0
                rectTransform.sizeDelta = Vector2.zero;
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.localScale = Vector3.one;
                rectTransform.localPosition = Vector3.zero;
            }

            BasePopup popup = popupInstance.GetComponent<BasePopup>();

            if (popup == null)
            {
                Debug.LogError($"[UIManager] BasePopup 컴포넌트를 찾을 수 없습니다: {popupName}");
                Destroy(popupInstance);
                return null;
            }

            // 정렬 순서 설정 (각 팝업이 이전 것 위에 표시되도록)
            Canvas popupInstanceCanvas = popupInstance.GetComponent<Canvas>();
            if (popupInstanceCanvas == null)
            {
                popupInstanceCanvas = popupInstance.AddComponent<Canvas>();
            }
            popupInstanceCanvas.overrideSorting = true;
            popupInstanceCanvas.sortingOrder = currentSortingOrder;
            currentSortingOrder += 10;

            // GraphicRaycaster가 없으면 추가
            if (popupInstance.GetComponent<GraphicRaycaster>() == null)
            {
                popupInstance.AddComponent<GraphicRaycaster>();
            }

            // 스택에 추가하고 표시
            activePopupStack.Push(popup);
            popup.Show();

            Debug.Log($"[UIManager] 팝업 표시: {popupName} (활성 팝업 수: {activePopupStack.Count})");

            return popup;
        }

        /// <summary>
        /// 특정 팝업을 닫습니다.
        /// </summary>
        /// <param name="popup">닫을 팝업</param>
        public void ClosePopup(BasePopup popup)
        {
            if (popup == null)
            {
                Debug.LogWarning("[UIManager] 닫을 팝업이 null입니다.");
                return;
            }

            // 스택에서 팝업 제거를 위해 임시 스택 사용
            Stack<BasePopup> tempStack = new Stack<BasePopup>();
            bool found = false;

            while (activePopupStack.Count > 0)
            {
                BasePopup current = activePopupStack.Pop();

                if (current == popup)
                {
                    found = true;
                    current.Hide();
                    Destroy(current.gameObject);
                    currentSortingOrder -= 10;
                    break;
                }
                else
                {
                    tempStack.Push(current);
                }
            }

            // 임시 스택의 팝업들을 다시 원래 스택에 복원
            while (tempStack.Count > 0)
            {
                activePopupStack.Push(tempStack.Pop());
            }

            if (!found)
            {
                Debug.LogWarning("[UIManager] 닫을 팝업을 스택에서 찾을 수 없습니다.");
            }
            else
            {
                Debug.Log($"[UIManager] 팝업 닫기 완료 (활성 팝업 수: {activePopupStack.Count})");
            }
        }

        /// <summary>
        /// 최상단 팝업을 닫습니다 (프리팹 기반).
        /// </summary>
        public void CloseCurrentActivePopup()
        {
            if (activePopupStack.Count == 0)
            {
                Debug.LogWarning("[UIManager] 닫을 팝업이 없습니다.");
                return;
            }

            BasePopup popup = activePopupStack.Pop();

            if (popup != null)
            {
                popup.Hide();
                Destroy(popup.gameObject);
                currentSortingOrder -= 10;
                Debug.Log($"[UIManager] 최상단 팝업 닫기 완료 (활성 팝업 수: {activePopupStack.Count})");
            }
        }

        /// <summary>
        /// 모든 활성 팝업을 닫습니다 (프리팹 기반).
        /// </summary>
        public void CloseAllActivePopups()
        {
            int count = activePopupStack.Count;

            while (activePopupStack.Count > 0)
            {
                BasePopup popup = activePopupStack.Pop();

                if (popup != null)
                {
                    popup.Hide();
                    Destroy(popup.gameObject);
                }
            }

            currentSortingOrder = baseSortingOrder;

            Debug.Log($"[UIManager] 모든 팝업 닫기 완료 ({count}개)");
        }

        /// <summary>
        /// 현재 활성화된 팝업의 수를 반환합니다.
        /// </summary>
        /// <returns>활성 팝업 수</returns>
        public int GetActivePopupCount()
        {
            return activePopupStack.Count;
        }

        /// <summary>
        /// 팝업이 등록되어 있는지 확인합니다.
        /// </summary>
        /// <param name="popupName">팝업 이름</param>
        /// <returns>등록 여부</returns>
        public bool IsPopupRegistered(string popupName)
        {
            return popupPrefabs.ContainsKey(popupName);
        }

        #endregion

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
            if (activePopupStack.Count > 0)
            {
                CloseCurrentActivePopup();
            }
            else if (popupStack.Count > 0)
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

    /// <summary>
    /// 팝업 프리팹 엔트리 (Inspector에서 설정용)
    /// </summary>
    [System.Serializable]
    public class PopupPrefabEntry
    {
        public string popupName;
        public GameObject prefab;
    }
}
