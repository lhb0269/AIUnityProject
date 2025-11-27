using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using MobileGame.Interfaces;

namespace MobileGame.Managers
{
    /// <summary>
    /// 씬 로딩과 전환을 관리하는 매니저
    /// 로딩 화면과 비동기 로딩을 지원
    /// DI를 통해 주입되어 사용됩니다.
    /// </summary>
    public class SceneLoader : MonoBehaviour, ISceneLoader
    {

        [Header("로딩 설정")]
        [SerializeField] private float minimumLoadingTime = 0.5f;

        public event Action<string> OnSceneLoadStarted;
        public event Action<string, float> OnSceneLoadProgress;
        public event Action<string> OnSceneLoadCompleted;

        public bool IsLoading { get; private set; }

        /// <summary>
        /// DI 컨테이너에서 호출하는 초기화 메서드
        /// </summary>
        public void Initialize()
        {
            Debug.Log("[SceneLoader] DI 초기화 완료");
        }

        private void Awake()
        {
            // 초기화 없음
        }

        /// <summary>
        /// 씬을 비동기로 로드
        /// </summary>
        public void LoadScene(string sceneName)
        {
            if (IsLoading)
            {
                Debug.LogWarning($"[SceneLoader] 이미 씬 로딩 중입니다: {sceneName}");
                return;
            }

            StartCoroutine(LoadSceneAsync(sceneName));
        }

        /// <summary>
        /// 씬 인덱스로 로드
        /// </summary>
        public void LoadScene(int sceneIndex)
        {
            if (sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogError($"[SceneLoader] 잘못된 씬 인덱스: {sceneIndex}");
                return;
            }

            LoadScene(SceneUtility.GetScenePathByBuildIndex(sceneIndex));
        }

        /// <summary>
        /// 현재 씬 다시 로드
        /// </summary>
        public void ReloadCurrentScene()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            LoadScene(currentSceneName);
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            IsLoading = true;
            float startTime = Time.realtimeSinceStartup;

            OnSceneLoadStarted?.Invoke(sceneName);
            Debug.Log($"[SceneLoader] 씬 로딩 시작: {sceneName}");

            // 비동기 씬 로딩 시작
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            // 로딩 진행률 업데이트
            while (!asyncLoad.isDone)
            {
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                OnSceneLoadProgress?.Invoke(sceneName, progress);

                // 로딩이 90% 완료되면 대기
                if (asyncLoad.progress >= 0.9f)
                {
                    // 최소 로딩 시간 보장 (너무 빠른 전환 방지)
                    float elapsedTime = Time.realtimeSinceStartup - startTime;
                    if (elapsedTime >= minimumLoadingTime)
                    {
                        asyncLoad.allowSceneActivation = true;
                    }
                }

                yield return null;
            }

            IsLoading = false;
            OnSceneLoadCompleted?.Invoke(sceneName);
            Debug.Log($"[SceneLoader] 씬 로딩 완료: {sceneName}");
        }

        /// <summary>
        /// 씬에 오브젝트 추가 로드 (Additive)
        /// </summary>
        public void LoadSceneAdditive(string sceneName)
        {
            StartCoroutine(LoadSceneAdditiveAsync(sceneName));
        }

        private IEnumerator LoadSceneAdditiveAsync(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Debug.Log($"[SceneLoader] Additive 씬 로딩 완료: {sceneName}");
        }

        /// <summary>
        /// 씬 언로드
        /// </summary>
        public void UnloadScene(string sceneName)
        {
            StartCoroutine(UnloadSceneAsync(sceneName));
        }

        private IEnumerator UnloadSceneAsync(string sceneName)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

            while (!asyncUnload.isDone)
            {
                yield return null;
            }

            Debug.Log($"[SceneLoader] 씬 언로드 완료: {sceneName}");
        }
    }
}
