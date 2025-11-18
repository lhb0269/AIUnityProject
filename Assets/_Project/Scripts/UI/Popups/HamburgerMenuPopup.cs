using UnityEngine;
using UnityEngine.UI;

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

        #region 버튼 참조 필드

        [Header("햄버거 메뉴 버튼")]
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

        #endregion

        /// <summary>
        /// 팝업을 표시합니다.
        /// </summary>
        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupName}] 햄버거 메뉴 팝업이 열렸습니다.");
        }
    }
}
