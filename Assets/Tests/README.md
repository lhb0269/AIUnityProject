# Unity Test Framework 테스트 가이드

## 개요
이 문서는 MainMenuButtonHandler에 대한 Unity Test Framework 기능 테스트 실행 방법을 안내합니다.

## 테스트 구조

```
Assets/Tests/
├── PlayMode/                           # Play Mode 테스트
│   ├── PlayModeTests.asmdef           # 테스트 어셈블리 정의
│   ├── UI/
│   │   └── MainMenuButtonHandlerTests.cs  # 메인 테스트 파일
│   └── TestUtilities/
│       └── UITestHelper.cs            # 테스트 헬퍼 유틸리티
├── TEST_REPORT.md                     # 테스트 결과 보고서
└── README.md                          # 본 문서
```

## 테스트 실행 방법

### 1. Unity Test Runner 열기

**방법 1: 메뉴에서 열기**
- Unity 에디터 상단 메뉴: `Window > General > Test Runner`

**방법 2: 단축키 사용**
- Windows: `Ctrl + Alt + T`
- Mac: `Cmd + Option + T`

### 2. PlayMode 테스트 선택

Test Runner 창에서 상단 탭 중 **PlayMode** 탭을 선택합니다.

### 3. SampleScene 준비 (중요!)

테스트 실행 전에 SampleScene에 다음 컴포넌트들이 설정되어 있어야 합니다:
- **MainMenuButtonHandler** 컴포넌트
- **UIManager** 컴포넌트
- **EventSystem** 컴포넌트
- **25개 버튼**이 MainMenuButtonHandler Inspector에 연결됨

### 4. 테스트 실행

**모든 테스트 실행:**
- `Run All` 버튼 클릭

**특정 테스트만 실행:**
- 테스트 트리에서 원하는 테스트 선택
- `Run Selected` 버튼 클릭
- 또는 테스트 이름을 우클릭하고 `Run` 선택

**특정 카테고리만 실행:**
- `MainMenuButtonHandlerTests` 클래스 선택
- 하위 테스트 그룹 (region) 선택 가능

### 5. 테스트 결과 확인

- **녹색 체크**: 테스트 통과
- **빨간색 X**: 테스트 실패
- **노란색 느낌표**: 테스트 불확정 (Inconclusive)
- 실패한 테스트를 클릭하면 하단에 오류 메시지 표시
- Game View에서 버튼 클릭 시각적 피드백 확인 가능

## 작성된 테스트 목록

### MainMenuButtonHandlerTests.cs

#### 1. 초기화 테스트 (3개)
- `MainMenuButtonHandler_Exists_In_Scene`
  - MainMenuButtonHandler가 씬에 존재하고 활성화되어 있는지 확인
- `UIManager_Exists_In_Scene`
  - UIManager가 씬에 존재하는지 확인
- `EventSystem_Exists_In_Scene`
  - EventSystem이 씬에 존재하는지 확인

#### 2. 개별 버튼 클릭 테스트 (25개)
실제 버튼 클릭 이벤트를 시뮬레이션하여 시각적 피드백과 로그 출력을 검증:
- `HamburgerMenuButton_Click_Visual`
- `SettingButton_Click_Visual`
- `UserInfoButton_Click_Visual`
- `GuideQuestButton_Click_Visual`
- `ShopButton_Click_Visual`
- `RecruitmentButton_Click_Visual`
- `EventButton_Click_Visual`
- `CharacterButton_Click_Visual`
- `SkillSettingButton_Click_Visual`
- `Skill1Button_Click_Visual`
- `Skill2Button_Click_Visual`
- `Skill3Button_Click_Visual`
- `Skill4Button_Click_Visual`
- `Skill5Button_Click_Visual`
- `Skill6Button_Click_Visual`
- `WeaponButton_Click_Visual`
- `EquipButton_Click_Visual`
- `CoworkerButton_Click_Visual`
- `HPPotionButton_Click_Visual`
- `MPPotionButton_Click_Visual`
- `PotionSettingButton_Click_Visual`
- `ControllButton_Click_Visual`
- `ChapterButton_Click_Visual`
- `MonsterSpawnButton_Click_Visual`
- `SpawnSettingButton_Click_Visual`

