# 싱글톤 vs DI: 오해와 진실

## 🤔 질문: Mock 매니저를 모두 싱글톤으로 등록하면 DI에 위배되지 않나요?

### 결론부터 말하면: **위배되지 않습니다!** ✅

여기서 사용하는 "싱글톤"은 전통적인 안티패턴 싱글톤이 아니라 **DI 컨테이너의 Lifetime 관리 방식**입니다.

---

## 📊 두 가지 싱글톤의 차이

### 1. ❌ 안티패턴 싱글톤 (DI 위배)

```csharp
// 안티패턴 싱글톤 (프로덕션 코드에서 사용 중)
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance  // ❌ 전역 접근
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }

    // 사용: UIManager.Instance.ShowPopup()  ❌ 하드코딩된 의존성
}
```

**문제점**:
- 전역 상태로 인한 테스트 격리 불가능
- 의존성이 코드에 하드코딩됨
- Mock으로 교체 불가능
- 테스트 간 상태 공유로 인한 간섭

---

### 2. ✅ DI 컨테이너 싱글톤 (올바른 방식)

```csharp
// TestContainerBuilder.cs
containerBuilder.Register<MockUIManager>(Lifetime.Singleton).As<IUIManager>();
//                                       ^^^^^^^^^^^^^^^^
//                                       DI 컨테이너의 Lifetime 관리
```

**핵심 차이점**:

| 구분 | 안티패턴 싱글톤 | DI 컨테이너 싱글톤 |
|-----|----------------|-------------------|
| **인스턴스 생성** | 클래스 내부에서 직접 | DI 컨테이너가 관리 |
| **접근 방식** | `UIManager.Instance` (전역) | `container.Resolve<IUIManager>()` (주입) |
| **의존성 주입** | 불가능 (하드코딩) | 가능 (인터페이스 기반) |
| **테스트 격리** | 불가능 (전역 상태) | 가능 (스코프별 인스턴스) |
| **Mock 교체** | 불가능 | 가능 |
| **범위** | 애플리케이션 전체 | 컨테이너 스코프 내 |

---

## 🎯 DI 컨테이너의 Lifetime.Singleton이란?

VContainer의 `Lifetime.Singleton`은 **컨테이너 스코프 내에서 단 하나의 인스턴스만 생성**한다는 의미입니다.

```csharp
// 각 테스트마다 독립적인 스코프 생성
[UnitySetUp]
public IEnumerator Setup()
{
    // 테스트 A의 스코프
    testScope = TestContainerBuilder.CreateCustomScope(includeUI: true);

    // 이 스코프 내에서만 MockUIManager가 싱글톤
    mockUIManager = TestContainerBuilder.GetMockUIManager(testScope.Container);
}

[UnityTearDown]
public IEnumerator Teardown()
{
    // 스코프 파괴 → 싱글톤 인스턴스도 파괴
    Object.Destroy(testScope.gameObject);
}
```

**테스트 A와 테스트 B의 관계**:
```
테스트 A 실행:
  → testScope A 생성
  → MockUIManager 인스턴스 A 생성 (스코프 A 내 싱글톤)
  → 테스트 완료
  → testScope A 파괴 → 인스턴스 A도 파괴

테스트 B 실행:
  → testScope B 생성 (완전히 새로운 스코프!)
  → MockUIManager 인스턴스 B 생성 (스코프 B 내 싱글톤)
  → 테스트 완료
  → testScope B 파괴 → 인스턴스 B도 파괴
```

✅ **각 테스트는 완전히 격리됩니다!**

---

## 🔍 왜 Lifetime.Singleton을 사용하는가?

### 1. **컴포넌트 내부에서 동일한 인스턴스 사용 보장**

```csharp
public class HamburgerMenuPopup : BasePopup
{
    [Inject]
    private IUIManager uiManager;  // 인스턴스 A 주입

    private void OnTownButtonClicked()
    {
        uiManager.ShowPopup(PopupID.Town);  // 인스턴스 A 사용
    }

    private void OnNoticeButtonClicked()
    {
        uiManager.ShowPopup(PopupID.Notice);  // 같은 인스턴스 A 사용 ✅
    }
}
```

만약 `Lifetime.Transient`(매번 새 인스턴스)를 사용하면:
```csharp
// ❌ Lifetime.Transient 사용 시
OnTownButtonClicked() → uiManager (인스턴스 A) 사용
OnNoticeButtonClicked() → uiManager (인스턴스 B) 사용  // 다른 인스턴스!

// 문제: ShowPopup 호출 추적 불가능
mockUIManager.ShownPopups.Count  // 인스턴스 A의 카운트만 확인
```

