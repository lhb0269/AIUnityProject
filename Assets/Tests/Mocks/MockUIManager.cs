using System.Collections.Generic;
using UnityEngine;
using MobileGame.Managers;
using MobileGame.Interfaces;
using MobileGame.UI;

namespace MobileGame.Tests.Mocks
{
    /// <summary>
    /// 테스트용 UIManager Mock 클래스
    /// IUIManager 인터페이스를 구현하여 테스트 격리 제공
    /// </summary>
    public class MockUIManager : IUIManager
    {
        // 팝업 표시 이력 추적
        public List<string> ShownPopups { get; private set; } = new List<string>();
        public List<BasePopup> ClosedPopups { get; private set; } = new List<BasePopup>();
        public List<string> ShownPanels { get; private set; } = new List<string>();
        public List<string> HiddenPanels { get; private set; } = new List<string>();

        // 가짜 팝업 카운트
        public int FakeActivePopupCount { get; set; } = 0;

        // 등록된 팝업 프리팹
        private Dictionary<string, GameObject> registeredPopupPrefabs = new Dictionary<string, GameObject>();

        // 등록된 패널
        private Dictionary<string, GameObject> registeredPanels = new Dictionary<string, GameObject>();

        public BasePopup ShowPopup(string popupName)
        {
            ShownPopups.Add(popupName);
            FakeActivePopupCount++;
            Debug.Log($"[MockUIManager] ShowPopup: {popupName}");

            // 실제 팝업 객체 대신 null 반환 (테스트에서는 팝업 인스턴스가 필요없음)
            return null;
        }

        public void ClosePopup(BasePopup popup)
        {
            ClosedPopups.Add(popup);
            if (FakeActivePopupCount > 0)
            {
                FakeActivePopupCount--;
            }
            Debug.Log($"[MockUIManager] ClosePopup: {popup?.GetType().Name}");
        }

        public void CloseCurrentActivePopup()
        {
            if (FakeActivePopupCount > 0)
            {
                FakeActivePopupCount--;
            }
            Debug.Log("[MockUIManager] CloseCurrentActivePopup");
        }

        public void CloseAllActivePopups()
        {
            FakeActivePopupCount = 0;
            Debug.Log("[MockUIManager] CloseAllActivePopups");
        }

        public int GetActivePopupCount()
        {
            return FakeActivePopupCount;
        }

        public bool IsPopupRegistered(string popupName)
        {
            return registeredPopupPrefabs.ContainsKey(popupName);
        }

        public void RegisterPopupPrefab(string name, GameObject prefab)
        {
            registeredPopupPrefabs[name] = prefab;
            Debug.Log($"[MockUIManager] RegisterPopupPrefab: {name}");
        }

        public void RegisterPanel(string panelName, GameObject panel)
        {
            registeredPanels[panelName] = panel;
            Debug.Log($"[MockUIManager] RegisterPanel: {panelName}");
        }

        public void ShowPanel(string panelName)
        {
            ShownPanels.Add(panelName);
            Debug.Log($"[MockUIManager] ShowPanel: {panelName}");
        }

        public void HidePanel(string panelName)
        {
            HiddenPanels.Add(panelName);
            Debug.Log($"[MockUIManager] HidePanel: {panelName}");
        }

        public void HideAllPanels()
        {
            Debug.Log("[MockUIManager] HideAllPanels");
        }

        /// <summary>
        /// 테스트 초기화 - 모든 기록 삭제
        /// </summary>
        public void Reset()
        {
            ShownPopups.Clear();
            ClosedPopups.Clear();
            ShownPanels.Clear();
            HiddenPanels.Clear();
            FakeActivePopupCount = 0;
            registeredPopupPrefabs.Clear();
            registeredPanels.Clear();
        }
    }
}
