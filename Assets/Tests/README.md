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

### 3. 테스트 실행

**모든 테스트 실행:**
- `Run All` 버튼 클릭

**특정 테스트만 실행:**
- 테스트 트리에서 원하는 테스트 선택
- `Run Selected` 버튼 클릭
- 또는 테스트 이름을 우클릭하고 `Run` 선택

**특정 카테고리만 실행:**
- `MainMenuButtonHandlerTests` 클래스 선택
- 하위 테스트 그룹 (region) 선택 가능

### 4. 테스트 결과 확인

- **녹색 체크**: 테스트 통과
- **빨간색 X**: 테스트 실패
- 실패한 테스트를 클릭하면 하단에 오류 메시지 표시

## 작성된 테스트 목록

### MainMenuButtonHandlerTests.cs

#### 1. 초기화 테스트 (2개)
- `MainMenuButtonHandler_Initializes_Successfully_With_UIManager`
  - UIManager가 존재할 때 정상 초기화 확인
- `Start_Warns_When_UIManager_Missing`
  - UIManager가 없을 때 경고 로그 출력 확인

#### 2. 버튼 핸들러 메서드 테스트 (25개)
모든 버튼 클릭 핸들러가 올바른 로그를 출력하는지 검증:
- `OnHamburgerMenuClicked_Logs_Correctly`
- `OnSettingClicked_Logs_Correctly`
- `OnUserInfoClicked_Logs_Correctly`
- `OnGuideQuestClicked_Logs_Correctly`
- `OnShopClicked_Logs_Correctly`
- `OnRecruitmentClicked_Logs_Correctly`
- `OnEventClicked_Logs_Correctly`
- `OnCharacterClicked_Logs_Correctly`
- `OnSkillSettingClicked_Logs_Correctly`
- `OnSkill1Clicked_Logs_Correctly`
- `OnSkill2Clicked_Logs_Correctly`
- `OnSkill3Clicked_Logs_Correctly`
- `OnSkill4Clicked_Logs_Correctly`
- `OnSkill5Clicked_Logs_Correctly`
- `OnSkill6Clicked_Logs_Correctly`
- `OnWeaponClicked_Logs_Correctly`
- `OnEquipClicked_Logs_Correctly`
- `OnCoworkerClicked_Logs_Correctly`
- `OnHPPotionClicked_Logs_Correctly`
- `OnMPPotionClicked_Logs_Correctly`
- `OnPotionSettingClicked_Logs_Correctly`
- `OnControllClicked_Logs_Correctly`
- `OnChapterClicked_Logs_Correctly`
- `OnMonsterSpawnClicked_Logs_Correctly`
- `OnSpawnSettingClicked_Logs_Correctly`

#### 3. 버튼 이벤트 등록 테스트 (2개)
- `RegisterButtonEvents_Subscribes_Valid_Buttons`
  - 유효한 버튼에 대해 이벤트가 등록되는지 확인
- `RegisterButtonEvents_Warns_For_Null_Buttons`
  - null 버튼에 대해 경고를 출력하는지 확인

#### 4. 버튼 이벤트 해제 테스트 (1개)
- `UnregisterButtonEvents_Removes_All_Listeners`
  - OnDestroy 시 모든 리스너가 제거되는지 확인

#### 5. 예외 처리 테스트 (2개)
- `RegisterButton_Handles_Null_Button_Gracefully`
  - null 버튼 등록 시 예외 없이 처리되는지 확인
- `UnregisterButton_Handles_Null_Button_Gracefully`
  - null 버튼 해제 시 예외 없이 처리되는지 확인

#### 6. UI 통합 테스트 (2개)
- `Button_Click_Invokes_Handler_Method`
  - 실제 버튼 클릭이 핸들러 메서드를 호출하는지 확인
- `Multiple_Button_Clicks_Work_Sequentially`
  - 여러 버튼을 연속으로 클릭해도 정상 동작하는지 확인

