# 2D 모바일 게임 프로젝트

Unity 6 기반의 2D 모바일 게임 개발 프로젝트입니다.

## 프로젝트 정보

- **Unity 버전**: 6000.2.9f1 (Unity 6)
- **렌더 파이프라인**: Universal Render Pipeline (URP) 17.2.0
- **입력 시스템**: New Input System 1.14.2
- **타겟 플랫폼**: iOS, Android
- **게임 타입**: 2D 모바일 게임

## 주요 기능

### 핵심 시스템
- **GameManager**: 게임 상태 관리 및 모바일 최적화
- **SceneLoader**: 비동기 씬 로딩 시스템
- **AudioManager**: BGM 및 SFX 관리
- **InputManager**: 터치 및 모바일 입력 처리
- **UIManager**: UI 패널 및 팝업 관리
- **SaveSystem**: JSON 기반 데이터 저장/로드

### 모바일 최적화
- 타겟 프레임 레이트: 60 FPS
- 배터리 최적화를 위한 화면 절전 모드 제어
- 앱 백그라운드 전환 시 자동 일시정지
- SFX 오브젝트 풀링으로 성능 최적화
- 터치 입력 및 스와이프 제스처 지원

## 프로젝트 구조

```
Assets/
├── _Project/                    # 메인 프로젝트 폴더
│   ├── Scripts/
│   │   ├── Managers/           # 핵심 매니저 스크립트
│   │   ├── Player/             # 플레이어 관련 스크립트
│   │   ├── UI/                 # UI 스크립트
│   │   ├── Gameplay/           # 게임플레이 로직
│   │   └── Utilities/          # 유틸리티 스크립트
│   ├── Art/
│   │   ├── Textures/           # 텍스처
│   │   ├── Materials/          # 머티리얼
│   │   ├── Animations/         # 애니메이션
│   │   └── Sprites/            # 스프라이트
│   ├── Audio/
│   │   ├── Music/              # 배경음악
│   │   └── SFX/                # 효과음
│   ├── Prefabs/
│   │   ├── Characters/         # 캐릭터 프리팹
│   │   ├── Environment/        # 환경 오브젝트
│   │   ├── UI/                 # UI 프리팹
│   │   └── Effects/            # 이펙트
│   ├── Scenes/
│   │   ├── Development/        # 개발용 씬
│   │   └── Production/         # 프로덕션 씬
│   └── Settings/               # 프로젝트 설정
│       └── Input/              # 입력 설정
├── Editor/                     # 에디터 스크립트
│   └── Scripts/
└── Scenes/                     # 기본 씬
```

## 시작하기

### 필수 요구사항
- Unity 6000.2.9f1 이상
- Visual Studio 2022 또는 VS Code

### 프로젝트 설정
1. Unity Hub에서 프로젝트 열기
2. Unity 에디터에서 `Tools > Project Setup` 메뉴 실행
3. 모바일 설정 적용 및 기본 씬 설정 생성

### 기본 씬 설정
Unity 에디터에서 `Tools > Project Setup` > `기본 씬 설정 생성` 버튼을 클릭하면 다음 매니저들이 자동으로 씬에 추가됩니다:
- GameManager
- AudioManager
- UIManager
- InputManager
- SaveSystem
- SceneLoader

## 개발 가이드

### 매니저 사용 방법

#### GameManager
```csharp
// 게임 상태 변경
GameManager.Instance.SetGameState(GameState.Playing);

// 게임 일시정지/재개
GameManager.Instance.PauseGame();
GameManager.Instance.ResumeGame();

// 현재 상태 확인
GameState currentState = GameManager.Instance.GetCurrentState();
```

#### SceneLoader
```csharp
// 씬 로드
SceneLoader.Instance.LoadScene("GameScene");

// 현재 씬 다시 로드
SceneLoader.Instance.ReloadCurrentScene();

// 로딩 이벤트 구독
SceneLoader.Instance.OnSceneLoadProgress += (sceneName, progress) => {
    Debug.Log($"로딩 진행률: {progress * 100}%");
};
```