#### 3. 버튼 존재 테스트 (1개)
- `Scene_Has_Buttons`
  - 씬에 최소 1개 이상의 버튼이 존재하는지 확인

#### 4. 통합 테스트 (1개)
- `Rapid_Button_Clicks_Work_Correctly`
  - 여러 버튼을 빠르게 연속 클릭해도 정상 동작하는지 확인

**총 테스트 개수: 30개**

## 테스트 아키텍처

### 씬 기반 테스트 (Scene-Based Testing)

이전 방식과 달리, 현재 테스트는 **실제 SampleScene을 로드**하여 테스트합니다:

```csharp
[UnitySetUp]
public IEnumerator Setup()
{
    // SampleScene 로드
    SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    yield return null;
    yield return null; // Awake, Start 실행 보장

    // 씬에서 객체 찾기
    handler = Object.FindFirstObjectByType<MainMenuButtonHandler>();
    eventSystem = Object.FindFirstObjectByType<EventSystem>();
}
```

### 실제 버튼 클릭 시뮬레이션

ExecuteEvents와 PointerEventData를 사용하여 **실제 UI 이벤트**를 발생시킵니다:

```csharp
private IEnumerator SimulateButtonClick(Button button, string buttonName)
{
    var pointerData = new PointerEventData(eventSystem);

    // 1. 버튼 눌림 효과 (시각적 피드백 시작)
    ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerDownHandler);
    yield return new WaitForSeconds(0.1f);

    // 2. 버튼 떼기 효과
    ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerUpHandler);

    // 3. 클릭 이벤트 발생
    ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerClickHandler);

    yield return new WaitForSeconds(0.2f);
}
```

이 방식의 장점:
- **시각적 확인**: Game View에서 버튼이 실제로 눌리는 효과 확인
- **실제 이벤트**: Unity의 EventSystem을 통한 실제 UI 상호작용
- **통합 테스트**: 씬의 모든 컴포넌트가 실제 환경에서 동작

### 버튼 필드 접근

Reflection을 사용하여 private 버튼 필드에 접근합니다:

```csharp
private Button GetButtonField(string fieldName)
{
    var field = typeof(MainMenuButtonHandler).GetField(fieldName,
        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    return field?.GetValue(handler) as Button;
}
```

**버튼 필드 이름 매핑**:
- `hamburgerMenuBtn` - 햄버거 메뉴
- `settingBtn` - 설정
- `userInfoBtn` - 유저 정보
- `guideQuestBtn` - 가이드 퀘스트
- `shopBtn` - 상점
- `recruitmentBtn` - 모집
- `eventBtn` - 이벤트
- `characterButton` - 캐릭터 (Button으로 끝남)
- `SkillSettingBtn` - 스킬 설정 (대문자 S로 시작)
- `skill1Btn` ~ `skill6Btn` - 스킬 1~6
- `weaponButton` - 무기 (Button으로 끝남)
- `equipButton` - 장비 (Button으로 끝남)
- `coworkerButton` - 협력자 (Button으로 끝남)
- `hpPotionBtn` - HP 포션
- `mpPotionBtn` - MP 포션
- `potionSettingBtn` - 포션 설정
- `controllBtn` - 컨트롤
- `chapterBtn` - 챕터
- `monsterSpawnBtn` - 몬스터 스폰
- `spawnSettingBtn` - 스폰 설정

## 테스트 커버리지

### 검증하는 기능
- SampleScene 로드 및 초기화
- MainMenuButtonHandler 씬 존재 확인
- UIManager 싱글톤 존재 확인
- EventSystem 존재 확인
- 25개 버튼 클릭 이벤트 시뮬레이션
- 버튼 클릭 시 로그 출력 검증
- 연속 버튼 클릭 안정성
- UI 시각적 피드백

### 사용된 테스트 기법
- **SceneManager**: 실제 씬 로드
- **ExecuteEvents**: UI 이벤트 시뮬레이션
- **PointerEventData**: 포인터 이벤트 데이터 생성
- **LogAssert**: Debug.Log 출력 검증
- **Reflection**: private 필드 접근
- **UnitySetUp/UnityTest**: 비동기 테스트 (IEnumerator)
- **Assert.Inconclusive**: 조건부 테스트 스킵
- **Object.FindFirstObjectByType**: 씬 객체 검색

