using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static AudioManager instance;

    // 배경음악 재생용 AudioSource
    public AudioSource bgmSource;
    
    // 효과음 재생용 AudioSource
    public AudioSource sfxSource;
    
    // 오디오 클립 딕셔너리
    public Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();
    
    // 인스펙터에서 할당할 수 있는 사운드 배열
    public Sound[] sounds;
    
    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // 딕셔너리에 사운드 추가
            foreach (Sound sound in sounds)
            {
                soundDictionary.Add(sound.name, sound.clip);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // 배경음악 재생
    public void PlayBGM(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            bgmSource.clip = soundDictionary[soundName];
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }
    
    // 효과음 재생
    public void PlaySFX(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            sfxSource.PlayOneShot(soundDictionary[soundName]);
        }
    }
    
    // 볼륨 조절 함수
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}