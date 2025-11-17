using UnityEngine;

namespace MobileGame.UI
{
    /// <summary>
    /// 설정 팝업
    /// 게임 설정 옵션을 표시합니다.
    /// </summary>
    public class SettingsPopup : BasePopup
    {
        /// <summary>
        /// 팝업 식별자
        /// </summary>
        public const string PopupName = "SettingsPopup";

        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupName}] 설정 팝업이 열렸습니다.");

            // TODO: 설정 콘텐츠 초기화
            // - 사운드 볼륨 슬라이더
            // - 그래픽 품질 설정
            // - 언어 선택
            // - 알림 설정
        }
    }
}
