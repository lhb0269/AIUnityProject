using UnityEngine;

namespace MobileGame.UI
{
    /// <summary>
    /// 협력자 팝업
    /// 협력자(동료) 관리를 표시합니다.
    /// </summary>
    public class CoworkerPopup : BasePopup
    {
        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupID.Coworker}] 협력자 팝업이 열렸습니다.");

            // TODO: 협력자 콘텐츠 초기화
            // - 협력자 목록
            // - 협력자 스탯 및 능력
            // - 파티 편성
            // - 협력자 강화
        }
    }
}
