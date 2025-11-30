using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;
using MobileGame.UI;
using MobileGame.Interfaces;
using MobileGame.Tests.Mocks;
using MobileGame.Tests.Helpers;

namespace MobileGame.Tests.PlayMode.UI
{
    /// <summary>
    /// HamburgerMenuPopup 기능 테스트 (DI 기반)
    /// - 팝업 열기/닫기 생명주기
    /// - DI를 통한 UIManager 주입 검증
    /// - 15개 버튼 클릭 이벤트 처리
    /// - 팝업 중복 열기 방지 로직
    /// </summary>
    public class HamburgerMenuPopupTests
    {
        #region Fields

        private LifetimeScope testScope;
        private HamburgerMenuPopup popup;
        private MockUIManager mockUIManager;

        // 로그만 출력하는 버튼 (12개)
        private Button missionBtn;
        private Button passBtn;
        private Button mailboxBtn;
        private Button costumeBtn;
        private Button heroPowerBtn;
        private Button equipSlotEnhanceBtn;
        private Button relicBtn;
        private Button friendBtn;
        private Button rankingBtn;
        private Button guildBtn;
        private Button growthDungeonBtn;
        private Button worldBossBtn;

        // 팝업을 여는 버튼 (3개)
        private Button townBtn;
        private Button noticeBtn;
        private Button gameSettingBtn;

        #endregion

        #region Setup & Teardown

        /// <summary>
        /// 각 테스트 전 초기화
        /// - DI 컨테이너 생성
        /// - Mock UIManager 주입
        /// - HamburgerMenuPopup 생성 및 버튼 설정
        /// </summary>
        [UnitySetUp]
        public IEnumerator Setup()
        {
            // 테스트 컨테이너 빌드
            testScope = TestContainerBuilder.CreateCustomScope(
                includeUI: true,
                includeGame: false,
                includeAudio: false
            );

            // 컨테이너에서 Mock 가져오기 (중요: 팝업이 주입받는 것과 동일한 인스턴스)
            mockUIManager = TestContainerBuilder.GetMockUIManager(testScope.Container);

            // 팝업 GameObject 생성
            var popupObj = new GameObject("TestHamburgerMenuPopup");
            popup = popupObj.AddComponent<HamburgerMenuPopup>();

            // UI 컴포넌트 설정
            SetupButtons(popupObj.transform);

            // DI 주입
            testScope.Container.Inject(popup);

            yield return null; // Start() 실행 대기
            yield return null; // 버튼 이벤트 등록 완료 대기
        }

        /// <summary>
        /// 각 테스트 후 정리
        /// </summary>
        [UnityTearDown]
        public IEnumerator Teardown()
        {
            mockUIManager?.Reset();

            if (popup != null)
                Object.Destroy(popup.gameObject);

            if (testScope != null)
                testScope.Dispose();

            yield return null;
        }

        #endregion

        #region Setup Helpers

