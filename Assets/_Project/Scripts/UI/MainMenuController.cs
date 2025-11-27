using UnityEngine;
using UnityEngine.UI;
using VContainer;
using MobileGame.Managers;
using MobileGame.Interfaces;
using MobileGame.UI;

namespace MobileGame.UI
{
    /// <summary>
    /// 메인 메뉴의 모든 버튼 이벤트를 관리하는 컨트롤러 (DI 기반)
    /// SampleScene의 모든 UI 버튼에 대한 통합 이벤트 관리
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        #region 의존성 주입

        [Inject] private IUIManager uiManager;
        [Inject] private IGameManager gameManager;
        [Inject] private IAudioManager audioManager;

        #endregion

        #region ButtonBinder 참조

        [Header("버튼 바인더")]
        [SerializeField] private ButtonBinder buttonBinder;

        #endregion

        #region Unity 생명주기

        /// <summary>
        /// 시작 시 초기화 및 버튼 이벤트 연결
        /// </summary>
        private void Start()
        {
            // 의존성 주입 확인
            if (uiManager == null)
            {
                Debug.LogError("[MainMenuController] IUIManager가 주입되지 않았습니다!");
                return;
            }

            if (buttonBinder == null)
            {
                Debug.LogError("[MainMenuController] ButtonBinder가 할당되지 않았습니다!");
                return;
            }

            // 모든 버튼 이벤트 연결
            RegisterButtonEvents();
        }

        /// <summary>
        /// 컴포넌트 비활성화 시 버튼 이벤트 해제
        /// </summary>
        private void OnDestroy()
        {
            UnregisterButtonEvents();
        }

        #endregion

        #region 버튼 이벤트 등록/해제

        /// <summary>
        /// 모든 버튼의 onClick 이벤트 등록
        /// </summary>
        private void RegisterButtonEvents()
        {
            // 메뉴 시스템
            RegisterButton(ButtonID.HamburgerMenu, OnHamburgerMenuClicked);
            RegisterButton(ButtonID.Setting, OnSettingClicked);

            // 정보 시스템
            RegisterButton(ButtonID.UserInfo, OnUserInfoClicked);
            RegisterButton(ButtonID.GuideQuest, OnGuideQuestClicked);

            // 상점/협력/이벤트
            RegisterButton(ButtonID.Shop, OnShopClicked);
            RegisterButton(ButtonID.Recruitment, OnRecruitmentClicked);
            RegisterButton(ButtonID.Event, OnEventClicked);

            // 전투 관련
            RegisterButton(ButtonID.Character, OnCharacterClicked);
            RegisterButton(ButtonID.SkillSetting, OnSkillSettingClicked);
            RegisterButton(ButtonID.Skill1, OnSkill1Clicked);
            RegisterButton(ButtonID.Skill2, OnSkill2Clicked);
            RegisterButton(ButtonID.Skill3, OnSkill3Clicked);
            RegisterButton(ButtonID.Skill4, OnSkill4Clicked);
            RegisterButton(ButtonID.Skill5, OnSkill5Clicked);
            RegisterButton(ButtonID.Skill6, OnSkill6Clicked);
            RegisterButton(ButtonID.Weapon, OnWeaponClicked);
            RegisterButton(ButtonID.Equip, OnEquipClicked);
            RegisterButton(ButtonID.Coworker, OnCoworkerClicked);
            RegisterButton(ButtonID.Jump, OnJumpClicked);
            RegisterButton(ButtonID.CoworkerSpawn, OnCoworkerSpawnClicked);

            // 아이템
            RegisterButton(ButtonID.HPPotion, OnHPPotionClicked);
            RegisterButton(ButtonID.MPPotion, OnMPPotionClicked);
            RegisterButton(ButtonID.PotionSetting, OnPotionSettingClicked);

            // 게임플레이 컨트롤
            RegisterButton(ButtonID.Control, OnControllClicked);
            RegisterButton(ButtonID.Chapter, OnChapterClicked);
            RegisterButton(ButtonID.MonsterSpawn, OnMonsterSpawnClicked);
            RegisterButton(ButtonID.SpawnSetting, OnSpawnSettingClicked);
            RegisterButton(ButtonID.ContinuousSpawn, OnContinuousSpawnClicked);

            // 추가 기능
            RegisterButton(ButtonID.QuickHunt, OnQuickHuntClicked);
            RegisterButton(ButtonID.AutoResult, OnAutoResultClicked);
            RegisterButton(ButtonID.Booster, OnBoosterClicked);
            RegisterButton(ButtonID.GrowUpGuide, OnGrowUpGuideClicked);
            RegisterButton(ButtonID.Quest, OnQuestClicked);
            RegisterButton(ButtonID.Chatting, OnChattingClicked);

            Debug.Log("[MainMenuController] 모든 버튼 이벤트 등록 완료");
        }

