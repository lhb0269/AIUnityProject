using UnityEngine;

namespace MobileGame.UI
{
    /// <summary>
    /// 캐릭터 팝업
    /// 캐릭터 관리 및 정보를 표시합니다.
    /// </summary>
    public class CharacterPopup : BasePopup
    {
        /// <summary>
        /// 팝업 식별자
        /// </summary>
        public const string PopupName = "CharacterPopup";

        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupName}] 캐릭터 팝업이 열렸습니다.");

            // TODO: 캐릭터 콘텐츠 초기화
            // - 캐릭터 목록 표시
            // - 스탯 정보
            // - 레벨업/강화 버튼
            // - 스킬 트리
        }
    }
}
