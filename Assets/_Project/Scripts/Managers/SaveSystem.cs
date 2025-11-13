using UnityEngine;
using System;
using System.IO;

namespace MobileGame.Managers
{
    /// <summary>
    /// 게임 데이터 저장 및 로드를 관리하는 시스템
    /// JSON 기반의 간단한 저장 시스템
    /// </summary>
    public class SaveSystem : MonoBehaviour
    {
        public static SaveSystem Instance { get; private set; }

        private string savePath;
        private const string SAVE_FILE_NAME = "gamedata.json";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeSavePath();
        }

        /// <summary>
        /// 저장 경로 초기화
        /// </summary>
        private void InitializeSavePath()
        {
            savePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
            Debug.Log($"[SaveSystem] 저장 경로: {savePath}");
        }

        /// <summary>
        /// 데이터 저장
        /// </summary>
        public void SaveData<T>(T data) where T : class
        {
            try
            {
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(savePath, json);
                Debug.Log($"[SaveSystem] 데이터 저장 완료: {typeof(T).Name}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] 데이터 저장 실패: {e.Message}");
            }
        }

        /// <summary>
        /// 데이터 로드
        /// </summary>
        public T LoadData<T>() where T : class, new()
        {
            try
            {
                if (File.Exists(savePath))
                {
                    string json = File.ReadAllText(savePath);
                    T data = JsonUtility.FromJson<T>(json);
                    Debug.Log($"[SaveSystem] 데이터 로드 완료: {typeof(T).Name}");
                    return data;
                }
                else
                {
                    Debug.LogWarning($"[SaveSystem] 저장 파일이 없습니다. 새로운 데이터 생성");
                    return new T();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] 데이터 로드 실패: {e.Message}");
                return new T();
            }
        }

        /// <summary>
        /// 저장 파일 존재 여부 확인
        /// </summary>
        public bool HasSaveData()
        {
            return File.Exists(savePath);
        }

        /// <summary>
        /// 저장 데이터 삭제
        /// </summary>
        public void DeleteSaveData()
        {
            try
            {
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                    Debug.Log("[SaveSystem] 저장 데이터 삭제 완료");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] 저장 데이터 삭제 실패: {e.Message}");
            }
        }

        /// <summary>
        /// PlayerPrefs를 사용한 간단한 값 저장
        /// </summary>
        public void SavePreference(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }

        public void SavePreference(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
        }

        public void SavePreference(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public int LoadPreferenceInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public float LoadPreferenceFloat(string key, float defaultValue = 0f)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public string LoadPreferenceString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }
    }

    /// <summary>
    /// 게임 데이터 예시 클래스
    /// 실제 게임에 맞게 수정하여 사용
    /// </summary>
    [Serializable]
    public class GameData
    {
        public int playerLevel = 1;
        public int playerScore = 0;
        public int coins = 0;
        public bool soundEnabled = true;
        public bool musicEnabled = true;

        // 추가 데이터 필드를 여기에 추가
    }
}
