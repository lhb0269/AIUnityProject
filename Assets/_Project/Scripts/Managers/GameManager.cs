using UnityEngine;
using System;

namespace MobileGame.Managers
{
    /// <summary>
    /// 게임의 전체적인 상태와 흐름을 관리하는 메인 매니저
    /// 싱글톤 패턴을 사용하여 어디서든 접근 가능
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("게임 상태")]
        [SerializeField] private GameState currentState = GameState.Menu;

        [Header("모바일 설정")]
        [SerializeField] private int targetFrameRate = 60;
        [SerializeField] private bool allowScreenDimming = false;

        public event Action<GameState> OnGameStateChanged;

        private void Awake()
        {
            // 싱글톤 패턴 구현
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeMobileSettings();
        }

        private void Start()
        {
            SetGameState(GameState.Menu);
        }

        /// <summary>
        /// 모바일 환경에 맞는 설정 초기화
        /// </summary>
        private void InitializeMobileSettings()
        {
            // 타겟 프레임 레이트 설정 (모바일 배터리 최적화)
            Application.targetFrameRate = targetFrameRate;

            // 화면 절전 모드 설정
            Screen.sleepTimeout = allowScreenDimming
                ? SleepTimeout.SystemSetting
                : SleepTimeout.NeverSleep;

            // 모바일 백그라운드 실행 설정
            Application.runInBackground = false;

            Debug.Log($"[GameManager] 모바일 설정 초기화 완료 - FPS: {targetFrameRate}");
        }

        /// <summary>
        /// 게임 상태 변경
        /// </summary>
        public void SetGameState(GameState newState)
        {
            if (currentState == newState) return;

            GameState previousState = currentState;
            currentState = newState;

            Debug.Log($"[GameManager] 게임 상태 변경: {previousState} -> {currentState}");
            OnGameStateChanged?.Invoke(currentState);

            HandleStateChange(newState);
        }

        /// <summary>
        /// 상태 변경에 따른 처리
        /// </summary>
        private void HandleStateChange(GameState state)
        {
            switch (state)
            {
                case GameState.Menu:
                    Time.timeScale = 1f;
                    break;

                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;

                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;

                case GameState.GameOver:
                    Time.timeScale = 0f;
                    break;
            }
        }

        public GameState GetCurrentState() => currentState;

        /// <summary>
        /// 게임 일시정지
        /// </summary>
        public void PauseGame()
        {
            if (currentState == GameState.Playing)
            {
                SetGameState(GameState.Paused);
            }
        }

        /// <summary>
        /// 게임 재개
        /// </summary>
        public void ResumeGame()
        {
            if (currentState == GameState.Paused)
            {
                SetGameState(GameState.Playing);
            }
        }

        /// <summary>
        /// 게임 종료
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("[GameManager] 게임 종료");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            // 모바일에서 앱이 백그라운드로 갈 때 자동 일시정지
            if (pauseStatus && currentState == GameState.Playing)
            {
                PauseGame();
                Debug.Log("[GameManager] 앱 백그라운드 전환 - 게임 일시정지");
            }
        }
    }

    /// <summary>
    /// 게임 상태 열거형
    /// </summary>
    public enum GameState
    {
        Menu,       // 메뉴 화면
        Playing,    // 게임 플레이 중
        Paused,     // 일시정지
        GameOver    // 게임 종료
    }
}
