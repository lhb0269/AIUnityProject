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
    /// HamburgerMenuPopup 기능 테스트
    /// - 팝업 열기/닫기 동작 검증
    /// - 팝업 내부 버튼 상호작용 검증
    /// - UI 싱글톤 격리 및 독립성 보장
    /// </summary>
    public class HamburgerMenuPopupTests
    {
        #region Constants

        private const float POPUP_SPAWN_TIMEOUT = 2f;
        private const float POPUP_DESTROY_TIMEOUT = 2f;
        private const float BUTTON_INTERACTION_DELAY = 0.1f;
        private const string TEST_SCENE_NAME = "SampleScene";

        #endregion

        #region Fields

        private static bool sceneLoaded = false;
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
            yield return WaitForComponent<EventSystem>();
            eventSystem = Object.FindFirstObjectByType<EventSystem>();
            Assert.IsNotNull(eventSystem, "EventSystem이 씬에 존재해야 합니다");

            yield return WaitForComponent<UIManager>();
            Assert.IsNotNull(UIManager.Instance, "UIManager 인스턴스가 존재해야 합니다");

            yield return WaitForComponent<MainMenuButtonHandler>();
            Assert.IsNotNull(Object.FindFirstObjectByType<MainMenuButtonHandler>(),
                "MainMenuButtonHandler가 씬에 존재해야 합니다");

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

        #region Tests

        /// <summary>
        /// 테스트: 햄버거 버튼 클릭 시 팝업이 나타나고 모든 버튼이 할당됨
        /// Given: 깨끗한 UI 상태
        /// When: 햄버거 메뉴 버튼 클릭
        /// Then: HamburgerMenuPopup이 열리고 12개 버튼이 모두 할당되어 있음
        /// </summary>
        [UnityTest]
        public IEnumerator WhenHamburgerButtonClicked_ThenPopupOpensWithAllButtonsAssigned()
        {
            // Arrange
            Button hamburgerBtn = GetMainMenuButton("hamburgerMenuBtn");
            Assert.IsNotNull(hamburgerBtn, "햄버거 메뉴 버튼이 MainMenuButtonHandler에 할당되어 있어야 합니다");

            // Act
            yield return ClickButton(hamburgerBtn, "햄버거 메뉴");
            yield return WaitUntilPopupAppears<HamburgerMenuPopup>();

            // Assert
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 나타나야 합니다");
            Assert.AreEqual(1, UIManager.Instance.GetActivePopupCount(),
                "활성 팝업 개수는 1이어야 합니다");

            // 12개 버튼 할당 검증
            AssertPopupButtonAssigned(popup, "missionBtn", "미션");
            AssertPopupButtonAssigned(popup, "passBtn", "패스");
            AssertPopupButtonAssigned(popup, "mailboxBtn", "우편함");
            AssertPopupButtonAssigned(popup, "costumeBtn", "코스튬");
            AssertPopupButtonAssigned(popup, "heroPowerBtn", "용사의 힘");
            AssertPopupButtonAssigned(popup, "equipSlotEnhanceBtn", "장비 슬롯 강화");
            AssertPopupButtonAssigned(popup, "relicBtn", "유물");
            AssertPopupButtonAssigned(popup, "friendBtn", "친구");
            AssertPopupButtonAssigned(popup, "rankingBtn", "랭킹");
            AssertPopupButtonAssigned(popup, "guildBtn", "길드");
            AssertPopupButtonAssigned(popup, "growthDungeonBtn", "성장 던전");
            AssertPopupButtonAssigned(popup, "worldBossBtn", "월드 보스");

            // Cleanup
            yield return ClosePopupWithButton(popup);
        }

        /// <summary>
        /// 테스트: 팝업 내 모든 버튼 클릭 시 각각 올바른 핸들러 호출
        /// Given: 햄버거 메뉴 팝업이 열린 상태
        /// When: 12개 버튼을 순차적으로 클릭
        /// Then: 각 버튼마다 올바른 로그 메시지 출력
        /// </summary>
        [UnityTest]
        public IEnumerator WhenAllPopupButtonsClicked_ThenEachTriggersCorrectHandler()
        {
            // Arrange
            yield return OpenHamburgerPopup();
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 열려 있어야 합니다");

            // Act & Assert - 각 버튼 클릭 및 로그 검증
            var buttonTests = new[]
            {
                ("missionBtn", "[HamburgerMenu] 미션 버튼 클릭", "미션"),
                ("passBtn", "[HamburgerMenu] 패스 버튼 클릭", "패스"),
                ("mailboxBtn", "[HamburgerMenu] 우편함 버튼 클릭", "우편함"),
                ("costumeBtn", "[HamburgerMenu] 코스튬 버튼 클릭", "코스튬"),
                ("heroPowerBtn", "[HamburgerMenu] 용사의 힘 버튼 클릭", "용사의 힘"),
                ("equipSlotEnhanceBtn", "[HamburgerMenu] 장비 슬롯 강화 버튼 클릭", "장비 슬롯 강화"),
                ("relicBtn", "[HamburgerMenu] 유물 버튼 클릭", "유물"),
                ("friendBtn", "[HamburgerMenu] 친구 버튼 클릭", "친구"),
                ("rankingBtn", "[HamburgerMenu] 랭킹 버튼 클릭", "랭킹"),
                ("guildBtn", "[HamburgerMenu] 길드 버튼 클릭", "길드"),
                ("growthDungeonBtn", "[HamburgerMenu] 성장 던전 버튼 클릭", "성장 던전"),
                ("worldBossBtn", "[HamburgerMenu] 월드 보스 버튼 클릭", "월드 보스")
            };

            foreach (var (fieldName, expectedLog, displayName) in buttonTests)
            {
                Button button = GetPopupButton(popup, fieldName);
                if (button != null)
                {
                    LogAssert.Expect(LogType.Log, expectedLog);
                    yield return ClickButton(button, displayName);
                }
                else
                {
                    Debug.LogWarning($"[테스트] {displayName} 버튼을 찾을 수 없습니다");
                }
            }

            // Cleanup
            yield return ClosePopupWithButton(popup);
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
        /// 버튼 클릭 시뮬레이션 (조건 기반 대기)
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

        /// <summary>
        /// 햄버거 팝업 열기
        /// </summary>
        private IEnumerator OpenHamburgerPopup()
        {
            Button hamburgerBtn = GetMainMenuButton("hamburgerMenuBtn");
            Assert.IsNotNull(hamburgerBtn, "햄버거 메뉴 버튼이 할당되어 있어야 합니다");

            yield return ClickButton(hamburgerBtn, "햄버거 메뉴");
            yield return WaitUntilPopupAppears<HamburgerMenuPopup>();
        }

        /// <summary>
        /// 팝업 닫기 버튼으로 팝업 닫기
        /// </summary>
        private IEnumerator ClosePopupWithButton(BasePopup popup)
        {
            Button closeButton = GetCloseButton(popup);
            Assert.IsNotNull(closeButton, "팝업에 닫기 버튼이 있어야 합니다");

            int initialCount = UIManager.Instance.GetActivePopupCount();
            yield return ClickButton(closeButton, "닫기");

            // 팝업이 닫힐 때까지 대기
            yield return new WaitUntil(() =>
                UIManager.Instance.GetActivePopupCount() < initialCount);

            Assert.AreEqual(0, UIManager.Instance.GetActivePopupCount(),
                "닫기 버튼 클릭 후 모든 팝업이 닫혀야 합니다");

            yield return WaitUntilNoActivePopups();
        }

        #endregion

        #region Helper Methods - Reflection

        /// <summary>
        /// MainMenuButtonHandler의 버튼 필드 가져오기
        /// </summary>
        private Button GetMainMenuButton(string fieldName)
        {
            var handler = Object.FindFirstObjectByType<MainMenuButtonHandler>();
            if (handler == null) return null;

            var field = typeof(MainMenuButtonHandler).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(handler) as Button;
        }

        /// <summary>
        /// HamburgerMenuPopup의 버튼 필드 가져오기
        /// </summary>
        private Button GetPopupButton(HamburgerMenuPopup popup, string fieldName)
        {
            var field = typeof(HamburgerMenuPopup).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(popup) as Button;
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

        #region Helper Methods - Assertions

        /// <summary>
        /// 팝업 버튼 할당 검증
        /// </summary>
        private void AssertPopupButtonAssigned(HamburgerMenuPopup popup, string fieldName, string displayName)
        {
            Button button = GetPopupButton(popup, fieldName);
            Assert.IsNotNull(button, $"{displayName} 버튼이 할당되어 있어야 합니다");
        }

        #endregion
    }
}
