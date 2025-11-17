using UnityEngine;

namespace MobileGame.UI
{
    /// <summary>
    /// 포션 설정 팝업
    /// 포션 자동 사용 및 설정을 관리합니다.
    /// </summary>
    public class PotionSettingPopup : BasePopup
    {
        /// <summary>
        /// 팝업 식별자
        /// </summary>
        public const string PopupName = "PotionSettingPopup";

        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupName}] 포션 설정 팝업이 열렸습니다.");

            // TODO: 포션 설정 콘텐츠 초기화
            // - 자동 사용 설정 토글
            // - HP/MP 임계값 슬라이더
            // - 포션 우선순위 설정
            // - 저장/취소 버튼
        }
    }
}
