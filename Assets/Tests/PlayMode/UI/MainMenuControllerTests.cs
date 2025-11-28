using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;
using MobileGame.UI;
using MobileGame.Managers;
using MobileGame.Interfaces;
using MobileGame.Tests.Mocks;
using MobileGame.Tests.Helpers;

namespace MobileGame.Tests.UI
{
    /// <summary>
    /// MainMenuController 기능 테스트 (DI 기반)
    /// - ButtonBinder를 통한 버튼 접근
    /// - Mock UIManager를 사용한 테스트 격리
    /// - Reflection 사용 제거
    /// </summary>
    public class MainMenuControllerTests
    {
        #region Fields

        private LifetimeScope testScope;
        private MainMenuController controller;
        private ButtonBinder buttonBinder;
        private MockUIManager mockUIManager;
        private GameObject controllerObject;

        #endregion

        #region Setup & Teardown

        /// <summary>
        /// 각 테스트 전 초기화
        /// - DI 컨테이너 생성
        /// - Mock 매니저 주입
        /// - MainMenuController 생성
        /// </summary>
        [UnitySetUp]
        public IEnumerator Setup()
        {
            // 테스트용 DI 컨테이너 생성
            testScope = TestContainerBuilder.CreateCustomScope(
                includeUI: true,
                includeGame: true,
                includeAudio: true
            );

            mockUIManager = TestContainerBuilder.GetMockUIManager(testScope.Container);
            Assert.IsNotNull(mockUIManager, "MockUIManager가 주입되어야 합니다");

            // MainMenuController GameObject 생성
            controllerObject = new GameObject("TestMainMenuController");
            controller = controllerObject.AddComponent<MainMenuController>();

            // ButtonBinder 생성 및 설정
            buttonBinder = controllerObject.AddComponent<ButtonBinder>();
            SetupButtonBinder();

            // ButtonBinder 참조 설정 (리플렉션)
            var binderField = typeof(MainMenuController).GetField("buttonBinder",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            binderField?.SetValue(controller, buttonBinder);

            // DI 주입
            testScope.Container.Inject(controller);

            yield return null; // Start() 실행 대기
            yield return null; // ButtonBinder 완전 초기화 대기
        }

        /// <summary>
        /// 각 테스트 후 정리
        /// </summary>
        [UnityTearDown]
        public IEnumerator Teardown()
        {
            mockUIManager?.Reset();

            if (controllerObject != null)
            {
                Object.Destroy(controllerObject);
            }

            if (testScope != null)
            {
                testScope.Dispose();
            }

            yield return null;
        }

        #endregion

        #region Setup Helpers

        /// <summary>
        /// ButtonBinder에 테스트용 버튼 등록
        /// </summary>
        private void SetupButtonBinder()
        {
            var entriesField = typeof(ButtonBinder).GetField("buttonEntries",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var entryList = new System.Collections.Generic.List<ButtonBinder.ButtonEntry>();

            // 메뉴 시스템
            entryList.Add(CreateButtonEntry(ButtonID.HamburgerMenu));
            entryList.Add(CreateButtonEntry(ButtonID.Setting));

            // 정보 시스템
            entryList.Add(CreateButtonEntry(ButtonID.UserInfo));
            entryList.Add(CreateButtonEntry(ButtonID.GuideQuest));

            // 상점/협력/이벤트
            entryList.Add(CreateButtonEntry(ButtonID.Shop));
            entryList.Add(CreateButtonEntry(ButtonID.Recruitment));
            entryList.Add(CreateButtonEntry(ButtonID.Event));

            // 전투 관련
            entryList.Add(CreateButtonEntry(ButtonID.Character));
            entryList.Add(CreateButtonEntry(ButtonID.SkillSetting));
            entryList.Add(CreateButtonEntry(ButtonID.Skill1));
            entryList.Add(CreateButtonEntry(ButtonID.Skill2));
            entryList.Add(CreateButtonEntry(ButtonID.Skill3));
            entryList.Add(CreateButtonEntry(ButtonID.Skill4));
            entryList.Add(CreateButtonEntry(ButtonID.Skill5));
            entryList.Add(CreateButtonEntry(ButtonID.Skill6));
            entryList.Add(CreateButtonEntry(ButtonID.Weapon));
            entryList.Add(CreateButtonEntry(ButtonID.Equip));
            entryList.Add(CreateButtonEntry(ButtonID.Coworker));
            entryList.Add(CreateButtonEntry(ButtonID.Jump));
            entryList.Add(CreateButtonEntry(ButtonID.CoworkerSpawn));

            // 아이템
            entryList.Add(CreateButtonEntry(ButtonID.HPPotion));
            entryList.Add(CreateButtonEntry(ButtonID.MPPotion));
            entryList.Add(CreateButtonEntry(ButtonID.PotionSetting));

            // 게임플레이 컨트롤
            entryList.Add(CreateButtonEntry(ButtonID.Control));
            entryList.Add(CreateButtonEntry(ButtonID.Chapter));
            entryList.Add(CreateButtonEntry(ButtonID.MonsterSpawn));
            entryList.Add(CreateButtonEntry(ButtonID.SpawnSetting));
            entryList.Add(CreateButtonEntry(ButtonID.ContinuousSpawn));

            // 추가 기능
            entryList.Add(CreateButtonEntry(ButtonID.QuickHunt));
            entryList.Add(CreateButtonEntry(ButtonID.AutoResult));
            entryList.Add(CreateButtonEntry(ButtonID.Booster));
            entryList.Add(CreateButtonEntry(ButtonID.GrowUpGuide));
            entryList.Add(CreateButtonEntry(ButtonID.Quest));
            entryList.Add(CreateButtonEntry(ButtonID.Chatting));

            entriesField?.SetValue(buttonBinder, entryList);

            Debug.Log($"[Test] ButtonBinder에 {entryList.Count}개 버튼 엔트리 설정 완료");

            // ButtonBinder 초기화 (private 메서드 호출)
            var initMethod = typeof(ButtonBinder).GetMethod("InitializeButtonMap",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (initMethod == null)
            {
                Debug.LogError("[Test] InitializeButtonMap 메서드를 찾을 수 없습니다!");
            }
            else
            {
                initMethod.Invoke(buttonBinder, null);
                Debug.Log("[Test] ButtonBinder 초기화 완료");

                // 초기화 확인
                var buttonCount = buttonBinder.ButtonCount;
                Debug.Log($"[Test] 등록된 버튼 수: {buttonCount}");
            }
        }

        /// <summary>
        /// 테스트용 버튼 엔트리 생성
        /// </summary>
        private ButtonBinder.ButtonEntry CreateButtonEntry(string buttonId)
        {
            var buttonObj = new GameObject($"Button_{buttonId}");
            buttonObj.transform.SetParent(controllerObject.transform);
            var button = buttonObj.AddComponent<Button>();

            var entry = new ButtonBinder.ButtonEntry
            {
                buttonID = buttonId,
                button = button
            };

            return entry;
        }

        #endregion

        #region Tests - Popup Opening

        /// <summary>
        /// 테스트: 햄버거 메뉴 버튼 클릭 시 ShowPopup 호출
        /// Given: MockUIManager 주입 완료
        /// When: 햄버거 메뉴 버튼 클릭
        /// Then: MockUIManager.ShowPopup(PopupID.HamburgerMenu) 호출됨
        /// </summary>
        [UnityTest]
        public IEnumerator WhenHamburgerMenuButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.HamburgerMenu, PopupID.HamburgerMenu, "햄버거 메뉴");
        }

        [UnityTest]
        public IEnumerator WhenUserInfoButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.UserInfo, PopupID.UserInfo, "유저 정보");
        }

