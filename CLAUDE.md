# CLAUDE.md

이 파일은 Claude Code (claude.ai/code)가 이 저장소의 코드로 작업할 때 참고할 가이드를 제공합니다.

## 프로젝트 개요

이 프로젝트는 Unity 버전 6000.2.9f1 (Unity 6)을 사용하는 Unity 2D 게임 프로젝트입니다. Universal Render Pipeline (URP)을 사용하는 2D 개발용으로 구성되어 있으며, 애니메이션, 스프라이트 처리, 타일맵 지원 등 다양한 Unity 2D 기능을 포함하고 있습니다.

## 개발 환경

### 프로젝트 타입
- Unity 6 프로젝트 (6000.2.9f1)
- 2D 게임 개발
- Universal Render Pipeline (URP) 17.2.0
- New Input System 활성화 (1.14.2)
- Visual Scripting 사용 가능 (1.9.8)

### IDE 구성
- 솔루션 파일: `AIProject.slnx`
- C# 프로젝트: `Assembly-CSharp.csproj`
- VS Code 설정: `.vscode/settings.json`에 구성됨
- 기본 솔루션: `AIProject.slnx`로 설정됨

## 주요 Unity 패키지

프로젝트는 다음의 주요 Unity 패키지를 사용합니다:
- `com.unity.2d.animation` (12.0.2) - 2D 캐릭터 애니메이션 및 리깅
- `com.unity.2d.sprite` (1.0.0) - 스프라이트 처리
- `com.unity.2d.tilemap` (1.0.0) - 레벨 디자인을 위한 타일맵 시스템
- `com.unity.2d.spriteshape` (12.0.1) - 스프라이트 셰이프 도구
- `com.unity.2d.psdimporter` (11.0.1) - Photoshop 파일 임포트
- `com.unity.2d.aseprite` (2.0.2) - Aseprite 파일 임포트
- `com.unity.render-pipelines.universal` (17.2.0) - URP 렌더링
- `com.unity.inputsystem` (1.14.2) - 새로운 Unity Input System
- `com.unity.timeline` (1.8.9) - 컷신 및 시퀀스
- `com.unity.ugui` (2.0.0) - Unity UI 시스템

## 프로젝트 구조

```
AIProject/
├── Assets/
│   ├── Scenes/               # Unity 씬
│   │   ├── SampleScene.unity # 메인 샘플 씬
│   │   └── NewMonoBehaviourScript.cs # 스크립트
│   ├── Settings/             # URP 및 렌더링 설정
│   │   ├── UniversalRP.asset
│   │   ├── Renderer2D.asset
│   │   └── Scenes/          # 씬 템플릿
│   ├── InputSystem_Actions.inputactions # 입력 액션 매핑
│   └── DefaultVolumeProfile.asset # URP 볼륨 설정
├── Library/                  # Unity 생성 파일 (무시됨)
├── Logs/                    # Unity 로그 (무시됨)
├── Packages/                # 패키지 종속성
│   └── manifest.json        # 패키지 매니페스트
├── ProjectSettings/         # Unity 프로젝트 설정
├── Temp/                    # 임시 Unity 파일 (무시됨)
└── UserSettings/            # 사용자별 설정 (무시됨)
```

## Unity C# 스크립트 작업

### 스크립트 위치
- 모든 사용자 정의 스크립트는 `Assets/` 또는 적절한 하위 폴더에 배치해야 함
- 씬별 스크립트는 현재 `Assets/Scenes/`에 위치함
- 더 나은 구조를 위해 `Assets/Scripts/`와 같은 폴더로 스크립트를 정리하는 것을 고려

### Unity MonoBehaviour 생명주기
Unity 스크립트는 일반적으로 `MonoBehaviour`를 상속하며 다음 주요 메서드를 사용합니다:
- `Start()` - 스크립트가 처음 초기화될 때 한 번 호출됨
- `Update()` - 매 프레임마다 호출됨
- `FixedUpdate()` - 고정된 시간 간격으로 호출됨 (물리 업데이트)
- `Awake()` - 스크립트 인스턴스가 로드될 때 호출됨
- `OnEnable()` / `OnDisable()` - 오브젝트가 활성화/비활성화될 때 호출됨

