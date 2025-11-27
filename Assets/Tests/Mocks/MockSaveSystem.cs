using System.Collections.Generic;
using UnityEngine;
using MobileGame.Managers;
using MobileGame.Interfaces;

namespace MobileGame.Tests.Mocks
{
    /// <summary>
    /// 테스트용 SaveSystem Mock 클래스
    /// ISaveSystem 인터페이스를 구현하여 테스트 격리 제공
    /// </summary>
    public class MockSaveSystem : ISaveSystem
    {
        // 인메모리 저장소 (실제 파일 시스템 사용 안함)
        private Dictionary<string, string> inMemoryStorage = new Dictionary<string, string>();
        private string savedJsonData = null;

        // 저장/로드 이력
        public int SaveCount { get; private set; } = 0;
        public int LoadCount { get; private set; } = 0;

        public void SaveData<T>(T data) where T : class
        {
            if (data == null)
            {
                Debug.LogWarning("[MockSaveSystem] SaveData: data is null");
                return;
            }

            savedJsonData = JsonUtility.ToJson(data);
            SaveCount++;
            Debug.Log($"[MockSaveSystem] SaveData: {typeof(T).Name}, Count: {SaveCount}");
        }

        public T LoadData<T>() where T : class, new()
        {
            LoadCount++;

            if (string.IsNullOrEmpty(savedJsonData))
            {
                Debug.Log($"[MockSaveSystem] LoadData: No data found, creating new {typeof(T).Name}");
                return new T();
            }

            T loadedData = JsonUtility.FromJson<T>(savedJsonData);
            Debug.Log($"[MockSaveSystem] LoadData: {typeof(T).Name}, Count: {LoadCount}");
            return loadedData;
        }

        public bool HasSaveData()
        {
            return !string.IsNullOrEmpty(savedJsonData);
        }

        public void DeleteSaveData()
        {
            savedJsonData = null;
            Debug.Log("[MockSaveSystem] DeleteSaveData");
        }

        public void SetInt(string key, int value)
        {
            inMemoryStorage[key] = value.ToString();
            Debug.Log($"[MockSaveSystem] SetInt: {key} = {value}");
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            if (inMemoryStorage.TryGetValue(key, out string value))
            {
                return int.Parse(value);
            }
            return defaultValue;
        }

        public void SetFloat(string key, float value)
        {
            inMemoryStorage[key] = value.ToString();
            Debug.Log($"[MockSaveSystem] SetFloat: {key} = {value}");
        }

        public float GetFloat(string key, float defaultValue = 0f)
        {
            if (inMemoryStorage.TryGetValue(key, out string value))
            {
                return float.Parse(value);
            }
            return defaultValue;
        }

        public void SetString(string key, string value)
        {
            inMemoryStorage[key] = value;
            Debug.Log($"[MockSaveSystem] SetString: {key} = {value}");
        }

        public string GetString(string key, string defaultValue = "")
        {
            if (inMemoryStorage.TryGetValue(key, out string value))
            {
                return value;
            }
            return defaultValue;
        }

        public bool HasKey(string key)
        {
            return inMemoryStorage.ContainsKey(key);
        }

        public void DeleteKey(string key)
        {
            inMemoryStorage.Remove(key);
            Debug.Log($"[MockSaveSystem] DeleteKey: {key}");
        }

        public void DeleteAll()
        {
            inMemoryStorage.Clear();
            savedJsonData = null;
            Debug.Log("[MockSaveSystem] DeleteAll");
        }

        /// <summary>
        /// 테스트 초기화 - 모든 기록 삭제
        /// </summary>
        public void Reset()
        {
            inMemoryStorage.Clear();
            savedJsonData = null;
            SaveCount = 0;
            LoadCount = 0;
        }
    }
}
