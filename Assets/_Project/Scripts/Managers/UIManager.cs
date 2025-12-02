using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MobileGame.UI;
using MobileGame.Interfaces;
using VContainer;
using VContainer.Unity;

namespace MobileGame.Managers
{
    /// <summary>
    /// UI 패널과 팝업 관리를 담당하는 매니저
    /// 팝업 프리팹 등록, 생성, 스택 관리를 포함합니다.
    /// DI를 통해 주입되어 사용됩니다.
    /// </summary>
    public class UIManager : MonoBehaviour, IUIManager
    {
        // VContainer DI 컨테이너 참조 (동적으로 생성되는 팝업에 의존성 주입용)
        [Inject] private IObjectResolver container;

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

        // 팝업 인스턴스 캐싱 (재사용을 위한 딕셔너리)
        private Dictionary<string, BasePopup> popupInstances = new Dictionary<string, BasePopup>();

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
            Debug.Log($"[UIManager] 등록 시작: {initialPopupPrefabs.Count}개 팝업");
            foreach (var entry in initialPopupPrefabs)
            {
                if (!string.IsNullOrEmpty(entry.popupName) && entry.prefab != null)
                {
                    RegisterPopupPrefab(entry.popupName, entry.prefab);
                    Debug.Log($"[UIManager] 팝업 등록 성공: {entry.popupName}");
                }
                else
                {
                    Debug.LogWarning($"[UIManager] 팝업 등록 실패: Name={entry.popupName ?? "null"}, Prefab={(entry.prefab != null ? "OK" : "null")}");
                }
            }
            Debug.Log($"[UIManager] 최종 등록된 팝업 수: {popupPrefabs.Count}개");
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
        /// 이미 생성된 인스턴스가 있으면 재사용하고, 없으면 새로 생성합니다.
        /// </summary>
        /// <param name="popupName">팝업 이름</param>
        /// <returns>생성/재사용된 BasePopup 인스턴스, 실패 시 null</returns>
        public BasePopup ShowPopup(string popupName)
        {
            if (string.IsNullOrEmpty(popupName))
            {
                Debug.LogError("[UIManager] 팝업 이름이 비어있습니다.");
                return null;
            }

            // 1. 이미 생성된 인스턴스가 있는지 확인 (재사용)
            if (popupInstances.TryGetValue(popupName, out BasePopup existingPopup))
            {
                // 이미 활성화되어 있으면 무시
                if (existingPopup.gameObject.activeSelf)
                {
                    Debug.LogWarning($"[UIManager] 팝업이 이미 열려있습니다: {popupName}");
                    return existingPopup;
                }

                // 비활성화된 인스턴스를 재사용
                existingPopup.gameObject.SetActive(true);

                // 정렬 순서 업데이트
                Canvas existingCanvas = existingPopup.GetComponent<Canvas>();
                if (existingCanvas != null)
                {
                    existingCanvas.sortingOrder = currentSortingOrder;
                    currentSortingOrder += 10;
                }

                // 스택에 추가하고 표시
                activePopupStack.Push(existingPopup);
                existingPopup.Show();

                Debug.Log($"[UIManager] 팝업 재사용: {popupName} (활성 팝업 수: {activePopupStack.Count})");
                return existingPopup;
            }

            // 2. 등록된 프리팹 확인
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

            // 3. 새 팝업 인스턴스 생성
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

            // VContainer를 통해 의존성 주입 (중요: 동적 생성된 오브젝트는 수동 주입 필요)
            if (container != null)
            {
                container.Inject(popup);
                Debug.Log($"[UIManager] 팝업에 DI 주입 완료: {popupName}");
            }
            else
            {
                Debug.LogWarning($"[UIManager] DI 컨테이너가 없어 의존성 주입 불가: {popupName}");
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

            // 4. 캐시에 저장 (재사용을 위해)
            popupInstances[popupName] = popup;

            // 스택에 추가하고 표시
            activePopupStack.Push(popup);
            popup.Show();

            Debug.Log($"[UIManager] 팝업 생성: {popupName} (활성 팝업 수: {activePopupStack.Count})");

            return popup;
        }

        /// <summary>
        /// 특정 팝업을 닫습니다.
        /// 인스턴스는 파괴하지 않고 비활성화하여 재사용 가능하도록 합니다.
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
                    // Destroy 대신 비활성화 (재사용을 위해 인스턴스 유지)
                    current.gameObject.SetActive(false);
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
        /// 최상단 팝업을 닫습니다.
        /// 인스턴스는 파괴하지 않고 비활성화하여 재사용 가능하도록 합니다.
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
                // Destroy 대신 비활성화 (재사용을 위해 인스턴스 유지)
                popup.gameObject.SetActive(false);
                currentSortingOrder -= 10;
                Debug.Log($"[UIManager] 최상단 팝업 닫기 완료 (활성 팝업 수: {activePopupStack.Count})");
            }
        }

        /// <summary>
        /// 모든 활성 팝업을 닫습니다.
        /// 인스턴스는 파괴하지 않고 비활성화하여 재사용 가능하도록 합니다.
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
                    // Destroy 대신 비활성화 (재사용을 위해 인스턴스 유지)
                    popup.gameObject.SetActive(false);
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

        /// <summary>
        /// 특정 팝업 인스턴스를 완전히 제거합니다 (메모리 정리용).
        /// 일반적으로는 필요하지 않지만, 메모리 관리가 필요한 경우 사용합니다.
        /// </summary>
        /// <param name="popupName">제거할 팝업 이름</param>
        public void DestroyPopupInstance(string popupName)
        {
            if (popupInstances.TryGetValue(popupName, out BasePopup popup))
            {
                if (popup != null && popup.gameObject != null)
                {
                    Destroy(popup.gameObject);
                }
                popupInstances.Remove(popupName);
                Debug.Log($"[UIManager] 팝업 인스턴스 제거: {popupName}");
            }
        }

        /// <summary>
        /// 모든 캐시된 팝업 인스턴스를 제거합니다 (메모리 정리용).
        /// 씬 전환 전이나 메모리 부족 시 호출합니다.
        /// </summary>
        public void DestroyAllPopupInstances()
        {
            int count = popupInstances.Count;

            foreach (var popup in popupInstances.Values)
            {
                if (popup != null && popup.gameObject != null)
                {
                    Destroy(popup.gameObject);
                }
            }

            popupInstances.Clear();
            activePopupStack.Clear();
            currentSortingOrder = baseSortingOrder;

            Debug.Log($"[UIManager] 모든 팝업 인스턴스 제거 완료 ({count}개)");
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
