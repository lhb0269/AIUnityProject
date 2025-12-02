using System.Collections.Generic;
using UnityEngine;
using MobileGame.Managers;
using MobileGame.Interfaces;

namespace MobileGame.Tests.Mocks
{
    /// <summary>
    /// 테스트용 AudioManager Mock 클래스
    /// IAudioManager 인터페이스를 구현하여 테스트 격리 제공
    /// </summary>
    public class MockAudioManager : IAudioManager
    {
        // BGM 재생 이력
        public List<AudioClip> PlayedBGMs { get; private set; } = new List<AudioClip>();
        public bool IsBGMPlaying { get; private set; } = false;
        public bool IsBGMPaused { get; private set; } = false;

        // SFX 재생 이력
        public List<AudioClip> PlayedSFXs { get; private set; } = new List<AudioClip>();

        // 볼륨 설정
        public float CurrentMasterVolume { get; private set; } = 1.0f;
        public float CurrentBGMVolume { get; private set; } = 1.0f;
        public float CurrentSFXVolume { get; private set; } = 1.0f;

        public void PlayBGM(AudioClip clip, bool loop = true)
        {
            PlayedBGMs.Add(clip);
            IsBGMPlaying = true;
            Debug.Log($"[MockAudioManager] PlayBGM: {clip?.name}, loop: {loop}");
        }

        public void StopBGM()
        {
            IsBGMPlaying = false;
            Debug.Log("[MockAudioManager] StopBGM");
        }

        public void PauseBGM(bool pause)
        {
            IsBGMPaused = pause;
            Debug.Log($"[MockAudioManager] PauseBGM: {pause}");
        }

        public void PlaySFX(AudioClip clip, float volumeScale = 1f)
        {
            PlayedSFXs.Add(clip);
            Debug.Log($"[MockAudioManager] PlaySFX: {clip?.name}, volume: {volumeScale}");
        }

        public void StopAllSFX()
        {
            Debug.Log("[MockAudioManager] StopAllSFX");
        }

        public void SetMasterVolume(float volume)
        {
            CurrentMasterVolume = Mathf.Clamp01(volume);
            Debug.Log($"[MockAudioManager] SetMasterVolume: {volume}");
        }

        public void SetBGMVolume(float volume)
        {
            CurrentBGMVolume = Mathf.Clamp01(volume);
            Debug.Log($"[MockAudioManager] SetBGMVolume: {volume}");
        }

        public void SetSFXVolume(float volume)
        {
            CurrentSFXVolume = Mathf.Clamp01(volume);
            Debug.Log($"[MockAudioManager] SetSFXVolume: {volume}");
        }

        public float GetMasterVolume()
        {
            return CurrentMasterVolume;
        }

        public float GetBGMVolume()
        {
            return CurrentBGMVolume;
        }

        public float GetSFXVolume()
        {
            return CurrentSFXVolume;
        }

        /// <summary>
        /// 테스트 초기화 - 모든 기록 삭제
        /// </summary>
        public void Reset()
        {
            PlayedBGMs.Clear();
            PlayedSFXs.Clear();
            IsBGMPlaying = false;
            IsBGMPaused = false;
            CurrentMasterVolume = 1.0f;
            CurrentBGMVolume = 1.0f;
            CurrentSFXVolume = 1.0f;
        }
    }
}
