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
        [Header("UI Manager 설정")]
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private Canvas popupCanvas;
        [SerializeField] private List<PopupPrefabEntry> initialPopupPrefabs = new List<PopupPrefabEntry>();

        [Header("Game Manager 설정")]
        [SerializeField] private int targetFrameRate = 60;
        [SerializeField] private bool allowScreenDimming = false;

        [Header("Audio Manager 설정")]
        [SerializeField, Range(0f, 1f)] private float masterVolume = 1f;
        [SerializeField, Range(0f, 1f)] private float bgmVolume = 0.7f;
        [SerializeField, Range(0f, 1f)] private float sfxVolume = 1f;

        protected override void Configure(IContainerBuilder builder)
        {
            // GameLifetimeScope 자체를 컨테이너에 등록 (설정값 접근을 위해)
            builder.RegisterComponent(this);

            // 필수 매니저
            builder.RegisterComponentInHierarchy<UIManager>().As<IUIManager>();
            builder.RegisterComponentInHierarchy<GameManager>().As<IGameManager>();
            builder.RegisterComponentInHierarchy<AudioManager>().As<IAudioManager>();
            builder.RegisterComponentInHierarchy<SceneLoader>().As<ISceneLoader>();

            // 선택적 매니저 (필요시 주석 해제)
            // builder.RegisterComponentInHierarchy<InputManager>().As<IInputManager>();
            // builder.RegisterComponentInHierarchy<SaveSystem>().As<ISaveSystem>();

            // UI 컨트롤러 자동 주입 (씬에 있는 MonoBehaviour에 DI)
            builder.RegisterComponentInHierarchy<MainMenuController>();

            // EntryPoint 등록 (게임 시작 시 초기화를 위한 진입점)
            builder.RegisterEntryPoint<GameInitializer>();
        }

        /// <summary>
        /// 게임 초기화 진입점
        /// VContainer가 자동으로 호출
        /// </summary>
        private class GameInitializer : IStartable
        {
            private readonly IGameManager gameManager;
            private readonly UIManager uiManager;
            private readonly GameLifetimeScope scope;

            public GameInitializer(IGameManager gameManager, UIManager uiManager, GameLifetimeScope scope)
            {
                this.gameManager = gameManager;
                this.uiManager = uiManager;
                this.scope = scope;
            }

            public void Start()
            {
                // UIManager 초기화 (Canvas와 팝업 프리팹 전달)
                uiManager.Initialize(scope.mainCanvas, scope.popupCanvas, scope.initialPopupPrefabs);

                Debug.Log("[GameLifetimeScope] 게임 초기화 완료 - DI 컨테이너 준비됨");
            }
        }
    }
}
