using UnityEngine;
using UnityEngine.UI;
using MobileGame.Managers;

namespace MobileGame.UI
{
    /// <summary>
    /// 햄버거 메뉴 팝업
    /// 사이드 메뉴 옵션을 표시합니다.
    /// </summary>
    public class HamburgerMenuPopup : BasePopup
    {
        #region 버튼 참조 필드

        [Header("일반 버튼 (로그만 출력)")]
        [SerializeField] private Button missionBtn;           // 미션 버튼
        [SerializeField] private Button passBtn;              // 패스 버튼
        [SerializeField] private Button mailboxBtn;           // 우편함 버튼
        [SerializeField] private Button costumeBtn;           // 코스튬 버튼
        [SerializeField] private Button heroPowerBtn;         // 용사의 힘 버튼
        [SerializeField] private Button equipSlotEnhanceBtn; // 장비 슬롯 강화 버튼
        [SerializeField] private Button relicBtn;             // 유물 버튼
        [SerializeField] private Button friendBtn;            // 친구 버튼
        [SerializeField] private Button rankingBtn;           // 랭킹 버튼
        [SerializeField] private Button guildBtn;             // 길드 버튼
        [SerializeField] private Button growthDungeonBtn;     // 성장 던전 버튼
        [SerializeField] private Button worldBossBtn;         // 월드 보스 버튼

        [Header("팝업 버튼 (추가 팝업 열기)")]
        [SerializeField] private Button townBtn;              // 마을 버튼
        [SerializeField] private Button noticeBtn;            // 공지사항 버튼
        [SerializeField] private Button gameSettingBtn;       // 게임 설정 버튼

        #endregion

        #region Unity 생명주기

        /// <summary>
        /// 시작 시 초기화 및 버튼 이벤트 연결
        /// </summary>
        protected override void Start()
        {
            base.Start();
            RegisterButtonEvents();
        }

        /// <summary>
        /// 파괴 시 이벤트 해제
        /// </summary>
        protected override void OnDestroy()
        {
            UnregisterButtonEvents();
            base.OnDestroy();
        }

        #endregion

        #region 버튼 이벤트 등록/해제

        /// <summary>
        /// 모든 버튼의 onClick 이벤트 등록
        /// </summary>
        private void RegisterButtonEvents()
        {
            RegisterButton(missionBtn, OnMissionClicked);
            RegisterButton(passBtn, OnPassClicked);
            RegisterButton(mailboxBtn, OnMailboxClicked);
            RegisterButton(costumeBtn, OnCostumeClicked);
            RegisterButton(heroPowerBtn, OnHeroPowerClicked);
            RegisterButton(equipSlotEnhanceBtn, OnEquipSlotEnhanceClicked);
            RegisterButton(relicBtn, OnRelicClicked);
            RegisterButton(friendBtn, OnFriendClicked);
            RegisterButton(rankingBtn, OnRankingClicked);
            RegisterButton(guildBtn, OnGuildClicked);
            RegisterButton(growthDungeonBtn, OnGrowthDungeonClicked);
            RegisterButton(worldBossBtn, OnWorldBossClicked);
            RegisterButton(townBtn, OnTownClicked);
            RegisterButton(noticeBtn, OnNoticeClicked);
            RegisterButton(gameSettingBtn, OnGameSettingClicked);

            Debug.Log("[HamburgerMenuPopup] 모든 버튼 이벤트 등록 완료");
        }

        /// <summary>
        /// 모든 버튼의 onClick 이벤트 해제
        /// </summary>
        private void UnregisterButtonEvents()
        {
            UnregisterButton(missionBtn, OnMissionClicked);
            UnregisterButton(passBtn, OnPassClicked);
            UnregisterButton(mailboxBtn, OnMailboxClicked);
            UnregisterButton(costumeBtn, OnCostumeClicked);
            UnregisterButton(heroPowerBtn, OnHeroPowerClicked);
            UnregisterButton(equipSlotEnhanceBtn, OnEquipSlotEnhanceClicked);
            UnregisterButton(relicBtn, OnRelicClicked);
            UnregisterButton(friendBtn, OnFriendClicked);
            UnregisterButton(rankingBtn, OnRankingClicked);
            UnregisterButton(guildBtn, OnGuildClicked);
            UnregisterButton(growthDungeonBtn, OnGrowthDungeonClicked);
            UnregisterButton(worldBossBtn, OnWorldBossClicked);
            UnregisterButton(townBtn, OnTownClicked);
            UnregisterButton(noticeBtn, OnNoticeClicked);
            UnregisterButton(gameSettingBtn, OnGameSettingClicked);
        }

        /// <summary>
        /// 버튼 이벤트 등록 헬퍼 메서드
        /// </summary>
        private void RegisterButton(Button button, UnityEngine.Events.UnityAction callback)
        {
            if (button != null)
            {
                button.onClick.AddListener(callback);
            }
            else
            {
                Debug.LogWarning($"[HamburgerMenuPopup] 버튼이 할당되지 않았습니다: {callback.Method.Name}");
            }
        }

        /// <summary>
        /// 버튼 이벤트 해제 헬퍼 메서드
        /// </summary>
        private void UnregisterButton(Button button, UnityEngine.Events.UnityAction callback)
        {
            if (button != null)
            {
                button.onClick.RemoveListener(callback);
            }
        }

        #endregion

        #region 버튼 핸들러

