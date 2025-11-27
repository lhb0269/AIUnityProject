using System;

namespace MobileGame.Interfaces
{
    /// <summary>
    /// 씬 로딩 관리 인터페이스
    /// 비동기 씬 전환 기능을 정의
    /// </summary>
    public interface ISceneLoader
    {
        // 이벤트
        event Action<string> OnSceneLoadStarted;
        event Action<string, float> OnSceneLoadProgress;
        event Action<string> OnSceneLoadCompleted;

        // 상태
        bool IsLoading { get; }

        // 씬 로딩
        void LoadScene(string sceneName);
        void LoadScene(int sceneIndex);
        void ReloadCurrentScene();
        void LoadSceneAdditive(string sceneName);
        void UnloadScene(string sceneName);
    }
}