### 2. **테스트 검증 가능**

```csharp
[UnityTest]
public IEnumerator WhenMultipleButtonsClicked_ThenAllPopupsTracked()
{
    // 테스트 코드에서 사용하는 Mock
    mockUIManager = TestContainerBuilder.GetMockUIManager(testScope.Container);
    // → 인스턴스 A

    // HamburgerMenuPopup에 주입된 Mock
    testScope.Container.Inject(popup);
    // → popup.uiManager = 인스턴스 A (동일!)

    // 버튼 클릭
    townBtn.onClick.Invoke();    // 인스턴스 A 사용
    noticeBtn.onClick.Invoke();  // 인스턴스 A 사용

    // 검증 가능 ✅
    Assert.AreEqual(2, mockUIManager.ShownPopups.Count);
}
```

### 3. **실제 프로덕션 환경 모사**

프로덕션 코드에서도 UIManager는 싱글톤처럼 동작합니다:
```csharp
// 실제 게임에서
UIManager.Instance.ShowPopup("Town");    // 같은 인스턴스
UIManager.Instance.ShowPopup("Notice");  // 같은 인스턴스
```

테스트에서도 동일한 동작을 모사해야 합니다:
```csharp
// 테스트에서 (DI 컨테이너 싱글톤)
mockUIManager.ShowPopup("Town");    // 같은 인스턴스
mockUIManager.ShowPopup("Notice");  // 같은 인스턴스
```

---

## 🎓 DI 원칙 준수 여부 확인

### SOLID 원칙 체크리스트

#### 1. ✅ **Dependency Inversion Principle (의존성 역전 원칙)**
```csharp
// 구체 클래스가 아닌 인터페이스에 의존
public class HamburgerMenuPopup : BasePopup
{
    [Inject]
    private IUIManager uiManager;  // ✅ 인터페이스 의존
    //      ^^^^^^^^^
    //      MockUIManager가 아닌 IUIManager
}

// 테스트 시 Mock 주입
containerBuilder.Register<MockUIManager>(Lifetime.Singleton).As<IUIManager>();
//                                                           ^^^
//                                                           인터페이스로 등록
```

#### 2. ✅ **Testability (테스트 가능성)**
```csharp
// 각 테스트마다 독립적인 스코프
testScope1 = CreateCustomScope();  // 스코프 1
testScope2 = CreateCustomScope();  // 스코프 2 (완전히 독립)

// Mock으로 쉽게 교체 가능
containerBuilder.Register<MockUIManager>().As<IUIManager>();  // 테스트
containerBuilder.Register<UIManager>().As<IUIManager>();       // 프로덕션
```

#### 3. ✅ **Isolation (격리성)**
```csharp
// 테스트 간 격리
[UnityTearDown]
public IEnumerator Teardown()
{
    Object.Destroy(testScope.gameObject);  // 스코프 파괴 → 인스턴스도 파괴
}
```

---

## 🆚 다른 Lifetime과의 비교

### VContainer의 3가지 Lifetime

```csharp
// 1. Singleton: 스코프 내 단 하나의 인스턴스
builder.Register<MockUIManager>(Lifetime.Singleton);
// → 첫 Resolve 시 생성, 이후 같은 인스턴스 반환

// 2. Transient: 매번 새 인스턴스
builder.Register<MockUIManager>(Lifetime.Transient);
// → 매 Resolve마다 새 인스턴스 생성

// 3. Scoped: HTTP 요청당 하나 (웹 환경, Unity에서는 거의 사용 안함)
builder.Register<MockUIManager>(Lifetime.Scoped);
```

### 왜 Singleton을 선택했는가?

| Lifetime | 장점 | 단점 | 테스트 적합성 |
|----------|------|------|--------------|
| **Singleton** | 상태 추적 가능, 실제 환경 모사 | - | ✅ **최적** |
| **Transient** | 항상 새 인스턴스 | 상태 추적 불가능, 메모리 낭비 | ❌ 부적합 |
| **Scoped** | 요청별 격리 | Unity에서 의미 없음 | ❌ 부적합 |

---

## 📋 실제 사례: 인스턴스 추적 비교

### Singleton을 사용한 경우 (✅ 현재 방식)

