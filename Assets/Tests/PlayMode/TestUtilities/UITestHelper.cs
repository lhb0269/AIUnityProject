using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

namespace MobileGame.Tests.Utilities
{
    /// <summary>
    /// UI 테스트를 위한 헬퍼 유틸리티 클래스
    /// 버튼 생성, 컴포넌트 설정 등 반복되는 테스트 설정을 제공
    /// </summary>
    public static class UITestHelper
    {
        #region 버튼 생성 헬퍼

        /// <summary>
        /// 테스트용 버튼 GameObject 생성
        /// </summary>
        /// <param name="buttonName">버튼 이름</param>
        /// <returns>생성된 버튼 컴포넌트</returns>
        public static Button CreateButton(string buttonName = "TestButton")
        {
            GameObject buttonObj = new GameObject(buttonName);
            Button button = buttonObj.AddComponent<Button>();

            // Image 컴포넌트 추가 (버튼에 필수)
            Image image = buttonObj.AddComponent<Image>();

            return button;
        }

        /// <summary>
        /// 여러 개의 테스트용 버튼 생성
        /// </summary>
        /// <param name="count">생성할 버튼 개수</param>
        /// <param name="namePrefix">버튼 이름 접두사</param>
        /// <returns>생성된 버튼 배열</returns>
        public static Button[] CreateButtons(int count, string namePrefix = "TestButton")
        {
            Button[] buttons = new Button[count];

            for (int i = 0; i < count; i++)
            {
                buttons[i] = CreateButton($"{namePrefix}_{i}");
            }

            return buttons;
        }

        #endregion

        #region Canvas 생성 헬퍼

        /// <summary>
        /// 테스트용 Canvas 생성
        /// </summary>
        /// <param name="canvasName">Canvas 이름</param>
        /// <returns>생성된 Canvas 컴포넌트</returns>
        public static Canvas CreateCanvas(string canvasName = "TestCanvas")
        {
            GameObject canvasObj = new GameObject(canvasName);
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // CanvasScaler 추가
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            // GraphicRaycaster 추가
            canvasObj.AddComponent<GraphicRaycaster>();

            return canvas;
        }

        #endregion

        #region Reflection 헬퍼

        /// <summary>
        /// 오브젝트의 private 필드 값을 설정하는 헬퍼
        /// </summary>
        /// <typeparam name="T">대상 타입</typeparam>
        /// <param name="obj">대상 오브젝트</param>
        /// <param name="fieldName">필드 이름</param>
        /// <param name="value">설정할 값</param>
        public static void SetPrivateField<T>(object obj, string fieldName, object value)
        {
            FieldInfo field = typeof(T).GetField(fieldName,
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null)
            {
                field.SetValue(obj, value);
            }
            else
            {
                Debug.LogWarning($"[UITestHelper] 필드를 찾을 수 없습니다: {fieldName}");
            }
        }

        /// <summary>
        /// 오브젝트의 private 필드 값을 가져오는 헬퍼
        /// </summary>
        /// <typeparam name="T">대상 타입</typeparam>
        /// <param name="obj">대상 오브젝트</param>
        /// <param name="fieldName">필드 이름</param>
        /// <returns>필드 값</returns>
        public static object GetPrivateField<T>(object obj, string fieldName)
        {
            FieldInfo field = typeof(T).GetField(fieldName,
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null)
            {
                return field.GetValue(obj);
            }
            else
            {
                Debug.LogWarning($"[UITestHelper] 필드를 찾을 수 없습니다: {fieldName}");
                return null;
            }
        }

        /// <summary>
        /// private 메서드를 호출하는 헬퍼
        /// </summary>
        /// <typeparam name="T">대상 타입</typeparam>
        /// <param name="obj">대상 오브젝트</param>
        /// <param name="methodName">메서드 이름</param>
        /// <param name="parameters">메서드 파라미터</param>
        /// <returns>메서드 반환 값</returns>
        public static object InvokePrivateMethod<T>(object obj, string methodName, params object[] parameters)
        {
            MethodInfo method = typeof(T).GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (method != null)
            {
                return method.Invoke(obj, parameters);
            }
            else
            {
                Debug.LogWarning($"[UITestHelper] 메서드를 찾을 수 없습니다: {methodName}");
                return null;
            }
        }

