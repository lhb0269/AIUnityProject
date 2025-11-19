using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using MobileGame.UI;
using MobileGame.Managers;

namespace MobileGame.Tests.UI
{
    /// <summary>
    /// MainMenuButtonHandler 기능 테스트
    /// - 메인 메뉴 버튼 팝업 열기 검증
    /// - 팝업 닫기 버튼 동작 검증
    /// - UI 싱글톤 격리 및 독립성 보장
    /// </summary>
    public class MainMenuButtonHandlerTests
    {
        #region Constants

        private const float POPUP_SPAWN_TIMEOUT = 2f;
        private const float POPUP_DESTROY_TIMEOUT = 2f;
        private const float BUTTON_INTERACTION_DELAY = 0.1f;
        private const string TEST_SCENE_NAME = "SampleScene";

        #endregion

        #region Fields

        private static bool sceneLoaded = false;
        private MainMenuButtonHandler handler;
        private EventSystem eventSystem;

        #endregion

        #region Setup & Teardown

        /// <summary>
        /// 각 테스트 전 초기화
        /// - 씬 로드 및 필수 컴포넌트 대기
        /// - 깨끗한 UI 상태로 시작
        /// </summary>
        [UnitySetUp]
        public IEnumerator Setup()
        {
            // 씬 로드
            if (!sceneLoaded || SceneManager.GetActiveScene().name != TEST_SCENE_NAME)
            {
                SceneManager.LoadScene(TEST_SCENE_NAME, LoadSceneMode.Single);
                yield return null;
                yield return null; // Awake, Start 실행 보장
                sceneLoaded = true;
            }

            // 필수 컴포넌트 대기
            yield return WaitForComponent<MainMenuButtonHandler>();
            handler = Object.FindFirstObjectByType<MainMenuButtonHandler>();
            Assert.IsNotNull(handler, "MainMenuButtonHandler가 씬에 존재해야 합니다");

            yield return WaitForComponent<EventSystem>();
            eventSystem = Object.FindFirstObjectByType<EventSystem>();
            Assert.IsNotNull(eventSystem, "EventSystem이 씬에 존재해야 합니다");

            yield return WaitForComponent<UIManager>();
            Assert.IsNotNull(UIManager.Instance, "UIManager 인스턴스가 존재해야 합니다");

            // 팝업 정리
            UIManager.Instance.CloseAllActivePopups();
            yield return WaitUntilNoActivePopups();
        }

        /// <summary>
        /// 각 테스트 후 정리
        /// - 열린 팝업 닫기
        /// - 상태 초기화
        /// </summary>
        [UnityTearDown]
        public IEnumerator Teardown()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.CloseAllActivePopups();
                yield return WaitUntilNoActivePopups();
            }

