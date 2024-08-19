using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private List<AudioClipWithType> audioClipsWithTypes = new List<AudioClipWithType>();
    [SerializeField] private AudioSource             skillsAudioSource;
    [SerializeField] private AudioSource             menuAudioSource;
    [SerializeField] private AudioSource             musicAudioSource;
    [SerializeField] private AudioMixer              audioMixer;

    private Dictionary<SoundType, AudioClip> _audio_clips = new Dictionary<SoundType, AudioClip>();

    public void playSound(SoundType sound_type)
    {
        if (_audio_clips.TryGetValue(sound_type, out AudioClip clip))
        {
        }
        else
        {
            clip = audioClipsWithTypes.FirstOrDefault(x => x.soundType == sound_type)?.audioClip;

            if (!clip)
                return;

            _audio_clips[sound_type] = clip;
        }

        if (sound_type is > SoundType.GAME_SFX and < SoundType.GAME_SFX_AFTER_LAST)
            skillsAudioSource.PlayOneShot(clip);
        else if (sound_type >= SoundType.GAME_MUSIC && sound_type <= SoundType.MENU_MUSIC)
        {
            musicAudioSource.clip = clip;
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }
        else if (sound_type == SoundType.CLICK)
        {
            menuAudioSource.PlayOneShot(clip);
        }
    }

    public void playSoundRandom(params SoundType[] sound_types)
    {
        playSound(sound_types[UnityEngine.Random.Range(0, sound_types.Length)]);
    }

    public void playSoundTest(string name) => playSound(Enum.Parse<SoundType>(name));

    public void playClickSound() => playSound(SoundType.CLICK);

    public void playMenuMusic() => playSound(SoundType.MENU_MUSIC);

    public void playGameMusic() => playSound(SoundType.GAME_MUSIC);

    public void fadeToMenuMusic()
    {
        fadeMusic(SoundType.MENU_MUSIC);
    }

    public void fadeToGameMusic()
    {
        fadeMusic(SoundType.GAME_MUSIC);
    }

    public void stopAllMusic()
    {
        musicAudioSource.DOFade(0.0f, 1.0f);
    }

    public void setMusicVolume(float volume) => audioMixer.SetFloat("MusicVolume", volume < -38 ? -80 : volume);
    public void setSFXVolume(float volume) => audioMixer.SetFloat("SFXVolume", volume < -38 ? -80 : volume);

    public void shakeCamera() => FindObjectOfType<CameraScript>().tweenDamageTaken();

    private void fadeMusic(SoundType end_music)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(musicAudioSource.DOFade(0.0f, 0.3f));
        sequence.AppendCallback(musicAudioSource.Stop);
        sequence.AppendCallback(() => 
            {
                musicAudioSource.clip = audioClipsWithTypes.FirstOrDefault(x => x.soundType == end_music)?.audioClip;
                musicAudioSource.Play();
            }
        );
        sequence.Append(musicAudioSource.DOFade(1.0f, 1.7f));
        sequence.Play();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}

public enum SoundType
{
    NONE,

    GAME_SFX,
    GRAPLING_SHOT_1,
    GRAPLING_SHOT_2,

    GRAPLING_HIT_1,
    GRAPLING_HIT_2,

    TITAN_SCREAM_1,
    TITAN_SCREAM_2,
    TITAN_SCREAM_3,

    PLAYER_DEATH_SMASHED,
    PLAYER_DEATH_EATEN,
    

    GAME_SFX_AFTER_LAST,

    CLICK,

    GAME_MUSIC = 100,
    //place for more music
    MENU_MUSIC
}

[Serializable]
public class AudioClipWithType
{
    public SoundType soundType;
    public AudioClip audioClip;
} 
