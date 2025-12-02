using UnityEngine;

namespace MobileGame.UI
{
    /// <summary>
    /// 장비 팝업
    /// 장비 관리 및 인벤토리를 표시합니다.
    /// </summary>
    public class EquipmentPopup : BasePopup
    {
        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupID.Equipment}] 장비 팝업이 열렸습니다.");

            // TODO: 장비 콘텐츠 초기화
            // - 장비 슬롯 표시
            // - 인벤토리 아이템 목록
            // - 장비 스탯 비교
            // - 장착/해제 버튼
        }
    }
}
