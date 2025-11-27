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
            // UIManager - MonoBehaviour이므로 ComponentInHierarchy 또는 Instance로 등록
            // 매니저들은 이 스크립트와 같은 GameObject에 추가될 예정
            builder.RegisterComponentInHierarchy<UIManager>().As<IUIManager>();
            builder.RegisterComponentInHierarchy<GameManager>().As<IGameManager>();
            builder.RegisterComponentInHierarchy<AudioManager>().As<IAudioManager>();
            builder.RegisterComponentInHierarchy<InputManager>().As<IInputManager>();
            builder.RegisterComponentInHierarchy<SceneLoader>().As<ISceneLoader>();
            builder.RegisterComponentInHierarchy<SaveSystem>().As<ISaveSystem>();

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
            private readonly IUIManager uiManager;

            public GameInitializer(IGameManager gameManager, IUIManager uiManager)
            {
                this.gameManager = gameManager;
                this.uiManager = uiManager;
            }

            public void Start()
            {
                Debug.Log("[GameLifetimeScope] 게임 초기화 완료 - DI 컨테이너 준비됨");
            }
        }
    }
}
