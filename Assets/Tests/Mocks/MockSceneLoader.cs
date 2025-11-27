using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobileGame.Managers;

namespace MobileGame.Tests.Mocks
{
    /// <summary>
    /// 테스트용 SceneLoader Mock 클래스
    /// ISceneLoader 인터페이스를 구현하여 테스트 격리 제공
    /// </summary>
    public class MockSceneLoader : ISceneLoader
    {
        // 씬 로딩 이벤트
        public event Action<string> OnSceneLoadStarted;
        public event Action<float> OnSceneLoadProgress;
        public event Action<string> OnSceneLoadCompleted;

        // 로딩 상태
        private bool isLoading = false;

        public bool IsLoading => isLoading;

        // 로드된 씬 이력
        public List<string> LoadedScenes { get; private set; } = new List<string>();
        public List<string> UnloadedScenes { get; private set; } = new List<string>();

        public void LoadScene(string sceneName)
        {
            LoadedScenes.Add(sceneName);
            isLoading = true;

            OnSceneLoadStarted?.Invoke(sceneName);
            OnSceneLoadProgress?.Invoke(0.5f);
            OnSceneLoadProgress?.Invoke(1.0f);
            OnSceneLoadCompleted?.Invoke(sceneName);

            isLoading = false;

            Debug.Log($"[MockSceneLoader] LoadScene (string): {sceneName}");
        }

        public void LoadScene(int sceneIndex)
        {
            string sceneName = $"Scene_{sceneIndex}";
            LoadedScenes.Add(sceneName);
            isLoading = true;

            OnSceneLoadStarted?.Invoke(sceneName);
            OnSceneLoadProgress?.Invoke(0.5f);
            OnSceneLoadProgress?.Invoke(1.0f);
            OnSceneLoadCompleted?.Invoke(sceneName);

            isLoading = false;

            Debug.Log($"[MockSceneLoader] LoadScene (int): {sceneIndex}");
        }

        public void ReloadCurrentScene()
        {
            string currentScene = "CurrentScene";
            LoadedScenes.Add(currentScene + "_Reload");
            Debug.Log("[MockSceneLoader] ReloadCurrentScene");
        }

        public void LoadSceneAdditive(string sceneName)
        {
            LoadedScenes.Add(sceneName + "_Additive");
            Debug.Log($"[MockSceneLoader] LoadSceneAdditive: {sceneName}");
        }

        public void UnloadScene(string sceneName)
        {
            UnloadedScenes.Add(sceneName);
            Debug.Log($"[MockSceneLoader] UnloadScene: {sceneName}");
        }

        /// <summary>
        /// 테스트 초기화 - 모든 기록 삭제
        /// </summary>
        public void Reset()
        {
            LoadedScenes.Clear();
            UnloadedScenes.Clear();
            isLoading = false;
            OnSceneLoadStarted = null;
            OnSceneLoadProgress = null;
            OnSceneLoadCompleted = null;
        }
    }
}