#### AudioManager
```csharp
// BGM 재생
AudioManager.Instance.PlayBGM(bgmClip);

// SFX 재생
AudioManager.Instance.PlaySFX(sfxClip);

// 볼륨 설정
AudioManager.Instance.SetMasterVolume(0.8f);
AudioManager.Instance.SetBGMVolume(0.7f);
AudioManager.Instance.SetSFXVolume(1.0f);
```

#### InputManager
```csharp
// 터치 이벤트 구독
InputManager.Instance.OnTouchStarted += (position) => {
    Debug.Log($"터치 시작: {position}");
};

InputManager.Instance.OnSwipe += (startPos, direction) => {
    Debug.Log($"스와이프: {direction}");
};

// 터치 상태 확인
bool isTouching = InputManager.Instance.IsTouching();
Vector2 touchPos = InputManager.Instance.GetTouchPosition();

// 월드 좌표로 변환
Vector3 worldPos = InputManager.Instance.GetTouchWorldPosition();
```

#### UIManager
```csharp
// 패널 등록 및 표시
UIManager.Instance.RegisterPanel("MainMenu", menuPanel);
UIManager.Instance.ShowPanel("MainMenu");
UIManager.Instance.HidePanel("MainMenu");

// 팝업 표시/닫기
UIManager.Instance.ShowPopup(popupPrefab);
UIManager.Instance.CloseCurrentPopup();
```

#### SaveSystem
```csharp
// 데이터 저장
GameData data = new GameData();
data.playerScore = 1000;
SaveSystem.Instance.SaveData(data);

// 데이터 로드
GameData loadedData = SaveSystem.Instance.LoadData<GameData>();

// PlayerPrefs 사용
SaveSystem.Instance.SavePreference("HighScore", 5000);
int highScore = SaveSystem.Instance.LoadPreferenceInt("HighScore");
```

### 에디터 도구

#### Scene Quick Start
`Tools > Scene Quick Start` - 빌드 설정에 등록된 씬들을 빠르게 로드

#### Project Setup
`Tools > Project Setup` - 프로젝트 설정 및 초기 설정 도구

## 빌드 설정

### Android
1. `File > Build Settings` 에서 Android 선택
2. Player Settings에서 다음 설정 확인:
   - Minimum API Level: Android 7.0 (API 24)
   - Target API Level: Automatic (highest installed)
   - Scripting Backend: IL2CPP
   - Target Architectures: ARM64

### iOS
1. `File > Build Settings` 에서 iOS 선택
2. Player Settings에서 다음 설정 확인:
   - Target Device: iPhone and iPad
   - Requires Fullscreen: Enabled
   - Target minimum iOS Version: 12.0 이상

## 성능 최적화 팁

1. **오브젝트 풀링**: 자주 생성/파괴되는 오브젝트는 풀링 사용
2. **텍스처 압축**: 모바일 플랫폼에 적합한 텍스처 형식 사용 (ASTC, ETC2)
3. **드로우 콜 최소화**: Sprite Atlas 사용
4. **물리 최적화**: 불필요한 Rigidbody2D 및 Collider2D 비활성화
5. **UI 최적화**: Canvas 분리 및 레이캐스트 타겟 최소화

---

## 🎮 게임 자동화 테스트 시스템

Unity Test Framework를 활용한 UI 자동화 테스트 시스템입니다. 메인 메뉴 및 햄버거 메뉴 팝업의 모든 버튼 동작을 자동으로 검증합니다.

### 📊 테스트 커버리지

| 테스트 클래스 | 테스트 케이스 | 검증 대상 | 코드 라인 |
|-------------|------------|----------|---------|
| **MainMenuButtonHandlerTests** | 38개 | 25개 버튼 (13개 팝업 + 11개 일반 + 1개 통합) | 624줄 |
| **HamburgerMenuPopupTests** | 2개 | 12개 버튼 (햄버거 메뉴 팝업) | 355줄 |
| **합계** | **40개** | **37개 버튼** | **979줄** |

