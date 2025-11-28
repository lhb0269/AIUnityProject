using UnityEngine;

namespace MobileGame.UI
{
    /// <summary>
    /// 상점 팝업
    /// 게임 내 상점 인터페이스를 표시합니다.
    /// </summary>
    public class ShopPopup : BasePopup
    {
        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupID.Shop}] 상점 팝업이 열렸습니다.");

            // TODO: 상점 콘텐츠 초기화
            // - 아이템 카테고리 탭
            // - 상품 목록 표시
            // - 구매 버튼 및 가격
            // - 인앱 구매 연동
        }
    }
}