        /// <summary>
        /// 모든 버튼의 onClick 이벤트 해제
        /// </summary>
        private void UnregisterButtonEvents()
        {
            if (buttonBinder == null) return;

            // 메뉴 시스템
            UnregisterButton(ButtonID.HamburgerMenu, OnHamburgerMenuClicked);
            UnregisterButton(ButtonID.Setting, OnSettingClicked);

            // 정보 시스템
            UnregisterButton(ButtonID.UserInfo, OnUserInfoClicked);
            UnregisterButton(ButtonID.GuideQuest, OnGuideQuestClicked);

            // 상점/협력/이벤트
            UnregisterButton(ButtonID.Shop, OnShopClicked);
            UnregisterButton(ButtonID.Recruitment, OnRecruitmentClicked);
            UnregisterButton(ButtonID.Event, OnEventClicked);

            // 전투 관련
            UnregisterButton(ButtonID.Character, OnCharacterClicked);
            UnregisterButton(ButtonID.SkillSetting, OnSkillSettingClicked);
            UnregisterButton(ButtonID.Skill1, OnSkill1Clicked);
            UnregisterButton(ButtonID.Skill2, OnSkill2Clicked);
            UnregisterButton(ButtonID.Skill3, OnSkill3Clicked);
            UnregisterButton(ButtonID.Skill4, OnSkill4Clicked);
            UnregisterButton(ButtonID.Skill5, OnSkill5Clicked);
            UnregisterButton(ButtonID.Skill6, OnSkill6Clicked);
            UnregisterButton(ButtonID.Weapon, OnWeaponClicked);
            UnregisterButton(ButtonID.Equip, OnEquipClicked);
            UnregisterButton(ButtonID.Coworker, OnCoworkerClicked);
            UnregisterButton(ButtonID.Jump, OnJumpClicked);
            UnregisterButton(ButtonID.CoworkerSpawn, OnCoworkerSpawnClicked);

            // 아이템
            UnregisterButton(ButtonID.HPPotion, OnHPPotionClicked);
            UnregisterButton(ButtonID.MPPotion, OnMPPotionClicked);
            UnregisterButton(ButtonID.PotionSetting, OnPotionSettingClicked);

            // 게임플레이 컨트롤
            UnregisterButton(ButtonID.Control, OnControllClicked);
            UnregisterButton(ButtonID.Chapter, OnChapterClicked);
            UnregisterButton(ButtonID.MonsterSpawn, OnMonsterSpawnClicked);
            UnregisterButton(ButtonID.SpawnSetting, OnSpawnSettingClicked);
            UnregisterButton(ButtonID.ContinuousSpawn, OnContinuousSpawnClicked);

            // 추가 기능
            UnregisterButton(ButtonID.QuickHunt, OnQuickHuntClicked);
            UnregisterButton(ButtonID.AutoResult, OnAutoResultClicked);
            UnregisterButton(ButtonID.Booster, OnBoosterClicked);
            UnregisterButton(ButtonID.GrowUpGuide, OnGrowUpGuideClicked);
            UnregisterButton(ButtonID.Quest, OnQuestClicked);
            UnregisterButton(ButtonID.Chatting, OnChattingClicked);
        }

        /// <summary>
        /// 버튼 이벤트 등록 헬퍼 메서드
        /// ButtonBinder를 통해 버튼 ID로 접근
        /// </summary>
        private void RegisterButton(string buttonId, UnityEngine.Events.UnityAction callback)
        {
            if (buttonBinder.TryGetButton(buttonId, out Button button))
            {
                button.onClick.AddListener(callback);
            }
            else
            {
                Debug.LogWarning($"[MainMenuController] 버튼을 찾을 수 없음: {buttonId}");
            }
        }

