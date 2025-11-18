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
    /// HamburgerMenuPopup의 12개 버튼 기능 테스트 클래스
    /// Unity Test Framework를 사용한 UI 반응성 및 기능 검증
    /// 실제 씬(SampleScene)을 로드하고 실제 버튼 클릭 이벤트를 시뮬레이션
    /// </summary>
    public class HamburgerMenuPopupTests
    {
        private static bool sceneLoaded = false;
        private EventSystem eventSystem;

        #region Setup & Teardown

        /// <summary>
        /// 각 테스트 실행 전 초기화
        /// SampleScene을 로드하고 실제 UI 환경에서 테스트
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

            // EventSystem 찾기
            eventSystem = Object.FindFirstObjectByType<EventSystem>();
            if (eventSystem == null)
            {
                Assert.Fail("EventSystem을 SampleScene에서 찾을 수 없습니다.");
            }

            // UIManager 확인
            if (UIManager.Instance == null)
            {
                Assert.Fail("UIManager.Instance를 찾을 수 없습니다.");
            }

            // 모든 팝업 닫기
            UIManager.Instance.CloseAllActivePopups();
            yield return null;
        }

        /// <summary>
        /// 각 테스트 실행 후 정리
        /// 씬의 객체는 그대로 유지
        /// </summary>
        [UnityTearDown]
        public IEnumerator Teardown()
        {
            // 모든 팝업 정리
            if (UIManager.Instance != null)
            {
                UIManager.Instance.CloseAllActivePopups();
                yield return null; // 팝업이 완전히 파괴될 때까지 대기
                yield return null; // 추가 프레임 대기로 완전한 정리 보장
            }

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
        /// Reflection을 사용하여 MainMenuButtonHandler의 private 버튼 필드 가져오기
        /// </summary>
        private Button GetMainMenuButtonField(string fieldName)
        {
            var handler = Object.FindFirstObjectByType<MainMenuButtonHandler>();
            if (handler == null) return null;

            var field = typeof(MainMenuButtonHandler).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(handler) as Button;
        }

        /// <summary>
        /// Reflection을 사용하여 HamburgerMenuPopup의 private 버튼 필드 가져오기
        /// </summary>
        private Button GetPopupButtonField(HamburgerMenuPopup popup, string fieldName)
        {
            var field = typeof(HamburgerMenuPopup).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(popup) as Button;
        }

        /// <summary>
        /// 햄버거 메뉴 팝업을 실제로 여는 헬퍼 메서드
        /// </summary>
        private IEnumerator OpenHamburgerPopup()
        {
            // MainMenuButtonHandler의 햄버거 메뉴 버튼 찾기
            Button hamburgerBtn = GetMainMenuButtonField("hamburgerMenuBtn");

            if (hamburgerBtn == null)
            {
                Assert.Fail("햄버거 메뉴 버튼을 찾을 수 없습니다.");
                yield break;
            }

            // 햄버거 메뉴 버튼 클릭
            yield return SimulateButtonClick(hamburgerBtn, "햄버거 메뉴");

            // 팝업이 열릴 때까지 대기
            yield return null;
        }

        #endregion

        #region 팝업 열기 테스트

        /// <summary>
        /// 햄버거 메뉴 팝업이 정상적으로 열리는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator HamburgerMenuPopup_Opens_Successfully()
        {
            // 햄버거 메뉴 팝업 열기
            yield return OpenHamburgerPopup();

            // 팝업이 열렸는지 확인
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 열려야 합니다");

            // 12개 버튼이 모두 할당되었는지 확인
            Assert.IsNotNull(GetPopupButtonField(popup, "missionBtn"), "미션 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetPopupButtonField(popup, "passBtn"), "패스 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetPopupButtonField(popup, "mailboxBtn"), "우편함 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetPopupButtonField(popup, "costumeBtn"), "코스튬 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetPopupButtonField(popup, "heroPowerBtn"), "용사의 힘 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetPopupButtonField(popup, "equipSlotEnhanceBtn"), "장비 슬롯 강화 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetPopupButtonField(popup, "relicBtn"), "유물 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetPopupButtonField(popup, "friendBtn"), "친구 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetPopupButtonField(popup, "rankingBtn"), "랭킹 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetPopupButtonField(popup, "guildBtn"), "길드 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetPopupButtonField(popup, "growthDungeonBtn"), "성장 던전 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetPopupButtonField(popup, "worldBossBtn"), "월드 보스 버튼이 할당되어야 합니다");

            // 팝업 닫기
            UIManager.Instance.CloseAllActivePopups();
            yield return null;
            yield return null; // 팝업 파괴 완료 대기
        }

        #endregion

        #region 버튼 클릭 테스트 (12개)

        /// <summary>
        /// 미션 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator MissionButton_Click_Triggers_Handler()
        {
            // 햄버거 메뉴 팝업 열기
            yield return OpenHamburgerPopup();

            // 팝업 찾기
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 열려야 합니다");

            // 미션 버튼 찾기
            Button missionBtn = GetPopupButtonField(popup, "missionBtn");
            if (missionBtn == null)
            {
                Assert.Inconclusive("미션 버튼이 할당되지 않았습니다");
                yield break;
            }

            // 로그 예상 설정
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 미션 버튼 클릭");

            // 버튼 클릭
            yield return SimulateButtonClick(missionBtn, "미션");

            // 팝업 닫기
            UIManager.Instance.CloseAllActivePopups();
            yield return null;
            yield return null; // 팝업 파괴 완료 대기
        }

        /// <summary>
        /// 패스 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator PassButton_Click_Triggers_Handler()
        {
            // 햄버거 메뉴 팝업 열기
            yield return OpenHamburgerPopup();

            // 팝업 찾기
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 열려야 합니다");

            // 패스 버튼 찾기
            Button passBtn = GetPopupButtonField(popup, "passBtn");
            if (passBtn == null)
            {
                Assert.Inconclusive("패스 버튼이 할당되지 않았습니다");
                yield break;
            }

            // 로그 예상 설정
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 패스 버튼 클릭");

            // 버튼 클릭
            yield return SimulateButtonClick(passBtn, "패스");

            // 팝업 닫기
            UIManager.Instance.CloseAllActivePopups();
            yield return null;
            yield return null; // 팝업 파괴 완료 대기
        }

        /// <summary>
        /// 우편함 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator MailboxButton_Click_Triggers_Handler()
        {
            // 햄버거 메뉴 팝업 열기
            yield return OpenHamburgerPopup();

            // 팝업 찾기
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 열려야 합니다");

            // 우편함 버튼 찾기
            Button mailboxBtn = GetPopupButtonField(popup, "mailboxBtn");
            if (mailboxBtn == null)
            {
                Assert.Inconclusive("우편함 버튼이 할당되지 않았습니다");
                yield break;
            }

            // 로그 예상 설정
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 우편함 버튼 클릭");

            // 버튼 클릭
            yield return SimulateButtonClick(mailboxBtn, "우편함");

            // 팝업 닫기
            UIManager.Instance.CloseAllActivePopups();
            yield return null;
            yield return null; // 팝업 파괴 완료 대기
        }

        /// <summary>
        /// 코스튬 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator CostumeButton_Click_Triggers_Handler()
        {
            // 햄버거 메뉴 팝업 열기
            yield return OpenHamburgerPopup();

            // 팝업 찾기
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 열려야 합니다");

            // 코스튬 버튼 찾기
            Button costumeBtn = GetPopupButtonField(popup, "costumeBtn");
            if (costumeBtn == null)
            {
                Assert.Inconclusive("코스튬 버튼이 할당되지 않았습니다");
                yield break;
            }

            // 로그 예상 설정
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 코스튬 버튼 클릭");

            // 버튼 클릭
            yield return SimulateButtonClick(costumeBtn, "코스튬");

            // 팝업 닫기
            UIManager.Instance.CloseAllActivePopups();
            yield return null;
            yield return null; // 팝업 파괴 완료 대기
        }

        /// <summary>
        /// 용사의 힘 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator HeroPowerButton_Click_Triggers_Handler()
        {
            // 햄버거 메뉴 팝업 열기
            yield return OpenHamburgerPopup();

            // 팝업 찾기
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 열려야 합니다");

            // 용사의 힘 버튼 찾기
            Button heroPowerBtn = GetPopupButtonField(popup, "heroPowerBtn");
            if (heroPowerBtn == null)
            {
                Assert.Inconclusive("용사의 힘 버튼이 할당되지 않았습니다");
                yield break;
            }

            // 로그 예상 설정
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 용사의 힘 버튼 클릭");

            // 버튼 클릭
            yield return SimulateButtonClick(heroPowerBtn, "용사의 힘");

            // 팝업 닫기
            UIManager.Instance.CloseAllActivePopups();
            yield return null;
            yield return null; // 팝업 파괴 완료 대기
        }

        /// <summary>
        /// 장비 슬롯 강화 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator EquipSlotEnhanceButton_Click_Triggers_Handler()
        {
            // 햄버거 메뉴 팝업 열기
            yield return OpenHamburgerPopup();

            // 팝업 찾기
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 열려야 합니다");

            // 장비 슬롯 강화 버튼 찾기
            Button equipSlotEnhanceBtn = GetPopupButtonField(popup, "equipSlotEnhanceBtn");
            if (equipSlotEnhanceBtn == null)
            {
                Assert.Inconclusive("장비 슬롯 강화 버튼이 할당되지 않았습니다");
                yield break;
            }

            // 로그 예상 설정
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 장비 슬롯 강화 버튼 클릭");

            // 버튼 클릭
            yield return SimulateButtonClick(equipSlotEnhanceBtn, "장비 슬롯 강화");

            // 팝업 닫기
            UIManager.Instance.CloseAllActivePopups();
            yield return null;
            yield return null; // 팝업 파괴 완료 대기
        }

        /// <summary>
        /// 유물 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator RelicButton_Click_Triggers_Handler()
        {
            // 햄버거 메뉴 팝업 열기
            yield return OpenHamburgerPopup();

            // 팝업 찾기
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 열려야 합니다");

            // 유물 버튼 찾기
            Button relicBtn = GetPopupButtonField(popup, "relicBtn");
            if (relicBtn == null)
            {
                Assert.Inconclusive("유물 버튼이 할당되지 않았습니다");
                yield break;
            }

            // 로그 예상 설정
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 유물 버튼 클릭");

            // 버튼 클릭
            yield return SimulateButtonClick(relicBtn, "유물");

            // 팝업 닫기
            UIManager.Instance.CloseAllActivePopups();
            yield return null;
            yield return null; // 팝업 파괴 완료 대기
        }

        /// <summary>
        /// 친구 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator FriendButton_Click_Triggers_Handler()
        {
            // 햄버거 메뉴 팝업 열기
            yield return OpenHamburgerPopup();

            // 팝업 찾기
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 열려야 합니다");

            // 친구 버튼 찾기
            Button friendBtn = GetPopupButtonField(popup, "friendBtn");
            if (friendBtn == null)
            {
                Assert.Inconclusive("친구 버튼이 할당되지 않았습니다");
                yield break;
            }

            // 로그 예상 설정
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 친구 버튼 클릭");

            // 버튼 클릭
            yield return SimulateButtonClick(friendBtn, "친구");

            // 팝업 닫기
            UIManager.Instance.CloseAllActivePopups();
            yield return null;
            yield return null; // 팝업 파괴 완료 대기
        }

        /// <summary>
        /// 랭킹 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator RankingButton_Click_Triggers_Handler()
        {
            // 햄버거 메뉴 팝업 열기
            yield return OpenHamburgerPopup();

            // 팝업 찾기
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 열려야 합니다");

            // 랭킹 버튼 찾기
            Button rankingBtn = GetPopupButtonField(popup, "rankingBtn");
            if (rankingBtn == null)
            {
                Assert.Inconclusive("랭킹 버튼이 할당되지 않았습니다");
                yield break;
            }

            // 로그 예상 설정
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 랭킹 버튼 클릭");

            // 버튼 클릭
            yield return SimulateButtonClick(rankingBtn, "랭킹");

            // 팝업 닫기
            UIManager.Instance.CloseAllActivePopups();
            yield return null;
            yield return null; // 팝업 파괴 완료 대기
        }

        /// <summary>
        /// 길드 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator GuildButton_Click_Triggers_Handler()
        {
            // 햄버거 메뉴 팝업 열기
            yield return OpenHamburgerPopup();

            // 팝업 찾기
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 열려야 합니다");

            // 길드 버튼 찾기
            Button guildBtn = GetPopupButtonField(popup, "guildBtn");
            if (guildBtn == null)
            {
                Assert.Inconclusive("길드 버튼이 할당되지 않았습니다");
                yield break;
            }

            // 로그 예상 설정
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 길드 버튼 클릭");

            // 버튼 클릭
            yield return SimulateButtonClick(guildBtn, "길드");

            // 팝업 닫기
            UIManager.Instance.CloseAllActivePopups();
            yield return null;
            yield return null; // 팝업 파괴 완료 대기
        }

        /// <summary>
        /// 성장 던전 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator GrowthDungeonButton_Click_Triggers_Handler()
        {
            // 햄버거 메뉴 팝업 열기
            yield return OpenHamburgerPopup();

            // 팝업 찾기
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 열려야 합니다");

            // 성장 던전 버튼 찾기
            Button growthDungeonBtn = GetPopupButtonField(popup, "growthDungeonBtn");
            if (growthDungeonBtn == null)
            {
                Assert.Inconclusive("성장 던전 버튼이 할당되지 않았습니다");
                yield break;
            }

            // 로그 예상 설정
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 성장 던전 버튼 클릭");

            // 버튼 클릭
            yield return SimulateButtonClick(growthDungeonBtn, "성장 던전");

            // 팝업 닫기
            UIManager.Instance.CloseAllActivePopups();
            yield return null;
            yield return null; // 팝업 파괴 완료 대기
        }

        /// <summary>
        /// 월드 보스 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator WorldBossButton_Click_Triggers_Handler()
        {
            // 햄버거 메뉴 팝업 열기
            yield return OpenHamburgerPopup();

            // 팝업 찾기
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 열려야 합니다");

            // 월드 보스 버튼 찾기
            Button worldBossBtn = GetPopupButtonField(popup, "worldBossBtn");
            if (worldBossBtn == null)
            {
                Assert.Inconclusive("월드 보스 버튼이 할당되지 않았습니다");
                yield break;
            }

            // 로그 예상 설정
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 월드 보스 버튼 클릭");

            // 버튼 클릭
            yield return SimulateButtonClick(worldBossBtn, "월드 보스");

            // 팝업 닫기
            UIManager.Instance.CloseAllActivePopups();
            yield return null;
            yield return null; // 팝업 파괴 완료 대기
        }

        #endregion

        #region 통합 테스트

        /// <summary>
        /// 여러 버튼을 연속으로 클릭해도 정상 동작하는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator Multiple_Button_Clicks_Work_Correctly()
        {
            // 햄버거 메뉴 팝업 열기
            yield return OpenHamburgerPopup();

            // 팝업 찾기
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 열려야 합니다");

            Button missionBtn = GetPopupButtonField(popup, "missionBtn");
            Button friendBtn = GetPopupButtonField(popup, "friendBtn");
            Button guildBtn = GetPopupButtonField(popup, "guildBtn");

            if (missionBtn == null || friendBtn == null || guildBtn == null)
            {
                Assert.Inconclusive("테스트에 필요한 버튼이 모두 할당되지 않았습니다");
                yield break;
            }

            Debug.Log("[테스트] 연속 클릭 테스트 시작");

            // 미션 버튼 클릭
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 미션 버튼 클릭");
            yield return SimulateButtonClick(missionBtn, "미션");

            // 친구 버튼 클릭
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 친구 버튼 클릭");
            yield return SimulateButtonClick(friendBtn, "친구");

            // 길드 버튼 클릭
            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 길드 버튼 클릭");
            yield return SimulateButtonClick(guildBtn, "길드");

            Debug.Log("[테스트] 연속 클릭 테스트 완료");

            // 팝업 닫기
            UIManager.Instance.CloseAllActivePopups();
            yield return null;
            yield return null; // 팝업 파괴 완료 대기
        }

        /// <summary>
        /// 모든 버튼이 순차적으로 클릭 가능한지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator All_Buttons_Can_Be_Clicked_Sequentially()
        {
            // 햄버거 메뉴 팝업 열기
            yield return OpenHamburgerPopup();

            // 팝업 찾기
            HamburgerMenuPopup popup = Object.FindFirstObjectByType<HamburgerMenuPopup>();
            Assert.IsNotNull(popup, "햄버거 메뉴 팝업이 열려야 합니다");

            // 12개 버튼 정보 배열
            var buttonInfos = new[]
            {
                new { FieldName = "missionBtn", LogMessage = "[HamburgerMenu] 미션 버튼 클릭", DisplayName = "미션" },
                new { FieldName = "passBtn", LogMessage = "[HamburgerMenu] 패스 버튼 클릭", DisplayName = "패스" },
                new { FieldName = "mailboxBtn", LogMessage = "[HamburgerMenu] 우편함 버튼 클릭", DisplayName = "우편함" },
                new { FieldName = "costumeBtn", LogMessage = "[HamburgerMenu] 코스튬 버튼 클릭", DisplayName = "코스튬" },
                new { FieldName = "heroPowerBtn", LogMessage = "[HamburgerMenu] 용사의 힘 버튼 클릭", DisplayName = "용사의 힘" },
                new { FieldName = "equipSlotEnhanceBtn", LogMessage = "[HamburgerMenu] 장비 슬롯 강화 버튼 클릭", DisplayName = "장비 슬롯 강화" },
                new { FieldName = "relicBtn", LogMessage = "[HamburgerMenu] 유물 버튼 클릭", DisplayName = "유물" },
                new { FieldName = "friendBtn", LogMessage = "[HamburgerMenu] 친구 버튼 클릭", DisplayName = "친구" },
                new { FieldName = "rankingBtn", LogMessage = "[HamburgerMenu] 랭킹 버튼 클릭", DisplayName = "랭킹" },
                new { FieldName = "guildBtn", LogMessage = "[HamburgerMenu] 길드 버튼 클릭", DisplayName = "길드" },
                new { FieldName = "growthDungeonBtn", LogMessage = "[HamburgerMenu] 성장 던전 버튼 클릭", DisplayName = "성장 던전" },
                new { FieldName = "worldBossBtn", LogMessage = "[HamburgerMenu] 월드 보스 버튼 클릭", DisplayName = "월드 보스" }
            };

            Debug.Log("[테스트] 전체 버튼 순차 클릭 테스트 시작");

            foreach (var info in buttonInfos)
            {
                Button button = GetPopupButtonField(popup, info.FieldName);
                if (button != null)
                {
                    LogAssert.Expect(LogType.Log, info.LogMessage);
                    yield return SimulateButtonClick(button, info.DisplayName);
                }
                else
                {
                    Debug.LogWarning($"[테스트] {info.FieldName} 버튼을 찾을 수 없습니다");
                }
            }

            Debug.Log("[테스트] 전체 버튼 순차 클릭 테스트 완료");

            // 팝업 닫기
            UIManager.Instance.CloseAllActivePopups();
            yield return null;
            yield return null; // 팝업 파괴 완료 대기
        }

        #endregion
    }
}