        /// <summary>
        /// 미션 버튼 클릭 핸들러
        /// </summary>
        public void OnMissionClicked()
        {
            Debug.Log("[HamburgerMenu] 미션 버튼 클릭");
            // TODO: 미션 화면 표시
        }

        /// <summary>
        /// 패스 버튼 클릭 핸들러
        /// </summary>
        public void OnPassClicked()
        {
            Debug.Log("[HamburgerMenu] 패스 버튼 클릭");
            // TODO: 패스 화면 표시
        }

        /// <summary>
        /// 우편함 버튼 클릭 핸들러
        /// </summary>
        public void OnMailboxClicked()
        {
            Debug.Log("[HamburgerMenu] 우편함 버튼 클릭");
            // TODO: 우편함 화면 표시
        }

        /// <summary>
        /// 코스튬 버튼 클릭 핸들러
        /// </summary>
        public void OnCostumeClicked()
        {
            Debug.Log("[HamburgerMenu] 코스튬 버튼 클릭");
            // TODO: 코스튬 화면 표시
        }

        /// <summary>
        /// 용사의 힘 버튼 클릭 핸들러
        /// </summary>
        public void OnHeroPowerClicked()
        {
            Debug.Log("[HamburgerMenu] 용사의 힘 버튼 클릭");
            // TODO: 용사의 힘 화면 표시
        }

        /// <summary>
        /// 장비 슬롯 강화 버튼 클릭 핸들러
        /// </summary>
        public void OnEquipSlotEnhanceClicked()
        {
            Debug.Log("[HamburgerMenu] 장비 슬롯 강화 버튼 클릭");
            // TODO: 장비 슬롯 강화 화면 표시
        }

        /// <summary>
        /// 유물 버튼 클릭 핸들러
        /// </summary>
        public void OnRelicClicked()
        {
            Debug.Log("[HamburgerMenu] 유물 버튼 클릭");
            // TODO: 유물 화면 표시
        }

        /// <summary>
        /// 친구 버튼 클릭 핸들러
        /// </summary>
        public void OnFriendClicked()
        {
            Debug.Log("[HamburgerMenu] 친구 버튼 클릭");
            // TODO: 친구 화면 표시
        }

        /// <summary>
        /// 랭킹 버튼 클릭 핸들러
        /// </summary>
        public void OnRankingClicked()
        {
            Debug.Log("[HamburgerMenu] 랭킹 버튼 클릭");
            // TODO: 랭킹 화면 표시
        }

        /// <summary>
        /// 길드 버튼 클릭 핸들러
        /// </summary>
        public void OnGuildClicked()
        {
            Debug.Log("[HamburgerMenu] 길드 버튼 클릭");
            // TODO: 길드 화면 표시
        }

        /// <summary>
        /// 성장 던전 버튼 클릭 핸들러
        /// </summary>
        public void OnGrowthDungeonClicked()
        {
            Debug.Log("[HamburgerMenu] 성장 던전 버튼 클릭");
            // TODO: 성장 던전 화면 표시
        }

        /// <summary>
        /// 월드 보스 버튼 클릭 핸들러
        /// </summary>
        public void OnWorldBossClicked()
        {
            Debug.Log("[HamburgerMenu] 월드 보스 버튼 클릭");
            // TODO: 월드 보스 화면 표시
        }

        /// <summary>
        /// 마을 버튼 클릭 핸들러
        /// </summary>
        public void OnTownClicked()
        {
            Debug.Log("[HamburgerMenu] 마을 버튼 클릭");

            if (uiManager == null)
            {
                Debug.LogWarning("[HamburgerMenu] UIManager가 주입되지 않았습니다.");
                return;
            }

            // 이미 다른 팝업이 열려있으면 중복 열기 방지
            if (uiManager.GetActivePopupCount() > 1)
            {
                Debug.LogWarning("[HamburgerMenu] 이미 다른 팝업이 열려있습니다. 먼저 닫아주세요.");
                return;
            }

            uiManager.ShowPopup(PopupID.Town);
        }

        /// <summary>
        /// 공지사항 버튼 클릭 핸들러
        /// </summary>
        public void OnNoticeClicked()
        {
            Debug.Log("[HamburgerMenu] 공지사항 버튼 클릭");

            if (uiManager == null)
            {
                Debug.LogWarning("[HamburgerMenu] UIManager가 주입되지 않았습니다.");
                return;
            }

            // 이미 다른 팝업이 열려있으면 중복 열기 방지
            if (uiManager.GetActivePopupCount() > 1)
            {
                Debug.LogWarning("[HamburgerMenu] 이미 다른 팝업이 열려있습니다. 먼저 닫아주세요.");
                return;
            }

            uiManager.ShowPopup(PopupID.Notice);
        }

        /// <summary>
        /// 게임 설정 버튼 클릭 핸들러
        /// </summary>
        public void OnGameSettingClicked()
        {
            Debug.Log("[HamburgerMenu] 게임 설정 버튼 클릭");

            if (uiManager == null)
            {
                Debug.LogWarning("[HamburgerMenu] UIManager가 주입되지 않았습니다.");
                return;
            }

            // 이미 다른 팝업이 열려있으면 중복 열기 방지
            if (uiManager.GetActivePopupCount() > 1)
            {
                Debug.LogWarning("[HamburgerMenu] 이미 다른 팝업이 열려있습니다. 먼저 닫아주세요.");
                return;
            }

            uiManager.ShowPopup(PopupID.GameSetting);
        }

        #endregion

        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupID.HamburgerMenu}] 햄버거 메뉴 팝업이 열렸습니다.");
        }
    }
}
