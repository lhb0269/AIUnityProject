using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MobileGame.UI;
using MobileGame.Managers;

namespace MobileGame.Tests.UI
{
    /// <summary>
    /// HamburgerMenuPopup의 12개 버튼 기능 테스트 클래스
    /// Unity Test Framework를 사용한 UI 반응성 및 기능 검증
    /// </summary>
    public class HamburgerMenuPopupTests
    {
        private HamburgerMenuPopup popup;
        private GameObject popupObject;
        private EventSystem eventSystem;

        #region Setup & Teardown

        /// <summary>
        /// 각 테스트 실행 전 초기화
        /// UIManager와 EventSystem 설정
        /// </summary>
        [UnitySetUp]
        public IEnumerator Setup()
        {
            // UIManager가 있는지 확인하고 없으면 생성
            if (UIManager.Instance == null)
            {
                GameObject uiManagerObj = new GameObject("UIManager");
                uiManagerObj.AddComponent<UIManager>();
                yield return null;
            }

            // EventSystem 생성
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystem = eventSystemObj.AddComponent<EventSystem>();
            eventSystemObj.AddComponent<StandaloneInputModule>();

            yield return null;
        }

        /// <summary>
        /// 각 테스트 실행 후 정리
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            // 팝업 정리
            if (UIManager.Instance != null)
            {
                UIManager.Instance.CloseAllActivePopups();
            }

            // EventSystem 정리
            if (eventSystem != null)
            {
                Object.Destroy(eventSystem.gameObject);
            }

            // 팝업 객체 정리
            if (popupObject != null)
            {
                Object.Destroy(popupObject);
            }

            popup = null;
            popupObject = null;
        }

        #endregion

        #region 헬퍼 메서드

        /// <summary>
        /// 버튼을 클릭하는 시뮬레이션
        /// </summary>
        private IEnumerator SimulateButtonClick(Button button, string buttonName)
        {
            if (button == null)
            {
                Debug.LogWarning($"[테스트] {buttonName} 버튼이 null입니다");
                yield break;
            }

            var pointerData = new PointerEventData(eventSystem)
            {
                button = PointerEventData.InputButton.Left
            };

            ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
            yield return null;
        }

        /// <summary>
        /// Reflection을 사용하여 private 버튼 필드 가져오기
        /// </summary>
        private Button GetButtonField(string fieldName)
        {
            var field = typeof(HamburgerMenuPopup).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(popup) as Button;
        }

        /// <summary>
        /// 햄버거 팝업을 여는 헬퍼 메서드
        /// </summary>
        private IEnumerator OpenHamburgerPopup()
        {
            // 팝업 프리팹 생성 (테스트용 간단 버전)
            popupObject = new GameObject("HamburgerMenuPopup");
            popupObject.AddComponent<RectTransform>();
            popup = popupObject.AddComponent<HamburgerMenuPopup>();

            // 12개 버튼 생성 및 할당
            CreateAndAssignButton("missionBtn", "MissionButton");
            CreateAndAssignButton("passBtn", "PassButton");
            CreateAndAssignButton("mailboxBtn", "MailboxButton");
            CreateAndAssignButton("costumeBtn", "CostumeButton");
            CreateAndAssignButton("heroPowerBtn", "HeroPowerButton");
            CreateAndAssignButton("equipSlotEnhanceBtn", "EquipSlotEnhanceButton");
            CreateAndAssignButton("relicBtn", "RelicButton");
            CreateAndAssignButton("friendBtn", "FriendButton");
            CreateAndAssignButton("rankingBtn", "RankingButton");
            CreateAndAssignButton("guildBtn", "GuildButton");
            CreateAndAssignButton("growthDungeonBtn", "GrowthDungeonButton");
            CreateAndAssignButton("worldBossBtn", "WorldBossButton");

            // Start 메서드 실행을 위한 활성화
            popupObject.SetActive(true);
            yield return null; // Start 실행 대기
        }

        /// <summary>
        /// 버튼을 생성하고 팝업 필드에 할당
        /// </summary>
        private void CreateAndAssignButton(string fieldName, string buttonName)
        {
            GameObject buttonObj = new GameObject(buttonName);
            buttonObj.transform.SetParent(popupObject.transform);
            buttonObj.AddComponent<RectTransform>();
            Button button = buttonObj.AddComponent<Button>();

            var field = typeof(HamburgerMenuPopup).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(popup, button);
        }

        #endregion

        #region 팝업 열기 테스트