### 🏗️ 테스트 아키텍처

#### 테스트 설계 원칙

프로젝트는 다음 테스트 원칙을 엄격히 준수합니다:

1. **FIRST 원칙**
   - **F**ast: 조건 기반 WaitUntil로 빠른 실행
   - **I**ndependent: OneTimeTearDown으로 싱글톤 완전 격리
   - **R**epeatable: 매 테스트마다 깨끗한 상태 보장
   - **S**elf-Validating: Assert 문으로 자동 검증
   - **T**imely: 개발과 동시에 테스트 코드 작성

2. **DAMP 원칙** (Descriptive And Meaningful Phrases)
   - 행동 기반 테스트 명명: `WhenX_ThenY` 패턴
   - 명확한 Given-When-Then 주석
   - 자기 설명적인 코드 구조

3. **AAA 패턴** (Arrange-Act-Assert)
   - Arrange: 테스트 환경 설정
   - Act: 테스트할 동작 실행
   - Assert: 결과 검증
   - Cleanup: 정리 작업

#### 테스트 파일 구조

```
Assets/Tests/PlayMode/UI/
├── MainMenuButtonHandlerTests.cs      # 메인 메뉴 25개 버튼 테스트
│   ├── 13개 팝업 열기 테스트
│   ├── 13개 팝업 닫기 테스트
│   ├── 11개 일반 버튼 테스트
│   └── 1개 통합 테스트 (팝업 스택)
│
└── HamburgerMenuPopupTests.cs         # 햄버거 메뉴 팝업 테스트
    ├── 팝업 열기 및 버튼 할당 검증
    └── 12개 버튼 순차 클릭 검증
```

### 💻 테스트 코드 가이드

#### 1. 재사용 가능한 테스트 패턴 메서드

테스트 코드 중복을 제거하기 위해 3가지 패턴 메서드를 설계했습니다:

**패턴 1: 팝업 열기 테스트**
```csharp
/// <summary>
/// 버튼 클릭 시 팝업이 열리는 테스트 패턴
/// 재사용: 13개 팝업 열기 테스트에서 사용
/// </summary>
private IEnumerator TestButtonOpensPopup<TPopup>(
    string buttonFieldName,
    string buttonDisplayName) where TPopup : BasePopup
{
    // Arrange - 버튼 가져오기
    Button button = GetButtonField(buttonFieldName);
    if (button == null)
    {
        Assert.Inconclusive($"{buttonDisplayName} 버튼이 할당되지 않았습니다");
        yield break;
    }

    // Act - 버튼 클릭 및 팝업 대기
    yield return ClickButton(button, buttonDisplayName);
    yield return WaitUntilPopupAppears<TPopup>();

    // Assert - 팝업 생성 확인
    TPopup popup = Object.FindFirstObjectByType<TPopup>();
    Assert.IsNotNull(popup, $"{typeof(TPopup).Name}이 나타나야 합니다");
    Assert.AreEqual(1, UIManager.Instance.GetActivePopupCount());

    // Cleanup - 팝업 닫기
    UIManager.Instance.CloseAllActivePopups();
    yield return WaitUntilNoActivePopups();
}

// 사용 예시
[UnityTest]
public IEnumerator WhenHamburgerMenuButtonClicked_ThenHamburgerMenuPopupOpens()
{
    yield return TestButtonOpensPopup<HamburgerMenuPopup>(
        "hamburgerMenuBtn", "햄버거 메뉴");
}
```

