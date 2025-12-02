using UnityEngine;
using MobileGame.UI;

namespace MobileGame.Interfaces
{
    /// <summary>
    /// UI 관리 인터페이스
    /// 팝업 및 패널 관리 기능을 정의
    /// </summary>
    public interface IUIManager
    {
        // 팝업 관리
        BasePopup ShowPopup(string popupName);
        void ClosePopup(BasePopup popup);
        void CloseCurrentActivePopup();
        void CloseAllActivePopups();
        int GetActivePopupCount();
        bool IsPopupRegistered(string popupName);

        // 팝업 프리팹 등록
        void RegisterPopupPrefab(string name, GameObject prefab);

        // 패널 관리
        void RegisterPanel(string panelName, GameObject panel);
        void ShowPanel(string panelName);
        void HidePanel(string panelName);
        void HideAllPanels();
    }
}
