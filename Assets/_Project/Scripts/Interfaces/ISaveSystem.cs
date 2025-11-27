namespace MobileGame.Interfaces
{
    /// <summary>
    /// 저장 시스템 인터페이스
    /// 게임 데이터 저장 및 로드 기능을 정의
    /// </summary>
    public interface ISaveSystem
    {
        // 데이터 저장/로드
        void SaveData<T>(T data) where T : class;
        T LoadData<T>() where T : class, new();

        // 파일 관리
        bool HasSaveFile();
        void DeleteSaveFile();

        // PlayerPrefs 편의 메서드
        void SavePreference(string key, int value);
        void SavePreference(string key, float value);
        void SavePreference(string key, string value);
        int LoadPreferenceInt(string key, int defaultValue = 0);
        float LoadPreferenceFloat(string key, float defaultValue = 0f);
        string LoadPreferenceString(string key, string defaultValue = "");
    }
}