```csharp
[UnitySetUp]
public IEnumerator Setup()
{
    testScope = CreateCustomScope(includeUI: true);
    mockUIManager = GetMockUIManager(testScope.Container);  // 인스턴스 A

    popup = AddComponent<HamburgerMenuPopup>();
    testScope.Container.Inject(popup);  // popup.uiManager = 인스턴스 A
}

[UnityTest]
public IEnumerator WhenMultipleButtonsClicked_ThenAllTracked()
{
    townBtn.onClick.Invoke();    // popup.uiManager (인스턴스 A) 사용
    noticeBtn.onClick.Invoke();  // popup.uiManager (인스턴스 A) 사용

    // ✅ 성공: 같은 인스턴스라서 모든 호출 추적됨
    Assert.AreEqual(2, mockUIManager.ShownPopups.Count);
}
```

### Transient를 사용한 경우 (❌ 문제 발생)

```csharp
[UnitySetUp]
public IEnumerator Setup()
{
    // Transient 등록
    testScope = LifetimeScope.Create(builder =>
    {
        builder.Register<MockUIManager>(Lifetime.Transient).As<IUIManager>();
    });

    mockUIManager = GetMockUIManager(testScope.Container);  // 인스턴스 A

    popup = AddComponent<HamburgerMenuPopup>();
    testScope.Container.Inject(popup);  // popup.uiManager = 인스턴스 B (새로 생성!)
}

[UnityTest]
public IEnumerator WhenMultipleButtonsClicked_ThenAllTracked()
{
    townBtn.onClick.Invoke();    // popup.uiManager (인스턴스 B) 사용
    noticeBtn.onClick.Invoke();  // popup.uiManager (인스턴스 B) 사용

    // ❌ 실패: 다른 인스턴스라서 추적 불가능
    Assert.AreEqual(2, mockUIManager.ShownPopups.Count);
    // mockUIManager는 인스턴스 A인데, popup은 인스턴스 B 사용
    // 실제 값: 0 (인스턴스 A에는 기록 없음)
}
```

---

## 💡 결론

### TestContainerBuilder의 싱글톤은 DI 원칙에 위배되지 않습니다!

**이유**:
1. **전역 싱글톤이 아닌 컨테이너 스코프 싱글톤**입니다
2. **인터페이스 기반 주입**으로 의존성 역전 원칙을 준수합니다
3. **각 테스트마다 독립적인 스코프**를 생성하여 격리성을 보장합니다
4. **Mock으로 쉽게 교체 가능**하여 테스트 가능성을 확보합니다

### 핵심 포인트

```
안티패턴 싱글톤 (❌):
  UIManager.Instance.ShowPopup()
  → 전역 상태, 테스트 불가능

DI 컨테이너 싱글톤 (✅):
  container.Resolve<IUIManager>().ShowPopup()
  → 스코프 내 싱글톤, 테스트 가능, 격리 보장
```

### 시각적 비교

```
┌─────────────────────────────────────────────────────────┐
│             안티패턴 싱글톤 (전역)                      │
├─────────────────────────────────────────────────────────┤
│                                                          │
│  ┌──────────────────────────────────────────────────┐  │
│  │       UIManager.Instance (전역 상태)             │  │
│  └──────────────────────────────────────────────────┘  │
│           ↑              ↑              ↑               │
│      테스트 A        테스트 B        테스트 C           │
│                                                          │
│  문제: 모든 테스트가 같은 인스턴스 공유 → 간섭 발생    │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│         DI 컨테이너 싱글톤 (스코프별 격리)              │
├─────────────────────────────────────────────────────────┤
│                                                          │
│  ┌───────────┐      ┌───────────┐      ┌───────────┐  │
│  │ Scope A   │      │ Scope B   │      │ Scope C   │  │
│  │ Instance A│      │ Instance B│      │ Instance C│  │
│  └───────────┘      └───────────┘      └───────────┘  │
│       ↑                  ↑                  ↑           │
│   테스트 A           테스트 B           테스트 C       │
│                                                          │
│  장점: 각 테스트가 독립적인 인스턴스 사용 → 격리 보장  │
└─────────────────────────────────────────────────────────┘
```

오히려 `Lifetime.Singleton`을 사용함으로써 **실제 프로덕션 환경을 정확히 모사**하면서도 **테스트 격리성을 완벽히 보장**하는 최선의 선택입니다! 🎯

---

## 📚 참고 자료

### DI 원칙과 싱글톤
- Martin Fowler - "Inversion of Control Containers and the Dependency Injection pattern"
- Mark Seemann - "Dependency Injection in .NET"

### VContainer 문서
- [VContainer 공식 문서 - Lifetime](https://vcontainer.hadashikick.jp/registering/register-type)
- [VContainer - Scoping](https://vcontainer.hadashikick.jp/scoping/lifetime-overview)

### Unity 테스트 패턴
- Unity Test Framework 공식 문서
- "Dependency Injection in Unity3D" - Infallible Code
