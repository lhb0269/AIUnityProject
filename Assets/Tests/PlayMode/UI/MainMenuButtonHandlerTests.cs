using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using MobileGame.UI;
using MobileGame.Managers;

namespace MobileGame.Tests.UI
{
    /// <summary>
    /// MainMenuButtonHandler의 기능 테스트 클래스
    /// Unity Test Framework를 사용한 UI 반응성 및 기능 검증
    /// 실제 씬에 있는 객체를 사용하여 테스트 (SampleScene을 열어둔 상태에서 실행)
    /// </summary>
    public class MainMenuButtonHandlerTests
    {
        private MainMenuButtonHandler handler;

        #region Setup & Teardown

        /// <summary>
        /// 각 테스트 실행 전 초기화
        /// 씬에서 MainMenuButtonHandler를 찾아서 사용
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // 씬에서 MainMenuButtonHandler 찾기
            handler = Object.FindFirstObjectByType<MainMenuButtonHandler>();

            if (handler == null)
            {
                Assert.Fail("MainMenuButtonHandler를 씬에서 찾을 수 없습니다. SampleScene을 열어두고 테스트를 실행하세요.");
            }
        }

        /// <summary>
        /// 각 테스트 실행 후 정리
        /// 씬의 객체는 그대로 유지
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            // 씬의 객체는 파괴하지 않음
            handler = null;
        }

        #endregion

        #region 초기화 테스트

        /// <summary>
        /// MainMenuButtonHandler가 씬에 정상적으로 존재하는지 테스트
        /// </summary>
        [Test]
        public void MainMenuButtonHandler_Exists_In_Scene()
        {
            // Assert: handler가 씬에 존재
            Assert.IsNotNull(handler, "MainMenuButtonHandler가 씬에 존재해야 합니다");
            Assert.IsTrue(handler.gameObject.activeInHierarchy, "MainMenuButtonHandler가 활성화되어 있어야 합니다");
        }

        /// <summary>
        /// UIManager가 씬에 존재하는지 테스트
        /// </summary>
        [Test]
        public void UIManager_Exists_In_Scene()
        {
            // Assert: UIManager 인스턴스가 존재
            var uiManager = Object.FindFirstObjectByType<UIManager>();
            Assert.IsNotNull(uiManager, "UIManager가 씬에 존재해야 합니다");
        }

        #endregion

        #region 버튼 핸들러 메서드 테스트 (25개)

        /// <summary>
        /// 햄버거 메뉴 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnHamburgerMenuClicked_Logs_Correctly()
        {
            // Arrange: 로그 예상 설정
            LogAssert.Expect(LogType.Log, "[MainMenu] 햄버거 메뉴 버튼 클릭");

            // Act: 메서드 호출
            handler.OnHamburgerMenuClicked();

            // Assert: LogAssert가 자동으로 검증
        }

        /// <summary>
        /// 설정 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnSettingClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 설정 버튼 클릭");
            handler.OnSettingClicked();
        }

        /// <summary>
        /// 유저 정보 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnUserInfoClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 유저 정보 버튼 클릭");
            handler.OnUserInfoClicked();
        }

        /// <summary>
        /// 가이드 퀘스트 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnGuideQuestClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 가이드 퀘스트 버튼 클릭");
            handler.OnGuideQuestClicked();
        }

        /// <summary>
        /// 상점 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnShopClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 상점 버튼 클릭");
            handler.OnShopClicked();
        }

        /// <summary>
        /// 모집 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnRecruitmentClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 모집 버튼 클릭");
            handler.OnRecruitmentClicked();
        }

        /// <summary>
        /// 이벤트 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnEventClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 이벤트 버튼 클릭");
            handler.OnEventClicked();
        }

        /// <summary>
        /// 캐릭터 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnCharacterClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 캐릭터 버튼 클릭");
            handler.OnCharacterClicked();
        }

        /// <summary>
        /// 스킬 설정 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnSkillSettingClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 스킬 설정 버튼 클릭");
            handler.OnSkillSettingClicked();
        }

        /// <summary>
        /// 스킬 1 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnSkill1Clicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 스킬 1 버튼 클릭");
            handler.OnSkill1Clicked();
        }

        /// <summary>
        /// 스킬 2 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnSkill2Clicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 스킬 2 버튼 클릭");
            handler.OnSkill2Clicked();
        }

        /// <summary>
        /// 스킬 3 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnSkill3Clicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 스킬 3 버튼 클릭");
            handler.OnSkill3Clicked();
        }

        /// <summary>
        /// 스킬 4 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnSkill4Clicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 스킬 4 버튼 클릭");
            handler.OnSkill4Clicked();
        }

        /// <summary>
        /// 스킬 5 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnSkill5Clicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 스킬 5 버튼 클릭");
            handler.OnSkill5Clicked();
        }

        /// <summary>
        /// 스킬 6 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnSkill6Clicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 스킬 6 버튼 클릭");
            handler.OnSkill6Clicked();
        }

        /// <summary>
        /// 무기 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnWeaponClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 무기 버튼 클릭");
            handler.OnWeaponClicked();
        }

        /// <summary>
        /// 장비 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnEquipClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 장비 버튼 클릭");
            handler.OnEquipClicked();
        }

        /// <summary>
        /// 협력자 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnCoworkerClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 협력자 버튼 클릭");
            handler.OnCoworkerClicked();
        }

        /// <summary>
        /// HP 포션 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnHPPotionClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] HP 포션 버튼 클릭");
            handler.OnHPPotionClicked();
        }

        /// <summary>
        /// MP 포션 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnMPPotionClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] MP 포션 버튼 클릭");
            handler.OnMPPotionClicked();
        }

        /// <summary>
        /// 포션 설정 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnPotionSettingClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 포션 설정 버튼 클릭");
            handler.OnPotionSettingClicked();
        }

        /// <summary>
        /// 컨트롤 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnControllClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 컨트롤 버튼 클릭");
            handler.OnControllClicked();
        }

        /// <summary>
        /// 챕터 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnChapterClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 챕터 버튼 클릭");
            handler.OnChapterClicked();
        }

        /// <summary>
        /// 몬스터 스폰 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnMonsterSpawnClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 몬스터 스폰 버튼 클릭");
            handler.OnMonsterSpawnClicked();
        }

        /// <summary>
        /// 스폰 설정 버튼 클릭 시 로그 출력 테스트
        /// </summary>
        [Test]
        public void OnSpawnSettingClicked_Logs_Correctly()
        {
            LogAssert.Expect(LogType.Log, "[MainMenu] 스폰 설정 버튼 클릭");
            handler.OnSpawnSettingClicked();
        }

        #endregion

        #region 버튼 존재 테스트

        /// <summary>
        /// 씬에 버튼이 존재하는지 테스트
        /// </summary>
        [Test]
        public void Scene_Has_Buttons()
        {
            // Assert: 씬에 버튼이 존재
            Button[] buttons = Object.FindObjectsByType<Button>(FindObjectsSortMode.None);
            Assert.IsTrue(buttons.Length > 0, "씬에 최소 1개 이상의 버튼이 존재해야 합니다");
            Debug.Log($"[테스트] 씬에서 {buttons.Length}개의 버튼을 찾았습니다");
        }

        /// <summary>
        /// 버튼 클릭 이벤트가 정상적으로 동작하는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator Button_Click_Triggers_Event()
        {
            // Arrange: 씬에서 버튼 찾기
            Button[] buttons = Object.FindObjectsByType<Button>(FindObjectsSortMode.None);

            if (buttons.Length == 0)
            {
                Assert.Inconclusive("씬에 버튼이 없습니다");
                yield break;
            }

            Button firstButton = buttons[0];
            Debug.Log($"[테스트] {firstButton.name} 버튼을 클릭합니다");

            // Act: 버튼 클릭 (시각적으로 확인 가능)
            firstButton.onClick.Invoke();

            // 시각적 확인을 위한 대기
            yield return new WaitForSeconds(0.5f);

            // Assert: 버튼이 존재하고 클릭 가능
            Assert.IsTrue(firstButton.interactable, "버튼이 상호작용 가능해야 합니다");
        }

        #endregion

        #region 예외 처리 테스트

        /// <summary>
        /// null 버튼으로 RegisterButton 호출 시 예외 없이 경고만 출력하는지 테스트
        /// </summary>
        [Test]
        public void RegisterButton_Handles_Null_Button_Gracefully()
        {
            // Arrange: Reflection으로 private 메서드 접근
            var registerMethod = typeof(MainMenuButtonHandler).GetMethod("RegisterButton",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act & Assert: null 버튼으로 호출 시 예외가 발생하지 않아야 함
            Assert.DoesNotThrow(() =>
            {
                // RegisterButton(null, someCallback)
                registerMethod?.Invoke(handler, new object[] { null, (UnityEngine.Events.UnityAction)handler.OnHamburgerMenuClicked });
            });
        }

        /// <summary>
        /// null 버튼으로 UnregisterButton 호출 시 예외 없이 정상 처리되는지 테스트
        /// </summary>
        [Test]
        public void UnregisterButton_Handles_Null_Button_Gracefully()
        {
            // Arrange: Reflection으로 private 메서드 접근
            var unregisterMethod = typeof(MainMenuButtonHandler).GetMethod("UnregisterButton",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act & Assert: null 버튼으로 호출 시 예외가 발생하지 않아야 함
            Assert.DoesNotThrow(() =>
            {
                unregisterMethod?.Invoke(handler, new object[] { null, (UnityEngine.Events.UnityAction)handler.OnHamburgerMenuClicked });
            });
        }

        #endregion

        #region UI 통합 테스트

        /// <summary>
        /// 실제 씬의 버튼을 클릭하여 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator Scene_Button_Click_Invokes_Handler()
        {
            // Arrange: 씬에서 버튼 찾기 (Reflection으로 private 필드 접근)
            var hamburgerField = typeof(MainMenuButtonHandler).GetField("hamburgerMenuBtn",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Button hamburgerButton = hamburgerField?.GetValue(handler) as Button;

            if (hamburgerButton == null)
            {
                Assert.Inconclusive("햄버거 메뉴 버튼이 씬에서 연결되지 않았습니다");
                yield break;
            }

            Debug.Log("[테스트] 햄버거 메뉴 버튼 클릭 (시각적으로 확인 가능)");

            // Act: 버튼 클릭 이벤트 발생
            LogAssert.Expect(LogType.Log, "[MainMenu] 햄버거 메뉴 버튼 클릭");
            hamburgerButton.onClick.Invoke();

            // 시각적 확인을 위한 대기
            yield return new WaitForSeconds(0.3f);

            // Assert: LogAssert가 자동으로 검증
        }

        /// <summary>
        /// 여러 버튼을 연속으로 클릭해도 정상 동작하는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator Multiple_Button_Clicks_Work_Sequentially()
        {
            // Act & Assert: 각 버튼 핸들러를 순차적으로 호출
            LogAssert.Expect(LogType.Log, "[MainMenu] 햄버거 메뉴 버튼 클릭");
            handler.OnHamburgerMenuClicked();
            yield return new WaitForSeconds(0.3f);

            LogAssert.Expect(LogType.Log, "[MainMenu] 설정 버튼 클릭");
            handler.OnSettingClicked();
            yield return new WaitForSeconds(0.3f);

            LogAssert.Expect(LogType.Log, "[MainMenu] 상점 버튼 클릭");
            handler.OnShopClicked();
            yield return new WaitForSeconds(0.3f);

            LogAssert.Expect(LogType.Log, "[MainMenu] 캐릭터 버튼 클릭");
            handler.OnCharacterClicked();
            yield return new WaitForSeconds(0.3f);

            LogAssert.Expect(LogType.Log, "[MainMenu] 스킬 1 버튼 클릭");
            handler.OnSkill1Clicked();
            yield return new WaitForSeconds(0.3f);

            // 모든 로그가 정상 출력되면 성공
        }

        /// <summary>
        /// 모든 버튼 핸들러를 순차적으로 클릭하여 시각적으로 확인
        /// </summary>
        [UnityTest]
        public IEnumerator All_Buttons_Click_Sequence_Visual()
        {
            Debug.Log("[테스트] 모든 버튼 순차 클릭 시작 (시각적 확인용)");

            // 모든 핸들러 메서드를 순차적으로 호출
            yield return ClickAndWait(() => handler.OnHamburgerMenuClicked(), "햄버거 메뉴");
            yield return ClickAndWait(() => handler.OnSettingClicked(), "설정");
            yield return ClickAndWait(() => handler.OnUserInfoClicked(), "유저 정보");
            yield return ClickAndWait(() => handler.OnGuideQuestClicked(), "가이드 퀘스트");
            yield return ClickAndWait(() => handler.OnShopClicked(), "상점");
            yield return ClickAndWait(() => handler.OnRecruitmentClicked(), "모집");
            yield return ClickAndWait(() => handler.OnEventClicked(), "이벤트");
            yield return ClickAndWait(() => handler.OnCharacterClicked(), "캐릭터");
            yield return ClickAndWait(() => handler.OnSkillSettingClicked(), "스킬 설정");
            yield return ClickAndWait(() => handler.OnSkill1Clicked(), "스킬 1");
            yield return ClickAndWait(() => handler.OnSkill2Clicked(), "스킬 2");
            yield return ClickAndWait(() => handler.OnSkill3Clicked(), "스킬 3");
            yield return ClickAndWait(() => handler.OnSkill4Clicked(), "스킬 4");
            yield return ClickAndWait(() => handler.OnSkill5Clicked(), "스킬 5");
            yield return ClickAndWait(() => handler.OnSkill6Clicked(), "스킬 6");
            yield return ClickAndWait(() => handler.OnWeaponClicked(), "무기");
            yield return ClickAndWait(() => handler.OnEquipClicked(), "장비");
            yield return ClickAndWait(() => handler.OnCoworkerClicked(), "협력자");
            yield return ClickAndWait(() => handler.OnHPPotionClicked(), "HP 포션");
            yield return ClickAndWait(() => handler.OnMPPotionClicked(), "MP 포션");
            yield return ClickAndWait(() => handler.OnPotionSettingClicked(), "포션 설정");
            yield return ClickAndWait(() => handler.OnControllClicked(), "컨트롤");
            yield return ClickAndWait(() => handler.OnChapterClicked(), "챕터");
            yield return ClickAndWait(() => handler.OnMonsterSpawnClicked(), "몬스터 스폰");
            yield return ClickAndWait(() => handler.OnSpawnSettingClicked(), "스폰 설정");

            Debug.Log("[테스트] 모든 버튼 순차 클릭 완료!");
        }

        /// <summary>
        /// 버튼 클릭 후 대기 헬퍼 메서드
        /// </summary>
        private IEnumerator ClickAndWait(System.Action clickAction, string buttonName)
        {
            Debug.Log($"[테스트] {buttonName} 버튼 클릭");
            clickAction.Invoke();
            yield return new WaitForSeconds(0.3f);
        }

        #endregion
    }
}
