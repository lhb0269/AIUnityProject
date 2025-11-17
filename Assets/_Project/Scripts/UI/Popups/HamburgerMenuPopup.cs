using UnityEngine;

namespace MobileGame.UI
{
    /// <summary>
    /// 햄버거 메뉴 팝업
    /// 사이드 메뉴 옵션을 표시합니다.
    /// </summary>
    public class HamburgerMenuPopup : BasePopup
    {
        /// <summary>
        /// 팝업 식별자
        /// </summary>
        public const string PopupName = "HamburgerMenuPopup";

        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupName}] 햄버거 메뉴 팝업이 열렸습니다.");

            // TODO: 햄버거 메뉴 콘텐츠 초기화
            // - 메뉴 아이템 목록 표시
            // - 사용자 설정 옵션
            // - 게임 종료, 로그아웃 등
        }
    }
}
