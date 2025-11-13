using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace MobileGame.Editor
{
    /// <summary>
    /// 에디터에서 씬을 빠르게 로드할 수 있는 도구
    /// </summary>
    public class SceneQuickStart : EditorWindow
    {
        [MenuItem("Tools/Scene Quick Start")]
        public static void ShowWindow()
        {
            GetWindow<SceneQuickStart>("Scene Quick Start");
        }

        private void OnGUI()
        {
            GUILayout.Label("빠른 씬 로드", EditorStyles.boldLabel);
            GUILayout.Space(10);

            // Build Settings에 등록된 씬들 표시
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                var scene = EditorBuildSettings.scenes[i];
                if (scene.enabled)
                {
                    string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);

                    if (GUILayout.Button($"{i}: {sceneName}"))
                    {
                        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        {
                            EditorSceneManager.OpenScene(scene.path);
                        }
                    }
                }
            }
        }
    }
}