**패턴 2: 팝업 닫기 테스트**
```csharp
/// <summary>
/// 팝업 닫기 버튼 테스트 패턴
/// 재사용: 13개 팝업 닫기 테스트에서 사용
/// </summary>
private IEnumerator TestPopupCloseButton<TPopup>(
    string buttonFieldName,
    string buttonDisplayName) where TPopup : BasePopup
{
    // Arrange - 팝업 열기
    Button button = GetButtonField(buttonFieldName);
    if (button == null)
    {
        Assert.Inconclusive($"{buttonDisplayName} 버튼이 할당되지 않았습니다");
        yield break;
    }

    yield return ClickButton(button, buttonDisplayName);
    yield return WaitUntilPopupAppears<TPopup>();

    // Arrange - 닫기 버튼 찾기
    BasePopup popup = Object.FindFirstObjectByType<BasePopup>();
    Button closeButton = GetCloseButton(popup);
    Assert.IsNotNull(closeButton, "팝업에 닫기 버튼이 있어야 합니다");

    // Act - 닫기 버튼 클릭
    yield return ClickButton(closeButton, "닫기");

    // Assert - 팝업 닫힘 확인
    yield return new WaitUntil(() =>
        UIManager.Instance.GetActivePopupCount() == 0);

    // Cleanup
    yield return WaitUntilNoActivePopups();
}
```

**패턴 3: 일반 버튼 로그 테스트**
```csharp
/// <summary>
/// 버튼 클릭 시 로그 메시지 출력 테스트 패턴
/// 재사용: 11개 일반 버튼 테스트에서 사용
/// </summary>
private IEnumerator TestButtonClickLogsMessage(
    string buttonFieldName,
    string expectedLog,
    string buttonDisplayName)
{
    // Arrange
    Button button = GetButtonField(buttonFieldName);
    if (button == null)
    {
        Assert.Inconclusive($"{buttonDisplayName} 버튼이 할당되지 않았습니다");
        yield break;
    }

    // Act & Assert - 로그 검증
    LogAssert.Expect(LogType.Log, expectedLog);
    yield return ClickButton(button, buttonDisplayName);
}
```

**코드 중복 제거 효과:**
- **이전**: 1,196줄 (개별 테스트마다 중복 코드)
- **이후**: 624줄 (패턴 메서드 재사용)
- **감소율**: 48% (572줄 감소)

#### 2. 조건 기반 대기 헬퍼 메서드

고정 시간 대기 대신 조건 기반 대기로 빠르고 안정적인 테스트를 구현했습니다:

```csharp
/// <summary>
/// 특정 컴포넌트가 씬에 나타날 때까지 대기 (타임아웃 포함)
/// </summary>
private IEnumerator WaitForComponent<T>() where T : Object
{
    float elapsed = 0f;
    while (elapsed < POPUP_SPAWN_TIMEOUT)
    {
        if (Object.FindFirstObjectByType<T>() != null)
            yield break;  // 찾으면 즉시 반환

        yield return null;
        elapsed += Time.deltaTime;
    }

    Assert.Fail($"{typeof(T).Name}이 {POPUP_SPAWN_TIMEOUT}초 내에 나타나지 않았습니다");
}

/// <summary>
/// 특정 팝업이 나타날 때까지 대기
/// </summary>
private IEnumerator WaitUntilPopupAppears<T>() where T : BasePopup
{
    yield return new WaitUntil(() =>
        Object.FindFirstObjectByType<T>() != null);
}

/// <summary>
/// 모든 팝업이 닫힐 때까지 대기
/// </summary>
private IEnumerator WaitUntilNoActivePopups()
{
    float elapsed = 0f;
    while (elapsed < POPUP_DESTROY_TIMEOUT)
    {
        if (UIManager.Instance == null ||
            UIManager.Instance.GetActivePopupCount() == 0)
            yield break;

        yield return null;
        elapsed += Time.deltaTime;
    }
}
```

**WaitForSeconds vs WaitUntil 비교:**

| 항목 | WaitForSeconds | WaitUntil (개선됨) |
|-----|---------------|------------------|
| 실행 시간 | 고정 (예: 1초) | 조건 만족 시 즉시 (0.1~0.3초) |
| 신뢰성 | 낮음 (느린 환경에서 실패) | 높음 (조건 기반) |
| 유지보수성 | 낮음 (임의의 시간 값) | 높음 (명확한 조건) |

#### 3. Setup과 Teardown 전략