        /// <summary>
        /// 버튼 이벤트 해제 헬퍼 메서드
        /// </summary>
        private void UnregisterButton(string buttonId, UnityEngine.Events.UnityAction callback)
        {
            if (buttonBinder.TryGetButton(buttonId, out Button button))
            {
                button.onClick.RemoveListener(callback);
            }
        }

        #endregion

        #region 메뉴 시스템 버튼 핸들러

        /// <summary>
        /// 햄버거 메뉴 버튼 클릭 핸들러
        /// </summary>
        public void OnHamburgerMenuClicked()
        {
            Debug.Log("[MainMenu] 햄버거 메뉴 버튼 클릭");
            uiManager?.ShowPopup(PopupID.HamburgerMenu);
        }

        /// <summary>
        /// 설정 버튼 클릭 핸들러
        /// </summary>
        public void OnSettingClicked()
        {
            Debug.Log("[MainMenu] 설정 버튼 클릭");
            uiManager?.ShowPopup(PopupID.Settings);
        }

        #endregion

        #region 정보 시스템 버튼 핸들러

        /// <summary>
        /// 유저 정보 버튼 클릭 핸들러
        /// </summary>
        public void OnUserInfoClicked()
        {
            Debug.Log("[MainMenu] 유저 정보 버튼 클릭");
            uiManager?.ShowPopup(PopupID.UserInfo);
        }

        /// <summary>
        /// 가이드 퀘스트 버튼 클릭 핸들러
        /// TODO: 가이드 퀘스트 패널 구현
        /// </summary>
        public void OnGuideQuestClicked()
        {
            Debug.Log("[MainMenu] 가이드 퀘스트 버튼 클릭");
            // TODO: 나중에 실제 가이드 퀘스트 패널 구현 시
            // uiManager?.ShowPanel("GuideQuestPanel");
        }

        #endregion

        #region 상점/협력/이벤트 버튼 핸들러

        /// <summary>
        /// 상점 버튼 클릭 핸들러
        /// </summary>
        public void OnShopClicked()
        {
            Debug.Log("[MainMenu] 상점 버튼 클릭");
            uiManager?.ShowPopup(PopupID.Shop);
        }

        /// <summary>
        /// 모집 버튼 클릭 핸들러
        /// </summary>
        public void OnRecruitmentClicked()
        {
            Debug.Log("[MainMenu] 모집 버튼 클릭");
            uiManager?.ShowPopup(PopupID.Recruitment);
        }

        /// <summary>
        /// 이벤트 버튼 클릭 핸들러
        /// </summary>
        public void OnEventClicked()
        {
            Debug.Log("[MainMenu] 이벤트 버튼 클릭");
            uiManager?.ShowPopup(PopupID.Event);
        }

        #endregion

        #region 전투 관련 버튼 핸들러

        /// <summary>
        /// 캐릭터 버튼 클릭 핸들러
        /// </summary>
        public void OnCharacterClicked()
        {
            Debug.Log("[MainMenu] 캐릭터 버튼 클릭");
            uiManager?.ShowPopup(PopupID.Character);
        }

        /// <summary>
        /// 스킬 설정 버튼 클릭 핸들러
        /// </summary>
        public void OnSkillSettingClicked()
        {
            Debug.Log("[MainMenu] 스킬 설정 버튼 클릭");
            uiManager?.ShowPopup(PopupID.SkillSetting);
        }

        /// <summary>
        /// 스킬 1 버튼 클릭 핸들러
        /// TODO: 스킬 1 사용 기능 구현
        /// </summary>
        public void OnSkill1Clicked()
        {
            Debug.Log("[MainMenu] 스킬 1 버튼 클릭");
            // TODO: 나중에 실제 스킬 1 사용 기능 구현 시
            // PlayerSkill.Instance.UseSkill(1);
        }

        /// <summary>
        /// 스킬 2 버튼 클릭 핸들러
        /// TODO: 스킬 2 사용 기능 구현
        /// </summary>
        public void OnSkill2Clicked()
        {
            Debug.Log("[MainMenu] 스킬 2 버튼 클릭");
            // TODO: 나중에 실제 스킬 2 사용 기능 구현 시
            // PlayerSkill.Instance.UseSkill(2);
        }