#### 7. 성능 테스트 (1개)
- `RegisterButtonEvents_Completes_In_Reasonable_Time`
  - 25개 버튼 이벤트 등록이 0.1초 이내에 완료되는지 확인

**총 테스트 개수: 35개**

## 테스트 커버리지

### 검증하는 기능
- MainMenuButtonHandler 초기화 프로세스
- UIManager 의존성 체크
- 25개 버튼 핸들러 메서드 동작
- 버튼 이벤트 등록/해제 메커니즘
- Null 안정성 (Null Safety)
- UI 통합 동작
- 성능 기준 (0.1초 이내 등록)

### 사용된 테스트 기법
- **LogAssert**: Debug.Log 출력 검증
- **Reflection**: private 필드/메서드 테스트
- **UnityTest (Coroutine)**: 비동기 테스트
- **SetUp/TearDown**: 테스트 전후 처리
- **Assert 메서드**: NUnit 어설션 사용

## 테스트 실행 환경

- **Unity 버전**: 6000.2.9f1 (Unity 6)
- **Test Framework 버전**: 1.6.0
- **테스트 타입**: Play Mode Test
- **실행 플랫폼**: Editor (Standalone 빌드에서도 실행 가능)

## 주의사항

### 1. Assembly Definition 의존성
테스트가 정상 실행되려면 다음 파일들이 필요합니다:
- `Assets/_Project/Scripts/MobileGame.asmdef` (메인 코드 어셈블리)
- `Assets/Tests/PlayMode/PlayModeTests.asmdef` (테스트 어셈블리)

### 2. Private 멤버 테스트
일부 테스트는 Reflection을 사용하여 private 필드/메서드에 접근합니다.
이는 테스트 목적으로만 사용되며, 실제 코드에서는 사용하지 않습니다.

### 3. UIManager 싱글톤
각 테스트는 독립적으로 UIManager 인스턴스를 생성합니다.
TearDown에서 정리되므로 테스트 간 간섭이 없습니다.

### 4. Play Mode 실행 시간
Play Mode 테스트는 Unity를 Play 모드로 전환하므로
Edit Mode 테스트보다 실행 시간이 길 수 있습니다.

## 테스트 헬퍼 유틸리티

`UITestHelper.cs`는 다음 기능을 제공합니다:
- 테스트용 버튼/Canvas 생성
- Reflection 헬퍼 (private 필드/메서드 접근)
- GameObject 정리 헬퍼
- 버튼 이벤트 검증 헬퍼
- 로그 캡처 헬퍼

다른 UI 테스트 작성 시 재사용 가능합니다.

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

## 추가 개선 사항

향후 테스트 확장 시 고려사항:
1. **Edit Mode 테스트 추가**: 컴파일 타임 검증
2. **Mock 프레임워크 도입**: NSubstitute 또는 Moq 사용
3. **커버리지 분석**: Unity Code Coverage 패키지 사용
4. **성능 벤치마크**: Performance Testing Extension 사용
5. **UI 자동화 테스트**: Unity UI Toolkit Test Framework 활용

## 문제 해결

### 테스트가 표시되지 않을 때
1. Test Runner 창에서 `Reload Tests` 버튼 클릭
2. Assembly Definition 파일 확인
3. Unity 에디터 재시작

### 컴파일 오류 발생 시
1. `MobileGame.asmdef` 파일이 존재하는지 확인
2. 네임스페이스가 올바른지 확인
3. Test Framework 패키지가 설치되어 있는지 확인

### 테스트 실패 시
1. Console 창에서 상세 오류 메시지 확인
2. Stack Trace를 통해 실패 지점 파악
3. 해당 테스트를 단독으로 실행하여 격리 테스트

---

## 발생한 문제 및 해결법

### 1. EditMode vs PlayMode 테스트 차이점

**문제**: PlayMode 탭에서 테스트가 보이지 않고, EditMode에서만 표시됨

**원인**: `PlayModeTests.asmdef`의 `includePlatforms` 설정이 잘못됨

