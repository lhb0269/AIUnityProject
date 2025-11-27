using UnityEngine;

namespace MobileGame.Interfaces
{
    /// <summary>
    /// 오디오 관리 인터페이스
    /// BGM 및 SFX 재생 기능을 정의
    /// </summary>
    public interface IAudioManager
    {
        // BGM 관리
        void PlayBGM(AudioClip clip, bool loop = true);
        void StopBGM();
        void PauseBGM();
        void ResumeBGM();

        // SFX 관리
        void PlaySFX(AudioClip clip);
        void PlaySFX(AudioClip clip, float volume);
        void StopAllSFX();

        // 볼륨 제어
        void SetMasterVolume(float volume);
        void SetBGMVolume(float volume);
        void SetSFXVolume(float volume);
        float GetMasterVolume();
        float GetBGMVolume();
        float GetSFXVolume();
    }
}
