using UnityEngine;
using UnityEngine.UI;
using MobileGame.Managers;
using MobileGame.UI;

namespace MobileGame.UI
{
    /// <summary>
    /// 메인 메뉴의 모든 버튼 이벤트를 관리하는 핸들러
    /// SampleScene의 17개 UI 버튼에 대한 통합 이벤트 관리
    /// </summary>
    public class MainMenuButtonHandler : MonoBehaviour
    {
        #region 버튼 참조 필드

        [Header("메뉴 시스템")]
        [SerializeField] private Button hamburgerMenuBtn;    // 햄버거 메뉴 버튼

        [Header("정보 시스템")]
        [SerializeField] private Button userInfoBtn;         // 유저 정보 버튼
        [SerializeField] private Button guideQuestBtn;       // 가이드 퀘스트 버튼

        [Header("상점/협력/이벤트")]
        [SerializeField] private Button shopBtn;             // 상점 버튼
        [SerializeField] private Button recruitmentBtn;      // 모집 버튼
        [SerializeField] private Button eventBtn;            // 이벤트 버튼

        [Header("전투 관련")]
        [SerializeField] private Button characterButton;     // 캐릭터 버튼
        [SerializeField] private Button SkillSettingBtn;      // 스킬 설정 버튼
        [SerializeField] private Button skill1Btn;           // 스킬 1 버튼
        [SerializeField] private Button skill2Btn;           // 스킬 2 버튼
        [SerializeField] private Button skill3Btn;           // 스킬 3 버튼
        [SerializeField] private Button skill4Btn;           // 스킬 4 버튼
        [SerializeField] private Button skill5Btn;           // 스킬 5 버튼
        [SerializeField] private Button skill6Btn;           // 스킬 6 버튼
        [SerializeField] private Button weaponButton;        // 무기 버튼
        [SerializeField] private Button equipButton;         // 장비 버튼
        [SerializeField] private Button coworkerButton;      // 협력자 버튼

        [Header("아이템")]
        [SerializeField] private Button hpPotionBtn;         // HP 포션 버튼
        [SerializeField] private Button mpPotionBtn;         // MP 포션 버튼
        [SerializeField] private Button potionSettingBtn;    // 포션 설정 버튼 (PotionGroup)

        [Header("게임플레이 컨트롤")]
        [SerializeField] private Button controllBtn;         // 컨트롤 버튼
        [SerializeField] private Button chapterBtn;          // 챕터 버튼 (ChapterGroup 내)
        [SerializeField] private Button monsterSpawnBtn;     // 몬스터 스폰 버튼 (MonsterSpawnGroup 관련)
        [SerializeField] private Button spawnSettingBtn;     // 스폰 설정 버튼 (SpawnGroup)

        [Header("추가 기능")]
        [SerializeField] private Button quickHuntBtn;        // 퀵 헌트 버튼
        [SerializeField] private Button autoResultBtn;       // 자동 결과 버튼
        [SerializeField] private Button boosterBtn;          // 부스터 버튼
        [SerializeField] private Button continuousSpawnBtn;  // 지속 스폰 버튼
        [SerializeField] private Button growUpGuideBtn;      // 성장 가이드 버튼
        [SerializeField] private Button questBtn;            // 퀘스트 버튼

        #endregion

        #region Unity 생명주기