            handler = null;
            eventSystem = null;
        }

        /// <summary>
        /// 모든 테스트 종료 후 싱글톤 정리
        /// - DontDestroyOnLoad 객체 파괴
        /// - 다음 테스트 클래스에 영향 방지
        /// </summary>
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            UIManager.ResetForTesting();
            sceneLoaded = false;
        }

        #endregion

        #region Tests - Popup Opening

        /// <summary>
        /// 테스트: 햄버거 메뉴 버튼 클릭 시 HamburgerMenuPopup 열림
        /// Given: 깨끗한 UI 상태
        /// When: 햄버거 메뉴 버튼 클릭
        /// Then: HamburgerMenuPopup이 열림
        /// </summary>
        [UnityTest]
        public IEnumerator WhenHamburgerMenuButtonClicked_ThenHamburgerMenuPopupOpens()
        {
            yield return TestButtonOpensPopup<HamburgerMenuPopup>("hamburgerMenuBtn", "햄버거 메뉴");
        }

        /// <summary>
        /// 테스트: 유저 정보 버튼 클릭 시 UserInfoPopup 열림
        /// </summary>
        [UnityTest]
        public IEnumerator WhenUserInfoButtonClicked_ThenUserInfoPopupOpens()
        {
            yield return TestButtonOpensPopup<UserInfoPopup>("userInfoBtn", "유저 정보");
        }

        /// <summary>
        /// 테스트: 상점 버튼 클릭 시 ShopPopup 열림
        /// </summary>
        [UnityTest]
        public IEnumerator WhenShopButtonClicked_ThenShopPopupOpens()
        {
            yield return TestButtonOpensPopup<ShopPopup>("shopBtn", "상점");
        }

        /// <summary>
        /// 테스트: 모집 버튼 클릭 시 RecruitmentPopup 열림
        /// </summary>
        [UnityTest]
        public IEnumerator WhenRecruitmentButtonClicked_ThenRecruitmentPopupOpens()
        {
            yield return TestButtonOpensPopup<RecruitmentPopup>("recruitmentBtn", "모집");
        }

        /// <summary>
        /// 테스트: 이벤트 버튼 클릭 시 EventPopup 열림
        /// </summary>
        [UnityTest]
        public IEnumerator WhenEventButtonClicked_ThenEventPopupOpens()
        {
            yield return TestButtonOpensPopup<EventPopup>("eventBtn", "이벤트");
        }

        /// <summary>
        /// 테스트: 캐릭터 버튼 클릭 시 CharacterPopup 열림
        /// </summary>
        [UnityTest]
        public IEnumerator WhenCharacterButtonClicked_ThenCharacterPopupOpens()
        {
            yield return TestButtonOpensPopup<CharacterPopup>("characterButton", "캐릭터");
        }

        /// <summary>
        /// 테스트: 스킬 설정 버튼 클릭 시 SkillSettingPopup 열림
        /// </summary>
        [UnityTest]
        public IEnumerator WhenSkillSettingButtonClicked_ThenSkillSettingPopupOpens()
        {
            yield return TestButtonOpensPopup<SkillSettingPopup>("SkillSettingBtn", "스킬 설정");
        }

        /// <summary>
        /// 테스트: 무기 버튼 클릭 시 WeaponPopup 열림
        /// </summary>
        [UnityTest]
        public IEnumerator WhenWeaponButtonClicked_ThenWeaponPopupOpens()
        {
            yield return TestButtonOpensPopup<WeaponPopup>("weaponButton", "무기");
        }

        /// <summary>
        /// 테스트: 장비 버튼 클릭 시 EquipmentPopup 열림
        /// </summary>
        [UnityTest]
        public IEnumerator WhenEquipButtonClicked_ThenEquipmentPopupOpens()
        {
            yield return TestButtonOpensPopup<EquipmentPopup>("equipButton", "장비");
        }

        /// <summary>
        /// 테스트: 협력자 버튼 클릭 시 CoworkerPopup 열림
        /// </summary>
        [UnityTest]
        public IEnumerator WhenCoworkerButtonClicked_ThenCoworkerPopupOpens()
        {
            yield return TestButtonOpensPopup<CoworkerPopup>("coworkerButton", "협력자");
        }

        /// <summary>
        /// 테스트: 포션 설정 버튼 클릭 시 PotionSettingPopup 열림
        /// </summary>
        [UnityTest]
        public IEnumerator WhenPotionSettingButtonClicked_ThenPotionSettingPopupOpens()
        {
            yield return TestButtonOpensPopup<PotionSettingPopup>("potionSettingBtn", "포션 설정");
        }

        /// <summary>
        /// 테스트: 챕터 버튼 클릭 시 ChapterPopup 열림
        /// </summary>
        [UnityTest]
        public IEnumerator WhenChapterButtonClicked_ThenChapterPopupOpens()
        {
            yield return TestButtonOpensPopup<ChapterPopup>("chapterBtn", "챕터");
        }

        /// <summary>
        /// 테스트: 스폰 설정 버튼 클릭 시 SpawnSettingPopup 열림
        /// </summary>
        [UnityTest]
        public IEnumerator WhenSpawnSettingButtonClicked_ThenSpawnSettingPopupOpens()
        {
            yield return TestButtonOpensPopup<SpawnSettingPopup>("spawnSettingBtn", "스폰 설정");
        }

        #endregion

        #region Tests - Popup Closing

        /// <summary>
        /// 테스트: 햄버거 메뉴 팝업 닫기 버튼 클릭 시 팝업 닫힘
        /// Given: HamburgerMenuPopup이 열린 상태
        /// When: 닫기 버튼 클릭
        /// Then: 팝업이 닫힘
        /// </summary>
        [UnityTest]
        public IEnumerator WhenHamburgerMenuPopupCloseButtonClicked_ThenPopupCloses()
        {
            yield return TestPopupCloseButton<HamburgerMenuPopup>("hamburgerMenuBtn", "햄버거 메뉴");
        }

        [UnityTest]
        public IEnumerator WhenUserInfoPopupCloseButtonClicked_ThenPopupCloses()
        {
            yield return TestPopupCloseButton<UserInfoPopup>("userInfoBtn", "유저 정보");
        }

        [UnityTest]
        public IEnumerator WhenShopPopupCloseButtonClicked_ThenPopupCloses()
        {
            yield return TestPopupCloseButton<ShopPopup>("shopBtn", "상점");
        }

        [UnityTest]
        public IEnumerator WhenRecruitmentPopupCloseButtonClicked_ThenPopupCloses()
        {
            yield return TestPopupCloseButton<RecruitmentPopup>("recruitmentBtn", "모집");
        }

        [UnityTest]
        public IEnumerator WhenEventPopupCloseButtonClicked_ThenPopupCloses()
        {
            yield return TestPopupCloseButton<EventPopup>("eventBtn", "이벤트");
        }

        [UnityTest]
        public IEnumerator WhenCharacterPopupCloseButtonClicked_ThenPopupCloses()
        {
            yield return TestPopupCloseButton<CharacterPopup>("characterButton", "캐릭터");
        }

        [UnityTest]
        public IEnumerator WhenSkillSettingPopupCloseButtonClicked_ThenPopupCloses()
        {
            yield return TestPopupCloseButton<SkillSettingPopup>("SkillSettingBtn", "스킬 설정");
        }

        [UnityTest]
        public IEnumerator WhenWeaponPopupCloseButtonClicked_ThenPopupCloses()
        {
            yield return TestPopupCloseButton<WeaponPopup>("weaponButton", "무기");
        }

        [UnityTest]
        public IEnumerator WhenEquipmentPopupCloseButtonClicked_ThenPopupCloses()
        {
            yield return TestPopupCloseButton<EquipmentPopup>("equipButton", "장비");
        }

        [UnityTest]
        public IEnumerator WhenCoworkerPopupCloseButtonClicked_ThenPopupCloses()
        {
            yield return TestPopupCloseButton<CoworkerPopup>("coworkerButton", "협력자");
        }

        [UnityTest]
        public IEnumerator WhenPotionSettingPopupCloseButtonClicked_ThenPopupCloses()
        {
            yield return TestPopupCloseButton<PotionSettingPopup>("potionSettingBtn", "포션 설정");
        }

        [UnityTest]
        public IEnumerator WhenChapterPopupCloseButtonClicked_ThenPopupCloses()
        {
            yield return TestPopupCloseButton<ChapterPopup>("chapterBtn", "챕터");
        }

        [UnityTest]
        public IEnumerator WhenSpawnSettingPopupCloseButtonClicked_ThenPopupCloses()
        {
            yield return TestPopupCloseButton<SpawnSettingPopup>("spawnSettingBtn", "스폰 설정");
        }

        #endregion

        #region Tests - Non-Popup Buttons

        /// <summary>
        /// 테스트: 가이드 퀘스트 버튼 클릭 시 핸들러 호출
        /// Given: 깨끗한 UI 상태
        /// When: 가이드 퀘스트 버튼 클릭
        /// Then: 올바른 로그 메시지 출력
        /// </summary>
        [UnityTest]
        public IEnumerator WhenGuideQuestButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickLogsMessage("guideQuestBtn", "[MainMenu] 가이드 퀘스트 버튼 클릭", "가이드 퀘스트");
        }

        [UnityTest]
        public IEnumerator WhenSkill1ButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickLogsMessage("skill1Btn", "[MainMenu] 스킬 1 버튼 클릭", "스킬 1");
        }

        [UnityTest]
        public IEnumerator WhenSkill2ButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickLogsMessage("skill2Btn", "[MainMenu] 스킬 2 버튼 클릭", "스킬 2");
        }

        [UnityTest]
        public IEnumerator WhenSkill3ButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickLogsMessage("skill3Btn", "[MainMenu] 스킬 3 버튼 클릭", "스킬 3");
        }

        [UnityTest]
        public IEnumerator WhenSkill4ButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickLogsMessage("skill4Btn", "[MainMenu] 스킬 4 버튼 클릭", "스킬 4");
        }

        [UnityTest]
        public IEnumerator WhenSkill5ButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickLogsMessage("skill5Btn", "[MainMenu] 스킬 5 버튼 클릭", "스킬 5");
        }

        [UnityTest]
        public IEnumerator WhenSkill6ButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickLogsMessage("skill6Btn", "[MainMenu] 스킬 6 버튼 클릭", "스킬 6");
        }

        [UnityTest]
        public IEnumerator WhenHPPotionButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickLogsMessage("hpPotionBtn", "[MainMenu] HP 포션 버튼 클릭", "HP 포션");
        }

        [UnityTest]
        public IEnumerator WhenMPPotionButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickLogsMessage("mpPotionBtn", "[MainMenu] MP 포션 버튼 클릭", "MP 포션");
        }

        [UnityTest]
        public IEnumerator WhenControllButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickLogsMessage("controllBtn", "[MainMenu] 컨트롤 버튼 클릭", "컨트롤");
        }

        [UnityTest]
        public IEnumerator WhenMonsterSpawnButtonClicked_ThenHandlerCalled()
        {
            yield return TestButtonClickLogsMessage("monsterSpawnBtn", "[MainMenu] 몬스터 스폰 버튼 클릭", "몬스터 스폰");
        }

        #endregion

        #region Tests - Integration

        /// <summary>
        /// 테스트: 여러 팝업 연속 열기
        /// Given: 깨끗한 UI 상태
        /// When: 3개 팝업 버튼 연속 클릭
        /// Then: 각 팝업이 스택에 쌓임
        /// </summary>
        [UnityTest]
        public IEnumerator WhenMultiplePopupButtonsClicked_ThenPopupsStackCorrectly()
        {
            // Arrange
            Button hamburgerBtn = GetButtonField("hamburgerMenuBtn");
            Button shopBtn = GetButtonField("shopBtn");
            Button characterBtn = GetButtonField("characterButton");

            if (hamburgerBtn == null || shopBtn == null || characterBtn == null)
            {
                Assert.Inconclusive("테스트에 필요한 버튼이 모두 할당되지 않았습니다");
                yield break;
            }

            // Act & Assert - 첫 번째 팝업
            yield return ClickButton(hamburgerBtn, "햄버거 메뉴");
            yield return WaitUntilPopupAppears<HamburgerMenuPopup>();
            Assert.AreEqual(1, UIManager.Instance.GetActivePopupCount(), "첫 번째 팝업이 열려야 합니다");

            // Act & Assert - 두 번째 팝업
            yield return ClickButton(shopBtn, "상점");
            yield return WaitUntilPopupAppears<ShopPopup>();
            Assert.AreEqual(2, UIManager.Instance.GetActivePopupCount(), "두 번째 팝업이 스택에 추가되어야 합니다");

            // Act & Assert - 세 번째 팝업
            yield return ClickButton(characterBtn, "캐릭터");
            yield return WaitUntilPopupAppears<CharacterPopup>();
            Assert.AreEqual(3, UIManager.Instance.GetActivePopupCount(), "세 번째 팝업이 스택에 추가되어야 합니다");

            // Cleanup
            UIManager.Instance.CloseAllActivePopups();
            yield return WaitUntilNoActivePopups();
        }

        #endregion

        #region Helper Methods - Test Patterns

        /// <summary>
        /// 버튼 클릭 시 팝업이 열리는 테스트 패턴
        /// </summary>
        private IEnumerator TestButtonOpensPopup<TPopup>(string buttonFieldName, string buttonDisplayName)
            where TPopup : BasePopup
        {
            // Arrange
            Button button = GetButtonField(buttonFieldName);
            if (button == null)
            {
                Assert.Inconclusive($"{buttonDisplayName} 버튼이 MainMenuButtonHandler에 할당되지 않았습니다");
                yield break;
            }

            // Act
            yield return ClickButton(button, buttonDisplayName);
            yield return WaitUntilPopupAppears<TPopup>();

            // Assert
            TPopup popup = Object.FindFirstObjectByType<TPopup>();
            Assert.IsNotNull(popup, $"{typeof(TPopup).Name}이 나타나야 합니다");
            Assert.AreEqual(1, UIManager.Instance.GetActivePopupCount(), "활성 팝업 개수는 1이어야 합니다");

            // Cleanup
            UIManager.Instance.CloseAllActivePopups();
            yield return WaitUntilNoActivePopups();
        }

        /// <summary>
        /// 팝업 닫기 버튼 테스트 패턴
        /// </summary>
        private IEnumerator TestPopupCloseButton<TPopup>(string buttonFieldName, string buttonDisplayName)
            where TPopup : BasePopup
        {
            // Arrange - 팝업 열기
            Button button = GetButtonField(buttonFieldName);
            if (button == null)
            {
                Assert.Inconclusive($"{buttonDisplayName} 버튼이 MainMenuButtonHandler에 할당되지 않았습니다");
                yield break;
            }

            yield return ClickButton(button, buttonDisplayName);
            yield return WaitUntilPopupAppears<TPopup>();
            Assert.AreEqual(1, UIManager.Instance.GetActivePopupCount(), "팝업이 열려 있어야 합니다");

            // Arrange - 닫기 버튼 찾기
            BasePopup popup = Object.FindFirstObjectByType<BasePopup>();
            Assert.IsNotNull(popup, "활성 팝업이 존재해야 합니다");

            Button closeButton = GetCloseButton(popup);
            Assert.IsNotNull(closeButton, "팝업에 닫기 버튼이 있어야 합니다");

            // Act
            yield return ClickButton(closeButton, "닫기");

            // Assert - 팝업이 닫힐 때까지 대기
            yield return new WaitUntil(() => UIManager.Instance.GetActivePopupCount() == 0);
            Assert.AreEqual(0, UIManager.Instance.GetActivePopupCount(), "닫기 버튼 클릭 후 팝업이 닫혀야 합니다");

            // Cleanup
            yield return WaitUntilNoActivePopups();
        }

        /// <summary>
        /// 버튼 클릭 시 로그 메시지 출력 테스트 패턴
        /// </summary>
        private IEnumerator TestButtonClickLogsMessage(string buttonFieldName, string expectedLog, string buttonDisplayName)
        {
            // Arrange
            Button button = GetButtonField(buttonFieldName);
            if (button == null)
            {
                Assert.Inconclusive($"{buttonDisplayName} 버튼이 MainMenuButtonHandler에 할당되지 않았습니다");
                yield break;
            }

            // Act & Assert
            LogAssert.Expect(LogType.Log, expectedLog);
            yield return ClickButton(button, buttonDisplayName);
        }

        #endregion

        #region Helper Methods - Component Waiting

        /// <summary>
        /// 특정 컴포넌트가 씬에 나타날 때까지 대기
        /// </summary>
        private IEnumerator WaitForComponent<T>() where T : Object
        {
            float elapsed = 0f;
            while (elapsed < POPUP_SPAWN_TIMEOUT)
            {
                if (Object.FindFirstObjectByType<T>() != null)
                    yield break;

                yield return null;
                elapsed += Time.deltaTime;
            }

            Assert.Fail($"{typeof(T).Name}이 {POPUP_SPAWN_TIMEOUT}초 내에 나타나지 않았습니다");
        }

        /// <summary>
        /// 특정 팝업이 나타날 때까지 대기
        /// </summary>
        private IEnumerator WaitUntilPopupAppears<T>() where T : BasePopup
        {
            yield return new WaitUntil(() => Object.FindFirstObjectByType<T>() != null);
        }

        /// <summary>
        /// 모든 팝업이 닫힐 때까지 대기
        /// </summary>
        private IEnumerator WaitUntilNoActivePopups()
        {
            float elapsed = 0f;
            while (elapsed < POPUP_DESTROY_TIMEOUT)
            {
                if (UIManager.Instance == null || UIManager.Instance.GetActivePopupCount() == 0)
                    yield break;

                yield return null;
                elapsed += Time.deltaTime;
            }
        }

        #endregion

        #region Helper Methods - Button Interaction

        /// <summary>
        /// 버튼 클릭 시뮬레이션
        /// </summary>
        private IEnumerator ClickButton(Button button, string buttonName)
        {
            Assert.IsNotNull(button, $"{buttonName} 버튼이 null이 아니어야 합니다");

            var pointerData = new PointerEventData(eventSystem)
            {
                button = PointerEventData.InputButton.Left
            };

            // PointerDown
            ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerDownHandler);
            yield return new WaitForSeconds(BUTTON_INTERACTION_DELAY);

            // PointerUp & Click
            ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerClickHandler);

            yield return null; // 이벤트 처리 대기
        }

        #endregion

        #region Helper Methods - Reflection

        /// <summary>
        /// MainMenuButtonHandler의 버튼 필드 가져오기
        /// </summary>
        private Button GetButtonField(string fieldName)
        {
            if (handler == null) return null;

            var field = typeof(MainMenuButtonHandler).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(handler) as Button;
        }

        /// <summary>
        /// BasePopup의 닫기 버튼 가져오기
        /// </summary>
        private Button GetCloseButton(BasePopup popup)
        {
            var field = typeof(BasePopup).GetField("closeButton",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(popup) as Button;
        }

        #endregion
    }
}