        /// <summary>
        /// 모든 버튼 생성 및 필드 설정
        /// </summary>
        private void SetupButtons(Transform parent)
        {
            // 로그만 출력하는 버튼 (12개)
            missionBtn = CreateButton("MissionButton", parent);
            passBtn = CreateButton("PassButton", parent);
            mailboxBtn = CreateButton("MailboxButton", parent);
            costumeBtn = CreateButton("CostumeButton", parent);
            heroPowerBtn = CreateButton("HeroPowerButton", parent);
            equipSlotEnhanceBtn = CreateButton("EquipSlotEnhanceButton", parent);
            relicBtn = CreateButton("RelicButton", parent);
            friendBtn = CreateButton("FriendButton", parent);
            rankingBtn = CreateButton("RankingButton", parent);
            guildBtn = CreateButton("GuildButton", parent);
            growthDungeonBtn = CreateButton("GrowthDungeonButton", parent);
            worldBossBtn = CreateButton("WorldBossButton", parent);

            // 팝업을 여는 버튼 (3개)
            townBtn = CreateButton("TownButton", parent);
            noticeBtn = CreateButton("NoticeButton", parent);
            gameSettingBtn = CreateButton("GameSettingButton", parent);

            // 리플렉션으로 private 필드에 버튼 연결
            SetPrivateField(popup, "missionBtn", missionBtn);
            SetPrivateField(popup, "passBtn", passBtn);
            SetPrivateField(popup, "mailboxBtn", mailboxBtn);
            SetPrivateField(popup, "costumeBtn", costumeBtn);
            SetPrivateField(popup, "heroPowerBtn", heroPowerBtn);
            SetPrivateField(popup, "equipSlotEnhanceBtn", equipSlotEnhanceBtn);
            SetPrivateField(popup, "relicBtn", relicBtn);
            SetPrivateField(popup, "friendBtn", friendBtn);
            SetPrivateField(popup, "rankingBtn", rankingBtn);
            SetPrivateField(popup, "guildBtn", guildBtn);
            SetPrivateField(popup, "growthDungeonBtn", growthDungeonBtn);
            SetPrivateField(popup, "worldBossBtn", worldBossBtn);
            SetPrivateField(popup, "townBtn", townBtn);
            SetPrivateField(popup, "noticeBtn", noticeBtn);
            SetPrivateField(popup, "gameSettingBtn", gameSettingBtn);
        }

        #endregion

        #region Tests - Basic Lifecycle

        /// <summary>
        /// 테스트: 팝업 Show 호출 시 GameObject 활성화
        /// Given: 팝업이 비활성화 상태
        /// When: Show() 호출
        /// Then: gameObject.activeSelf가 true
        /// </summary>
        [UnityTest]
        public IEnumerator WhenShow_ThenGameObjectActivated()
        {
            // Arrange
            popup.gameObject.SetActive(false);

            // Act
            popup.Show();
            yield return null;

            // Assert
            Assert.IsTrue(popup.gameObject.activeSelf, "팝업이 활성화되어야 합니다.");
        }

