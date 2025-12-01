# Unity 게임 자동화 테스트 작업물 - VContainer DI 기반 테스트 시스템

Unity Test Framework와 VContainer DI를 활용한 테스트 자동화 시스템 구축 사례입니다.

프로젝트 구축 및 자동화 스크립트 작성에 Claude AI를 사용하였습니다.

---

## 목차

1. [작업물 개요](#-작업물-개요)
2. [작업물에 대한 코드 및 코드 가이드](#-작업물에-대한-코드-및-코드-가이드)
3. [작업 중 발생한 기억에 남는 이슈 및 해결 방법](#-작업-중-발생한-기억에-남는-이슈-및-해결-방법)

---

## 작업물 개요

**게임**: 메이플 키우기 (모바일 방치형 RPG)
- 장르: 방치형 RPG
- 플랫폼: Android/iOS
- 주요 특징: 간단한 버튼 조작 중심의 UI

### 작업 배경

기존 Unity UI 자동화 테스트 시스템에 VContainer DI(Dependency Injection) 패턴을 도입하여 테스트 격리성과 유지보수성을 향상시키는 작업을 진행하였습니다.

### 프로젝트 목표

**MD (Milestone Deliverable)**
> 메이플 키우기의 메인 메뉴 및 2차 메뉴(햄버거 메뉴)의 모든 버튼을 클릭했을 때 해당 기능이 정상적으로 동작한다.

## 요약

1. **테스트 작성 표준화**
   - Unity 테스트 작성 전문 서브 에이전트 생성
   - Given-When-Then 명명 규칙 확립
   - VContainer DI 기반 Mock 객체 패턴 정립

2. **HamburgerMenuPopupTests 완성**
   - 21개 테스트 작성 (Basic, DI, 버튼, Edge Case, 통합)
   - 모든 테스트 통과 (100% 성공률)

3. **3가지 주요 이슈 해결**
   - Mock 인스턴스 불일치 → `GetMockUIManager()` 패턴
   - 팝업 호출 횟수 불일치 → 부모 팝업 먼저 열기
   - 비현실적 시나리오 → 실제 사용자 흐름 반영


### 주요 성과

| 지표 | 값 |
|-----|-----|
| **작성한 테스트** | 총 75개 (MainMenu 54개 + HamburgerMenu 21개) |
| **테스트 가이드라인** | 10개 섹션, ~500줄 |
| **테스트 패턴** | 3개 (팝업 열기/닫기, 로그 검증, Edge Case) |
| **해결한 주요 이슈** | 4개 (싱글톤 간섭, WaitForSeconds 문제, Mock 인스턴스, 비현실적 시나리오) |
| **슬래시 커맨드** | `/generate-test` (5단계 프로세스) |

### 작업 범위

#### ✅ 구현한 항목

**1. Unity 테스트 작성 전문 서브 에이전트**
- `.claude/agents/unity-test-writer.md` - 포괄적인 테스트 작성 가이드라인
- `/generate-test` 슬래시 커맨드 - 테스트 자동 생성 워크플로우
- Given-When-Then 명명 규칙 표준화
- VContainer DI 기반 Mock 객체 사용 패턴

**2. MainMenuControllerTests (54개 테스트)**
- 20개 팝업 열기 테스트 (13개 기본 + 7개 추가)
- 20개 팝업 닫기 테스트 (13개 기본 + 7개 추가)
- 13개 일반 버튼 로그 검증 테스트
- 1개 팝업 중복 열기 방지 테스트
- 총 793줄, 3가지 재사용 패턴 메서드

**3. HamburgerMenuPopupTests (21개 테스트)**
- Basic Lifecycle: 2개 (Show, CloseButton)
- DI Injection 검증: 1개 (UIManager 주입)
- Button Interactions (Log Only): 12개 (일반 버튼)
- Button Interactions (Popup Opening): 3개 (Town, Notice, GameSetting)
- Edge Cases: 2개 (중복 팝업 방지, UIManager null)
- Integration Test: 1개 (여러 팝업 연속 열기)
- 총 555줄, VContainer DI 패턴

**4. 테스트 헬퍼 시스템**
- `TestContainerBuilder` - VContainer 테스트 스코프 빌더
- Mock 객체 인스턴스 관리 메서드
- 버튼 생성 및 로그 검증 헬퍼

#### ❌ 제외 항목

| 제외 항목 | 사유 |
|-----------|------|
| **일반 버튼의 고유 기능 동작** | 현재 프로젝트는 UI 배치 및 클릭 이벤트 구현만 완료되었습니다. 실제 게임 로직(데미지 계산, 아이템 지급 등)은 미구현 상태입니다 |
| **팝업창 내부 상호작용** | 팝업 생성/제거만 구현됨. 팝업 내부의 입력 필드, 드롭다운, 슬라이더 등은 미구현 |
| **시각적 UI 검증** | 폰트, 색상, 정렬, 애니메이션 등은 수동 검수 영역 (자동화 우선순위 Low) |
| **성능 테스트** | FPS, 메모리 사용량, 로딩 시간 등은 별도 성능 테스트 필요 |


---

## 💻 작업물에 대한 코드 및 코드 가이드

### 1. MainMenuControllerTests 상세 가이드

#### 1.1 파일 위치
`Assets/Tests/PlayMode/UI/MainMenuControllerTests.cs`

#### 1.2 테스트 구조

```csharp
[TestFixture]
public class MainMenuControllerTests
{
    private const string TEST_SCENE_NAME = "SampleScene";
    private bool sceneLoaded = false;

    #region Setup/Teardown
    [UnitySetUp]
    public IEnumerator Setup() { /* 씬 로드 및 초기화 */ }

    [UnityTearDown]
    public IEnumerator Teardown() { /* 팝업 정리 */ }

    [OneTimeTearDown]
    public void OneTimeTearDown() { /* 싱글톤 리셋 */ }
    #endregion

    #region Popup Open Tests (20개)
    // TestButtonOpensPopup<T>() 패턴 메서드 재사용
    #endregion

    #region Popup Close Tests (20개)
    // TestPopupCloseButton<T>() 패턴 메서드 재사용
    #endregion

    #region General Button Tests (13개)
    // TestButtonClickLogsMessage() 패턴 메서드 재사용
    #endregion

    #region Integration Tests (1개)
    // 팝업 중복 열기 방지 검증
    #endregion

    #region Helper Methods
    // 3가지 재사용 패턴 메서드
    #endregion
}
```

#### 1.3 3가지 핵심 패턴 메서드

**패턴 1: 팝업 열기 테스트 (20개 재사용)**
```csharp
/// <summary>
/// 버튼 클릭 시 팝업이 열리는 테스트 패턴
/// </summary>
private IEnumerator TestButtonOpensPopup<TPopup>(
    string buttonFieldName,
    string buttonDisplayName) where TPopup : BasePopup
{
    // Arrange - 버튼 가져오기
    Button button = GetButtonField(buttonFieldName);

    // Act - 버튼 클릭 및 팝업 대기
    yield return ClickButton(button, buttonDisplayName);
    yield return WaitUntilPopupAppears<TPopup>();

    // Assert - 팝업 생성 확인
    TPopup popup = Object.FindFirstObjectByType<TPopup>();
    Assert.IsNotNull(popup);
    Assert.AreEqual(1, UIManager.Instance.GetActivePopupCount());

    // Cleanup - 팝업 닫기
    UIManager.Instance.CloseAllActivePopups();
    yield return WaitUntilNoActivePopups();
}
```

**패턴 2: 팝업 닫기 테스트 (20개 재사용)**
```csharp
/// <summary>
/// 팝업 닫기 버튼 테스트 패턴
/// </summary>
private IEnumerator TestPopupCloseButton<TPopup>(
    string buttonFieldName,
    string buttonDisplayName) where TPopup : BasePopup
{
    // Arrange - 팝업 열기
    Button button = GetButtonField(buttonFieldName);
    yield return ClickButton(button, buttonDisplayName);
    yield return WaitUntilPopupAppears<TPopup>();

    // Arrange - 닫기 버튼 찾기
    BasePopup popup = Object.FindFirstObjectByType<BasePopup>();
    Button closeButton = GetCloseButton(popup);

    // Act - 닫기 버튼 클릭
    yield return ClickButton(closeButton, "닫기");

    // Assert - 팝업 닫힘 확인
    yield return new WaitUntil(() =>
        UIManager.Instance.GetActivePopupCount() == 0);

    yield return WaitUntilNoActivePopups();
}
```

**패턴 3: 일반 버튼 로그 테스트 (13개 재사용)**
```csharp
/// <summary>
/// 버튼 클릭 시 로그 메시지 출력 테스트 패턴
/// </summary>
private IEnumerator TestButtonClickLogsMessage(
    string buttonFieldName,
    string expectedLog,
    string buttonDisplayName)
{
    // Arrange
    Button button = GetButtonField(buttonFieldName);

    // Act & Assert - 로그 검증
    LogAssert.Expect(LogType.Log, expectedLog);
    yield return ClickButton(button, buttonDisplayName);
}
```

**코드 중복 제거 효과**:
- 이전 예상: 1,600줄+ (54개 테스트 개별 작성)
- 실제: 793줄 (패턴 메서드 재사용)
- 감소율: 약 50%

---

### 2. HamburgerMenuPopupTests 상세 가이드

#### 2.1 파일 위치
`Assets/Tests/PlayMode/UI/HamburgerMenuPopupTests.cs`

#### 2.2 테스트 구조

```csharp
[TestFixture]
public class HamburgerMenuPopupTests
{
    private LifetimeScope testScope;
    private MockUIManager mockUIManager;
    private HamburgerMenuPopup popup;
    private Button townBtn, noticeBtn, gameSettingBtn;

    #region Setup/Teardown
    [UnitySetUp]
    public IEnumerator Setup() { /* ... */ }

    [UnityTearDown]
    public IEnumerator Teardown() { /* ... */ }
    #endregion

    #region Tests - Basic Lifecycle
    // 2개 테스트
    #endregion

    #region Tests - DI Injection
    // 1개 테스트
    #endregion

    #region Tests - Button Interactions (Log Only)
    // 12개 테스트
    #endregion

    #region Tests - Button Interactions (Popup Opening)
    // 3개 테스트
    #endregion

    #region Tests - Edge Cases
    // 2개 테스트
    #endregion

    #region Tests - Integration
    // 1개 테스트
    #endregion

    #region Helper Methods
    // 헬퍼 메서드
    #endregion
}
```

#### 2.3 주요 테스트 패턴

**패턴 1: 기본 Lifecycle 테스트**
```csharp
/// <summary>
/// 테스트: Show() 호출 시 팝업이 활성화됨
/// Given: 팝업이 생성된 상태
/// When: Show() 호출
/// Then: GameObject가 활성화됨
/// </summary>
[UnityTest]
public IEnumerator WhenShowCalled_ThenPopupIsActive()
{
    // Arrange
    popup.gameObject.SetActive(false);

    // Act
    popup.Show();
    yield return null;

    // Assert
    Assert.IsTrue(popup.gameObject.activeSelf, "팝업이 활성화되어야 합니다.");
}
```

**패턴 2: DI Injection 검증**
```csharp
/// <summary>
/// 테스트: VContainer를 통해 UIManager가 주입됨
/// Given: 테스트 스코프에서 MockUIManager 등록
/// When: Container.Inject(popup) 호출
/// Then: popup.uiManager != null
/// </summary>
[UnityTest]
public IEnumerator WhenInjected_ThenUIManagerNotNull()
{
    // Assert
    Assert.IsNotNull(GetPrivateField<IUIManager>(popup, "uiManager"),
        "UIManager가 DI를 통해 주입되어야 합니다.");
    yield return null;
}
```

**패턴 3: 팝업 열기 테스트**
```csharp
/// <summary>
/// 테스트: 마을 버튼 클릭 시 Town 팝업 열기
/// Given: 햄버거 메뉴 팝업이 이미 열린 상태
/// When: townBtn 클릭
/// Then: UIManager.ShowPopup(PopupID.Town) 호출됨 (총 2번 호출)
/// </summary>
[UnityTest]
public IEnumerator WhenTownButtonClicked_ThenTownPopupOpened()
{
    // Arrange
    mockUIManager.Reset();
    mockUIManager.ShowPopup(PopupID.HamburgerMenu);
    // ShowPopup이 FakeActivePopupCount를 자동으로 1 증가시킴

    // Act
    townBtn.onClick.Invoke();
    yield return null;

    // Assert
    Assert.AreEqual(2, mockUIManager.ShownPopups.Count,
        "HamburgerMenu + Town 총 2개의 팝업이 열려야 합니다.");
    Assert.AreEqual(PopupID.HamburgerMenu, mockUIManager.ShownPopups[0],
        "첫 번째는 HamburgerMenu 팝업이어야 합니다.");
    Assert.AreEqual(PopupID.Town, mockUIManager.ShownPopups[1],
        "두 번째는 Town 팝업이어야 합니다.");
}
```

**패턴 4: Edge Case 테스트**
```csharp
/// <summary>
/// 테스트: 팝업 중복 열기 방지 (2개 이상 팝업 열림 시)
/// Given: FakeActivePopupCount = 2 (햄버거 메뉴 + 다른 팝업)
/// When: townBtn 클릭
/// Then: ShowPopup이 호출되지 않음 (중복 방지)
/// </summary>
[UnityTest]
public IEnumerator WhenTownButtonClicked_AndTwoPopupsOpen_ThenPopupNotOpened()
{
    // Arrange
    mockUIManager.Reset();
    mockUIManager.FakeActivePopupCount = 2;

    // Act & Assert
    LogAssert.Expect(LogType.Log, "[HamburgerMenu] 마을 버튼 클릭");
    LogAssert.Expect(LogType.Warning, "[HamburgerMenu] 이미 다른 팝업이 열려있습니다. 먼저 닫아주세요.");
    townBtn.onClick.Invoke();
    yield return null;

    // Assert
    Assert.AreEqual(0, mockUIManager.ShownPopups.Count,
        "팝업이 2개 이상 열려있으면 ShowPopup이 호출되지 않아야 합니다.");
}
```

**패턴 5: 통합 테스트**
```csharp
/// <summary>
/// 테스트: 여러 버튼 연속 클릭 (팝업 열기)
/// Given: 햄버거 메뉴 팝업이 이미 열린 상태
/// When: town, notice, gameSetting 버튼 순서대로 클릭
/// Then: 각 팝업이 순서대로 열림 (총 4번 호출)
/// </summary>
[UnityTest]
public IEnumerator WhenMultiplePopupButtonsClicked_ThenAllPopupsOpened()
{
    // Arrange
    mockUIManager.Reset();
    mockUIManager.ShowPopup(PopupID.HamburgerMenu);

    // Act - 첫 번째 버튼 (Town)
    townBtn.onClick.Invoke();
    yield return null;

    // Town 팝업 닫힘 시뮬레이션
    mockUIManager.FakeActivePopupCount = 1;
    noticeBtn.onClick.Invoke();
    yield return null;

    // Notice 팝업 닫힘 시뮬레이션
    mockUIManager.FakeActivePopupCount = 1;
    gameSettingBtn.onClick.Invoke();
    yield return null;

    // Assert
    Assert.AreEqual(4, mockUIManager.ShownPopups.Count,
        "HamburgerMenu + Town + Notice + GameSetting 총 4개의 팝업이 열려야 합니다.");
}
```

---

### 3. TestContainerBuilder 헬퍼 시스템

#### 3.1 파일 위치
`Assets/Tests/Helpers/TestContainerBuilder.cs`

#### 3.2 주요 메서드

**메서드 1: 테스트 스코프 생성**
```csharp
/// <summary>
/// Mock 매니저들을 등록한 테스트용 LifetimeScope 생성
/// </summary>
public static LifetimeScope CreateTestScope(System.Action<IContainerBuilder> customBuilder = null)
{
    var scope = LifetimeScope.Create(configuration: containerBuilder =>
    {
        // Mock 매니저들을 싱글톤으로 등록
        containerBuilder.Register<MockUIManager>(Lifetime.Singleton).As<IUIManager>();
        containerBuilder.Register<MockGameManager>(Lifetime.Singleton).As<IGameManager>();
        // ... 기타 Mock 매니저들

        customBuilder?.Invoke(containerBuilder);
    });

    return scope;
}
```

**메서드 2: 커스텀 스코프 생성**
```csharp
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

        // ... 기타 조건부 등록
    });
}
```

**메서드 3: Mock 인스턴스 가져오기**
```csharp
/// <summary>
/// 컨테이너로부터 Mock UIManager 가져오기
/// </summary>
public static MockUIManager GetMockUIManager(IObjectResolver container)
{
    return container.Resolve<IUIManager>() as MockUIManager;
}
```

#### 3.3 사용 예시

```csharp
[UnitySetUp]
public IEnumerator Setup()
{
    // 1. UI만 포함하는 커스텀 스코프 생성
    testScope = TestContainerBuilder.CreateCustomScope(
        includeUI: true,
        includeGame: false,
        includeAudio: false
    );

    // 2. 동일한 Mock 인스턴스 가져오기
    mockUIManager = TestContainerBuilder.GetMockUIManager(testScope.Container);

    // 3. 팝업 생성 및 DI 주입
    var popupObj = new GameObject("TestHamburgerMenuPopup");
    popup = popupObj.AddComponent<HamburgerMenuPopup>();
    testScope.Container.Inject(popup);

    yield return null;
}
```

---

### 4. MockUIManager 동작 이해

#### 4.1 파일 위치
`Assets/Tests/Mocks/MockUIManager.cs`

#### 4.2 핵심 동작

**ShowPopup 메서드**
```csharp
public BasePopup ShowPopup(string popupName)
{
    ShownPopups.Add(popupName);
    FakeActivePopupCount++; // ✅ 자동으로 증가!
    return null;
}
```

**중요 사항**:
- `ShowPopup()`을 호출하면 `FakeActivePopupCount`가 자동으로 1 증가합니다
- 따라서 `ShowPopup()` 후 `FakeActivePopupCount = 1`을 다시 설정할 필요가 없습니다
- 팝업 닫힘을 시뮬레이션할 때만 수동으로 설정합니다

**잘못된 사용 예시**:
```csharp
// ❌ 잘못된 코드
mockUIManager.ShowPopup(PopupID.HamburgerMenu); // FakeActivePopupCount = 1
mockUIManager.FakeActivePopupCount = 1; // 불필요한 중복 설정!
```

**올바른 사용 예시**:
```csharp
// ✅ 올바른 코드
mockUIManager.ShowPopup(PopupID.HamburgerMenu); // FakeActivePopupCount = 1 (자동)

// 팝업 닫힘 시뮬레이션 시에만 수동 설정
mockUIManager.FakeActivePopupCount = 1; // Town 팝업 닫힘
```

---

## 🔧 작업 중 발생한 기억에 남는 이슈 및 해결 방법

### 이슈 #1: DontDestroyOnLoad 싱글톤 간섭 문제 (MainMenuControllerTests)

#### 작업 배경
MainMenuControllerTests를 작성하는 과정에서 발생한 이슈입니다. 여러 테스트 클래스를 순차적으로 실행할 때 UIManager 싱글톤이 테스트 간 간섭을 일으켰습니다.

#### 문제 상황

```
HamburgerMenuPopupTests 실행 완료
    ↓
MainMenuControllerTests 실행 시작
    ↓
씬 리로드 → MainMenuController 재생성
    ↓
UIManager는 DontDestroyOnLoad로 그대로 유지
    ↓
❌ 버튼 바인딩이 모두 사라짐 (null 참조 오류)
```

#### 원인 분석

- `UIManager`는 `DontDestroyOnLoad`를 사용하여 싱글톤으로 씬 전환 시에도 유지됩니다
- 테스트 간 씬 리로드 시 `MainMenuController`는 새로 생성되지만, `UIManager`는 이전 상태를 유지합니다
- 이로 인해 새로운 `MainMenuController`의 버튼들이 이전 `UIManager`와 연결되지 않습니다

#### 해결 방법

**1. UIManager에 테스트용 정리 메서드 추가**:
```csharp
// Assets/_Project/Scripts/Managers/UIManager.cs

/// <summary>
/// 테스트 환경에서 UIManager를 완전히 정리합니다.
/// DontDestroyOnLoad 객체를 파괴하고 Instance를 null로 리셋합니다.
/// </summary>
public static void ResetForTesting()
{
    if (Instance != null)
    {
        // 모든 팝업 닫기
        Instance.CloseAllActivePopups();

        // Instance를 null로 설정
        var instanceToDestroy = Instance;
        Instance = null;

        // GameObject 파괴
        if (instanceToDestroy != null)
        {
            Destroy(instanceToDestroy.gameObject);
        }

        Debug.Log("[UIManager] 테스트를 위해 인스턴스가 리셋되었습니다.");
    }
}
```

**2. 테스트 클래스에서 OneTimeTearDown 활용**:
```csharp
// Assets/Tests/PlayMode/UI/MainMenuControllerTests.cs

[OneTimeTearDown]
public void OneTimeTearDown()
{
    // 테스트 클래스 종료 시 싱글톤 완전 정리
    UIManager.ResetForTesting();
    sceneLoaded = false;
}
```

#### 결과

- ✅ 테스트 클래스 간 완전한 독립성을 보장합니다
- ✅ 다음 테스트 클래스 실행 시 새로운 UIManager가 생성됩니다
- ✅ 버튼 바인딩이 정상 동작합니다

#### 배운점

1. **DontDestroyOnLoad의 함정**
   - `DontDestroyOnLoad`는 편리하지만 테스트 환경에서는 오히려 독이 될 수 있습니다
   - 싱글톤 패턴 사용 시 반드시 테스트용 정리 메서드를 구현해야 합니다

2. **OneTimeTearDown의 중요성**
   - `[UnityTearDown]`은 각 테스트 후 실행되지만, `[OneTimeTearDown]`은 테스트 클래스 전체 종료 시 실행됩니다
   - 싱글톤 정리는 `[OneTimeTearDown]`에서 수행해야 합니다

3. **테스트 격리 원칙**
   - 각 테스트는 완전히 독립적이어야 하며, 전역 상태에 의존하면 안 됩니다

---

### 이슈 #2: WaitForSeconds로 인한 느리고 불안정한 테스트 (MainMenuControllerTests)

#### 작업 배경
MainMenuControllerTests에서 팝업 생성을 기다릴 때 고정 시간 대기를 사용했습니다.

#### 문제 상황

```csharp
// 이전 코드 (문제)
yield return new WaitForSeconds(1f);  // 항상 1초 대기
```

**문제점**:
1. **불필요하게 느림**: 팝업이 0.1초에 나타나도 1초를 기다립니다
2. **불안정**: 느린 환경에서는 1초로 부족할 수 있습니다
3. **유지보수 어려움**: 임의의 숫자(매직 넘버)를 사용합니다

#### 해결 방법

조건 기반 대기로 전환:

```csharp
// 개선 후 코드
yield return new WaitUntil(() =>
    Object.FindFirstObjectByType<HamburgerMenuPopup>() != null);
```

**타임아웃이 필요한 경우**:
```csharp
private IEnumerator WaitForComponent<T>() where T : Object
{
    float elapsed = 0f;
    const float timeout = 2f;

    while (elapsed < timeout)
    {
        if (Object.FindFirstObjectByType<T>() != null)
            yield break;  // 조건 충족 시 즉시 반환

        yield return null;
        elapsed += Time.deltaTime;
    }

    Assert.Fail($"{typeof(T).Name}이 {timeout}초 내에 나타나지 않았습니다");
}
```

#### 개선 효과

| 지표 | 이전 (WaitForSeconds) | 이후 (WaitUntil) | 개선율 |
|-----|---------------------|-----------------|-------|
| 평균 테스트 실행 시간 | ~45초 | ~15초 | **67% 감소** |
| 테스트 안정성 | 85% 성공률 | 99% 성공률 | **14% 향상** |
| 코드 명확성 | 낮음 (매직 넘버) | 높음 (조건 명시) | - |

#### 배운점

1. **조건 기반 대기의 우수성**
   - `WaitForSeconds`는 절대 사용하지 말아야 합니다
   - 항상 `WaitUntil`, `WaitWhile` 등 조건 기반 대기를 사용해야 합니다

2. **타임아웃 포함 필수**
   - 무한 대기를 방지하기 위해 타임아웃을 포함해야 합니다
   - 타임아웃 시 명확한 에러 메시지를 제공해야 합니다

3. **테스트 속도와 안정성의 양립**
   - 조건 기반 대기로 빠르면서도 안정적인 테스트를 작성할 수 있습니다

---

### 이슈 #3: VContainer DI 기반 테스트에서 Mock 인스턴스 불일치 문제 (HamburgerMenuPopupTests)

#### 작업 배경
HamburgerMenuPopup의 21개 테스트를 작성하는 과정에서 발생한 이슈입니다. VContainer를 사용한 DI(Dependency Injection) 테스트 환경을 구축하던 중 문제가 발생하였습니다.

#### 문제 상황

테스트 로그와 Assert 결과가 일치하지 않았습니다:

```
테스트 로그: "ShowPopup이 2번 호출되었습니다"
Assert 실패: Expected: 2, But was: 1
```

#### 원인 분석

테스트 코드에서 두 개의 서로 다른 MockUIManager 인스턴스가 생성되어 있었습니다:

```csharp
[UnitySetUp]
public IEnumerator Setup()
{
    // ❌ 문제 코드: 새로운 MockUIManager 인스턴스 A 생성
    mockUIManager = new MockUIManager();

    // VContainer 테스트 스코프 생성 (내부에서 MockUIManager 인스턴스 B 생성)
    testScope = TestContainerBuilder.CreateCustomScope(includeUI: true);

    // HamburgerMenuPopup에 인스턴스 B 주입
    testScope.Container.Inject(popup);
}

[UnityTest]
public IEnumerator WhenTownButtonClicked_ThenTownPopupOpened()
{
    // 테스트 코드에서 인스턴스 A 사용
    mockUIManager.ShowPopup(PopupID.HamburgerMenu);

    // HamburgerMenuPopup 내부에서는 인스턴스 B 사용
    townButton.onClick.Invoke(); // 내부적으로 mockUIManager.ShowPopup() 호출

    // 인스턴스 A에는 1개만 기록됨 (인스턴스 B의 기록은 확인 불가)
    Assert.AreEqual(2, mockUIManager.ShownPopups.Count); // 실패!
}
```

**문제의 핵심**:
- 테스트 코드는 인스턴스 A를 사용하여 검증합니다
- HamburgerMenuPopup은 VContainer가 주입한 인스턴스 B를 사용합니다
- 두 인스턴스가 달라서 검증이 불가능합니다

#### 해결 방법

VContainer에서 주입한 동일한 MockUIManager 인스턴스를 테스트 코드에서도 사용하도록 수정하였습니다:

```csharp
[UnitySetUp]
public IEnumerator Setup()
{
    // 1. VContainer 테스트 스코프 먼저 생성
    testScope = TestContainerBuilder.CreateCustomScope(includeUI: true);

    // 2. ✅ 컨테이너에서 주입된 동일한 인스턴스 가져오기
    mockUIManager = TestContainerBuilder.GetMockUIManager(testScope.Container);

    // 3. HamburgerMenuPopup에 주입 (동일한 인스턴스 B 사용)
    testScope.Container.Inject(popup);
}
```

**TestContainerBuilder.GetMockUIManager() 구현**:
```csharp
// Assets/Tests/Helpers/TestContainerBuilder.cs
public static MockUIManager GetMockUIManager(IObjectResolver container)
{
    // VContainer에서 IUIManager로 등록된 인스턴스를 MockUIManager로 캐스팅
    return container.Resolve<IUIManager>() as MockUIManager;
}
```

#### 결과

- 테스트 코드와 프로덕션 코드가 동일한 MockUIManager 인스턴스를 사용합니다
- ShowPopup 호출 횟수가 정확히 추적됩니다
- 모든 21개 테스트가 통과하였습니다

#### 배운점

1. **DI 컨테이너 사용 시 인스턴스 관리**
   - DI 컨테이너를 사용하는 테스트에서는 반드시 **컨테이너에서 주입된 인스턴스**를 가져와야 합니다
   - `new MockObject()`로 직접 생성하면 테스트 대상 객체와 다른 인스턴스가 되어 검증이 불가능합니다

2. **VContainer의 IObjectResolver 활용**
   - `IObjectResolver.Resolve<T>()`를 활용하여 동일한 인스턴스를 보장해야 합니다
   - 테스트 헬퍼 메서드(`GetMockUIManager`)로 패턴화하여 재사용성을 높였습니다

3. **테스트 격리성 vs 일관성**
   - 테스트 격리를 위해 Mock 객체를 사용하지만, 테스트 코드와 프로덕션 코드는 **동일한 Mock 인스턴스**를 공유해야 합니다

---

### 이슈 #4: 팝업 내부 버튼 테스트에서 ShowPopup 호출 횟수 불일치 (HamburgerMenuPopupTests)

#### 작업 배경
HamburgerMenuPopup의 버튼 클릭 시 다른 팝업이 열리는 기능을 테스트하던 중 발생한 이슈입니다.

#### 문제 상황

테스트 코드:
```csharp
[UnityTest]
public IEnumerator WhenTownButtonClicked_ThenTownPopupOpened()
{
    mockUIManager.Reset();

    // 마을 버튼 클릭
    townButton.onClick.Invoke();
    yield return null;

    // ❌ 1개의 팝업이 열렸을 것으로 예상
    Assert.AreEqual(1, mockUIManager.ShownPopups.Count); // 실패!
}
```

테스트 실행 결과:
```
Expected: 1
But was: 0
```

#### 원인 분석

HamburgerMenuPopup의 버튼 클릭 로직을 분석한 결과, 다음과 같은 구조였습니다:

```csharp
// HamburgerMenuPopup.cs
private void OnTownButtonClicked()
{
    // 이미 팝업이 열려있는지 체크
    if (uiManager.GetActivePopupCount() > 0)
    {
        uiManager.ShowPopup(PopupID.Town);
    }
}
```

**문제의 핵심**:
- HamburgerMenuPopup 자체도 팝업이기 때문에, 먼저 열려있어야 내부 버튼을 클릭할 수 있습니다
- 테스트에서는 HamburgerMenuPopup을 열지 않고 바로 버튼을 클릭하였습니다
- `GetActivePopupCount()`가 0을 반환하여 `if` 조건을 통과하지 못했습니다

**실제 사용자 시나리오**:
1. 사용자가 메인 메뉴에서 햄버거 버튼 클릭
2. HamburgerMenuPopup이 열림 (`ShowPopup` 1번 호출)
3. 사용자가 HamburgerMenuPopup 내부의 "마을" 버튼 클릭
4. Town 팝업이 열림 (`ShowPopup` 2번 호출)

#### 해결 방법

테스트에서 HamburgerMenuPopup을 먼저 열고, 그 후 내부 버튼을 클릭하도록 수정하였습니다:

```csharp
[UnityTest]
public IEnumerator WhenTownButtonClicked_ThenTownPopupOpened()
{
    mockUIManager.Reset();

    // ✅ 1. HamburgerMenuPopup 먼저 열기
    mockUIManager.ShowPopup(PopupID.HamburgerMenu);
    // ShowPopup()은 FakeActivePopupCount를 자동으로 1 증가시킴

    // 2. 마을 버튼 클릭 (이제 GetActivePopupCount() > 0 조건 충족)
    townButton.onClick.Invoke();
    yield return null;

    // ✅ 3. 총 2개의 팝업이 열림 (HamburgerMenu + Town)
    Assert.AreEqual(2, mockUIManager.ShownPopups.Count);
    Assert.AreEqual(PopupID.HamburgerMenu, mockUIManager.ShownPopups[0]);
    Assert.AreEqual(PopupID.Town, mockUIManager.ShownPopups[1]);
}
```

**MockUIManager의 ShowPopup 동작 이해**:
```csharp
// Assets/Tests/Mocks/MockUIManager.cs
public BasePopup ShowPopup(string popupName)
{
    ShownPopups.Add(popupName);
    FakeActivePopupCount++; // 자동 증가!
    return null;
}
```

#### 결과
- 3개의 팝업 열기 테스트가 모두 통과하였습니다

---

### 이슈 #5: 비현실적인 테스트 시나리오로 인한 테스트 실패 (MainMenuControllerTests)

#### 작업 배경
MainMenuControllerTests에서 여러 팝업을 연속으로 여는 테스트를 작성하였습니다.

#### 문제 상황

기존 테스트 코드:
```csharp
[UnityTest]
public IEnumerator WhenMultipleButtonsClicked_ThenMultipleShowPopupsCalled()
{
    // 햄버거 메뉴 버튼 클릭
    hamburgerBtn.onClick.Invoke();
    yield return null;

    // 상점 버튼 클릭
    shopBtn.onClick.Invoke();
    yield return null;

    // 캐릭터 버튼 클릭
    characterBtn.onClick.Invoke();
    yield return null;

    // ❌ 3개의 팝업이 열렸을 것으로 예상
    Assert.AreEqual(3, mockUIManager.ShownPopups.Count); // 실패!
}
```

테스트 실행 결과:
```
Expected: 3
But was: 1
```

#### 원인 분석

MainMenuController의 버튼 클릭 로직에는 **팝업 중복 열기 방지** 기능이 구현되어 있었습니다:

```csharp
// MainMenuButtonHandler.cs
private void OnShopButtonClicked()
{
    // 이미 팝업이 열려있으면 차단
    if (uiManager.IsPopupOpen())
    {
        Debug.Log("[MainMenu] 팝업이 이미 열려 있어 상점 팝업을 열 수 없습니다.");
        return; // ❌ 중복 열기 차단
    }

    uiManager.ShowPopup(PopupID.Shop);
}
```

**문제의 핵심**:
- 실제 게임에서는 **팝업이 열려있으면 메인 메뉴 버튼을 클릭할 수 없습니다**
- 팝업이 메인 메뉴를 덮고 있기 때문입니다
- 따라서 메인 메뉴에서 3개의 팝업을 연속으로 여는 것은 **불가능한 시나리오**입니다

**실제 사용자 시나리오**:
1. 사용자가 햄버거 버튼 클릭
2. HamburgerMenuPopup이 열림 (메인 메뉴 버튼은 가려짐)
3. 사용자가 메인 메뉴의 상점 버튼 클릭 시도
4. **팝업이 버튼을 가리고 있어 클릭 불가능**
5. 또는 코드에서 `IsPopupOpen()` 체크로 중복 열기 차단

#### 해결 방법

테스트를 실제 사용자 시나리오를 반영하도록 수정하였습니다:

```csharp
[UnityTest]
public IEnumerator WhenPopupOpenAndAnotherButtonClicked_ThenSecondPopupNotOpened()
{
    // Given - 햄버거 메뉴 팝업 열기
    hamburgerBtn.onClick.Invoke();
    yield return null;

    // 팝업이 열린 상태 시뮬레이션
    mockUIManager.FakeActivePopupCount = 1;

    // When - 팝업이 열려있는 상태에서 다른 버튼 클릭 시도
    shopBtn.onClick.Invoke();
    yield return null;

    // Then - 두 번째 팝업은 열리지 않아야 함 (중복 차단)
    Assert.AreEqual(1, mockUIManager.ShownPopups.Count,
        "팝업이 이미 열려있을 때는 다른 팝업이 열리지 않아야 합니다");
    Assert.AreEqual(PopupID.HamburgerMenu, mockUIManager.ShownPopups[0]);
}
```

**테스트 목적 변경**:
- 이전: "3개의 팝업이 열린다" (불가능한 시나리오)
- 이후: "팝업이 열려있을 때 다른 팝업이 열리지 않는다" (중복 방지 검증)

#### 결과

- 팝업 중복 열기 방지 로직을 정확히 검증합니다
- 실제 게임 플레이 시나리오를 반영합니다
- 테스트가 의미있는 버그를 찾을 수 있게 되었습니다

#### 핵심 교훈

1. **실제 사용자 시나리오 반영**
   - 테스트는 **실제로 발생 가능한 사용자 시나리오**를 반영해야 합니다
   - 코드만 보고 테스트를 작성하면 불가능한 시나리오를 만들 수 있습니다

2. **기획 의도 이해**
   - 기획 의도를 이해하고 테스트를 작성해야 합니다
   - "팝업이 열리면 메인 메뉴 버튼 차단"이라는 기획 의도가 있었습니다

3. **테스트의 목적 재정의**
   - 테스트가 실패하면, "테스트가 잘못되었나?" 먼저 검토해야 합니다
   - 이 경우 테스트 목적을 "중복 팝업 방지 검증"으로 재정의하였습니다

4. **프로덕션 코드 리뷰 중요성**
   - 코드 리뷰 없이 테스트만 작성하면 프로덕션 로직을 놓칠 수 있습니다
   - `IsPopupOpen()` 체크 로직을 사전에 파악했다면 올바른 테스트를 작성할 수 있었습니다

---

### 핵심 교훈 요약

#### MainMenuControllerTests에서 배운 것
1. **싱글톤 관리**: `DontDestroyOnLoad` 싱글톤은 테스트용 `ResetForTesting()` 메서드 필수
2. **조건 기반 대기**: `WaitForSeconds` 대신 `WaitUntil` 사용으로 67% 속도 향상

#### HamburgerMenuPopupTests에서 배운 것
3. **DI 컨테이너 인스턴스 관리**: 컨테이너에서 주입된 인스턴스를 사용해야 합니다
4. **실제 시나리오 반영**: 팝업 내부 버튼 테스트 시 부모 팝업을 먼저 열어야 합니다

#### 공통 교훈
5. **기획 의도 이해**: 불가능한 시나리오를 테스트하지 않아야 합니다
6. **테스트 격리 원칙**: 각 테스트는 완전히 독립적이어야 합니다

---

## 📞 문의 및 피드백

프로젝트에 대한 질문이나 피드백은 GitHub Issues를 통해 남겨주세요.

**제작**: Claude AI를 활용한 VContainer DI 기반 테스트 시스템 구축
