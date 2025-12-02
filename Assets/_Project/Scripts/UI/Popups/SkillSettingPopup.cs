using UnityEngine;

namespace MobileGame.UI
{
    /// <summary>
    /// 스킬 설정 팝업
    /// 스킬 배치 및 설정을 관리합니다.
    /// </summary>
    public class SkillSettingPopup : BasePopup
    {
        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupID.SkillSetting}] 스킬 설정 팝업이 열렸습니다.");

            // TODO: 스킬 설정 콘텐츠 초기화
            // - 사용 가능한 스킬 목록
            // - 스킬 슬롯 배치
            // - 스킬 정보 표시
            // - 저장/취소 버튼
        }
    }
}