        #endregion

        #region GameObject 정리 헬퍼

        /// <summary>
        /// 여러 GameObject를 한 번에 제거하는 헬퍼
        /// </summary>
        /// <param name="gameObjects">제거할 GameObject 배열</param>
        public static void DestroyGameObjects(params GameObject[] gameObjects)
        {
            foreach (GameObject obj in gameObjects)
            {
                if (obj != null)
                {
                    Object.DestroyImmediate(obj);
                }
            }
        }

        /// <summary>
        /// 버튼 배열을 정리하는 헬퍼
        /// </summary>
        /// <param name="buttons">정리할 버튼 배열</param>
        public static void DestroyButtons(params Button[] buttons)
        {
            foreach (Button button in buttons)
            {
                if (button != null && button.gameObject != null)
                {
                    Object.DestroyImmediate(button.gameObject);
                }
            }
        }

        #endregion

        #region 버튼 이벤트 검증 헬퍼

        /// <summary>
        /// 버튼에 리스너가 등록되어 있는지 확인
        /// </summary>
        /// <param name="button">확인할 버튼</param>
        /// <returns>리스너 등록 여부</returns>
        public static bool HasListeners(Button button)
        {
            if (button == null || button.onClick == null)
            {
                return false;
            }

            // Persistent 리스너 개수 확인
            int persistentCount = button.onClick.GetPersistentEventCount();

            // Runtime 리스너는 직접 확인 불가능하므로
            // Reflection을 통해 내부 리스너 확인
            var onClickField = typeof(Button.ButtonClickedEvent)
                .GetField("m_Calls", BindingFlags.NonPublic | BindingFlags.Instance);

            if (onClickField != null)
            {
                var calls = onClickField.GetValue(button.onClick);
                if (calls != null)
                {
                    var runtimeCallsField = calls.GetType()
                        .GetField("m_RuntimeCalls", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (runtimeCallsField != null)
                    {
                        var runtimeCalls = runtimeCallsField.GetValue(calls) as System.Collections.IList;
                        return runtimeCalls != null && runtimeCalls.Count > 0;
                    }
                }
            }

            return persistentCount > 0;
        }

        /// <summary>
        /// 버튼 클릭을 시뮬레이션
        /// </summary>
        /// <param name="button">클릭할 버튼</param>
        public static void SimulateButtonClick(Button button)
        {
            if (button != null && button.onClick != null)
            {
                button.onClick.Invoke();
            }
        }

        #endregion

        #region 로그 검증 헬퍼

        /// <summary>
        /// 특정 로그가 출력되었는지 확인하는 플래그
        /// </summary>
        private static bool lastLogReceived = false;
        private static string lastLogMessage = string.Empty;

        /// <summary>
        /// 로그 수신 리스너 등록
        /// </summary>
        public static void StartLogCapture()
        {
            Application.logMessageReceived += OnLogReceived;
            lastLogReceived = false;
            lastLogMessage = string.Empty;
        }

        /// <summary>
        /// 로그 수신 리스너 해제
        /// </summary>
        public static void StopLogCapture()
        {
            Application.logMessageReceived -= OnLogReceived;
        }

        /// <summary>
        /// 마지막 로그 메시지 가져오기
        /// </summary>
        public static string GetLastLogMessage()
        {
            return lastLogMessage;
        }

        /// <summary>
        /// 로그가 수신되었는지 확인
        /// </summary>
        public static bool WasLogReceived()
        {
            return lastLogReceived;
        }

        private static void OnLogReceived(string logString, string stackTrace, LogType type)
        {
            lastLogReceived = true;
            lastLogMessage = logString;
        }

        #endregion
    }
}