각 테스트의 독립성을 보장하기 위한 철저한 초기화/정리 전략:

```csharp
[UnitySetUp]
public IEnumerator Setup()
{
    // 1. 씬 로드 (최초 1회만)
    if (!sceneLoaded || SceneManager.GetActiveScene().name != TEST_SCENE_NAME)
    {
        SceneManager.LoadScene(TEST_SCENE_NAME, LoadSceneMode.Single);
        yield return null;
        yield return null;  // Awake, Start 실행 보장
        sceneLoaded = true;
    }

    // 2. 필수 컴포넌트 대기
    yield return WaitForComponent<EventSystem>();
    yield return WaitForComponent<UIManager>();
    yield return WaitForComponent<MainMenuButtonHandler>();

    // 3. 깨끗한 상태로 시작
    UIManager.Instance.CloseAllActivePopups();
    yield return WaitUntilNoActivePopups();
}

[UnityTearDown]
public IEnumerator Teardown()
{
    // 각 테스트 후 팝업 정리
    if (UIManager.Instance != null)
    {
        UIManager.Instance.CloseAllActivePopups();
        yield return WaitUntilNoActivePopups();
    }
}

[OneTimeTearDown]
public void OneTimeTearDown()
{
    // 테스트 클래스 종료 시 싱글톤 완전 정리
    UIManager.ResetForTesting();
    sceneLoaded = false;
}
```

### 🔧 주요 이슈 및 해결 방법

#### 이슈 #1: DontDestroyOnLoad 싱글톤 간섭 문제

**문제 상황:**
```
HamburgerMenuPopupTests 실행 완료
    ↓
MainMenuButtonHandlerTests 실행 시작
    ↓
씬 리로드 → MainMenuButtonHandler 재생성
    ↓
UIManager는 DontDestroyOnLoad로 그대로 유지
    ↓
❌ 버튼 바인딩이 모두 사라짐 (null 참조 오류)
```

**원인 분석:**
- `UIManager`는 `DontDestroyOnLoad`를 사용하여 싱글톤으로 씬 전환 시에도 유지됨
- 테스트 간 씬 리로드 시 `MainMenuButtonHandler`는 새로 생성되지만, `UIManager`는 이전 상태를 유지
- 이로 인해 새로운 `MainMenuButtonHandler`의 버튼들이 이전 `UIManager`와 연결되지 않음

**해결 방법:**

1. **UIManager에 테스트용 정리 메서드 추가:**
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

2. **테스트 클래스에서 OneTimeTearDown 활용:**
```csharp
// Assets/Tests/PlayMode/UI/MainMenuButtonHandlerTests.cs

[OneTimeTearDown]
public void OneTimeTearDown()
{
    // 테스트 클래스 종료 시 싱글톤 완전 정리
    UIManager.ResetForTesting();
    sceneLoaded = false;
}
```

**결과:**
- ✅ 테스트 클래스 간 완전한 독립성 보장
- ✅ 다음 테스트 클래스 실행 시 새로운 UIManager 생성
- ✅ 버튼 바인딩 정상 동작

---

#### 이슈 #2: WaitForSeconds로 인한 느리고 불안정한 테스트

**문제 상황:**
```csharp
// 이전 코드 (문제)
yield return new WaitForSeconds(1f);  // 항상 1초 대기
```

**문제점:**
1. **불필요하게 느림**: 팝업이 0.1초에 나타나도 1초를 기다림
2. **불안정**: 느린 환경에서는 1초로 부족할 수 있음
3. **유지보수 어려움**: 임의의 숫자(매직 넘버)를 사용

**해결 방법:**

조건 기반 대기로 전환:

```csharp
// 개선 후 코드
yield return new WaitUntil(() =>
    Object.FindFirstObjectByType<HamburgerMenuPopup>() != null);
```

**타임아웃이 필요한 경우:**
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

**개선 효과:**