        /// <summary>
        /// 테스트: 닫기 버튼 클릭 시 UIManager.ClosePopup 호출됨
        /// Given: 팝업이 열린 상태, closeButton이 설정됨
        /// When: closeButton 클릭
        /// Then: UIManager.ClosePopup이 1번 호출됨
        /// </summary>
        [UnityTest]
        public IEnumerator WhenCloseButtonClicked_ThenUIManagerClosePopupCalled()
        {
            // Arrange
            var closeButton = CreateButton("CloseButton", popup.transform);
            SetPrivateField(popup, "closeButton", closeButton);

            // closeButton 이벤트 등록을 위해 Start 재호출 (리플렉션)
            var startMethod = typeof(BasePopup).GetMethod("Start",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            startMethod?.Invoke(popup, null);

            popup.Show();
            yield return null;
            mockUIManager.Reset();

            // Act
            closeButton.onClick.Invoke();
            yield return null;

            // Assert
            Assert.AreEqual(1, mockUIManager.ClosedPopups.Count,
                "UIManager.ClosePopup이 1번 호출되어야 합니다.");
        }

        #endregion

        #region Tests - DI Injection

        /// <summary>
        /// 테스트: VContainer를 통해 UIManager 주입 확인
        /// Given: VContainer 컨테이너 설정 완료
        /// When: Inject() 호출 후
        /// Then: uiManager 필드가 null이 아님
        /// </summary>
        [UnityTest]
        public IEnumerator WhenInjected_ThenUIManagerNotNull()
        {
            // Assert
            var uiManagerField = typeof(BasePopup)
                .GetField("uiManager",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);
            var injectedUIManager = uiManagerField?.GetValue(popup);

            Assert.IsNotNull(injectedUIManager, "UIManager가 주입되어야 합니다.");
            yield return null;
        }

        #endregion

        #region Tests - Button Interactions (Log Only)

        /// <summary>
        /// 테스트: 미션 버튼 클릭 시 로그 출력
        /// Given: 버튼이 준비된 상태
        /// When: 미션 버튼 클릭
        /// Then: "[HamburgerMenu] 미션 버튼 클릭" 로그 출력
        /// </summary>
        [UnityTest]
        public IEnumerator WhenMissionButtonClicked_ThenLogOutput()
        {
            yield return TestButtonClickWithLog(missionBtn, "[HamburgerMenu] 미션 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenPassButtonClicked_ThenLogOutput()
        {
            yield return TestButtonClickWithLog(passBtn, "[HamburgerMenu] 패스 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenMailboxButtonClicked_ThenLogOutput()
        {
            yield return TestButtonClickWithLog(mailboxBtn, "[HamburgerMenu] 우편함 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenCostumeButtonClicked_ThenLogOutput()
        {
            yield return TestButtonClickWithLog(costumeBtn, "[HamburgerMenu] 코스튬 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenHeroPowerButtonClicked_ThenLogOutput()
        {
            yield return TestButtonClickWithLog(heroPowerBtn, "[HamburgerMenu] 용사의 힘 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenEquipSlotEnhanceButtonClicked_ThenLogOutput()
        {
            yield return TestButtonClickWithLog(equipSlotEnhanceBtn, "[HamburgerMenu] 장비 슬롯 강화 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenRelicButtonClicked_ThenLogOutput()
        {
            yield return TestButtonClickWithLog(relicBtn, "[HamburgerMenu] 유물 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenFriendButtonClicked_ThenLogOutput()
        {
            yield return TestButtonClickWithLog(friendBtn, "[HamburgerMenu] 친구 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenRankingButtonClicked_ThenLogOutput()
        {
            yield return TestButtonClickWithLog(rankingBtn, "[HamburgerMenu] 랭킹 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenGuildButtonClicked_ThenLogOutput()
        {
            yield return TestButtonClickWithLog(guildBtn, "[HamburgerMenu] 길드 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenGrowthDungeonButtonClicked_ThenLogOutput()
        {
            yield return TestButtonClickWithLog(growthDungeonBtn, "[HamburgerMenu] 성장 던전 버튼 클릭");
        }

        [UnityTest]
        public IEnumerator WhenWorldBossButtonClicked_ThenLogOutput()
        {
            yield return TestButtonClickWithLog(worldBossBtn, "[HamburgerMenu] 월드 보스 버튼 클릭");
        }

        #endregion

        #region Tests - Button Interactions (Popup Opening)

        /// <summary>
        /// 테스트: 마을 버튼 클릭 시 Town 팝업 열기
        /// Given: 햄버거 메뉴 팝업이 이미 열린 상태
        /// When: townBtn 클릭
        /// Then: UIManager.ShowPopup(PopupID.Town) 호출됨 (총 2번 호출)
        /// </summary>
        [UnityTest]
        public IEnumerator WhenTownButtonClicked_ThenTownPopupOpened()
        {
            // Arrange
            mockUIManager.Reset();
            // 햄버거 메뉴를 먼저 ShowPopup으로 열기 (실제 시나리오 반영)
            mockUIManager.ShowPopup(PopupID.HamburgerMenu);
            // ShowPopup이 FakeActivePopupCount를 자동으로 1 증가시킴

            // Act
            townBtn.onClick.Invoke();
            yield return null;

            // Assert
            Assert.AreEqual(2, mockUIManager.ShownPopups.Count,
                "HamburgerMenu + Town 총 2개의 팝업이 열려야 합니다.");
            Assert.AreEqual(PopupID.HamburgerMenu, mockUIManager.ShownPopups[0],
                "첫 번째는 HamburgerMenu 팝업이어야 합니다.");
            Assert.AreEqual(PopupID.Town, mockUIManager.ShownPopups[1],
                "두 번째는 Town 팝업이어야 합니다.");
        }

        /// <summary>
        /// 테스트: 공지사항 버튼 클릭 시 Notice 팝업 열기
        /// Given: 햄버거 메뉴 팝업이 이미 열린 상태
        /// When: noticeBtn 클릭
        /// Then: UIManager.ShowPopup(PopupID.Notice) 호출됨 (총 2번 호출)
        /// </summary>
        [UnityTest]
        public IEnumerator WhenNoticeButtonClicked_ThenNoticePopupOpened()
        {
            // Arrange
            mockUIManager.Reset();
            mockUIManager.ShowPopup(PopupID.HamburgerMenu);
            // ShowPopup이 FakeActivePopupCount를 자동으로 1 증가시킴

            // Act
            noticeBtn.onClick.Invoke();
            yield return null;

            // Assert
            Assert.AreEqual(2, mockUIManager.ShownPopups.Count,
                "HamburgerMenu + Notice 총 2개의 팝업이 열려야 합니다.");
            Assert.AreEqual(PopupID.HamburgerMenu, mockUIManager.ShownPopups[0],
                "첫 번째는 HamburgerMenu 팝업이어야 합니다.");
            Assert.AreEqual(PopupID.Notice, mockUIManager.ShownPopups[1],
                "두 번째는 Notice 팝업이어야 합니다.");
        }

        /// <summary>
        /// 테스트: 게임 설정 버튼 클릭 시 GameSetting 팝업 열기
        /// Given: 햄버거 메뉴 팝업이 이미 열린 상태
        /// When: gameSettingBtn 클릭
        /// Then: UIManager.ShowPopup(PopupID.GameSetting) 호출됨 (총 2번 호출)
        /// </summary>
        [UnityTest]
        public IEnumerator WhenGameSettingButtonClicked_ThenGameSettingPopupOpened()
        {
            // Arrange
            mockUIManager.Reset();
            mockUIManager.ShowPopup(PopupID.HamburgerMenu);
            // ShowPopup이 FakeActivePopupCount를 자동으로 1 증가시킴

            // Act
            gameSettingBtn.onClick.Invoke();
            yield return null;

            // Assert
            Assert.AreEqual(2, mockUIManager.ShownPopups.Count,
                "HamburgerMenu + GameSetting 총 2개의 팝업이 열려야 합니다.");
            Assert.AreEqual(PopupID.HamburgerMenu, mockUIManager.ShownPopups[0],
                "첫 번째는 HamburgerMenu 팝업이어야 합니다.");
            Assert.AreEqual(PopupID.GameSetting, mockUIManager.ShownPopups[1],
                "두 번째는 GameSetting 팝업이어야 합니다.");
        }

        #endregion

        #region Tests - Edge Cases

        /// <summary>
        /// 테스트: 팝업 중복 열기 방지 (2개 이상 팝업 열림 시)
        /// Given: 이미 2개의 팝업이 열린 상태 (햄버거 메뉴 + 다른 팝업)
        /// When: townBtn 클릭
        /// Then: ShowPopup이 호출되지 않음 (중복 방지)
        /// </summary>
        [UnityTest]
        public IEnumerator WhenTownButtonClicked_AndTwoPopupsOpen_ThenPopupNotOpened()
        {
            // Arrange
            mockUIManager.Reset();
            mockUIManager.FakeActivePopupCount = 2; // 햄버거 메뉴 + 다른 팝업

            // Act & Assert
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 마을 버튼 클릭");
            LogAssert.Expect(LogType.Warning, "[HamburgerMenu] 이미 다른 팝업이 열려있습니다. 먼저 닫아주세요.");
            townBtn.onClick.Invoke();
            yield return null;

            // Assert
            Assert.AreEqual(0, mockUIManager.ShownPopups.Count,
                "팝업이 2개 이상 열려있으면 ShowPopup이 호출되지 않아야 합니다.");
        }

        /// <summary>
        /// 테스트: UIManager가 null인 경우 팝업 열기 실패
        /// Given: UIManager가 주입되지 않은 상태
        /// When: townBtn 클릭
        /// Then: 경고 로그 출력, ShowPopup 호출 안됨
        /// </summary>
        [UnityTest]
        public IEnumerator WhenTownButtonClicked_AndUIManagerNull_ThenWarningLogged()
        {
            // Arrange
            var uiManagerField = typeof(BasePopup)
                .GetField("uiManager",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);
            uiManagerField?.SetValue(popup, null); // uiManager를 null로 설정

            mockUIManager.Reset();

            // Act & Assert
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 마을 버튼 클릭");
            LogAssert.Expect(LogType.Warning, "[HamburgerMenu] UIManager가 주입되지 않았습니다.");
            townBtn.onClick.Invoke();
            yield return null;

            // Assert
            Assert.AreEqual(0, mockUIManager.ShownPopups.Count,
                "UIManager가 null이면 ShowPopup이 호출되지 않아야 합니다.");
        }

        #endregion

        #region Tests - Integration

        /// <summary>
        /// 테스트: 여러 버튼 연속 클릭 (팝업 열기)
        /// Given: 햄버거 메뉴 팝업이 이미 열린 상태
        /// When: town, notice, gameSetting 버튼 순서대로 클릭
        /// Then: 각 팝업이 순서대로 열림 (총 4번 호출)
        /// </summary>
        [UnityTest]
        public IEnumerator WhenMultiplePopupButtonsClicked_ThenAllPopupsOpened()
        {
            // Arrange
            mockUIManager.Reset();
            mockUIManager.ShowPopup(PopupID.HamburgerMenu);
            // ShowPopup이 FakeActivePopupCount를 1로 설정 (햄버거 메뉴만 열림)

            // Act - 첫 번째 버튼 (Town)
            townBtn.onClick.Invoke();
            yield return null;

            // Town 팝업 닫힘 시뮬레이션 (다시 햄버거 메뉴만 열림)
            mockUIManager.FakeActivePopupCount = 1;
            noticeBtn.onClick.Invoke();
            yield return null;

            // Notice 팝업 닫힘 시뮬레이션 (다시 햄버거 메뉴만 열림)
            mockUIManager.FakeActivePopupCount = 1;
            gameSettingBtn.onClick.Invoke();
            yield return null;

            // Assert
            Assert.AreEqual(4, mockUIManager.ShownPopups.Count,
                "HamburgerMenu + Town + Notice + GameSetting 총 4개의 팝업이 열려야 합니다.");
            Assert.AreEqual(PopupID.HamburgerMenu, mockUIManager.ShownPopups[0], "첫 번째는 HamburgerMenu");
            Assert.AreEqual(PopupID.Town, mockUIManager.ShownPopups[1], "두 번째는 Town");
            Assert.AreEqual(PopupID.Notice, mockUIManager.ShownPopups[2], "세 번째는 Notice");
            Assert.AreEqual(PopupID.GameSetting, mockUIManager.ShownPopups[3], "네 번째는 GameSetting");
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// 버튼 생성 헬퍼 메서드
        /// </summary>
        private Button CreateButton(string name, Transform parent)
        {
            var btnObj = new GameObject(name);
            btnObj.transform.SetParent(parent);
            return btnObj.AddComponent<Button>();
        }

        /// <summary>
        /// private 필드에 값 설정 (리플렉션)
        /// </summary>
        private void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            field?.SetValue(obj, value);
        }

        /// <summary>
        /// 버튼 클릭 시 로그 출력 테스트 패턴
        /// </summary>
        private IEnumerator TestButtonClickWithLog(Button button, string expectedLog)
        {
            // Arrange
            Assert.IsNotNull(button, "버튼이 존재해야 합니다.");
            mockUIManager.Reset();

            // Act & Assert
            LogAssert.Expect(LogType.Log, expectedLog);
            button.onClick.Invoke();
            yield return null;

            // Assert - 팝업이 열리지 않았는지 확인
            Assert.AreEqual(0, mockUIManager.ShownPopups.Count,
                "로그만 출력하는 버튼은 팝업을 열지 않아야 합니다.");
        }

        #endregion
    }
}
