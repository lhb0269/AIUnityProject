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
    /// MainMenuButtonHandler의 기능 테스트 클래스
    /// Unity Test Framework를 사용한 UI 반응성 및 기능 검증
    /// 실제 씬(SampleScene)을 로드하고 실제 버튼 클릭 이벤트를 시뮬레이션
    /// </summary>
    public class MainMenuButtonHandlerTests
    {
        private MainMenuButtonHandler handler;
        private static bool sceneLoaded = false;
        private EventSystem eventSystem;

        #region Setup & Teardown

        /// <summary>
        /// 각 테스트 실행 전 초기화
        /// SampleScene을 로드하고 MainMenuButtonHandler를 찾아서 사용
        /// </summary>
        [UnitySetUp]
        public IEnumerator Setup()
        {
            // 씬이 아직 로드되지 않았으면 로드
            if (!sceneLoaded || SceneManager.GetActiveScene().name != "SampleScene")
            {
                SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
                // 씬 로드가 완료될 때까지 대기
                yield return null;
                yield return null; // 추가 프레임 대기 (Awake, Start 실행 보장)
                sceneLoaded = true;
            }

            // 씬 로드 후 객체 찾기
            handler = Object.FindFirstObjectByType<MainMenuButtonHandler>();

            if (handler == null)
            {
                Assert.Fail("MainMenuButtonHandler를 SampleScene에서 찾을 수 없습니다. SampleScene에 MainMenuButtonHandler 컴포넌트를 추가하세요.");
            }

            // EventSystem 찾기
            eventSystem = Object.FindFirstObjectByType<EventSystem>();
            if (eventSystem == null)
            {
                Assert.Fail("EventSystem을 SampleScene에서 찾을 수 없습니다. Canvas에 EventSystem이 필요합니다.");
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
            eventSystem = null;
        }

        #endregion

        #region 헬퍼 메서드

        /// <summary>
        /// 버튼을 실제로 클릭하는 시뮬레이션 (시각적 피드백 포함)
        /// </summary>
        private IEnumerator SimulateButtonClick(Button button, string buttonName)
        {
            if (button == null)
            {
                Debug.LogWarning($"[테스트] {buttonName} 버튼이 null입니다");
                yield break;
            }

            Debug.Log($"[테스트] {buttonName} 버튼 클릭 시작");

            // PointerEventData 생성
            var pointerData = new PointerEventData(eventSystem)
            {
                button = PointerEventData.InputButton.Left
            };

            // 버튼 눌림 효과 (PointerDown)
            ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerDownHandler);
            yield return new WaitForSeconds(0.1f);

            // 버튼 떼기 효과 (PointerUp)
            ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerUpHandler);

            // 클릭 이벤트 발생
            ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerClickHandler);

            Debug.Log($"[테스트] {buttonName} 버튼 클릭 완료");

            // 다음 클릭 전 대기
            yield return new WaitForSeconds(0.2f);
        }

        /// <summary>
        /// Reflection을 사용하여 private 버튼 필드 가져오기
        /// </summary>
        private Button GetButtonField(string fieldName)
        {
            var field = typeof(MainMenuButtonHandler).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(handler) as Button;
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

        /// <summary>
        /// EventSystem이 씬에 존재하는지 테스트
        /// </summary>
        [Test]
        public void EventSystem_Exists_In_Scene()
        {
            Assert.IsNotNull(eventSystem, "EventSystem이 씬에 존재해야 합니다");
        }

        #endregion

        #region 개별 버튼 클릭 테스트 (25개)

        /// <summary>
        /// 햄버거 메뉴 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator HamburgerMenuButton_Click_Visual()
        {
            Button button = GetButtonField("hamburgerMenuBtn");
            if (button == null)
            {
                Assert.Inconclusive("햄버거 메뉴 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 햄버거 메뉴 버튼 클릭");
            yield return SimulateButtonClick(button, "햄버거 메뉴");
        }

        /// <summary>
        /// 설정 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator SettingButton_Click_Visual()
        {
            Button button = GetButtonField("settingBtn");
            if (button == null)
            {
                Assert.Inconclusive("설정 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 설정 버튼 클릭");
            yield return SimulateButtonClick(button, "설정");
        }

        /// <summary>
        /// 유저 정보 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator UserInfoButton_Click_Visual()
        {
            Button button = GetButtonField("userInfoBtn");
            if (button == null)
            {
                Assert.Inconclusive("유저 정보 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 유저 정보 버튼 클릭");
            yield return SimulateButtonClick(button, "유저 정보");
        }

        /// <summary>
        /// 가이드 퀘스트 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator GuideQuestButton_Click_Visual()
        {
            Button button = GetButtonField("guideQuestBtn");
            if (button == null)
            {
                Assert.Inconclusive("가이드 퀘스트 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 가이드 퀘스트 버튼 클릭");
            yield return SimulateButtonClick(button, "가이드 퀘스트");
        }

        /// <summary>
        /// 상점 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator ShopButton_Click_Visual()
        {
            Button button = GetButtonField("shopBtn");
            if (button == null)
            {
                Assert.Inconclusive("상점 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 상점 버튼 클릭");
            yield return SimulateButtonClick(button, "상점");
        }

        /// <summary>
        /// 모집 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator RecruitmentButton_Click_Visual()
        {
            Button button = GetButtonField("recruitmentBtn");
            if (button == null)
            {
                Assert.Inconclusive("모집 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 모집 버튼 클릭");
            yield return SimulateButtonClick(button, "모집");
        }

        /// <summary>
        /// 이벤트 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator EventButton_Click_Visual()
        {
            Button button = GetButtonField("eventBtn");
            if (button == null)
            {
                Assert.Inconclusive("이벤트 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 이벤트 버튼 클릭");
            yield return SimulateButtonClick(button, "이벤트");
        }

        /// <summary>
        /// 캐릭터 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator CharacterButton_Click_Visual()
        {
            Button button = GetButtonField("characterButton");
            if (button == null)
            {
                Assert.Inconclusive("캐릭터 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 캐릭터 버튼 클릭");
            yield return SimulateButtonClick(button, "캐릭터");
        }

        /// <summary>
        /// 스킬 설정 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator SkillSettingButton_Click_Visual()
        {
            Button button = GetButtonField("SkillSettingBtn");
            if (button == null)
            {
                Assert.Inconclusive("스킬 설정 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 스킬 설정 버튼 클릭");
            yield return SimulateButtonClick(button, "스킬 설정");
        }

        /// <summary>
        /// 스킬 1 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator Skill1Button_Click_Visual()
        {
            Button button = GetButtonField("skill1Btn");
            if (button == null)
            {
                Assert.Inconclusive("스킬 1 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 스킬 1 버튼 클릭");
            yield return SimulateButtonClick(button, "스킬 1");
        }

        /// <summary>
        /// 스킬 2 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator Skill2Button_Click_Visual()
        {
            Button button = GetButtonField("skill2Btn");
            if (button == null)
            {
                Assert.Inconclusive("스킬 2 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 스킬 2 버튼 클릭");
            yield return SimulateButtonClick(button, "스킬 2");
        }

        /// <summary>
        /// 스킬 3 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator Skill3Button_Click_Visual()
        {
            Button button = GetButtonField("skill3Btn");
            if (button == null)
            {
                Assert.Inconclusive("스킬 3 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 스킬 3 버튼 클릭");
            yield return SimulateButtonClick(button, "스킬 3");
        }

        /// <summary>
        /// 스킬 4 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator Skill4Button_Click_Visual()
        {
            Button button = GetButtonField("skill4Btn");
            if (button == null)
            {
                Assert.Inconclusive("스킬 4 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 스킬 4 버튼 클릭");
            yield return SimulateButtonClick(button, "스킬 4");
        }

        /// <summary>
        /// 스킬 5 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator Skill5Button_Click_Visual()
        {
            Button button = GetButtonField("skill5Btn");
            if (button == null)
            {
                Assert.Inconclusive("스킬 5 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 스킬 5 버튼 클릭");
            yield return SimulateButtonClick(button, "스킬 5");
        }

        /// <summary>
        /// 스킬 6 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator Skill6Button_Click_Visual()
        {
            Button button = GetButtonField("skill6Btn");
            if (button == null)
            {
                Assert.Inconclusive("스킬 6 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 스킬 6 버튼 클릭");
            yield return SimulateButtonClick(button, "스킬 6");
        }

        /// <summary>
        /// 무기 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator WeaponButton_Click_Visual()
        {
            Button button = GetButtonField("weaponButton");
            if (button == null)
            {
                Assert.Inconclusive("무기 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 무기 버튼 클릭");
            yield return SimulateButtonClick(button, "무기");
        }

        /// <summary>
        /// 장비 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator EquipButton_Click_Visual()
        {
            Button button = GetButtonField("equipButton");
            if (button == null)
            {
                Assert.Inconclusive("장비 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 장비 버튼 클릭");
            yield return SimulateButtonClick(button, "장비");
        }

        /// <summary>
        /// 협력자 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator CoworkerButton_Click_Visual()
        {
            Button button = GetButtonField("coworkerButton");
            if (button == null)
            {
                Assert.Inconclusive("협력자 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 협력자 버튼 클릭");
            yield return SimulateButtonClick(button, "협력자");
        }

        /// <summary>
        /// HP 포션 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator HPPotionButton_Click_Visual()
        {
            Button button = GetButtonField("hpPotionBtn");
            if (button == null)
            {
                Assert.Inconclusive("HP 포션 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] HP 포션 버튼 클릭");
            yield return SimulateButtonClick(button, "HP 포션");
        }

        /// <summary>
        /// MP 포션 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator MPPotionButton_Click_Visual()
        {
            Button button = GetButtonField("mpPotionBtn");
            if (button == null)
            {
                Assert.Inconclusive("MP 포션 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] MP 포션 버튼 클릭");
            yield return SimulateButtonClick(button, "MP 포션");
        }

        /// <summary>
        /// 포션 설정 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator PotionSettingButton_Click_Visual()
        {
            Button button = GetButtonField("potionSettingBtn");
            if (button == null)
            {
                Assert.Inconclusive("포션 설정 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 포션 설정 버튼 클릭");
            yield return SimulateButtonClick(button, "포션 설정");
        }

        /// <summary>
        /// 컨트롤 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator ControllButton_Click_Visual()
        {
            Button button = GetButtonField("controllBtn");
            if (button == null)
            {
                Assert.Inconclusive("컨트롤 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 컨트롤 버튼 클릭");
            yield return SimulateButtonClick(button, "컨트롤");
        }

        /// <summary>
        /// 챕터 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator ChapterButton_Click_Visual()
        {
            Button button = GetButtonField("chapterBtn");
            if (button == null)
            {
                Assert.Inconclusive("챕터 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 챕터 버튼 클릭");
            yield return SimulateButtonClick(button, "챕터");
        }

        /// <summary>
        /// 몬스터 스폰 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator MonsterSpawnButton_Click_Visual()
        {
            Button button = GetButtonField("monsterSpawnBtn");
            if (button == null)
            {
                Assert.Inconclusive("몬스터 스폰 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 몬스터 스폰 버튼 클릭");
            yield return SimulateButtonClick(button, "몬스터 스폰");
        }

        /// <summary>
        /// 스폰 설정 버튼 실제 클릭 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator SpawnSettingButton_Click_Visual()
        {
            Button button = GetButtonField("spawnSettingBtn");
            if (button == null)
            {
                Assert.Inconclusive("스폰 설정 버튼이 연결되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[MainMenu] 스폰 설정 버튼 클릭");
            yield return SimulateButtonClick(button, "스폰 설정");
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

        #endregion

        #region 통합 테스트

        /// <summary>
        /// 여러 버튼을 빠르게 연속 클릭해도 정상 동작하는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator Rapid_Button_Clicks_Work_Correctly()
        {
            Button hamburgerBtn = GetButtonField("hamburgerMenuBtn");
            Button settingBtn = GetButtonField("settingBtn");
            Button shopBtn = GetButtonField("shopBtn");

            if (hamburgerBtn == null || settingBtn == null || shopBtn == null)
            {
                Assert.Inconclusive("테스트에 필요한 버튼이 모두 연결되지 않았습니다");
                yield break;
            }

            Debug.Log("[테스트] 빠른 연속 클릭 테스트 시작");

            // 짧은 간격으로 여러 버튼 클릭
            LogAssert.Expect(LogType.Log, "[MainMenu] 햄버거 메뉴 버튼 클릭");
            yield return SimulateButtonClick(hamburgerBtn, "햄버거 메뉴");

            LogAssert.Expect(LogType.Log, "[MainMenu] 설정 버튼 클릭");
            yield return SimulateButtonClick(settingBtn, "설정");

            LogAssert.Expect(LogType.Log, "[MainMenu] 상점 버튼 클릭");
            yield return SimulateButtonClick(shopBtn, "상점");

            Debug.Log("[테스트] 빠른 연속 클릭 테스트 완료");
        }

        #endregion
    }
}