        /// <summary>
        /// 스킬 3 버튼 클릭 핸들러
        /// TODO: 스킬 3 사용 기능 구현
        /// </summary>
        public void OnSkill3Clicked()
        {
            Debug.Log("[MainMenu] 스킬 3 버튼 클릭");
            // TODO: 나중에 실제 스킬 3 사용 기능 구현 시
            // PlayerSkill.Instance.UseSkill(3);
        }

        /// <summary>
        /// 스킬 4 버튼 클릭 핸들러
        /// TODO: 스킬 4 사용 기능 구현
        /// </summary>
        public void OnSkill4Clicked()
        {
            Debug.Log("[MainMenu] 스킬 4 버튼 클릭");
            // TODO: 나중에 실제 스킬 4 사용 기능 구현 시
            // PlayerSkill.Instance.UseSkill(4);
        }

        /// <summary>
        /// 스킬 5 버튼 클릭 핸들러
        /// TODO: 스킬 5 사용 기능 구현
        /// </summary>
        public void OnSkill5Clicked()
        {
            Debug.Log("[MainMenu] 스킬 5 버튼 클릭");
            // TODO: 나중에 실제 스킬 5 사용 기능 구현 시
            // PlayerSkill.Instance.UseSkill(5);
        }

        /// <summary>
        /// 스킬 6 버튼 클릭 핸들러
        /// TODO: 스킬 6 사용 기능 구현
        /// </summary>
        public void OnSkill6Clicked()
        {
            Debug.Log("[MainMenu] 스킬 6 버튼 클릭");
            // TODO: 나중에 실제 스킬 6 사용 기능 구현 시
            // PlayerSkill.Instance.UseSkill(6);
        }

        /// <summary>
        /// 무기 버튼 클릭 핸들러
        /// </summary>
        public void OnWeaponClicked()
        {
            Debug.Log("[MainMenu] 무기 버튼 클릭");
            uiManager?.ShowPopup(PopupID.Weapon);
        }

        /// <summary>
        /// 장비 버튼 클릭 핸들러
        /// </summary>
        public void OnEquipClicked()
        {
            Debug.Log("[MainMenu] 장비 버튼 클릭");
            uiManager?.ShowPopup(PopupID.Equipment);
        }

        /// <summary>
        /// 협력자 버튼 클릭 핸들러
        /// </summary>
        public void OnCoworkerClicked()
        {
            Debug.Log("[MainMenu] 협력자 버튼 클릭");
            uiManager?.ShowPopup(PopupID.Coworker);
        }

        /// <summary>
        /// 점프 버튼 클릭 핸들러
        /// TODO: 점프 기능 구현
        /// </summary>
        public void OnJumpClicked()
        {
            Debug.Log("[MainMenu] 점프 버튼 클릭");
            // TODO: 나중에 실제 점프 기능 구현 시
            // PlayerController.Instance.Jump();
        }

        /// <summary>
        /// 협력자 스폰 버튼 클릭 핸들러
        /// TODO: 협력자 스폰 기능 구현
        /// </summary>
        public void OnCoworkerSpawnClicked()
        {
            Debug.Log("[MainMenu] 협력자 스폰 버튼 클릭");
            // TODO: 나중에 실제 협력자 스폰 기능 구현 시
            // CoworkerManager.Instance.SpawnCoworker();
        }

        #endregion

        #region 아이템 버튼 핸들러

        /// <summary>
        /// HP 포션 버튼 클릭 핸들러
        /// TODO: 포션 사용 기능 구현
        /// </summary>
        public void OnHPPotionClicked()
        {
            Debug.Log("[MainMenu] HP 포션 버튼 클릭");
            // TODO: 나중에 실제 HP 포션 사용 기능 구현 시
            // PlayerInventory.Instance.UseHPPotion();
        }

        /// <summary>
        /// MP 포션 버튼 클릭 핸들러
        /// TODO: 포션 사용 기능 구현
        /// </summary>
        public void OnMPPotionClicked()
        {
            Debug.Log("[MainMenu] MP 포션 버튼 클릭");
            // TODO: 나중에 실제 MP 포션 사용 기능 구현 시
            // PlayerInventory.Instance.UseMPPotion();
        }