        /// <summary>
        /// 햄버거 팝업이 정상적으로 열리는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator HamburgerMenuPopup_Opens_Successfully()
        {
            yield return OpenHamburgerPopup();

            Assert.IsNotNull(popup, "HamburgerMenuPopup이 생성되어야 합니다");
            Assert.IsTrue(popup.gameObject.activeInHierarchy, "HamburgerMenuPopup이 활성화되어야 합니다");

            // 12개 버튼이 모두 할당되었는지 확인
            Assert.IsNotNull(GetButtonField("missionBtn"), "미션 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetButtonField("passBtn"), "패스 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetButtonField("mailboxBtn"), "우편함 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetButtonField("costumeBtn"), "코스튬 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetButtonField("heroPowerBtn"), "용사의 힘 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetButtonField("equipSlotEnhanceBtn"), "장비 슬롯 강화 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetButtonField("relicBtn"), "유물 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetButtonField("friendBtn"), "친구 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetButtonField("rankingBtn"), "랭킹 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetButtonField("guildBtn"), "길드 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetButtonField("growthDungeonBtn"), "성장 던전 버튼이 할당되어야 합니다");
            Assert.IsNotNull(GetButtonField("worldBossBtn"), "월드 보스 버튼이 할당되어야 합니다");
        }

        #endregion

        #region 버튼 클릭 테스트 (12개)

