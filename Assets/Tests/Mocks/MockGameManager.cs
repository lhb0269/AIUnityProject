using System;
using UnityEngine;
using MobileGame.Managers;

namespace MobileGame.Tests.Mocks
{
    /// <summary>
    /// 테스트용 GameManager Mock 클래스
    /// IGameManager 인터페이스를 구현하여 테스트 격리 제공
    /// </summary>
    public class MockGameManager : IGameManager
    {
        // 현재 게임 상태
        private GameState currentState = GameState.Menu;

        // 게임 상태 변경 이벤트
        public event Action<GameState> OnGameStateChanged;

        // 상태 변경 이력 추적
        public int StateChangeCount { get; private set; } = 0;
        public GameState LastState { get; private set; } = GameState.Menu;

        public void SetGameState(GameState newState)
        {
            if (currentState != newState)
            {
                currentState = newState;
                LastState = newState;
                StateChangeCount++;
                OnGameStateChanged?.Invoke(newState);
                Debug.Log($"[MockGameManager] SetGameState: {newState}");
            }
        }

        public void PauseGame()
        {
            SetGameState(GameState.Paused);
            Debug.Log("[MockGameManager] PauseGame");
        }

        public void ResumeGame()
        {
            SetGameState(GameState.Playing);
            Debug.Log("[MockGameManager] ResumeGame");
        }

        public void QuitGame()
        {
            Debug.Log("[MockGameManager] QuitGame");
        }

        /// <summary>
        /// 테스트 초기화 - 모든 기록 삭제
        /// </summary>
        public void Reset()
        {
            currentState = GameState.Menu;
            StateChangeCount = 0;
            LastState = GameState.Menu;
            OnGameStateChanged = null;
        }
    }
}
