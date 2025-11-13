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

## 라이선스

이 프로젝트는 개인 학습 및 개발 목적으로 사용됩니다.

## 문의

프로젝트 관련 문의사항은 GitHub Issues를 통해 남겨주세요.