### 네임스페이스
일반적인 Unity 네임스페이스:
- `UnityEngine` - 핵심 Unity 기능
- `UnityEngine.UI` - UI 컴포넌트 (UGUI)
- `UnityEngine.InputSystem` - 새로운 Input System
- `UnityEngine.Tilemaps` - 타일맵 기능

## 빌드 및 테스트

### 프로젝트 열기
1. Unity Hub 열기
2. `C:\Users\lhb02\OneDrive\Documents\UnityProject\AIProject`에서 프로젝트 추가
3. Unity 6000.2.9f1이 설치되어 있는지 확인

### Unity 에디터에서 테스트
- Unity 에디터의 Play 버튼을 사용하여 씬 테스트
- 메인 씬: `Assets/Scenes/SampleScene.unity`

### 프로젝트 빌드
빌드 설정은 Unity 에디터에서:
- File > Build Settings
- 타겟 플랫폼 선택 (Windows, Mac, Linux 등)
- 빌드할 씬 추가
- "Build" 또는 "Build And Run" 클릭

## Input System

프로젝트는 Unity의 새로운 Input System을 사용합니다 (레거시 Input Manager가 아님):
- 입력 액션은 `Assets/InputSystem_Actions.inputactions`에 정의됨
- 코드에서 `InputAction` 및 `InputActionMap` API 사용
- Input System 패키지를 통해 액션 참조
- 이전 `Input.GetKey()` 메서드 사용을 피하고 Input Actions를 사용할 것

## 렌더링 구성

### Universal Render Pipeline (URP)
- URP 에셋: `Assets/Settings/UniversalRP.asset`
- 2D 렌더러: `Assets/Settings/Renderer2D.asset`
- 볼륨 프로파일: `Assets/DefaultVolumeProfile.asset`
- 전역 설정: `Assets/UniversalRenderPipelineGlobalSettings.asset`

렌더링 작업 시:
- URP 셰이더 사용 (예: Universal Render Pipeline/2D/Lit)
- Volume Profiles를 통해 라이팅 및 포스트 프로세싱 구성
- 2D 전용 렌더링은 Renderer2D 에셋에서 처리됨

## 파일 제외 사항

다음 디렉토리는 Unity에서 생성되며 직접 편집하면 안 됩니다:
- `Library/` - Unity 캐시 및 임시 데이터
- `Temp/` - 임시 빌드 파일
- `Logs/` - Unity 에디터 로그
- `obj/` - 빌드 아티팩트
- `.meta` 파일 - Unity 메타데이터 (에셋과 함께 자동 생성됨)

## 일반적인 패턴

### 새 스크립트 만들기
1. 스크립트를 `Assets/Scripts/`에 배치 (필요시 폴더 생성)
2. 적절한 명명 규칙 사용: 클래스는 PascalCase, 변수는 camelCase
3. GameObject에 연결되는 컴포넌트는 `MonoBehaviour` 상속
4. Inspector에서 보이는 private 필드는 `[SerializeField]` 사용
5. Unity의 컴포넌트 기반 아키텍처를 따를 것

### 2D 에셋 작업
- 스프라이트는 Texture Type을 "Sprite (2D and UI)"로 설정하여 임포트
- 2D 렌더링에는 Sprite Renderer 컴포넌트 사용
- 타일맵은 Tilemap 및 Tilemap Renderer 컴포넌트 사용
- 애니메이션은 2D Animation 패키지 기능과 함께 Animator 컴포넌트 사용

### 버전 관리 고려사항
Git을 사용하는 경우 `.gitignore`에 다음 항목이 포함되어야 합니다:
- `Library/`
- `Temp/`
- `Logs/`
- `obj/`
- `*.csproj` (자동 생성됨)
- `*.sln` / `*.slnx` (선택사항, 자동 생성됨)
- `UserSettings/`

## Visual Scripting

프로젝트는 그래프 기반 프로그래밍을 가능하게 하는 Visual Scripting (1.9.8)을 포함합니다:
- Visual 스크립트는 C# 스크립트의 대안
- 기존 C# 코드와 혼합 가능
- 디자이너와 빠른 프로토타이핑에 유용함