| 설정 | EditMode | PlayMode |
|------|----------|----------|
| `includePlatforms` | `["Editor"]` | `[]` (빈 배열) |
| MonoBehaviour 생명주기 | Start/Update 실행 안 됨 | Start/Update 정상 실행 |
| 테스트 속성 | `[Test]` | `[UnityTest]` + `IEnumerator` |

**해결법**:
```json
// PlayModeTests.asmdef
{
  "includePlatforms": [],  // 빈 배열로 설정
  "references": [
    "MobileGame",
    "UnityEngine.TestRunner",
    "UnityEditor.TestRunner"
  ]
}
```

### 2. Burst 컴파일러 Assembly Resolution 에러

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

### 3. 중복 Assembly Reference 에러

**에러 메시지**:
```
Assembly has duplicate references: UnityEngine.TestRunner, UnityEditor.TestRunner
```

**원인**: `optionalUnityReferences`와 `references`에 동시에 TestRunner 추가됨

**해결법**:
`optionalUnityReferences`를 제거하고 `references`에만 추가
```json
{
  "references": [
    "MobileGame",
    "UnityEngine.TestRunner",
    "UnityEditor.TestRunner"
  ]
  // optionalUnityReferences 제거
}
```

### 4. InputSystem 네임스페이스 에러

**에러 메시지**:
```
CS0234: The type or namespace name 'InputSystem' does not exist in the namespace 'UnityEngine'
```

**원인**: 메인 게임 코드에서 Input System을 사용하지만 asmdef에 참조가 없음

**해결법**:
`MobileGame.asmdef`에 Unity.InputSystem 참조 추가
```json
// Assets/_Project/Scripts/MobileGame.asmdef
{
  "references": [
    "Unity.InputSystem"
  ]
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
TearDown()에서 Reflection으로 static Instance를 null로 초기화
```csharp
[TearDown]
public void Teardown()
{
    // ... GameObject 파괴 ...

    // 싱글톤 인스턴스 초기화
    var instanceField = typeof(UIManager).GetField("Instance",
        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
    instanceField?.SetValue(null, null);
}
```

### 6. MonoBehaviour Start() 실행 타이밍 문제

**에러 메시지**:
```
Expected log did not appear: [Log] [MainMenu] 햄버거 메뉴 버튼 클릭
```

**원인**:
- `yield return null` 한 번으로는 Start() 메서드가 완전히 실행되지 않음
- LogAssert.Expect()가 로그 발생 후에 호출됨

**해결법**:
1. 프레임 대기를 2회로 증가
2. LogAssert.Expect()를 yield return null 이전에 호출

```csharp
[UnityTest]
public IEnumerator Button_Click_Invokes_Handler_Method()
{
    // 버튼 설정...

    // Start() 완전 실행을 위해 2프레임 대기
    yield return null;
    yield return null;

    // LogAssert는 로그 발생 전에 설정
    LogAssert.Expect(LogType.Log, "[MainMenu] 햄버거 메뉴 버튼 클릭");
    hamburgerButton.onClick.Invoke();
}
```

### 7. Null 버튼 경고 테스트

**문제**: null 버튼 경고가 1개만 예상되지만 25개 발생

**원인**: MainMenuButtonHandler에는 25개 버튼이 있고, 모두 null일 때 각각 경고 출력

**해결법**:
모든 null 버튼에 대한 경고를 예상하는 루프 추가
```csharp
// 25개 버튼에 대한 경고 예상
for (int i = 0; i < 25; i++)
{
    LogAssert.Expect(LogType.Warning,
        new System.Text.RegularExpressions.Regex("버튼이 할당되지 않았습니다"));
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

## 참고 자료

- [Unity Test Framework 공식 문서](https://docs.unity3d.com/Packages/com.unity.test-framework@latest)
- [NUnit 문서](https://docs.nunit.org/)
- [Unity 테스트 모범 사례](https://docs.unity3d.com/Manual/testing-editortestsrunner.html)

---

**작성일**: 2025-11-14
**버전**: 1.0.0
**작성자**: Claude Code
