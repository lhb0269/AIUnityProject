using UnityEngine;

namespace MobileGame.UI
{
    /// <summary>
    /// 모집 팝업
    /// 캐릭터/아이템 모집(가챠) 시스템을 표시합니다.
    /// </summary>
    public class RecruitmentPopup : BasePopup
    {
        /// <summary>
        /// 팝업 식별자
        /// </summary>
        public const string PopupName = "RecruitmentPopup";

        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupName}] 모집 팝업이 열렸습니다.");

            // TODO: 모집 콘텐츠 초기화
            // - 모집 배너 표시
            // - 단일/10연 뽑기 버튼
            // - 확률 정보
            // - 보유 재화 표시
        }
    }
}