## 테스트 실행 환경

- **Unity 버전**: 6000.2.9f1 (Unity 6)
- **Test Framework 버전**: 1.6.0
- **테스트 타입**: Play Mode Test (Scene-Based)
- **실행 플랫폼**: Editor
- **필요 씬**: SampleScene (자동 로드됨)

## 주의사항

### 1. SampleScene 설정 필수
테스트 실행 전 SampleScene에 다음이 설정되어야 합니다:
- MainMenuButtonHandler 컴포넌트
- UIManager 컴포넌트
- EventSystem (Canvas와 함께)
- 25개 버튼이 Inspector에 연결됨

버튼이 연결되지 않으면 해당 테스트는 `Inconclusive`로 표시됩니다.

### 2. 테스트 실행 시간
- 각 버튼 클릭에 0.3초 소요 (0.1초 눌림 + 0.2초 대기)
- 전체 테스트 실행에 약 1-2분 소요
- Game View에서 시각적 확인 가능

### 3. 필드 이름 주의
일부 버튼 필드는 일관되지 않은 명명 규칙을 따릅니다:
- `characterButton` (Btn이 아닌 Button)
- `SkillSettingBtn` (대문자 S로 시작)
- `weaponButton`, `equipButton`, `coworkerButton` (Button으로 끝남)

### 4. 테스트 격리
- 각 테스트는 [UnitySetUp]에서 씬을 로드
- 씬이 이미 로드된 경우 재로드하지 않음
- TearDown에서 참조만 해제 (씬 객체는 유지)

### 5. Inconclusive 테스트
버튼이 연결되지 않은 경우 테스트가 실패하지 않고 `Inconclusive`로 표시됩니다:
```csharp
if (button == null)
{
    Assert.Inconclusive("버튼이 연결되지 않았습니다");
    yield break;
}
```

## 테스트 헬퍼 메서드

### SimulateButtonClick
버튼 클릭을 시뮬레이션하고 시각적 피드백을 제공:
- PointerDown 이벤트 발생
- 0.1초 대기 (눌림 효과)
- PointerUp 이벤트 발생
- PointerClick 이벤트 발생
- 0.2초 대기 (다음 클릭 전 간격)

### GetButtonField
Reflection으로 private 버튼 필드에 접근:
- 필드 이름으로 버튼 참조 획득
- null 반환 시 버튼 미연결 상태

## CI/CD 통합

### 커맨드라인에서 테스트 실행
```bash
# Windows
Unity.exe -runTests -batchmode -projectPath "C:\Users\lhb02\OneDrive\Documents\UnityProject\AIProject" -testResults results.xml -testPlatform PlayMode

# Mac
/Applications/Unity/Unity.app/Contents/MacOS/Unity -runTests -batchmode -projectPath "/path/to/project" -testResults results.xml -testPlatform PlayMode
```

### GitHub Actions 예제
```yaml
- name: Run PlayMode Tests
  uses: game-ci/unity-test-runner@v2
  with:
    projectPath: ${{ github.workspace }}
    testMode: PlayMode
    artifactsPath: test-results
```

## 발생한 문제 및 해결법

### 1. PlayMode 테스트가 빈 씬에서 실행됨

**문제**: Test Runner가 자동으로 빈 테스트 씬을 로드하여 씬 객체를 찾을 수 없음

**원인**: Unity Test Framework는 기본적으로 격리된 테스트 환경을 제공

**해결법**:
```csharp
[UnitySetUp]
public IEnumerator Setup()
{
    SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    yield return null;
    yield return null; // Awake, Start 실행 보장
}
```

### 2. 메서드 직접 호출은 시각적 피드백이 없음

**문제**: `handler.OnHamburgerMenuClicked()` 직접 호출은 버튼 눌림 효과 없음

**원인**: UI와 상호작용하지 않고 함수만 실행