        /// <summary>
        /// 포션 설정 버튼 클릭 핸들러 (PotionGroup)
        /// </summary>
        public void OnPotionSettingClicked()
        {
            Debug.Log("[MainMenu] 포션 설정 버튼 클릭");
            uiManager?.ShowPopup(PopupID.PotionSetting);
        }

        #endregion

        #region 게임플레이 컨트롤 버튼 핸들러

        /// <summary>
        /// 컨트롤 버튼 클릭 핸들러
        /// TODO: 컨트롤 설정 패널 구현
        /// </summary>
        public void OnControllClicked()
        {
            Debug.Log("[MainMenu] 컨트롤 버튼 클릭");
            // TODO: 나중에 실제 컨트롤 설정 패널 구현 시
            // uiManager?.ShowPanel("ControlSettingsPanel");
        }

        /// <summary>
        /// 챕터 버튼 클릭 핸들러
        /// </summary>
        public void OnChapterClicked()
        {
            Debug.Log("[MainMenu] 챕터 버튼 클릭");
            uiManager?.ShowPopup(PopupID.Chapter);
        }

        /// <summary>
        /// 몬스터 스폰 버튼 클릭 핸들러
        /// TODO: 몬스터 스폰 기능 구현
        /// </summary>
        public void OnMonsterSpawnClicked()
        {
            Debug.Log("[MainMenu] 몬스터 스폰 버튼 클릭");
            // TODO: 나중에 실제 몬스터 스폰 기능 구현 시
            // gameManager?.SpawnMonsters();
        }

        /// <summary>
        /// 스폰 설정 버튼 클릭 핸들러 (SpawnGroup)
        /// </summary>
        public void OnSpawnSettingClicked()
        {
            Debug.Log("[MainMenu] 스폰 설정 버튼 클릭");
            uiManager?.ShowPopup(PopupID.SpawnSetting);
        }

        #endregion

        #region 추가 기능 버튼 핸들러

        /// <summary>
        /// 퀵 헌트 버튼 클릭 핸들러
        /// </summary>
        public void OnQuickHuntClicked()
        {
            Debug.Log("[MainMenu] 퀵 헌트 버튼 클릭");
            uiManager?.ShowPopup(PopupID.QuickHunt);
        }

        /// <summary>
        /// 자동 결과 버튼 클릭 핸들러
        /// </summary>
        public void OnAutoResultClicked()
        {
            Debug.Log("[MainMenu] 자동 결과 버튼 클릭");
            uiManager?.ShowPopup(PopupID.AutoResult);
        }

        /// <summary>
        /// 부스터 버튼 클릭 핸들러
        /// </summary>
        public void OnBoosterClicked()
        {
            Debug.Log("[MainMenu] 부스터 버튼 클릭");
            uiManager?.ShowPopup(PopupID.Booster);
        }

        /// <summary>
        /// 지속 스폰 버튼 클릭 핸들러
        /// </summary>
        public void OnContinuousSpawnClicked()
        {
            Debug.Log("[MainMenu] 지속 스폰 버튼 클릭");
            uiManager?.ShowPopup(PopupID.ContinuousSpawn);
        }

        /// <summary>
        /// 성장 가이드 버튼 클릭 핸들러
        /// </summary>
        public void OnGrowUpGuideClicked()
        {
            Debug.Log("[MainMenu] 성장 가이드 버튼 클릭");
            uiManager?.ShowPopup(PopupID.GrowUpGuide);
        }

        /// <summary>
        /// 퀘스트 버튼 클릭 핸들러
        /// </summary>
        public void OnQuestClicked()
        {
            Debug.Log("[MainMenu] 퀘스트 버튼 클릭");
            uiManager?.ShowPopup(PopupID.Quest);
        }

        /// <summary>
        /// 채팅 버튼 클릭 핸들러
        /// </summary>
        public void OnChattingClicked()
        {
            Debug.Log("[MainMenu] 채팅 버튼 클릭");
            uiManager?.ShowPopup(PopupID.Chatting);
        }

        #endregion
    }
}
