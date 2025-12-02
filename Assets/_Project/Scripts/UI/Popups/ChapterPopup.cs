using UnityEngine;

namespace MobileGame.UI
{
    /// <summary>
    /// 챕터 팝업
    /// 게임 챕터 선택 및 정보를 표시합니다.
    /// </summary>
    public class ChapterPopup : BasePopup
    {
        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupID.Chapter}] 챕터 팝업이 열렸습니다.");

            // TODO: 챕터 콘텐츠 초기화
            // - 챕터 목록 표시
            // - 진행률 표시
            // - 보상 정보
            // - 챕터 시작 버튼
        }
    }
}