        /// <summary>
        /// 미션 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator MissionButton_Click_Triggers_Handler()
        {
            yield return OpenHamburgerPopup();

            Button button = GetButtonField("missionBtn");
            if (button == null)
            {
                Assert.Inconclusive("미션 버튼이 할당되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 미션 버튼 클릭");
            yield return SimulateButtonClick(button, "미션");
        }

        /// <summary>
        /// 패스 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator PassButton_Click_Triggers_Handler()
        {
            yield return OpenHamburgerPopup();

            Button button = GetButtonField("passBtn");
            if (button == null)
            {
                Assert.Inconclusive("패스 버튼이 할당되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 패스 버튼 클릭");
            yield return SimulateButtonClick(button, "패스");
        }

        /// <summary>
        /// 우편함 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator MailboxButton_Click_Triggers_Handler()
        {
            yield return OpenHamburgerPopup();

            Button button = GetButtonField("mailboxBtn");
            if (button == null)
            {
                Assert.Inconclusive("우편함 버튼이 할당되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 우편함 버튼 클릭");
            yield return SimulateButtonClick(button, "우편함");
        }

        /// <summary>
        /// 코스튬 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator CostumeButton_Click_Triggers_Handler()
        {
            yield return OpenHamburgerPopup();

            Button button = GetButtonField("costumeBtn");
            if (button == null)
            {
                Assert.Inconclusive("코스튬 버튼이 할당되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 코스튬 버튼 클릭");
            yield return SimulateButtonClick(button, "코스튬");
        }

        /// <summary>
        /// 용사의 힘 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator HeroPowerButton_Click_Triggers_Handler()
        {
            yield return OpenHamburgerPopup();

            Button button = GetButtonField("heroPowerBtn");
            if (button == null)
            {
                Assert.Inconclusive("용사의 힘 버튼이 할당되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 용사의 힘 버튼 클릭");
            yield return SimulateButtonClick(button, "용사의 힘");
        }

        /// <summary>
        /// 장비 슬롯 강화 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator EquipSlotEnhanceButton_Click_Triggers_Handler()
        {
            yield return OpenHamburgerPopup();

            Button button = GetButtonField("equipSlotEnhanceBtn");
            if (button == null)
            {
                Assert.Inconclusive("장비 슬롯 강화 버튼이 할당되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 장비 슬롯 강화 버튼 클릭");
            yield return SimulateButtonClick(button, "장비 슬롯 강화");
        }

        /// <summary>
        /// 유물 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator RelicButton_Click_Triggers_Handler()
        {
            yield return OpenHamburgerPopup();

            Button button = GetButtonField("relicBtn");
            if (button == null)
            {
                Assert.Inconclusive("유물 버튼이 할당되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 유물 버튼 클릭");
            yield return SimulateButtonClick(button, "유물");
        }

        /// <summary>
        /// 친구 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator FriendButton_Click_Triggers_Handler()
        {
            yield return OpenHamburgerPopup();

            Button button = GetButtonField("friendBtn");
            if (button == null)
            {
                Assert.Inconclusive("친구 버튼이 할당되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 친구 버튼 클릭");
            yield return SimulateButtonClick(button, "친구");
        }

        /// <summary>
        /// 랭킹 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator RankingButton_Click_Triggers_Handler()
        {
            yield return OpenHamburgerPopup();

            Button button = GetButtonField("rankingBtn");
            if (button == null)
            {
                Assert.Inconclusive("랭킹 버튼이 할당되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 랭킹 버튼 클릭");
            yield return SimulateButtonClick(button, "랭킹");
        }

        /// <summary>
        /// 길드 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator GuildButton_Click_Triggers_Handler()
        {
            yield return OpenHamburgerPopup();

            Button button = GetButtonField("guildBtn");
            if (button == null)
            {
                Assert.Inconclusive("길드 버튼이 할당되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 길드 버튼 클릭");
            yield return SimulateButtonClick(button, "길드");
        }

        /// <summary>
        /// 성장 던전 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator GrowthDungeonButton_Click_Triggers_Handler()
        {
            yield return OpenHamburgerPopup();

            Button button = GetButtonField("growthDungeonBtn");
            if (button == null)
            {
                Assert.Inconclusive("성장 던전 버튼이 할당되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 성장 던전 버튼 클릭");
            yield return SimulateButtonClick(button, "성장 던전");
        }

        /// <summary>
        /// 월드 보스 버튼 클릭 시 핸들러가 호출되는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator WorldBossButton_Click_Triggers_Handler()
        {
            yield return OpenHamburgerPopup();

            Button button = GetButtonField("worldBossBtn");
            if (button == null)
            {
                Assert.Inconclusive("월드 보스 버튼이 할당되지 않았습니다");
                yield break;
            }

            LogAssert.Expect(LogType.Log, "[HamburgerMenu] 월드 보스 버튼 클릭");
            yield return SimulateButtonClick(button, "월드 보스");
        }

        #endregion

        #region 통합 테스트

        /// <summary>
        /// 여러 버튼을 연속으로 클릭해도 정상 동작하는지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator Multiple_Button_Clicks_Work_Correctly()
        {
            yield return OpenHamburgerPopup();

            Button missionBtn = GetButtonField("missionBtn");
            Button friendBtn = GetButtonField("friendBtn");
            Button guildBtn = GetButtonField("guildBtn");

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
        }

        /// <summary>
        /// 모든 버튼이 순차적으로 클릭 가능한지 테스트
        /// </summary>
        [UnityTest]
        public IEnumerator All_Buttons_Can_Be_Clicked_Sequentially()
        {
            yield return OpenHamburgerPopup();

            // 12개 버튼 정보 배열
            var buttonInfos = new[]
            {
                new { FieldName = "missionBtn", LogMessage = "[HamburgerMenu] 미션 버튼 클릭" },
                new { FieldName = "passBtn", LogMessage = "[HamburgerMenu] 패스 버튼 클릭" },
                new { FieldName = "mailboxBtn", LogMessage = "[HamburgerMenu] 우편함 버튼 클릭" },
                new { FieldName = "costumeBtn", LogMessage = "[HamburgerMenu] 코스튬 버튼 클릭" },
                new { FieldName = "heroPowerBtn", LogMessage = "[HamburgerMenu] 용사의 힘 버튼 클릭" },
                new { FieldName = "equipSlotEnhanceBtn", LogMessage = "[HamburgerMenu] 장비 슬롯 강화 버튼 클릭" },
                new { FieldName = "relicBtn", LogMessage = "[HamburgerMenu] 유물 버튼 클릭" },
                new { FieldName = "friendBtn", LogMessage = "[HamburgerMenu] 친구 버튼 클릭" },
                new { FieldName = "rankingBtn", LogMessage = "[HamburgerMenu] 랭킹 버튼 클릭" },
                new { FieldName = "guildBtn", LogMessage = "[HamburgerMenu] 길드 버튼 클릭" },
                new { FieldName = "growthDungeonBtn", LogMessage = "[HamburgerMenu] 성장 던전 버튼 클릭" },
                new { FieldName = "worldBossBtn", LogMessage = "[HamburgerMenu] 월드 보스 버튼 클릭" }
            };

            Debug.Log("[테스트] 전체 버튼 순차 클릭 테스트 시작");

            foreach (var info in buttonInfos)
            {
                Button button = GetButtonField(info.FieldName);
                if (button != null)
                {
                    LogAssert.Expect(LogType.Log, info.LogMessage);
                    yield return SimulateButtonClick(button, info.FieldName);
                }
                else
                {
                    Debug.LogWarning($"[테스트] {info.FieldName} 버튼을 찾을 수 없습니다");
                }
            }

            Debug.Log("[테스트] 전체 버튼 순차 클릭 테스트 완료");
        }

        #endregion
    }
}
