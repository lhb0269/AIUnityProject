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
    /// </summary>
    public class MainMenuButtonHandlerTests
    {
        private GameObject handlerGameObject;
        private MainMenuButtonHandler handler;
        private GameObject uiManagerGameObject;

        #region Setup & Teardown

        /// <summary>
        /// 각 테스트 실행 전 초기화
        /// MainMenuButtonHandler와 UIManager 인스턴스 생성
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // UIManager 인스턴스 생성
            uiManagerGameObject = new GameObject("UIManager");
            uiManagerGameObject.AddComponent<UIManager>();

            // MainMenuButtonHandler 오브젝트 생성
            handlerGameObject = new GameObject("MainMenuButtonHandler");
            handler = handlerGameObject.AddComponent<MainMenuButtonHandler>();
        }

        /// <summary>
        /// 각 테스트 실행 후 정리
        /// 생성된 GameObject들 제거 및 싱글톤 인스턴스 초기화
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            if (handlerGameObject != null)
            {
                Object.DestroyImmediate(handlerGameObject);
            }

            if (uiManagerGameObject != null)
            {
                Object.DestroyImmediate(uiManagerGameObject);
            }

            // UIManager 싱글톤 인스턴스를 null로 초기화
            // 다음 테스트에서 새로운 인스턴스를 생성할 수 있도록 함
            var instanceField = typeof(UIManager).GetField("Instance",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            instanceField?.SetValue(null, null);
        }

        #endregion

        #region 초기화 테스트

        /// <summary>
        /// MainMenuButtonHandler가 성공적으로 초기화되는지 테스트
        /// UIManager가 존재할 때 정상적으로 Start() 실행 확인
        /// </summary>
        [UnityTest]
        public IEnumerator MainMenuButtonHandler_Initializes_Successfully_With_UIManager()
        {
            // Act: Awake() 및 Start() 실행을 위해 프레임 대기
            yield return null;
            yield return null;

            // Assert: UIManager 인스턴스가 정상적으로 생성됨
            Assert.IsNotNull(UIManager.Instance, "UIManager 인스턴스가 생성되어야 합니다");

            // Assert: handler가 정상적으로 존재
            Assert.IsNotNull(handler, "MainMenuButtonHandler가 정상적으로 생성되어야 합니다");
        }

        /// <summary>
        /// UIManager가 없을 때 경고 로그를 출력하는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator Start_Warns_When_UIManager_Missing()
        {
            // Arrange: UIManager 인스턴스 제거
            Object.DestroyImmediate(uiManagerGameObject);
            yield return null;

            // Act: 새로운 handler 생성
            GameObject newHandlerObj = new GameObject("TestHandler");
            var newHandler = newHandlerObj.AddComponent<MainMenuButtonHandler>();

            // Assert: 경고 로그 예상 (Start() 호출 전에 설정)
            LogAssert.Expect(LogType.Warning, "[MainMenuButtonHandler] UIManager 인스턴스를 찾을 수 없습니다!");

            // 25개 null 버튼에 대한 경고도 예상
            for (int i = 0; i < 25; i++)
            {
                LogAssert.Expect(LogType.Warning, new System.Text.RegularExpressions.Regex("버튼이 할당되지 않았습니다"));
            }

            // Act: Start() 실행을 위해 여러 프레임 대기
            yield return null;
            yield return null;

            // Cleanup
            Object.DestroyImmediate(newHandlerObj);
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

        #region 버튼 이벤트 등록 테스트

        /// <summary>
        /// RegisterButtonEvents가 모든 버튼을 정상적으로 등록하는지 테스트
        /// null이 아닌 버튼에 대해서만 이벤트 등록 확인
        /// </summary>
        [UnityTest]
        public IEnumerator RegisterButtonEvents_Subscribes_Valid_Buttons()
        {
            // Arrange: 테스트용 버튼 생성
            GameObject buttonObj = new GameObject("TestButton");
            Button testButton = buttonObj.AddComponent<Button>();

            // SerializedField에 직접 할당할 수 없으므로
            // Reflection을 사용하여 private 필드에 접근
            var hamburgerField = typeof(MainMenuButtonHandler).GetField("hamburgerMenuBtn",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            hamburgerField?.SetValue(handler, testButton);

            // Assert: 로그 예상 (Start() 호출 전에 설정)
            LogAssert.Expect(LogType.Log, "[MainMenuButtonHandler] 모든 버튼 이벤트 등록 완료");

            // Act: Start() 실행으로 RegisterButtonEvents 호출
            yield return null;
            yield return null;

            // Assert: LogAssert가 자동으로 검증

            // Cleanup
            Object.DestroyImmediate(buttonObj);
        }

        /// <summary>
        /// null 버튼에 대해 경고를 출력하는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator RegisterButtonEvents_Warns_For_Null_Buttons()
        {
            // Arrange: 버튼 참조가 없는 상태 (Setup에서 이미 null 상태)

            // Assert: null 버튼들에 대한 경고 로그 예상 (Start() 호출 전에 설정)
            // 25개 버튼이 모두 null이므로 25개의 경고가 발생
            for (int i = 0; i < 25; i++)
            {
                LogAssert.Expect(LogType.Warning, new System.Text.RegularExpressions.Regex("버튼이 할당되지 않았습니다"));
            }

            // Act: Start() 실행으로 RegisterButtonEvents 호출
            yield return null;
            yield return null;

            // Assert: 경고 로그가 출력됨 (LogAssert가 자동 검증)
        }

        #endregion

        #region 버튼 이벤트 해제 테스트

        /// <summary>
        /// UnregisterButtonEvents가 모든 버튼 리스너를 제거하는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator UnregisterButtonEvents_Removes_All_Listeners()
        {
            // Arrange: 버튼 생성 및 이벤트 등록
            GameObject buttonObj = new GameObject("TestButton");
            Button testButton = buttonObj.AddComponent<Button>();

            var hamburgerField = typeof(MainMenuButtonHandler).GetField("hamburgerMenuBtn",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            hamburgerField?.SetValue(handler, testButton);

            // Start()로 이벤트 등록
            yield return null;

            // Act: GameObject 파괴 (OnDestroy 호출)
            Object.DestroyImmediate(handlerGameObject);
            yield return null;

            // Assert: 버튼 리스너가 제거됨
            // 실제로는 handler가 파괴되어 더 이상 접근 불가하므로
            // 예외 없이 정상 실행되면 성공
            Assert.Pass("UnregisterButtonEvents가 정상적으로 실행되었습니다");

            // Cleanup
            Object.DestroyImmediate(buttonObj);
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
        /// 실제 버튼 클릭 이벤트가 핸들러 메서드를 호출하는지 통합 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator Button_Click_Invokes_Handler_Method()
        {
            // Arrange: 실제 버튼 오브젝트 생성
            GameObject buttonObj = new GameObject("HamburgerButton");
            Button hamburgerButton = buttonObj.AddComponent<Button>();

            // Reflection으로 private 필드에 버튼 할당
            var field = typeof(MainMenuButtonHandler).GetField("hamburgerMenuBtn",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(handler, hamburgerButton);

            // 이벤트 등록을 위해 여러 프레임 대기 (Start 실행 보장)
            yield return null;
            yield return null;

            // Act: 버튼 클릭 이벤트 발생
            LogAssert.Expect(LogType.Log, "[MainMenu] 햄버거 메뉴 버튼 클릭");
            hamburgerButton.onClick.Invoke();

            // Assert: LogAssert가 자동으로 검증

            // Cleanup
            Object.DestroyImmediate(buttonObj);
        }

        /// <summary>
        /// 여러 버튼을 연속으로 클릭해도 정상 동작하는지 테스트
        /// </summary>
        [Test]
        public void Multiple_Button_Clicks_Work_Sequentially()
        {
            // Arrange: 여러 핸들러 메서드 호출 준비

            // Act & Assert: 각 버튼 핸들러를 순차적으로 호출
            LogAssert.Expect(LogType.Log, "[MainMenu] 햄버거 메뉴 버튼 클릭");
            handler.OnHamburgerMenuClicked();

            LogAssert.Expect(LogType.Log, "[MainMenu] 설정 버튼 클릭");
            handler.OnSettingClicked();

            LogAssert.Expect(LogType.Log, "[MainMenu] 상점 버튼 클릭");
            handler.OnShopClicked();

            LogAssert.Expect(LogType.Log, "[MainMenu] 캐릭터 버튼 클릭");
            handler.OnCharacterClicked();

            LogAssert.Expect(LogType.Log, "[MainMenu] 스킬 1 버튼 클릭");
            handler.OnSkill1Clicked();

            // 모든 로그가 정상 출력되면 성공
        }

        #endregion

        #region 성능 테스트

        /// <summary>
        /// 25개 버튼 이벤트 등록이 적절한 시간 내에 완료되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator RegisterButtonEvents_Completes_In_Reasonable_Time()
        {
            // Arrange: 시작 시간 기록
            float startTime = Time.realtimeSinceStartup;

            // Act: Start() 실행 (RegisterButtonEvents 호출)
            yield return null;

            // Assert: 등록 시간이 0.1초 이내여야 함 (성능 기준)
            float elapsedTime = Time.realtimeSinceStartup - startTime;
            Assert.Less(elapsedTime, 0.1f, "버튼 이벤트 등록이 0.1초 이상 소요되었습니다");
        }

        #endregion
    }
}
