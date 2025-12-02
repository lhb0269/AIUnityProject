using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace MobileGame.UI
{
    /// <summary>
    /// 버튼 ID 기반 바인딩 시스템
    /// Inspector에서 버튼 ID와 Button 컴포넌트를 매핑
    /// 테스트에서 리플렉션 없이 버튼을 찾을 수 있도록 합니다.
    /// </summary>
    public class ButtonBinder : MonoBehaviour
    {
        [Serializable]
        public class ButtonEntry
        {
            [Tooltip("ButtonID 상수 값 (예: ButtonID.HamburgerMenu)")]
            public string buttonID;

            [Tooltip("해당 ID에 매핑될 Button 컴포넌트")]
            public Button button;
        }

        [SerializeField] private List<ButtonEntry> buttonEntries = new List<ButtonEntry>();

        private Dictionary<string, Button> buttonMap = new Dictionary<string, Button>();
        private bool isInitialized = false;

        private void Awake()
        {
            InitializeButtonMap();
        }

        /// <summary>
        /// 버튼 맵 초기화
        /// </summary>
        private void InitializeButtonMap()
        {
            if (isInitialized) return;

            buttonMap.Clear();
            int validCount = 0;
            int invalidCount = 0;

            foreach (var entry in buttonEntries)
            {
                if (string.IsNullOrEmpty(entry.buttonID))
                {
                    Debug.LogWarning($"[ButtonBinder] 빈 buttonID가 발견되었습니다: {gameObject.name}");
                    invalidCount++;
                    continue;
                }

                if (entry.button == null)
                {
                    Debug.LogWarning($"[ButtonBinder] Button이 null입니다: {entry.buttonID} on {gameObject.name}");
                    invalidCount++;
                    continue;
                }

                if (buttonMap.ContainsKey(entry.buttonID))
                {
                    Debug.LogError($"[ButtonBinder] 중복된 buttonID: {entry.buttonID} on {gameObject.name}");
                    invalidCount++;
                    continue;
                }

                buttonMap[entry.buttonID] = entry.button;
                validCount++;
            }

            isInitialized = true;
            Debug.Log($"[ButtonBinder] 초기화 완료: {validCount}개 버튼 등록, {invalidCount}개 경고/오류 ({gameObject.name})");
        }

        /// <summary>
        /// 버튼 ID로 Button 컴포넌트 가져오기
        /// </summary>
        /// <param name="buttonID">버튼 ID (ButtonID 상수 값)</param>
        /// <returns>Button 컴포넌트, 없으면 null</returns>
        public Button GetButton(string buttonID)
        {
            if (!isInitialized)
            {
                InitializeButtonMap();
            }

            if (buttonMap.TryGetValue(buttonID, out var button))
            {
                return button;
            }

            Debug.LogWarning($"[ButtonBinder] 버튼을 찾을 수 없습니다: {buttonID} on {gameObject.name}");
            return null;
        }

        /// <summary>
        /// 버튼 ID로 Button 컴포넌트 가져오기 (안전한 버전)
        /// </summary>
        /// <param name="buttonID">버튼 ID</param>
        /// <param name="button">출력 Button 컴포넌트</param>
        /// <returns>버튼을 찾았으면 true, 아니면 false</returns>
        public bool TryGetButton(string buttonID, out Button button)
        {
            if (!isInitialized)
            {
                InitializeButtonMap();
            }

            return buttonMap.TryGetValue(buttonID, out button);
        }

        /// <summary>
        /// 등록된 버튼이 있는지 확인
        /// </summary>
        /// <param name="buttonID">버튼 ID</param>
        /// <returns>등록되어 있으면 true</returns>
        public bool HasButton(string buttonID)
        {
            if (!isInitialized)
            {
                InitializeButtonMap();
            }

            return buttonMap.ContainsKey(buttonID);
        }

        /// <summary>
        /// 등록된 모든 버튼 ID 가져오기
        /// </summary>
        /// <returns>버튼 ID 리스트</returns>
        public List<string> GetAllButtonIDs()
        {
            if (!isInitialized)
            {
                InitializeButtonMap();
            }

            return new List<string>(buttonMap.Keys);
        }

        /// <summary>
        /// 등록된 버튼 수
        /// </summary>
        public int ButtonCount
        {
            get
            {
                if (!isInitialized)
                {
                    InitializeButtonMap();
                }
                return buttonMap.Count;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// 에디터에서 버튼 맵 검증 (Inspector에서 호출 가능)
        /// </summary>
        [ContextMenu("Validate Button Map")]
        private void ValidateButtonMap()
        {
            isInitialized = false;
            InitializeButtonMap();
        }
#endif
    }
}
