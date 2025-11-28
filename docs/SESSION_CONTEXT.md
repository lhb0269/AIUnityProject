# 세션 컨텍스트 - UI 컴포넌트 DI 전환

## 프로젝트 개요

**프로젝트명**: Unity 모바일 게임 - AI Project
**Unity 버전**: 6000.2.9f1 (Unity 6)
**현재 브랜치**: `ui-components-di-conversion`
**베이스 브랜치**: `project-restructuring`
**DI 프레임워크**: VContainer 1.17.0

## 이전 작업 완료 현황 (Phase 1-5)

### ✅ 완료된 작업

1. **VContainer 설치 및 설정**
   - OpenUPM을 통한 VContainer 1.17.0 설치
   - Assembly Definition 업데이트 (GUID 기반 참조)
   - UI 식별자 시스템 구축 (PopupID, ButtonID)

2. **인터페이스 기반 아키텍처 전환**
   - 6개 매니저 인터페이스 생성 (IUIManager, IGameManager, IAudioManager, IInputManager, ISceneLoader, ISaveSystem)
   - Singleton 패턴 완전 제거 (52곳)
   - 매니저 클래스들이 인터페이스 구현

3. **DI 컨테이너 구현**
   - GameLifetimeScope 클래스 생성
   - 4개 필수 매니저 등록 (UIManager, GameManager, AudioManager, SceneLoader)
   - MainMenuController DI 자동 주입

4. **UI 시스템 전환**
   - BasePopup에 DI 주입
   - HamburgerMenuPopup DI 전환 (3곳 수정)
   - ButtonBinder 시스템 구현 (34개 버튼 ID 기반 접근)
   - MainMenuController 생성 (MainMenuButtonHandler 대체)

5. **테스트 인프라 구축**
   - 6개 Mock 매니저 클래스 생성
   - TestContainerBuilder 헬퍼 클래스
   - MainMenuControllerTests 작성 및 통과 (34개 버튼 테스트)

### 📊 주요 성과

- **Singleton 의존성**: 52곳 → 0곳 (100% 제거)
- **테스트 커버리지**: 34개 버튼 로직 테스트 통과
- **커밋 수**: 20개
- **Unity Play 모드**: 정상 동작 확인

## 현재 작업: 나머지 UI 컴포넌트 DI 전환

### 작업 목표

BasePopup을 상속한 모든 팝업 클래스를 DI 기반으로 전환합니다.

### 대상 파일 (예상)

현재 프로젝트에 있는 팝업 클래스들:
- `HamburgerMenuPopup.cs` - ✅ 이미 완료 (3곳 전환)
- 기타 Popup 클래스들 (아직 파악 필요)

### 작업 단계

1. **팝업 클래스 파악**
   - `Assets/_Project/Scripts/UI/Popups/` 디렉토리 탐색
   - BasePopup 상속 클래스 검색
   - `UIManager.Instance` 사용처 검색

2. **각 팝업 DI 전환**
   - `UIManager.Instance` → `uiManager` (BasePopup에서 상속)
   - 추가 매니저 필요 시 `[Inject]` 속성으로 주입
   - PopupID 상수 사용

3. **테스트 작성**
   - 각 팝업별 단위 테스트
   - Mock 매니저 활용

4. **Unity 에디터 검증**
   - Play 모드에서 동작 확인
   - 팝업 열기/닫기 테스트

## 프로젝트 구조

```
AIProject/
├── Assets/
│   ├── _Project/
│   │   └── Scripts/
│   │       ├── DI/
│   │       │   └── GameLifetimeScope.cs
│   │       ├── Interfaces/
│   │       │   ├── IUIManager.cs
│   │       │   ├── IGameManager.cs
│   │       │   └── ... (6개 인터페이스)
│   │       ├── Managers/
│   │       │   ├── UIManager.cs
│   │       │   ├── GameManager.cs
│   │       │   └── ... (6개 매니저)
│   │       └── UI/
│   │           ├── PopupID.cs
│   │           ├── ButtonID.cs
│   │           ├── ButtonBinder.cs
│   │           ├── MainMenuController.cs
│   │           ├── BasePopup.cs
│   │           └── Popups/
│   │               ├── HamburgerMenuPopup.cs ✅
│   │               └── ... (기타 팝업)
│   ├── Tests/
│   │   ├── Mocks/ (6개 Mock 클래스)
│   │   ├── Helpers/ (TestContainerBuilder)
│   │   └── PlayMode/
│   │       └── UI/
│   │           └── MainMenuControllerTests.cs ✅
│   └── Scenes/
│       └── SampleScene.unity
└── docs/
    ├── DI_REFACTORING.md (Phase 1-5 문서)
    └── SESSION_CONTEXT.md (이 파일)
```

