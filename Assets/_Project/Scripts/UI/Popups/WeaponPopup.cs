using UnityEngine;

namespace MobileGame.UI
{
    /// <summary>
    /// 무기 팝업
    /// 무기 관리 및 강화를 표시합니다.
    /// </summary>
    public class WeaponPopup : BasePopup
    {
        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupID.Weapon}] 무기 팝업이 열렸습니다.");

            // TODO: 무기 콘텐츠 초기화
            // - 보유 무기 목록
            // - 무기 스탯 표시
            // - 강화/진화 버튼
            // - 장착 버튼
        }
    }
}