        /// <summary>
        /// 시작 시 초기화 및 버튼 이벤트 연결
        /// </summary>
        private void Start()
        {
            // UIManager 인스턴스 존재 확인
            if (UIManager.Instance == null)
            {
                Debug.LogWarning("[MainMenuButtonHandler] UIManager 인스턴스를 찾을 수 없습니다!");
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
            RegisterButton(hamburgerMenuBtn, OnHamburgerMenuClicked);

            // 정보 시스템
            RegisterButton(userInfoBtn, OnUserInfoClicked);
            RegisterButton(guideQuestBtn, OnGuideQuestClicked);

            // 상점/협력/이벤트
            RegisterButton(shopBtn, OnShopClicked);
            RegisterButton(recruitmentBtn, OnRecruitmentClicked);
            RegisterButton(eventBtn, OnEventClicked);

            // 전투 관련
            RegisterButton(characterButton, OnCharacterClicked);
            RegisterButton(SkillSettingBtn, OnSkillSettingClicked);
            RegisterButton(skill1Btn, OnSkill1Clicked);
            RegisterButton(skill2Btn, OnSkill2Clicked);
            RegisterButton(skill3Btn, OnSkill3Clicked);
            RegisterButton(skill4Btn, OnSkill4Clicked);
            RegisterButton(skill5Btn, OnSkill5Clicked);
            RegisterButton(skill6Btn, OnSkill6Clicked);
            RegisterButton(weaponButton, OnWeaponClicked);
            RegisterButton(equipButton, OnEquipClicked);
            RegisterButton(coworkerButton, OnCoworkerClicked);

            // 아이템
            RegisterButton(hpPotionBtn, OnHPPotionClicked);
            RegisterButton(mpPotionBtn, OnMPPotionClicked);
            RegisterButton(potionSettingBtn, OnPotionSettingClicked);

            // 게임플레이 컨트롤
            RegisterButton(controllBtn, OnControllClicked);
            RegisterButton(chapterBtn, OnChapterClicked);
            RegisterButton(monsterSpawnBtn, OnMonsterSpawnClicked);
            RegisterButton(spawnSettingBtn, OnSpawnSettingClicked);

            // 추가 기능
            RegisterButton(quickHuntBtn, OnQuickHuntClicked);
            RegisterButton(autoResultBtn, OnAutoResultClicked);
            RegisterButton(boosterBtn, OnBoosterClicked);
            RegisterButton(continuousSpawnBtn, OnContinuousSpawnClicked);
            RegisterButton(growUpGuideBtn, OnGrowUpGuideClicked);
            RegisterButton(questBtn, OnQuestClicked);

            Debug.Log("[MainMenuButtonHandler] 모든 버튼 이벤트 등록 완료");
        }

        /// <summary>
        /// 모든 버튼의 onClick 이벤트 해제
        /// </summary>
        private void UnregisterButtonEvents()
        {
            // 메뉴 시스템
            UnregisterButton(hamburgerMenuBtn, OnHamburgerMenuClicked);

            // 정보 시스템
            UnregisterButton(userInfoBtn, OnUserInfoClicked);
            UnregisterButton(guideQuestBtn, OnGuideQuestClicked);

            // 상점/협력/이벤트
            UnregisterButton(shopBtn, OnShopClicked);
            UnregisterButton(recruitmentBtn, OnRecruitmentClicked);
            UnregisterButton(eventBtn, OnEventClicked);

            // 전투 관련
            UnregisterButton(characterButton, OnCharacterClicked);
            UnregisterButton(SkillSettingBtn, OnSkillSettingClicked);
            UnregisterButton(skill1Btn, OnSkill1Clicked);
            UnregisterButton(skill2Btn, OnSkill2Clicked);
            UnregisterButton(skill3Btn, OnSkill3Clicked);
            UnregisterButton(skill4Btn, OnSkill4Clicked);
            UnregisterButton(skill5Btn, OnSkill5Clicked);
            UnregisterButton(skill6Btn, OnSkill6Clicked);
            UnregisterButton(weaponButton, OnWeaponClicked);
            UnregisterButton(equipButton, OnEquipClicked);
            UnregisterButton(coworkerButton, OnCoworkerClicked);

            // 아이템
            UnregisterButton(hpPotionBtn, OnHPPotionClicked);
            UnregisterButton(mpPotionBtn, OnMPPotionClicked);
            UnregisterButton(potionSettingBtn, OnPotionSettingClicked);

            // 게임플레이 컨트롤
            UnregisterButton(controllBtn, OnControllClicked);
            UnregisterButton(chapterBtn, OnChapterClicked);
            UnregisterButton(monsterSpawnBtn, OnMonsterSpawnClicked);
            UnregisterButton(spawnSettingBtn, OnSpawnSettingClicked);

            // 추가 기능
            UnregisterButton(quickHuntBtn, OnQuickHuntClicked);
            UnregisterButton(autoResultBtn, OnAutoResultClicked);
            UnregisterButton(boosterBtn, OnBoosterClicked);
            UnregisterButton(continuousSpawnBtn, OnContinuousSpawnClicked);
            UnregisterButton(growUpGuideBtn, OnGrowUpGuideClicked);
            UnregisterButton(questBtn, OnQuestClicked);
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
                Debug.LogWarning($"[MainMenuButtonHandler] 버튼이 할당되지 않았습니다: {callback.Method.Name}");
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

        #region 메뉴 시스템 버튼 핸들러

        /// <summary>
        /// 햄버거 메뉴 버튼 클릭 핸들러
        /// </summary>
        public void OnHamburgerMenuClicked()
        {
            Debug.Log("[MainMenu] 햄버거 메뉴 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(HamburgerMenuPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 설정 버튼 클릭 핸들러
        /// </summary>
        public void OnSettingClicked()
        {
            Debug.Log("[MainMenu] 설정 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(SettingsPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        #endregion

        #region 정보 시스템 버튼 핸들러

        /// <summary>
        /// 유저 정보 버튼 클릭 핸들러
        /// </summary>
        public void OnUserInfoClicked()
        {
            Debug.Log("[MainMenu] 유저 정보 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(UserInfoPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 가이드 퀘스트 버튼 클릭 핸들러
        /// TODO: 가이드 퀘스트 패널 구현
        /// </summary>
        public void OnGuideQuestClicked()
        {
            Debug.Log("[MainMenu] 가이드 퀘스트 버튼 클릭");
            // TODO: 나중에 실제 가이드 퀘스트 패널 구현 시
            // UIManager.Instance.ShowPanel("GuideQuestPanel");
        }

        #endregion

        #region 상점/협력/이벤트 버튼 핸들러

        /// <summary>
        /// 상점 버튼 클릭 핸들러
        /// </summary>
        public void OnShopClicked()
        {
            Debug.Log("[MainMenu] 상점 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(ShopPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 모집 버튼 클릭 핸들러
        /// </summary>
        public void OnRecruitmentClicked()
        {
            Debug.Log("[MainMenu] 모집 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(RecruitmentPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 이벤트 버튼 클릭 핸들러
        /// </summary>
        public void OnEventClicked()
        {
            Debug.Log("[MainMenu] 이벤트 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(EventPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        #endregion

        #region 전투 관련 버튼 핸들러

        /// <summary>
        /// 캐릭터 버튼 클릭 핸들러
        /// </summary>
        public void OnCharacterClicked()
        {
            Debug.Log("[MainMenu] 캐릭터 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(CharacterPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 스킬 설정 버튼 클릭 핸들러
        /// </summary>
        public void OnSkillSettingClicked()
        {
            Debug.Log("[MainMenu] 스킬 설정 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(SkillSettingPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
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

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(WeaponPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 장비 버튼 클릭 핸들러
        /// </summary>
        public void OnEquipClicked()
        {
            Debug.Log("[MainMenu] 장비 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(EquipmentPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 협력자 버튼 클릭 핸들러
        /// </summary>
        public void OnCoworkerClicked()
        {
            Debug.Log("[MainMenu] 협력자 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(CoworkerPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
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

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(PotionSettingPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
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
            // UIManager.Instance.ShowPanel("ControlSettingsPanel");
        }

        /// <summary>
        /// 챕터 버튼 클릭 핸들러
        /// </summary>
        public void OnChapterClicked()
        {
            Debug.Log("[MainMenu] 챕터 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(ChapterPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 몬스터 스폰 버튼 클릭 핸들러
        /// TODO: 몬스터 스폰 기능 구현
        /// </summary>
        public void OnMonsterSpawnClicked()
        {
            Debug.Log("[MainMenu] 몬스터 스폰 버튼 클릭");
            // TODO: 나중에 실제 몬스터 스폰 기능 구현 시
            // GameManager.Instance.SpawnMonsters();
        }

        /// <summary>
        /// 스폰 설정 버튼 클릭 핸들러 (SpawnGroup)
        /// </summary>
        public void OnSpawnSettingClicked()
        {
            Debug.Log("[MainMenu] 스폰 설정 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(SpawnSettingPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        #endregion

        #region 추가 기능 버튼 핸들러

        /// <summary>
        /// 퀵 헌트 버튼 클릭 핸들러
        /// </summary>
        public void OnQuickHuntClicked()
        {
            Debug.Log("[MainMenu] 퀵 헌트 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(QuickHuntPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 자동 결과 버튼 클릭 핸들러
        /// </summary>
        public void OnAutoResultClicked()
        {
            Debug.Log("[MainMenu] 자동 결과 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(AutoResultPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 부스터 버튼 클릭 핸들러
        /// </summary>
        public void OnBoosterClicked()
        {
            Debug.Log("[MainMenu] 부스터 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(BoosterPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 지속 스폰 버튼 클릭 핸들러
        /// </summary>
        public void OnContinuousSpawnClicked()
        {
            Debug.Log("[MainMenu] 지속 스폰 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(ContinuousSpawnPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 성장 가이드 버튼 클릭 핸들러
        /// </summary>
        public void OnGrowUpGuideClicked()
        {
            Debug.Log("[MainMenu] 성장 가이드 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(GrowUpGuidePopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 퀘스트 버튼 클릭 핸들러
        /// </summary>
        public void OnQuestClicked()
        {
            Debug.Log("[MainMenu] 퀘스트 버튼 클릭");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup(QuestPopup.PopupName);
            }
            else
            {
                Debug.LogWarning("[MainMenu] UIManager 인스턴스를 찾을 수 없습니다.");
            }
        }

        #endregion
    }
}
