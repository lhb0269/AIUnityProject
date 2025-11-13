using UnityEngine;
using System.Collections.Generic;

namespace MobileGame.Managers
{
    /// <summary>
    /// 오디오 재생과 관리를 담당하는 매니저
    /// BGM과 SFX를 분리하여 관리
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("오디오 소스")]
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("볼륨 설정")]
        [SerializeField, Range(0f, 1f)] private float masterVolume = 1f;
        [SerializeField, Range(0f, 1f)] private float bgmVolume = 0.7f;
        [SerializeField, Range(0f, 1f)] private float sfxVolume = 1f;

        [Header("모바일 최적화")]
        [SerializeField] private int maxSFXSources = 5;

        private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
        private List<AudioSource> sfxSources = new List<AudioSource>();
        private int currentSFXIndex = 0;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeAudioSources();
            LoadPlayerPrefs();
        }

        /// <summary>
        /// 오디오 소스 초기화
        /// </summary>
        private void InitializeAudioSources()
        {
            // BGM 소스가 없으면 생성
            if (bgmSource == null)
            {
                bgmSource = gameObject.AddComponent<AudioSource>();
                bgmSource.loop = true;
                bgmSource.playOnAwake = false;
            }

            // SFX 소스가 없으면 생성
            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.playOnAwake = false;
            }

            // 추가 SFX 소스 풀 생성 (모바일 최적화)
            for (int i = 0; i < maxSFXSources; i++)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                sfxSources.Add(source);
            }

            ApplyVolume();
        }

        /// <summary>
        /// BGM 재생
        /// </summary>
        public void PlayBGM(AudioClip clip, bool loop = true)
        {
            if (clip == null)
            {
                Debug.LogWarning("[AudioManager] BGM 클립이 null입니다.");
                return;
            }

            bgmSource.clip = clip;
            bgmSource.loop = loop;
            bgmSource.Play();

            Debug.Log($"[AudioManager] BGM 재생: {clip.name}");
        }

        /// <summary>
        /// BGM 정지
        /// </summary>
        public void StopBGM()
        {
            bgmSource.Stop();
        }

        /// <summary>
        /// BGM 일시정지/재개
        /// </summary>
        public void PauseBGM(bool pause)
        {
            if (pause)
                bgmSource.Pause();
            else
                bgmSource.UnPause();
        }

        /// <summary>
        /// SFX 재생 (AudioClip)
        /// </summary>
        public void PlaySFX(AudioClip clip, float volumeScale = 1f)
        {
            if (clip == null)
            {
                Debug.LogWarning("[AudioManager] SFX 클립이 null입니다.");
                return;
            }

            // SFX 소스 풀에서 사용 가능한 소스 찾기
            AudioSource availableSource = GetAvailableSFXSource();
            availableSource.PlayOneShot(clip, volumeScale);
        }

        /// <summary>
        /// 사용 가능한 SFX 소스 가져오기
        /// </summary>
        private AudioSource GetAvailableSFXSource()
        {
            // 재생 중이지 않은 소스 찾기
            foreach (var source in sfxSources)
            {
                if (!source.isPlaying)
                    return source;
            }

            // 모두 재생 중이면 라운드 로빈 방식으로 선택
            currentSFXIndex = (currentSFXIndex + 1) % sfxSources.Count;
            return sfxSources[currentSFXIndex];
        }

        /// <summary>
        /// 마스터 볼륨 설정
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            ApplyVolume();
            SavePlayerPrefs();
        }

        /// <summary>
        /// BGM 볼륨 설정
        /// </summary>
        public void SetBGMVolume(float volume)
        {
            bgmVolume = Mathf.Clamp01(volume);
            ApplyVolume();
            SavePlayerPrefs();
        }

        /// <summary>
        /// SFX 볼륨 설정
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            ApplyVolume();
            SavePlayerPrefs();
        }

        /// <summary>
        /// 볼륨 적용
        /// </summary>
        private void ApplyVolume()
        {
            bgmSource.volume = masterVolume * bgmVolume;

            foreach (var source in sfxSources)
            {
                source.volume = masterVolume * sfxVolume;
            }
        }

        /// <summary>
        /// PlayerPrefs에서 볼륨 설정 로드
        /// </summary>
        private void LoadPlayerPrefs()
        {
            masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
            bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.7f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            ApplyVolume();
        }

        /// <summary>
        /// PlayerPrefs에 볼륨 설정 저장
        /// </summary>
        private void SavePlayerPrefs()
        {
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
            PlayerPrefs.Save();
        }

        public float GetMasterVolume() => masterVolume;
        public float GetBGMVolume() => bgmVolume;
        public float GetSFXVolume() => sfxVolume;
    }
}
