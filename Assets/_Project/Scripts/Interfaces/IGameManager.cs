using System;

namespace MobileGame.Interfaces
{
    /// <summary>
    /// 게임 상태 관리 인터페이스
    /// </summary>
    public interface IGameManager
    {
        // 이벤트
        event Action<GameState> OnGameStateChanged;

        // 상태 관리
        void SetGameState(GameState newState);
        void PauseGame();
        void ResumeGame();
        void QuitGame();
    }

    /// <summary>
    /// 게임 상태 열거형
    /// </summary>
    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        GameOver
    }
}