        [UnityTest]
        public IEnumerator WhenShopButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.Shop, PopupID.Shop, "상점");
        }

        [UnityTest]
        public IEnumerator WhenRecruitmentButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.Recruitment, PopupID.Recruitment, "모집");
        }

        [UnityTest]
        public IEnumerator WhenEventButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.Event, PopupID.Event, "이벤트");
        }

        [UnityTest]
        public IEnumerator WhenCharacterButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.Character, PopupID.Character, "캐릭터");
        }

        [UnityTest]
        public IEnumerator WhenSkillSettingButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.SkillSetting, PopupID.SkillSetting, "스킬 설정");
        }

        [UnityTest]
        public IEnumerator WhenWeaponButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.Weapon, PopupID.Weapon, "무기");
        }

        [UnityTest]
        public IEnumerator WhenEquipButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.Equip, PopupID.Equipment, "장비");
        }

        [UnityTest]
        public IEnumerator WhenCoworkerButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.Coworker, PopupID.Coworker, "협력자");
        }

        [UnityTest]
        public IEnumerator WhenPotionSettingButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.PotionSetting, PopupID.PotionSetting, "포션 설정");
        }

        [UnityTest]
        public IEnumerator WhenChapterButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.Chapter, PopupID.Chapter, "챕터");
        }

        [UnityTest]
        public IEnumerator WhenSpawnSettingButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.SpawnSetting, PopupID.SpawnSetting, "스폰 설정");
        }

        [UnityTest]
        public IEnumerator WhenQuickHuntButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.QuickHunt, PopupID.QuickHunt, "퀵 헌트");
        }

        [UnityTest]
        public IEnumerator WhenAutoResultButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.AutoResult, PopupID.AutoResult, "자동 결과");
        }

        [UnityTest]
        public IEnumerator WhenBoosterButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.Booster, PopupID.Booster, "부스터");
        }

        [UnityTest]
        public IEnumerator WhenContinuousSpawnButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.ContinuousSpawn, PopupID.ContinuousSpawn, "지속 스폰");
        }

        [UnityTest]
        public IEnumerator WhenGrowUpGuideButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.GrowUpGuide, PopupID.GrowUpGuide, "성장 가이드");
        }

        [UnityTest]
        public IEnumerator WhenQuestButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.Quest, PopupID.Quest, "퀘스트");
        }

        [UnityTest]
        public IEnumerator WhenChattingButtonClicked_ThenShowPopupCalled()
        {
            yield return TestButtonOpensPopup(ButtonID.Chatting, PopupID.Chatting, "채팅");
        }

        #endregion

        #region Tests - Non-Popup Buttons

        /// <summary>
        /// 테스트: 가이드 퀘스트 버튼 클릭 시 핸들러 호출 (팝업 미열림)
        /// </summary>
        [UnityTest]
        public IEnumerator WhenGuideQuestButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickWithoutPopup(ButtonID.GuideQuest, "[MainMenu] 가이드 퀘스트 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenSkill1ButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickWithoutPopup(ButtonID.Skill1, "[MainMenu] 스킬 1 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenSkill2ButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickWithoutPopup(ButtonID.Skill2, "[MainMenu] 스킬 2 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenSkill3ButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickWithoutPopup(ButtonID.Skill3, "[MainMenu] 스킬 3 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenSkill4ButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickWithoutPopup(ButtonID.Skill4, "[MainMenu] 스킬 4 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenSkill5ButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickWithoutPopup(ButtonID.Skill5, "[MainMenu] 스킬 5 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenSkill6ButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickWithoutPopup(ButtonID.Skill6, "[MainMenu] 스킬 6 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenHPPotionButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickWithoutPopup(ButtonID.HPPotion, "[MainMenu] HP 포션 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenMPPotionButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickWithoutPopup(ButtonID.MPPotion, "[MainMenu] MP 포션 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenControllButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickWithoutPopup(ButtonID.Control, "[MainMenu] 컨트롤 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenMonsterSpawnButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickWithoutPopup(ButtonID.MonsterSpawn, "[MainMenu] 몬스터 스폰 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenJumpButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickWithoutPopup(ButtonID.Jump, "[MainMenu] 점프 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenCoworkerSpawnButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickWithoutPopup(ButtonID.CoworkerSpawn, "[MainMenu] 협력자 스폰 버튼 클릭");
        }

        #endregion

        #region Tests - Integration

        /// <summary>
        /// 테스트: 여러 팝업 연속 열기
        /// Given: MockUIManager 준비
        /// When: 3개 버튼 연속 클릭
        /// Then: 각 팝업 ShowPopup이 3번 호출됨
        /// </summary>
        [UnityTest]
        public IEnumerator WhenMultipleButtonsClicked_ThenMultipleShowPopupsCalled()
        {
            // Arrange
            Button hamburgerBtn = buttonBinder.GetButton(ButtonID.HamburgerMenu);
            Button shopBtn = buttonBinder.GetButton(ButtonID.Shop);
            Button characterBtn = buttonBinder.GetButton(ButtonID.Character);

            // Act
            hamburgerBtn.onClick.Invoke();
            yield return null;

            shopBtn.onClick.Invoke();
            yield return null;

            characterBtn.onClick.Invoke();
            yield return null;

            // Assert
            Assert.AreEqual(3, mockUIManager.ShownPopups.Count, "3개의 팝업이 열려야 합니다");
            Assert.AreEqual(PopupID.HamburgerMenu, mockUIManager.ShownPopups[0], "첫 번째는 햄버거 메뉴");
            Assert.AreEqual(PopupID.Shop, mockUIManager.ShownPopups[1], "두 번째는 상점");
            Assert.AreEqual(PopupID.Character, mockUIManager.ShownPopups[2], "세 번째는 캐릭터");
        }

        #endregion

        #region Helper Methods - Test Patterns

        /// <summary>
        /// 버튼 클릭 시 팝업 열기 테스트 패턴
        /// </summary>
        private IEnumerator TestButtonOpensPopup(string buttonId, string expectedPopupId, string buttonDisplayName)
        {
            // Arrange
            Debug.Log($"[Test] {buttonDisplayName} 버튼 테스트 시작 - ButtonID: {buttonId}");
            Debug.Log($"[Test] ButtonBinder 등록된 버튼 수: {buttonBinder.ButtonCount}");

            Button button = buttonBinder.GetButton(buttonId);

            if (button == null)
            {
                var allButtons = buttonBinder.GetAllButtonIDs();
                Debug.LogError($"[Test] {buttonDisplayName} 버튼을 찾을 수 없습니다! ButtonID: {buttonId}");
                Debug.LogError($"[Test] 등록된 버튼 ID 목록: {string.Join(", ", allButtons)}");
            }

            Assert.IsNotNull(button, $"{buttonDisplayName} 버튼이 존재해야 합니다");

            mockUIManager.Reset();

            // Act
            button.onClick.Invoke();
            yield return null;

            // Assert
            Assert.AreEqual(1, mockUIManager.ShownPopups.Count, $"{buttonDisplayName} 버튼 클릭 시 1개의 팝업이 열려야 합니다");
            Assert.AreEqual(expectedPopupId, mockUIManager.ShownPopups[0], $"열린 팝업은 {expectedPopupId}이어야 합니다");
        }

        /// <summary>
        /// 버튼 클릭 시 팝업을 열지 않는 테스트 패턴
        /// </summary>
        private IEnumerator TestButtonClickWithoutPopup(string buttonId, string expectedLog)
        {
            // Arrange
            Button button = buttonBinder.GetButton(buttonId);
            Assert.IsNotNull(button, $"{buttonId} 버튼이 존재해야 합니다");

            mockUIManager.Reset();

            // Act & Assert
            LogAssert.Expect(LogType.Log, expectedLog);
            button.onClick.Invoke();
            yield return null;

            // Assert - 팝업이 열리지 않았는지 확인
            Assert.AreEqual(0, mockUIManager.ShownPopups.Count, "팝업이 열리면 안 됩니다");
        }

        #endregion
    }
}
