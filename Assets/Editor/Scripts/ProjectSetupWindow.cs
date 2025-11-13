using UnityEngine;
using UnityEditor;
using System.IO;

namespace MobileGame.Editor
{
    /// <summary>
    /// 프로젝트 설정을 도와주는 에디터 윈도우
    /// </summary>
    public class ProjectSetupWindow : EditorWindow
    {
        private string projectName = "MyMobileGame";
        private bool createFolders = true;
        private bool setupMobileSettings = true;

        [MenuItem("Tools/Project Setup")]
        public static void ShowWindow()
        {
            GetWindow<ProjectSetupWindow>("Project Setup");
        }

        private void OnGUI()
        {
            GUILayout.Label("프로젝트 설정", EditorStyles.boldLabel);
            GUILayout.Space(10);

            projectName = EditorGUILayout.TextField("프로젝트 이름", projectName);
            GUILayout.Space(5);

            createFolders = EditorGUILayout.Toggle("폴더 구조 생성", createFolders);
            setupMobileSettings = EditorGUILayout.Toggle("모바일 설정 적용", setupMobileSettings);

            GUILayout.Space(20);

            if (GUILayout.Button("설정 적용", GUILayout.Height(30)))
            {
                ApplySettings();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("기본 씬 설정 생성"))
            {
                CreateDefaultSceneSetup();
            }
        }

        private void ApplySettings()
        {
            if (setupMobileSettings)
            {
                ApplyMobileSettings();
            }

            EditorUtility.DisplayDialog("완료", "프로젝트 설정이 적용되었습니다.", "확인");
        }

        private void ApplyMobileSettings()
        {
            // 모바일 플랫폼 설정
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            PlayerSettings.accelerometerFrequency = 60;

            // Android 설정
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;

            // iOS 설정
            PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneAndiPad;
            PlayerSettings.iOS.requiresFullScreen = true;

            // 그래픽 설정
            PlayerSettings.colorSpace = ColorSpace.Linear;
            PlayerSettings.gpuSkinning = true;

            // 품질 설정
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;

            Debug.Log("[ProjectSetup] 모바일 설정 적용 완료");
        }

        private void CreateDefaultSceneSetup()
        {
            // 기본 씬 설정 생성 (GameManager, EventSystem 등)
            GameObject managers = new GameObject("--- Managers ---");

            // GameManager 추가
            GameObject gameManager = new GameObject("GameManager");
            gameManager.transform.SetParent(managers.transform);
            gameManager.AddComponent<Managers.GameManager>();

            // AudioManager 추가
            GameObject audioManager = new GameObject("AudioManager");
            audioManager.transform.SetParent(managers.transform);
            audioManager.AddComponent<Managers.AudioManager>();

            // UIManager 추가
            GameObject uiManager = new GameObject("UIManager");
            uiManager.transform.SetParent(managers.transform);
            uiManager.AddComponent<Managers.UIManager>();

            // InputManager 추가
            GameObject inputManager = new GameObject("InputManager");
            inputManager.transform.SetParent(managers.transform);
            inputManager.AddComponent<Managers.InputManager>();

            // SaveSystem 추가
            GameObject saveSystem = new GameObject("SaveSystem");
            saveSystem.transform.SetParent(managers.transform);
            saveSystem.AddComponent<Managers.SaveSystem>();

            // SceneLoader 추가
            GameObject sceneLoader = new GameObject("SceneLoader");
            sceneLoader.transform.SetParent(managers.transform);
            sceneLoader.AddComponent<Managers.SceneLoader>();

            Debug.Log("[ProjectSetup] 기본 씬 설정 생성 완료");
            EditorUtility.DisplayDialog("완료", "기본 씬 설정이 생성되었습니다.", "확인");
        }
    }
}
