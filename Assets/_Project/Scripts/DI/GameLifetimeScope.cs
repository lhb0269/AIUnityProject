using VContainer;
using VContainer.Unity;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MobileGame.Managers;
using MobileGame.Interfaces;
using MobileGame.UI;

namespace MobileGame.DI
{
    /// <summary>
    /// 게임 전체의 DI 컨테이너 루트
    /// 모든 매니저를 등록하고 의존성을 주입
    /// </summary>
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("매니저 참조")]
        [SerializeField] private UIManager uiManager;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private SceneLoader sceneLoader;

        [Header("UI Manager 설정")]
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private Canvas popupCanvas;
        [SerializeField] private List<PopupPrefabEntry> initialPopupPrefabs = new List<PopupPrefabEntry>();

        protected override void Configure(IContainerBuilder builder)
        {
            // 필수 매니저 - SerializeField로 직접 참조한 인스턴스를 등록
            if (uiManager != null)
            {
                builder.RegisterComponent(uiManager).As<IUIManager>();
            }
            else
            {
                Debug.LogError("[GameLifetimeScope] UIManager가 할당되지 않았습니다!");
            }

            if (gameManager != null)
            {
                builder.RegisterComponent(gameManager).As<IGameManager>();
            }
            else
            {
                Debug.LogError("[GameLifetimeScope] GameManager가 할당되지 않았습니다!");
            }

            if (audioManager != null)
            {
                builder.RegisterComponent(audioManager).As<IAudioManager>();
            }
            else
            {
                Debug.LogError("[GameLifetimeScope] AudioManager가 할당되지 않았습니다!");
            }

            if (sceneLoader != null)
            {
                builder.RegisterComponent(sceneLoader).As<ISceneLoader>();
            }
            else
            {
                Debug.LogError("[GameLifetimeScope] SceneLoader가 할당되지 않았습니다!");
            }

            // UI 컨트롤러 자동 주입 (씬에 있는 MonoBehaviour에 DI)
            builder.RegisterComponentInHierarchy<MainMenuController>();

            // EntryPoint 등록 (게임 시작 시 초기화를 위한 진입점)
            builder.RegisterEntryPoint<GameInitializer>(Lifetime.Scoped)
                .WithParameter(mainCanvas)
                .WithParameter(popupCanvas)
                .WithParameter(initialPopupPrefabs);
        }

        /// <summary>
        /// 게임 초기화 진입점
        /// VContainer가 자동으로 호출
        /// </summary>
        private class GameInitializer : IStartable
        {
            private readonly IGameManager gameManager;
            private readonly UIManager uiManager;
            private readonly Canvas mainCanvas;
            private readonly Canvas popupCanvas;
            private readonly List<PopupPrefabEntry> initialPopupPrefabs;

            public GameInitializer(
                IGameManager gameManager,
                UIManager uiManager,
                Canvas mainCanvas,
                Canvas popupCanvas,
                List<PopupPrefabEntry> initialPopupPrefabs)
            {
                this.gameManager = gameManager;
                this.uiManager = uiManager;
                this.mainCanvas = mainCanvas;
                this.popupCanvas = popupCanvas;
                this.initialPopupPrefabs = initialPopupPrefabs;
            }

            public void Start()
            {
                // UIManager 초기화 (Canvas와 팝업 프리팹 전달)
                uiManager.Initialize(mainCanvas, popupCanvas, initialPopupPrefabs);

                Debug.Log("[GameLifetimeScope] 게임 초기화 완료 - DI 컨테이너 준비됨");
            }
        }
    }
}
