using UnityEngine;

namespace MobileGame.UI
{
    /// <summary>
    /// 유저 정보 팝업
    /// 사용자 프로필 정보를 표시합니다.
    /// </summary>
    public class UserInfoPopup : BasePopup
    {
        /// <summary>
        /// 팝업 식별자
        /// </summary>
        public const string PopupName = "UserInfoPopup";

        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupName}] 유저 정보 팝업이 열렸습니다.");

            // TODO: 유저 정보 콘텐츠 초기화
            // - 사용자 이름, 레벨
            // - 경험치 바
            // - 게임 통계
            // - 업적 요약
        }
    }
}
