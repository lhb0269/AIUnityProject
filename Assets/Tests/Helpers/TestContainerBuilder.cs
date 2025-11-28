using VContainer;
using VContainer.Unity;
using MobileGame.Managers;
using MobileGame.Interfaces;
using MobileGame.Tests.Mocks;

namespace MobileGame.Tests.Helpers
{
    /// <summary>
    /// 테스트용 DI 컨테이너 빌더
    /// VContainer를 사용하여 테스트 격리를 위한 Mock 매니저를 주입
    /// </summary>
    public static class TestContainerBuilder
    {
        /// <summary>
        /// Mock 매니저들을 등록한 테스트용 LifetimeScope 생성
        /// </summary>
        public static LifetimeScope CreateTestScope(IContainerBuilder builder = null)
        {
            var scope = LifetimeScope.Create(configuration: containerBuilder =>
            {
                // Mock 매니저들을 싱글톤으로 등록
                containerBuilder.Register<MockUIManager>(Lifetime.Singleton).As<IUIManager>();
                containerBuilder.Register<MockGameManager>(Lifetime.Singleton).As<IGameManager>();
                containerBuilder.Register<MockAudioManager>(Lifetime.Singleton).As<IAudioManager>();
                containerBuilder.Register<MockInputManager>(Lifetime.Singleton).As<IInputManager>();
                containerBuilder.Register<MockSceneLoader>(Lifetime.Singleton).As<ISceneLoader>();
                containerBuilder.Register<MockSaveSystem>(Lifetime.Singleton).As<ISaveSystem>();

                // 추가 커스텀 설정이 있으면 적용
                builder?.Invoke(containerBuilder);
            });

            return scope;
        }

        /// <summary>
        /// Mock UIManager만 등록한 간단한 테스트용 스코프 생성
        /// </summary>
        public static LifetimeScope CreateUITestScope()
        {
            return LifetimeScope.Create(configuration: builder =>
            {
                builder.Register<MockUIManager>(Lifetime.Singleton).As<IUIManager>();
            });
        }

        /// <summary>
        /// 특정 Mock 매니저들만 등록한 커스텀 스코프 생성
        /// </summary>
        public static LifetimeScope CreateCustomScope(
            bool includeUI = true,
            bool includeGame = false,
            bool includeAudio = false,
            bool includeInput = false,
            bool includeScene = false,
            bool includeSave = false)
        {
            return LifetimeScope.Create(configuration: builder =>
            {
                if (includeUI)
                    builder.Register<MockUIManager>(Lifetime.Singleton).As<IUIManager>();

                if (includeGame)
                    builder.Register<MockGameManager>(Lifetime.Singleton).As<IGameManager>();

                if (includeAudio)
                    builder.Register<MockAudioManager>(Lifetime.Singleton).As<IAudioManager>();

                if (includeInput)
                    builder.Register<MockInputManager>(Lifetime.Singleton).As<IInputManager>();

                if (includeScene)
                    builder.Register<MockSceneLoader>(Lifetime.Singleton).As<ISceneLoader>();

                if (includeSave)
                    builder.Register<MockSaveSystem>(Lifetime.Singleton).As<ISaveSystem>();
            });
        }

        /// <summary>
        /// 컨테이너로부터 Mock UIManager 가져오기
        /// </summary>
        public static MockUIManager GetMockUIManager(IObjectResolver container)
        {
            return container.Resolve<IUIManager>() as MockUIManager;
        }

        /// <summary>
        /// 컨테이너로부터 Mock GameManager 가져오기
        /// </summary>
        public static MockGameManager GetMockGameManager(IObjectResolver container)
        {
            return container.Resolve<IGameManager>() as MockGameManager;
        }

        /// <summary>
        /// 컨테이너로부터 Mock AudioManager 가져오기
        /// </summary>
        public static MockAudioManager GetMockAudioManager(IObjectResolver container)
        {
            return container.Resolve<IAudioManager>() as MockAudioManager;
        }

        /// <summary>
        /// 컨테이너로부터 Mock InputManager 가져오기
        /// </summary>
        public static MockInputManager GetMockInputManager(IObjectResolver container)
        {
            return container.Resolve<IInputManager>() as MockInputManager;
        }

        /// <summary>
        /// 컨테이너로부터 Mock SceneLoader 가져오기
        /// </summary>
        public static MockSceneLoader GetMockSceneLoader(IObjectResolver container)
        {
            return container.Resolve<ISceneLoader>() as MockSceneLoader;
        }

        /// <summary>
        /// 컨테이너로부터 Mock SaveSystem 가져오기
        /// </summary>
        public static MockSaveSystem GetMockSaveSystem(IObjectResolver container)
        {
            return container.Resolve<ISaveSystem>() as MockSaveSystem;
        }
    }
}
