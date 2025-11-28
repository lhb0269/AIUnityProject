using UnityEngine;

namespace MobileGame.UI
{
    /// <summary>
    /// 스폰 설정 팝업
    /// 몬스터 스폰 설정을 관리합니다.
    /// </summary>
    public class SpawnSettingPopup : BasePopup
    {
        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupID.SpawnSetting}] 스폰 설정 팝업이 열렸습니다.");

            // TODO: 스폰 설정 콘텐츠 초기화
            // - 스폰 간격 설정
            // - 최대 몬스터 수
            // - 난이도 조절
            // - 저장/취소 버튼
        }
    }
}
