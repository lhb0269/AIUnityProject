# 개발 가이드

Unity 모바일 게임 프로젝트의 개발자를 위한 실전 가이드입니다. DI 기반 아키텍처를 활용하여 새로운 기능을 추가하고, 테스트를 작성하며, 일반적인 문제를 해결하는 방법을 설명합니다.

## 목차

1. [새 팝업 추가하기](#새-팝업-추가하기)
2. [새 매니저 추가하기](#새-매니저-추가하기)
3. [테스트 작성하기](#테스트-작성하기)
4. [일반적인 패턴](#일반적인-패턴)
5. [문제 해결](#문제-해결)
6. [모범 사례](#모범-사례)

---

## 새 팝업 추가하기

### 1단계: PopupID 추가

먼저 `UIIdentifiers.cs`에 팝업 ID를 추가합니다.

```csharp
// Assets/_Project/Scripts/UI/UIIdentifiers.cs
public static class PopupID
{
    // 기존 팝업들...
    public const string HamburgerMenu = "HamburgerMenuPopup";

    // 새 팝업 추가
    public const string MyNewPopup = "MyNewPopup";  // ← 여기에 추가
}
```

### 2단계: 팝업 클래스 작성

`Assets/_Project/Scripts/UI/Popups/` 폴더에 새 팝업 클래스를 만듭니다.

```csharp
// Assets/_Project/Scripts/UI/Popups/MyNewPopup.cs
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using MobileGame.Interfaces;

namespace MobileGame.UI
{
    /// <summary>
    /// 내 새 팝업 설명
    /// </summary>
    public class MyNewPopup : BasePopup
    {
        #region 의존성 주입 (필요한 매니저만 추가)

        // BasePopup에서 이미 uiManager를 주입받으므로 별도 선언 불필요
        // 추가 매니저가 필요한 경우만 선언:
        [Inject] private IGameManager gameManager;
        [Inject] private IAudioManager audioManager;

        #endregion

        #region UI 컴포넌트 참조

        [Header("버튼")]
        [SerializeField] private Button confirmBtn;
        [SerializeField] private Button cancelBtn;

        [Header("텍스트")]
        [SerializeField] private Text titleText;

        #endregion

        #region Unity 생명주기

        protected override void Start()
        {
            base.Start();  // 반드시 호출!
            RegisterButtonEvents();
        }

        protected override void OnDestroy()
        {
            UnregisterButtonEvents();
            base.OnDestroy();  // 반드시 호출!
        }

        #endregion

        #region 버튼 이벤트

        private void RegisterButtonEvents()
        {
            if (confirmBtn != null)
                confirmBtn.onClick.AddListener(OnConfirmClicked);
            if (cancelBtn != null)
                cancelBtn.onClick.AddListener(OnCancelClicked);
        }

        private void UnregisterButtonEvents()
        {
            if (confirmBtn != null)
                confirmBtn.onClick.RemoveListener(OnConfirmClicked);
            if (cancelBtn != null)
                cancelBtn.onClick.RemoveListener(OnCancelClicked);
        }

        private void OnConfirmClicked()
        {
            Debug.Log("[MyNewPopup] 확인 버튼 클릭");

            // 예: 게임 매니저 사용
            gameManager?.StartGame();

            // 예: 오디오 재생
            audioManager?.PlaySFX("ButtonClick");

            // 팝업 닫기
            ClosePopup();
        }

        private void OnCancelClicked()
        {
            Debug.Log("[MyNewPopup] 취소 버튼 클릭");
            ClosePopup();
        }

        #endregion

        #region 팝업 생명주기 오버라이드

        public override void Show()
        {
            base.Show();
            Debug.Log($"[{PopupID.MyNewPopup}] 팝업이 열렸습니다.");

            // 팝업 열릴 때 초기화 로직
            InitializePopup();
        }

        public override void ClosePopup()
        {
            Debug.Log($"[{PopupID.MyNewPopup}] 팝업이 닫힙니다.");
            base.ClosePopup();
        }

        #endregion

        #region 커스텀 로직

        private void InitializePopup()
        {
            // 팝업 데이터 초기화
            if (titleText != null)
                titleText.text = "내 새 팝업";
        }

        #endregion
    }
}
```

### 3단계: Unity 에디터에서 프리팹 생성

1. **Hierarchy에서 UI 오브젝트 생성**:
   - Popup Canvas 하위에 빈 GameObject 생성
   - 이름: `MyNewPopup`

2. **컴포넌트 추가**:
   - `MyNewPopup` 스크립트 추가
   - UI 요소 추가 (Panel, Button, Text 등)

3. **프리팹으로 저장**:
   - Hierarchy에서 `MyNewPopup` 드래그
   - `Assets/_Project/Prefabs/UI/MyNewPopup.prefab`으로 저장

4. **참조 연결**:
   - Inspector에서 버튼, 텍스트 등을 스크립트 필드에 드래그 앤 드롭

### 4단계: GameLifetimeScope에 등록

Unity 에디터에서 `GameLifetimeScope` 오브젝트의 Inspector:

1. **Initial Popup Prefabs** 리스트 확장
2. **새 항목 추가**:
   - Popup Name: `MyNewPopup` (PopupID와 정확히 일치!)
   - Prefab: `MyNewPopup.prefab` 드래그

### 5단계: 팝업 열기

이제 다른 곳에서 팝업을 열 수 있습니다:

```csharp
// MainMenuController.cs 또는 다른 컨트롤러에서
public class SomeController : MonoBehaviour
{
    [Inject] private IUIManager uiManager;

    public void OnButtonClicked()
    {
        uiManager?.ShowPopup(PopupID.MyNewPopup);
    }
}
```

---

## 새 매니저 추가하기

### 1단계: 인터페이스 정의

```csharp
// Assets/_Project/Scripts/Interfaces/IMyManager.cs
namespace MobileGame.Interfaces
{
    public interface IMyManager
    {
        void Initialize();
        void DoSomething();
        bool IsReady { get; }
    }
}
```

### 2단계: 매니저 구현

```csharp
// Assets/_Project/Scripts/Managers/MyManager.cs
using UnityEngine;
using MobileGame.Interfaces;

namespace MobileGame.Managers
{
    /// <summary>
    /// 내 매니저 설명
    /// </summary>
    public class MyManager : MonoBehaviour, IMyManager
    {
        public bool IsReady { get; private set; }

        public void Initialize()
        {
            Debug.Log("[MyManager] 초기화 중...");
            IsReady = true;
        }

        public void DoSomething()
        {
            if (!IsReady)
            {
                Debug.LogWarning("[MyManager] 아직 초기화되지 않았습니다.");
                return;
            }

            Debug.Log("[MyManager] DoSomething 실행");
        }
    }
}
```

### 3단계: GameLifetimeScope에 등록

```csharp
// Assets/_Project/Scripts/DI/GameLifetimeScope.cs
public class GameLifetimeScope : LifetimeScope
{
    [Header("매니저 참조")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameManager gameManager;
    // ... 기존 매니저들
    [SerializeField] private MyManager myManager;  // ← 추가

    protected override void Configure(IContainerBuilder builder)
    {
        // 기존 매니저 등록...

        // 새 매니저 등록
        if (myManager != null)
        {
            builder.RegisterComponent(myManager).As<IMyManager>();
        }
        else
        {
            Debug.LogError("[GameLifetimeScope] MyManager가 할당되지 않았습니다!");
        }
    }
}
```

### 4단계: Unity 에디터에서 연결

1. Hierarchy에서 `GameLifetimeScope` GameObject 하위에 빈 GameObject 생성
2. 이름: `MyManager`
3. `MyManager` 스크립트 컴포넌트 추가
4. `GameLifetimeScope` Inspector에서 `My Manager` 필드에 드래그

### 5단계: 매니저 사용

이제 다른 클래스에서 주입받아 사용:

```csharp
using VContainer;
using MobileGame.Interfaces;

public class SomeClass : MonoBehaviour
{
    [Inject] private IMyManager myManager;

    private void Start()
    {
        myManager?.Initialize();
        myManager?.DoSomething();
    }
}
```

---

## 테스트 작성하기

### Mock 매니저 작성

```csharp
// Assets/Tests/Mocks/MockMyManager.cs
using MobileGame.Interfaces;

namespace MobileGame.Tests.Mocks
{
    public class MockMyManager : IMyManager
    {
        public bool IsReady { get; set; } = true;

        public int InitializeCallCount { get; private set; }
        public int DoSomethingCallCount { get; private set; }

        public void Initialize()
        {
            InitializeCallCount++;
        }

        public void DoSomething()
        {
            DoSomethingCallCount++;
        }

        public void Reset()
        {
            InitializeCallCount = 0;
            DoSomethingCallCount = 0;
            IsReady = true;
        }
    }
}
```

### 팝업 테스트 작성

```csharp
// Assets/Tests/PlayMode/UI/MyNewPopupTests.cs
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;
using MobileGame.UI;
using MobileGame.Tests.Mocks;
using MobileGame.Tests.Helpers;

namespace MobileGame.Tests.PlayMode.UI
{
    public class MyNewPopupTests
    {
        private LifetimeScope testScope;
        private MyNewPopup popup;
        private MockUIManager mockUIManager;
        private MockGameManager mockGameManager;
        private MockAudioManager mockAudioManager;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            // Mock 객체 생성
            mockUIManager = new MockUIManager();
            mockGameManager = new MockGameManager();
            mockAudioManager = new MockAudioManager();

            // 테스트 컨테이너 빌드
            testScope = TestContainerBuilder.CreateTestScope(builder =>
            {
                builder.Register<MockUIManager>(Lifetime.Singleton).As<IUIManager>();
                builder.Register<MockGameManager>(Lifetime.Singleton).As<IGameManager>();
                builder.Register<MockAudioManager>(Lifetime.Singleton).As<IAudioManager>();
            });

            // 팝업 생성
            var popupObj = new GameObject("TestPopup");
            popup = popupObj.AddComponent<MyNewPopup>();

            // UI 컴포넌트 추가
            var canvas = popupObj.AddComponent<Canvas>();
            var confirmBtnObj = new GameObject("ConfirmButton");
            confirmBtnObj.transform.SetParent(popupObj.transform);
            var confirmBtn = confirmBtnObj.AddComponent<Button>();

            // DI 주입
            testScope.Container.Inject(popup);

            yield return null;
        }

        [UnityTearDown]
        public IEnumerator Teardown()
        {
            if (popup != null)
                Object.Destroy(popup.gameObject);
            if (testScope != null)
                testScope.Dispose();

            yield return null;
        }

        [UnityTest]
        public IEnumerator Show_호출시_팝업이_표시됨()
        {
            // Act
            popup.Show();
            yield return null;

            // Assert
            Assert.IsTrue(popup.gameObject.activeSelf, "팝업이 활성화되어야 합니다.");
        }

        [UnityTest]
        public IEnumerator ClosePopup_호출시_UIManager의_ClosePopup_호출됨()
        {
            // Arrange
            popup.Show();
            yield return null;
            mockUIManager.Reset();

            // Act
            popup.ClosePopup();
            yield return null;

            // Assert
            Assert.AreEqual(1, mockUIManager.ClosePopupCallCount,
                "UIManager.ClosePopup이 1번 호출되어야 합니다.");
        }
    }
}
```

---

## 일반적인 패턴

### 1. 팝업 중복 열기 방지

메인 메뉴나 다른 팝업에서 팝업을 열 때:

```csharp
public void OnButtonClicked()
{
    // 이미 팝업이 열려있으면 차단
    if (IsPopupOpen()) return;

    Debug.Log("[MainMenu] 버튼 클릭");
    uiManager?.ShowPopup(PopupID.SomePopup);
}

private bool IsPopupOpen()
{
    return uiManager != null && uiManager.GetActivePopupCount() > 0;
}
```

### 2. 팝업 간 데이터 전달

팝업에 데이터를 전달하려면:

```csharp
// 1. 팝업 클래스에 데이터 설정 메서드 추가
public class MyNewPopup : BasePopup
{
    private string playerName;

    public void SetPlayerData(string name, int level)
    {
        this.playerName = name;
        titleText.text = $"{name} (Lv.{level})";
    }
}

// 2. ShowPopup 후 데이터 설정
var popup = uiManager.ShowPopup(PopupID.MyNewPopup) as MyNewPopup;
popup?.SetPlayerData("홍길동", 42);
```

### 3. 팝업 닫기 콜백

팝업이 닫힐 때 콜백 실행:

```csharp
// BasePopup에 이미 구현된 패턴
popup.onPopupClosed += () =>
{
    Debug.Log("팝업이 닫혔습니다!");
    // 후속 작업...
};
```

### 4. 조건부 매니저 사용

매니저가 null일 수 있는 경우:

```csharp
// Null 조건부 연산자 사용 (권장)
uiManager?.ShowPopup(PopupID.SomePopup);

// 또는 명시적 null 체크
if (uiManager != null)
{
    uiManager.ShowPopup(PopupID.SomePopup);
}
else
{
    Debug.LogWarning("UIManager가 주입되지 않았습니다.");
}
```

### 5. 버튼 이벤트 등록/해제 패턴

메모리 누수 방지를 위한 올바른 패턴:

```csharp
protected override void Start()
{
    base.Start();
    RegisterButtonEvents();
}

protected override void OnDestroy()
{
    UnregisterButtonEvents();
    base.OnDestroy();
}

private void RegisterButtonEvents()
{
    if (myButton != null)
        myButton.onClick.AddListener(OnMyButtonClicked);
}

private void UnregisterButtonEvents()
{
    if (myButton != null)
        myButton.onClick.RemoveListener(OnMyButtonClicked);
}
```

---

## 문제 해결

### VContainer 주입 오류

**문제**: `NullReferenceException: Object reference not set to an instance of an object`

**원인**: DI 컨테이너가 의존성을 주입하지 못함

**해결책**:

1. **GameLifetimeScope에 등록 확인**:
   ```csharp
   protected override void Configure(IContainerBuilder builder)
   {
       builder.RegisterComponent(myManager).As<IMyManager>();  // ← 이 줄이 있는지 확인
   }
   ```

2. **Inspector에서 매니저 할당 확인**:
   - GameLifetimeScope GameObject 선택
   - Inspector에서 매니저 필드가 비어있지 않은지 확인

3. **[Inject] 어트리뷰트 확인**:
   ```csharp
   [Inject] private IUIManager uiManager;  // ← [Inject] 있는지 확인
   ```

### 팝업이 열리지 않음

**문제**: `ShowPopup()` 호출해도 팝업이 나타나지 않음

**해결책**:

1. **PopupID 일치 확인**:
   ```csharp
   // UIIdentifiers.cs
   public const string MyNewPopup = "MyNewPopup";

   // GameLifetimeScope Inspector
   Popup Name: "MyNewPopup"  // ← 정확히 일치해야 함!
   ```

2. **프리팹 할당 확인**:
   - GameLifetimeScope의 Initial Popup Prefabs 리스트 확인
   - 프리팹이 null이 아닌지 확인

3. **Canvas 확인**:
   - Popup Canvas가 활성화되어 있는지 확인
   - Canvas Scaler 설정 확인

### 동적 생성 팝업 DI 주입 안됨

**문제**: `Instantiate()`로 생성한 팝업에서 `uiManager`가 null

**해결책**: UIManager에서 `container.Inject()` 호출 확인

```csharp
// UIManager.cs의 ShowPopup() 메서드
GameObject popupInstance = Instantiate(prefab, popupCanvas.transform);
BasePopup popup = popupInstance.GetComponent<BasePopup>();

// 수동 DI 주입 (필수!)
if (container != null)
{
    container.Inject(popup);
}
```

### IsPopupOpen() 업데이트 안됨

**문제**: 팝업을 닫아도 `GetActivePopupCount()`가 감소하지 않음

**원인**: BasePopup의 `ClosePopup()`에서 스택 관리 안됨

**해결책**: UIManager의 `ClosePopup()` 구현 확인

```csharp
public void ClosePopup(BasePopup popup)
{
    if (popup == null) return;

    // 스택에서 제거 (중요!)
    if (activePopupStack.Contains(popup))
    {
        activePopupStack.Remove(popup);
    }

    popup.gameObject.SetActive(false);
}
```

### Assembly Definition 참조 오류

**문제**: `The type or namespace name 'VContainer' could not be found`

**해결책**: Assembly Definition에서 VContainer 참조 추가

```json
// Assembly-CSharp.asmdef
{
    "references": [
        "GUID:b0214a6008ed146ff8f122a6a9c2f6cc"  // VContainer
    ]
}
```

---

## 모범 사례

### 1. DI 사용 원칙

✅ **올바른 예**:
```csharp
public class MyController : MonoBehaviour
{
    [Inject] private IUIManager uiManager;  // 인터페이스 주입

    public void DoSomething()
    {
        uiManager?.ShowPopup(PopupID.SomePopup);
    }
}
```

❌ **잘못된 예**:
```csharp
public class MyController : MonoBehaviour
{
    public void DoSomething()
    {
        UIManager.Instance.ShowPopup("SomePopup");  // Singleton 사용 금지!
    }
}
```

### 2. ID 기반 참조

✅ **올바른 예**:
```csharp
uiManager.ShowPopup(PopupID.HamburgerMenu);  // 상수 사용
```

❌ **잘못된 예**:
```csharp
uiManager.ShowPopup("HamburgerMenuPopup");  // 문자열 하드코딩 금지!
```

### 3. 인터페이스 기반 설계

✅ **올바른 예**:
```csharp
[Inject] private IUIManager uiManager;      // 인터페이스 타입
[Inject] private IGameManager gameManager;  // 인터페이스 타입
```

❌ **잘못된 예**:
```csharp
[Inject] private UIManager uiManager;       // 구체 클래스 금지!
[Inject] private GameManager gameManager;   // 구체 클래스 금지!
```

### 4. 팝업 생명주기 관리

✅ **올바른 예**:
```csharp
protected override void Start()
{
    base.Start();  // 반드시 호출!
    RegisterButtonEvents();
}

protected override void OnDestroy()
{
    UnregisterButtonEvents();
    base.OnDestroy();  // 반드시 호출!
}
```

❌ **잘못된 예**:
```csharp
protected override void Start()
{
    // base.Start() 호출 누락!
    RegisterButtonEvents();
}
```

### 5. Null 안전성

✅ **올바른 예**:
```csharp
public void OnButtonClicked()
{
    if (uiManager == null)
    {
        Debug.LogWarning("UIManager가 주입되지 않았습니다.");
        return;
    }

    uiManager.ShowPopup(PopupID.SomePopup);
}

// 또는 Null 조건부 연산자 사용
uiManager?.ShowPopup(PopupID.SomePopup);
```

❌ **잘못된 예**:
```csharp
public void OnButtonClicked()
{
    uiManager.ShowPopup(PopupID.SomePopup);  // NullReferenceException 위험!
}
```

### 6. 테스트 격리

✅ **올바른 예**:
```csharp
[UnitySetUp]
public IEnumerator Setup()
{
    // 매 테스트마다 새로운 Mock 생성
    mockUIManager = new MockUIManager();

    testScope = TestContainerBuilder.CreateTestScope(builder =>
    {
        builder.Register<MockUIManager>(Lifetime.Singleton).As<IUIManager>();
    });

    yield return null;
}

[UnityTearDown]
public IEnumerator Teardown()
{
    // 테스트 후 정리
    if (testScope != null)
        testScope.Dispose();
    yield return null;
}
```

❌ **잘못된 예**:
```csharp
private static MockUIManager mockUIManager;  // 공유 상태 금지!

[UnitySetUp]
public IEnumerator Setup()
{
    if (mockUIManager == null)
        mockUIManager = new MockUIManager();  // 재사용 금지!
    yield return null;
}
```

---

## 추가 리소스

### 관련 문서
- [README.md](../README.md) - 프로젝트 개요 및 아키텍처
- [SESSION_CONTEXT.md](SESSION_CONTEXT.md) - 최신 작업 컨텍스트
- [CLAUDE.md](../CLAUDE.md) - 프로젝트 가이드

### 외부 참고
- [VContainer 공식 문서](https://vcontainer.hadashikick.jp/)
- [Unity 스크립팅 API](https://docs.unity3d.com/ScriptReference/)
- [Unity Test Framework](https://docs.unity3d.com/Packages/com.unity.test-framework@latest)

---

**최종 업데이트**: 2025-01-29
**작성자**: Claude Code + 개발자
**피드백**: 이 가이드에 대한 개선 사항이나 질문은 프로젝트 이슈 트래커에 제출해 주세요.