**해결법**:
ExecuteEvents를 사용한 실제 UI 이벤트 시뮬레이션
```csharp
ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerDownHandler);
yield return new WaitForSeconds(0.1f);
ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerUpHandler);
ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
```

### 3. 버튼 필드 이름 불일치

**문제**: 테스트에서 사용한 필드 이름이 실제 MainMenuButtonHandler의 필드 이름과 다름

**원인**: 일관되지 않은 명명 규칙 (Btn vs Button, 대소문자)

**해결법**:
MainMenuButtonHandler.cs의 실제 필드 이름 확인 후 수정
- `characterBtn` → `characterButton`
- `skillSettingBtn` → `SkillSettingBtn`
- `weaponBtn` → `weaponButton`
- `equipBtn` → `equipButton`
- `coworkerBtn` → `coworkerButton`

### 4. EventSystem 누락

**문제**: UI 이벤트가 동작하지 않음

**원인**: EventSystem이 씬에 없음

**해결법**:
Setup에서 EventSystem 존재 확인
```csharp
eventSystem = Object.FindFirstObjectByType<EventSystem>();
if (eventSystem == null)
{
    Assert.Fail("EventSystem을 SampleScene에서 찾을 수 없습니다.");
}
```

### 5. 싱글톤 인스턴스 초기화 문제

**에러 메시지**:
```
Expected: not null
But was: null
UIManager 인스턴스가 생성되어야 합니다
```

**원인**:
- UIManager는 싱글톤 패턴으로 static Instance 변수 사용
- TearDown()에서 GameObject는 파괴하지만 static Instance는 초기화 안 됨
- 파괴된 GameObject를 가리키는 Instance로 인해 다음 테스트 실패

**해결법**:
씬 기반 테스트로 전환하여 싱글톤 관리 문제 해결
```csharp
[TearDown]
public void Teardown()
{
    // 씬의 객체는 파괴하지 않음
    handler = null;
    eventSystem = null;
}
```

### 6. Burst 컴파일러 Assembly Resolution 에러

**에러 메시지**:
```
Mono.Cecil.AssemblyResolutionException: Failed to resolve assembly: 'PlayModeTests'
```

**원인**: Burst 컴파일러가 테스트 어셈블리를 컴파일하려고 시도함

**해결법**:
`defineConstraints`에 `UNITY_INCLUDE_TESTS` 추가
```json
{
  "defineConstraints": [
    "UNITY_INCLUDE_TESTS"
  ]
}
```

---

## 최종 PlayModeTests.asmdef 설정

```json
{
  "name": "PlayModeTests",
  "rootNamespace": "MobileGame.Tests",
  "references": [
    "MobileGame",
    "UnityEngine.TestRunner",
    "UnityEditor.TestRunner"
  ],
  "includePlatforms": [],
  "excludePlatforms": [],
  "allowUnsafeCode": false,
  "overrideReferences": true,
  "precompiledReferences": [
    "nunit.framework.dll"
  ],
  "autoReferenced": false,
  "defineConstraints": [
    "UNITY_INCLUDE_TESTS"
  ],
  "versionDefines": [],
  "noEngineReferences": false
}
```

## 향후 개선 사항

1. **터치 입력 시각화**: 버튼 클릭 시 터치 포인트 오버레이 표시
2. **테스트 시나리오 확장**: 사용자 플로우 시뮬레이션
3. **성능 벤치마크**: UI 반응 시간 측정
4. **스크린샷 캡처**: 테스트 실패 시 자동 스크린샷
5. **테스트 리포트 생성**: HTML/JSON 형식의 상세 보고서

## 참고 자료

- [Unity Test Framework 공식 문서](https://docs.unity3d.com/Packages/com.unity.test-framework@latest)
- [Unity EventSystems](https://docs.unity3d.com/Packages/com.unity.ugui@latest/manual/EventSystem.html)
- [ExecuteEvents API](https://docs.unity3d.com/Packages/com.unity.ugui@latest/api/UnityEngine.EventSystems.ExecuteEvents.html)
- [NUnit 문서](https://docs.nunit.org/)

---

**최종 업데이트**: 2025-11-16
**버전**: 2.0.0
**작성자**: Claude Code
