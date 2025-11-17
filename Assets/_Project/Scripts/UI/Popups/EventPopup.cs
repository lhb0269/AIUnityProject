using UnityEngine;

namespace MobileGame.UI
{
    /// <summary>
    /// 이벤트 팝업
    /// 게임 내 이벤트 정보를 표시합니다.
    /// </summary>
    public class EventPopup : BasePopup
    {
        /// <summary>
        /// 팝업 식별자
        /// </summary>
        public const string PopupName = "EventPopup";

        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupName}] 이벤트 팝업이 열렸습니다.");

            // TODO: 이벤트 콘텐츠 초기화
            // - 현재 진행 중인 이벤트 목록
            // - 이벤트 보상 정보
            // - 남은 기간 표시
            // - 이벤트 참여 버튼
        }
    }
}
