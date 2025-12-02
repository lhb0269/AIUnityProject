# TestContainerBuilder 헬퍼 시스템 상세 설명

## 🎯 목적

TestContainerBuilder는 **VContainer DI 기반 테스트 환경을 쉽게 구축**하기 위한 헬퍼 클래스입니다. 테스트에서 Mock 객체를 주입하고 관리하는 작업을 표준화하여 코드 중복을 제거하고 테스트 작성을 간소화합니다.

---

## 📋 주요 기능

### 1. **3가지 테스트 스코프 생성 메서드**

#### 1.1 CreateTestScope() - 전체 Mock 매니저 등록

모든 Mock 매니저를 한 번에 등록하는 가장 포괄적인 스코프입니다.

```csharp
public static LifetimeScope CreateTestScope(System.Action<IContainerBuilder> customBuilder = null)
{
    var scope = LifetimeScope.Create(configuration: containerBuilder =>
    {
        // 6개의 Mock 매니저를 모두 싱글톤으로 등록
        containerBuilder.Register<MockUIManager>(Lifetime.Singleton).As<IUIManager>();
        containerBuilder.Register<MockGameManager>(Lifetime.Singleton).As<IGameManager>();
        containerBuilder.Register<MockAudioManager>(Lifetime.Singleton).As<IAudioManager>();
        containerBuilder.Register<MockInputManager>(Lifetime.Singleton).As<IInputManager>();
        containerBuilder.Register<MockSceneLoader>(Lifetime.Singleton).As<ISceneLoader>();
        containerBuilder.Register<MockSaveSystem>(Lifetime.Singleton).As<ISaveSystem>();

        // 추가 커스텀 설정이 있으면 적용
        customBuilder?.Invoke(containerBuilder);
    });

    return scope;
}
```

**사용 시나리오**:
- 여러 매니저에 의존하는 복잡한 컴포넌트 테스트
- 통합 테스트

**사용 예시**:
```csharp
[UnitySetUp]
public IEnumerator Setup()
{
    // 모든 Mock 매니저 포함
    testScope = TestContainerBuilder.CreateTestScope();

    // Mock 인스턴스 가져오기
    mockUIManager = TestContainerBuilder.GetMockUIManager(testScope.Container);
    mockGameManager = TestContainerBuilder.GetMockGameManager(testScope.Container);

    yield return null;
}
```

---

#### 1.2 CreateUITestScope() - UI만 포함

UI 테스트에 특화된 가벼운 스코프입니다.

```csharp
public static LifetimeScope CreateUITestScope()
{
    return LifetimeScope.Create(configuration: builder =>
    {
        builder.Register<MockUIManager>(Lifetime.Singleton).As<IUIManager>();
    });
}
```

**사용 시나리오**:
- UI 컴포넌트만 테스트 (팝업, 버튼 등)
- 빠른 테스트 실행이 필요한 경우

**사용 예시**:
```csharp
[UnitySetUp]
public IEnumerator Setup()
{
    // UI 매니저만 포함
    testScope = TestContainerBuilder.CreateUITestScope();
    mockUIManager = TestContainerBuilder.GetMockUIManager(testScope.Container);

    yield return null;
}
```

---

#### 1.3 CreateCustomScope() - 선택적 Mock 등록 ⭐ 가장 유용

필요한 Mock 매니저만 선택적으로 등록하는 유연한 스코프입니다.

```csharp
public static LifetimeScope CreateCustomScope(
    bool includeUI = true,      // UIManager 포함 여부
    bool includeGame = false,   // GameManager 포함 여부
    bool includeAudio = false,  // AudioManager 포함 여부
    bool includeInput = false,  // InputManager 포함 여부
    bool includeScene = false,  // SceneLoader 포함 여부
    bool includeSave = false)   // SaveSystem 포함 여부
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
```

**사용 시나리오**:
- 특정 매니저만 필요한 테스트
- 테스트 격리를 위해 불필요한 의존성 제거
- **HamburgerMenuPopupTests에서 사용 중** ✅