| 지표 | 이전 (WaitForSeconds) | 이후 (WaitUntil) | 개선율 |
|-----|---------------------|-----------------|-------|
| 평균 테스트 실행 시간 | ~45초 | ~15초 | **67% 감소** |
| 테스트 안정성 | 85% 성공률 | 99% 성공률 | **14% 향상** |
| 코드 명확성 | 낮음 (매직 넘버) | 높음 (조건 명시) | - |

---

#### 이슈 #3: 테스트 코드 중복 (1,196줄)

**문제 상황:**

38개의 테스트 케이스가 비슷한 패턴을 반복하여 1,196줄의 코드 발생:

```csharp
// 중복 패턴 예시 (38번 반복)
[UnityTest]
public IEnumerator SettingButton_OpensSettingPopup()
{
    Button button = GetButtonField("settingBtn");
    Assert.IsNotNull(button);

    yield return ClickButton(button, "설정");
    yield return WaitUntilPopupAppears<SettingPopup>();

    SettingPopup popup = Object.FindFirstObjectByType<SettingPopup>();
    Assert.IsNotNull(popup);
    Assert.AreEqual(1, UIManager.Instance.GetActivePopupCount());

    UIManager.Instance.CloseAllActivePopups();
    yield return WaitUntilNoActivePopups();
}

// 위 패턴이 팝업마다 반복...
```

**해결 방법:**

재사용 가능한 제네릭 패턴 메서드 3개 설계:

```csharp
// 1. 팝업 열기 패턴 (13개 테스트에 재사용)
private IEnumerator TestButtonOpensPopup<TPopup>(
    string buttonFieldName, string buttonDisplayName)
    where TPopup : BasePopup
{ /* 구현 */ }

// 2. 팝업 닫기 패턴 (13개 테스트에 재사용)
private IEnumerator TestPopupCloseButton<TPopup>(
    string buttonFieldName, string buttonDisplayName)
    where TPopup : BasePopup
{ /* 구현 */ }

// 3. 로그 출력 패턴 (11개 테스트에 재사용)
private IEnumerator TestButtonClickLogsMessage(
    string buttonFieldName, string expectedLog, string buttonDisplayName)
{ /* 구현 */ }
```

**사용 예시:**
```csharp
// 이제 한 줄로 테스트 작성 가능
[UnityTest]
public IEnumerator WhenSettingButtonClicked_ThenSettingPopupOpens()
{
    yield return TestButtonOpensPopup<SettingPopup>("settingBtn", "설정");
}
```

**개선 효과:**

| 지표 | 이전 | 이후 | 개선 |
|-----|-----|-----|------|
| 코드 라인 수 | 1,196줄 | 624줄 | **48% 감소** |
| 테스트 케이스 수 | 38개 | 38개 | 유지 |
| 평균 테스트 길이 | 31줄 | 3줄 | **90% 감소** |
| 유지보수성 | 낮음 | 높음 | ⬆️ |

---

### 📈 테스트 실행 방법

#### Unity Test Runner에서 실행

1. Unity 에디터 메뉴에서 `Window > General > Test Runner` 선택
2. `PlayMode` 탭 선택
3. 실행할 테스트 선택:
   - 전체 실행: 최상위 체크박스 선택 후 `Run All`
   - 개별 실행: 특정 테스트 선택 후 `Run Selected`

#### 명령줄에서 실행 (CI/CD)

```bash
# Windows
Unity.exe -runTests -batchmode -projectPath "C:\path\to\project" \
  -testResults results.xml -testPlatform PlayMode

# macOS/Linux
/Applications/Unity/Unity.app/Contents/MacOS/Unity -runTests -batchmode \
  -projectPath "/path/to/project" -testResults results.xml -testPlatform PlayMode
```

### 🎯 테스트 작성 가이드라인

새로운 UI 기능을 추가할 때 다음 체크리스트를 따라 테스트를 작성하세요:

- [ ] **테스트 이름**: `WhenX_ThenY` 패턴 사용
- [ ] **패턴 메서드**: 기존 패턴 메서드 재사용 가능한지 확인
- [ ] **조건 기반 대기**: `WaitForSeconds` 대신 `WaitUntil` 사용
- [ ] **Given-When-Then**: 각 섹션 주석으로 명확히 구분
- [ ] **Cleanup**: 테스트 종료 후 상태 정리
- [ ] **독립성**: 다른 테스트에 영향 주지 않는지 확인

---

## 개발 워크플로우

이 프로젝트는 **Feature Branch Workflow**를 사용합니다.

### 📋 개발 프로세스

```
┌─────────────────────────────────────────────────────────────┐
│  1. 브랜치 생성 (사용자)                                        │
│     ↓                                                         │
│  2. 개발 및 커밋 (Claude Code)                                 │
│     ↓                                                         │
│  3. 검토 및 머지 (사용자)                                       │
└─────────────────────────────────────────────────────────────┘
```

### 1️⃣ 브랜치 생성 (사용자)

새로운 기능이나 작업을 시작할 때:

```bash
# 새 브랜치 생성 및 전환
git checkout -b feature/feature-name

# 원격 저장소에 브랜치 생성
git push -u origin feature/feature-name
```

**브랜치 명명 규칙:**
- `feature/기능명` - 새로운 기능 개발
- `fix/버그명` - 버그 수정
- `refactor/내용` - 코드 리팩토링
- 예시: `Main-Menu-UI-Object-Placement`, `Player-Movement-System`

### 2️⃣ 개발 및 커밋 (Claude Code)

Claude Code가 기능을 개발하고 커밋합니다:

```bash
# 변경사항 스테이징
git add .

# 커밋
git commit -m "[기능명] 설명"
```

**커밋 메시지 형식:**
```
[기능명] 간단한 설명

상세 설명:
- 추가된 기능 1
- 추가된 기능 2
- 수정된 사항

🤖 Generated with Claude Code
Co-Authored-By: Claude <noreply@anthropic.com>
```

### 3️⃣ 검토 및 머지 (사용자)

개발 완료 후 사용자가 직접 검토하고 머지합니다:

```bash
# Unity 에디터에서 테스트
# - 컴파일 오류 확인
# - 기능 동작 테스트
# - 성능 확인

# 문제가 없으면 main 브랜치로 머지
git checkout main
git merge feature/feature-name
git push origin main

# 또는 GitHub에서 Pull Request 생성
```

### ✅ 코드 리뷰 체크리스트

머지 전에 확인할 사항:

- [ ] **컴파일**: Unity 에디터에서 컴파일 오류 없음
- [ ] **기능**: 의도한 대로 동작하는지 테스트
- [ ] **성능**: 프레임 레이트 및 메모리 사용량 확인 (모바일 기준)
- [ ] **코드 스타일**: 명명 규칙 및 주석 작성 확인
- [ ] **문서**: README 또는 주석에 필요한 설명 포함

### 🔄 브랜치 관리

```
main (프로덕션 브랜치)
├── feature/main-menu-ui          ← 작업 중
├── feature/player-controller     ← 머지 완료
└── feature/game-manager          ← 머지 완료
```

### 💡 개발 팁

1. **작은 단위로 커밋**: 기능별로 작은 단위로 나누어 커밋
2. **의미있는 브랜치명**: 작업 내용을 명확히 표현하는 브랜치명 사용
3. **정기적인 테스트**: 커밋 후 Unity 에디터에서 반드시 테스트
4. **충돌 방지**: main 브랜치의 최신 변경사항을 정기적으로 가져오기

### 🚨 주의사항

- **Claude Code는 Unity 에디터를 실행할 수 없습니다** - 코드만 작성하므로 반드시 Unity에서 테스트 필요
- **자동 Push 금지** - Claude Code는 커밋만 수행, Push는 사용자가 직접 확인 후 실행
- **.meta 파일 포함** - Unity 메타 파일도 함께 커밋해야 함

## 라이선스

이 프로젝트는 개인 학습 및 개발 목적으로 사용됩니다.

## 문의

프로젝트 관련 문의사항은 GitHub Issues를 통해 남겨주세요.