## 코딩 스타일 및 규칙

### DI 패턴

**BasePopup 상속 시**:
```csharp
public class MyPopup : BasePopup
{
    // BasePopup에서 uiManager 상속받음
    // 추가 매니저 필요 시:
    [Inject] private IGameManager gameManager;
    [Inject] private IAudioManager audioManager;

    public override void Show()
    {
        base.Show();
        // uiManager 사용 가능
    }

    public void OnButtonClick()
    {
        uiManager.ShowPopup(PopupID.SomePopup);
    }
}
```

### PopupID 사용

```csharp
// Before
uiManager.ShowPopup("TownPopup");

// After
uiManager.ShowPopup(PopupID.Town);
```

### 테스트 작성

```csharp
[UnityTest]
public IEnumerator WhenButtonClicked_ThenPopupShown()
{
    // Arrange
    var popup = CreateTestPopup();
    mockUIManager.Reset();

    // Act
    popup.OnButtonClick();
    yield return null;

    // Assert
    Assert.AreEqual(1, mockUIManager.ShownPopups.Count);
    Assert.AreEqual(PopupID.Expected, mockUIManager.ShownPopups[0]);
}
```

## 커밋 메시지 형식

```
[카테고리] 간단한 설명

상세 설명:
- 변경사항 1
- 변경사항 2

🤖 Generated with Claude Code

Co-Authored-By: Claude <noreply@anthropic.com>
```

**카테고리 예시**:
- `[리팩토링]` - 코드 구조 변경
- `[추가]` - 새 기능 추가
- `[수정]` - 버그 수정
- `[테스트]` - 테스트 코드
- `[문서]` - 문서 작성

## 개발 워크플로우

1. **브랜치 전략**
   - 기능별로 브랜치 생성
   - 작업 완료 후 Pull Request
   - main 브랜치로 머지

2. **작업 순서**
   ```
   코드 작성 → 테스트 작성 → Unity 검증 → 커밋 → (반복)
   ```

3. **주의사항**
   - 컴파일 오류 없이 커밋
   - 테스트 통과 후 커밋
   - Unity Play 모드 확인

## 유용한 명령어

### 팝업 클래스 찾기
```bash
# BasePopup 상속 클래스 검색
Get-ChildItem -Path "Assets/_Project/Scripts/UI" -Filter "*.cs" -Recurse | Select-String -Pattern "class.*:.*BasePopup"

# UIManager.Instance 사용처 검색
Get-ChildItem -Path "Assets/_Project/Scripts/UI" -Filter "*.cs" -Recurse | Select-String -Pattern "UIManager\.Instance"
```

### Git 명령어
```bash
# 현재 브랜치 확인
git branch

# 변경사항 확인
git status

# 커밋
git add .
git commit -m "메시지"

# 푸시
git push
```

## 문제 해결 가이드

### VContainer 참조 오류
- Assembly Definition에서 GUID 기반 참조 사용
- `GUID:b0214a6008ed146ff8f122a6a9c2f6cc` (VContainer)

### ButtonBinder 초기화 문제
- `isInitialized` 플래그를 false로 리셋
- Reflection으로 `InitializeButtonMap()` 호출

### Mock 인터페이스 불일치
- 실제 인터페이스 정의 확인
- 누락된 메서드 추가

## 참고 문서

- **DI 리팩토링 상세**: `docs/DI_REFACTORING.md`
- **VContainer 공식 문서**: https://vcontainer.hadashikick.jp/
- **프로젝트 가이드**: `CLAUDE.md`

## 다음 세션 시작 시

1. 이 문서를 읽고 컨텍스트 파악
2. 현재 브랜치 확인: `ui-components-di-conversion`
3. 팝업 클래스 파악부터 시작
4. 하나씩 DI 전환 진행

---

**작성일**: 2025-01-29
**작성자**: Claude Code + 개발자
**다음 작업**: 나머지 UI 컴포넌트 DI 전환 (옵션 1)