**사용 예시 (HamburgerMenuPopupTests)**:
```csharp
[UnitySetUp]
public IEnumerator Setup()
{
    // UI만 필요하므로 UI만 포함
    testScope = TestContainerBuilder.CreateCustomScope(
        includeUI: true,
        includeGame: false,
        includeAudio: false
    );

    mockUIManager = TestContainerBuilder.GetMockUIManager(testScope.Container);

    var popupObj = new GameObject("TestHamburgerMenuPopup");
    popup = popupObj.AddComponent<HamburgerMenuPopup>();
    testScope.Container.Inject(popup);  // DI 주입

    yield return null;
}
```

---

### 2. **6가지 Mock 인스턴스 가져오기 메서드**

VContainer에서 등록된 Mock 인스턴스를 안전하게 가져오는 헬퍼 메서드들입니다.

#### 2.1 GetMockUIManager() ⭐ 가장 많이 사용

```csharp
public static MockUIManager GetMockUIManager(IObjectResolver container)
{
    return container.Resolve<IUIManager>() as MockUIManager;
}
```

**중요성**:
- VContainer에서 주입된 **동일한 인스턴스**를 가져옵니다
- `new MockUIManager()`로 직접 생성하면 안 됩니다! (이슈 #1 참고)

**사용 예시**:
```csharp
// ❌ 잘못된 방법
mockUIManager = new MockUIManager();  // 다른 인스턴스!

// ✅ 올바른 방법
mockUIManager = TestContainerBuilder.GetMockUIManager(testScope.Container);
```

#### 2.2 기타 Mock 가져오기 메서드

동일한 패턴으로 다른 Mock 매니저도 가져올 수 있습니다:

```csharp
// GameManager
mockGameManager = TestContainerBuilder.GetMockGameManager(testScope.Container);

// AudioManager
mockAudioManager = TestContainerBuilder.GetMockAudioManager(testScope.Container);

// InputManager
mockInputManager = TestContainerBuilder.GetMockInputManager(testScope.Container);

// SceneLoader
mockSceneLoader = TestContainerBuilder.GetMockSceneLoader(testScope.Container);

// SaveSystem
mockSaveSystem = TestContainerBuilder.GetMockSaveSystem(testScope.Container);
```

---

## 🔍 동작 원리

### VContainer DI 동작 흐름

```
1. CreateCustomScope() 호출
   ↓
2. LifetimeScope.Create() - 새 DI 컨테이너 생성
   ↓
3. containerBuilder.Register<MockUIManager>() - Mock 등록
   ↓
4. testScope.Container.Inject(popup) - 팝업에 Mock 주입
   ↓
5. popup 내부의 IUIManager 필드에 MockUIManager 인스턴스 주입
   ↓
6. GetMockUIManager(testScope.Container) - 동일한 인스턴스 가져오기
   ↓
7. 테스트 코드와 프로덕션 코드가 같은 Mock 사용
```

### 인스턴스 관리

```csharp
// 테스트 Setup
testScope = CreateCustomScope(includeUI: true);
// → MockUIManager 인스턴스 A 생성 (컨테이너 내부)

testScope.Container.Inject(popup);
// → popup.uiManager = 인스턴스 A (주입)

mockUIManager = GetMockUIManager(testScope.Container);
// → mockUIManager = 인스턴스 A (동일!)

// 테스트 실행
popup.OnTownButtonClicked();
// → popup 내부에서 uiManager.ShowPopup() 호출 (인스턴스 A 사용)

mockUIManager.ShownPopups.Count;
// → 인스턴스 A의 기록 확인 가능 ✅
```

---

## 💡 장점

### 1. **테스트 격리성 향상**
- 각 테스트마다 독립적인 LifetimeScope를 생성합니다
- 테스트 간 Mock 상태가 공유되지 않습니다

### 2. **코드 중복 제거**
```csharp
// ❌ 이전: 매 테스트마다 반복
[UnitySetUp]
public IEnumerator Setup()
{
    var scope = LifetimeScope.Create(configuration: builder =>
    {
        builder.Register<MockUIManager>(Lifetime.Singleton).As<IUIManager>();
        // ... 매번 반복
    });
}

// ✅ 이후: 한 줄로 간소화
testScope = TestContainerBuilder.CreateCustomScope(includeUI: true);
```

### 3. **일관성 보장**
- 모든 테스트가 동일한 방식으로 Mock을 등록합니다
- `GetMockUIManager()` 패턴으로 인스턴스 불일치 방지

### 4. **유연성**
- `CreateCustomScope()`로 필요한 Mock만 선택적으로 등록 가능
- 불필요한 의존성을 제거하여 테스트 속도 향상

---

## 📊 실제 사용 사례 (HamburgerMenuPopupTests)

```csharp
public class HamburgerMenuPopupTests
{
    private LifetimeScope testScope;
    private MockUIManager mockUIManager;
    private HamburgerMenuPopup popup;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        // 1. UI만 포함하는 커스텀 스코프 생성
        testScope = TestContainerBuilder.CreateCustomScope(
            includeUI: true,    // UIManager만 필요
            includeGame: false,
            includeAudio: false
        );

        // 2. 동일한 Mock 인스턴스 가져오기
        mockUIManager = TestContainerBuilder.GetMockUIManager(testScope.Container);

        // 3. 팝업 생성 및 DI 주입
        var popupObj = new GameObject("TestHamburgerMenuPopup");
        popup = popupObj.AddComponent<HamburgerMenuPopup>();
        SetupButtons(popupObj.transform);

        testScope.Container.Inject(popup);  // Mock 주입!

        yield return null;
    }

    [UnityTest]
    public IEnumerator WhenTownButtonClicked_ThenTownPopupOpened()
    {
        mockUIManager.Reset();
        mockUIManager.ShowPopup(PopupID.HamburgerMenu);  // 인스턴스 A 사용

        townBtn.onClick.Invoke();  // popup 내부에서 인스턴스 A 사용
        yield return null;

        Assert.AreEqual(2, mockUIManager.ShownPopups.Count);  // 인스턴스 A 검증 ✅
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        if (testScope != null)
        {
            Object.Destroy(testScope.gameObject);
        }
        yield return null;
    }
}
```

---

## ⚠️ 주의사항

### 1. **반드시 GetMock~() 메서드 사용**
```csharp
// ❌ 절대 직접 생성하지 마세요!
mockUIManager = new MockUIManager();

// ✅ 반드시 컨테이너에서 가져오세요
mockUIManager = TestContainerBuilder.GetMockUIManager(testScope.Container);
```

### 2. **Teardown에서 스코프 파괴**
```csharp
[UnityTearDown]
public IEnumerator Teardown()
{
    if (testScope != null)
    {
        Object.Destroy(testScope.gameObject);  // 메모리 누수 방지
    }
    yield return null;
}
```

### 3. **필요한 Mock만 등록**
```csharp
// ❌ 불필요한 Mock도 전부 등록
testScope = CreateTestScope();  // 6개 전부

// ✅ 필요한 것만 등록 (테스트 속도 향상)
testScope = CreateCustomScope(includeUI: true);  // UI만
```

---

## 🎓 핵심 정리

1. **TestContainerBuilder는 VContainer DI 테스트 환경을 표준화**합니다
2. **3가지 스코프 생성 메서드**로 다양한 테스트 시나리오 지원합니다
3. **GetMock~() 메서드로 동일한 인스턴스 보장**이 가장 중요합니다
4. **HamburgerMenuPopupTests에서 이슈 #1을 해결한 핵심 도구**입니다

TestContainerBuilder 덕분에 VContainer DI 기반 테스트를 간단하고 일관되게 작성할 수 있게 되었습니다! 🎉
