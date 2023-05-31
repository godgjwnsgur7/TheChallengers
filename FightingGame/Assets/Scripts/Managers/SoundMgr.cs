using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public enum ENUM_SOUND_TYPE
{
    BGM = 0,
    SFX = 1,
    MASTER = 2,
}

public class AudioClipVolume
{
    public AudioClip audioClip;
    public float volume;
    public AudioClipVolume(AudioClip _audioClip, float _volume)
    {
        audioClip = _audioClip;
        volume = _volume;
    }
}

public class SoundMgr
{
    AudioSource[] audioSources = new AudioSource[(int)ENUM_SOUND_TYPE.MASTER]; // BGM, SFX
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>(); // 키 : 파일경로

    Coroutine fadeOutInBGMCoroutine;
    Coroutine bgmStopCoroutine;

    VolumeData volumeData = null;

    AudioListener audioListener = null;

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            UnityEngine.Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(ENUM_SOUND_TYPE)); // BGM, SFX
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            audioSources[(int)ENUM_SOUND_TYPE.BGM].loop = true; // BGM은 반복 무한 재생

            volumeData = PlayerPrefsManagement.Load_VolumeData();
            audioSources[(int)ENUM_SOUND_TYPE.BGM].mute = volumeData.isBgmMute;
            audioSources[(int)ENUM_SOUND_TYPE.SFX].mute = volumeData.isSfxMute;
        }
    }
    
    public void Clear()
    {
        audioListener = null;
    }

    public void Set_SFXSoundSetting(AudioSource audioSource)
    {
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;

        GameInfo gameInfo = Managers.Data.gameInfo;

        audioSource.minDistance = gameInfo.soundMinDistance;
        audioSource.maxDistance = gameInfo.soundMaxDistance;

        audioSource.spatialBlend = 1.0f;
        audioSource.reverbZoneMix = 0.0f;

        audioSource.dopplerLevel = 0.0f;
    }

    public void Set_Vibration(bool _isVibration)
    {
        volumeData.isVibration = _isVibration;
    }

    public VolumeData Get_CurrVolumeData()
    {
        volumeData = PlayerPrefsManagement.Load_VolumeData();

        return volumeData;
    }

    public float Get_AudioListenerWorldPosX()
    {
        if(audioListener == null)
            audioListener = MonoBehaviour.FindObjectOfType<AudioListener>();

        return audioListener.gameObject.transform.position.x;
    }

    public void Save_CurrVolumeData()
    {
        PlayerPrefsManagement.Save_VolumeData(volumeData);
    }

    public void Update_VolumeData(VolumeData _volumeData)
    {
        volumeData = _volumeData;
        audioSources[(int)ENUM_SOUND_TYPE.BGM].volume = volumeData.masterVolume * volumeData.bgmVolume;
        audioSources[(int)ENUM_SOUND_TYPE.SFX].volume = volumeData.masterVolume * volumeData.sfxVolume;
    }
    public void Update_MasterVolumeData(float _volumeValue)
    {
        volumeData.masterVolume = _volumeValue;
        audioSources[(int)ENUM_SOUND_TYPE.BGM].volume = volumeData.masterVolume * volumeData.bgmVolume;
        audioSources[(int)ENUM_SOUND_TYPE.SFX].volume = volumeData.masterVolume * volumeData.sfxVolume;
    }
    public void Update_BGMVolumeData(float _volumeValue)
    {
        volumeData.bgmVolume = _volumeValue;
        audioSources[(int)ENUM_SOUND_TYPE.BGM].volume = volumeData.masterVolume * volumeData.bgmVolume;
    }
    public void Update_SFXVolumeData(float _volumeValue)
    {
        volumeData.sfxVolume = _volumeValue;
        audioSources[(int)ENUM_SOUND_TYPE.SFX].volume = volumeData.masterVolume * volumeData.sfxVolume;
    }

    public void Update_SoundMuteData(ENUM_SOUND_TYPE _soundType, bool _isMute)
    {
        switch (_soundType)
        {
            case ENUM_SOUND_TYPE.MASTER:
                volumeData.isBgmMute = _isMute;
                volumeData.isSfxMute = _isMute;
                audioSources[(int)ENUM_SOUND_TYPE.BGM].mute = _isMute;
                audioSources[(int)ENUM_SOUND_TYPE.SFX].mute = _isMute;
                break;
            case ENUM_SOUND_TYPE.BGM:
                volumeData.isBgmMute = _isMute;
                audioSources[(int)ENUM_SOUND_TYPE.BGM].mute = _isMute;
                break;
            case ENUM_SOUND_TYPE.SFX:
                volumeData.isSfxMute = _isMute;
                audioSources[(int)ENUM_SOUND_TYPE.SFX].mute = _isMute;
                break;
        }
    }

    private AudioClip GetOrAddAudioClip(string path)
    {
        AudioClip audioClip = null;
        if (audioClips.TryGetValue(path, out audioClip) == false)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
            audioClips.Add(path, audioClip);
        }

        if (audioClip == null)
        {
            Debug.Log($"AudioClip Error! : {path}");
        }

        return audioClip;
    }

    public void Stop_BGM()
    {
        if (bgmStopCoroutine != null)
            CoroutineHelper.StopCoroutine(bgmStopCoroutine);

        bgmStopCoroutine = CoroutineHelper.StartCoroutine(IStop_BGM());
    }

    public void Play_BGM(ENUM_BGM_TYPE bgmType)
    {
        if (fadeOutInBGMCoroutine != null)
            CoroutineHelper.StopCoroutine(fadeOutInBGMCoroutine);

        // BGM 사운드 구현부는 코루틴 안에서 수행
        fadeOutInBGMCoroutine = CoroutineHelper.StartCoroutine(IFadeOutIn_BGM(bgmType));
    }

    public void Play_SFX(ENUM_SFX_TYPE sfxType)
    {
        float _currSfxVolume = volumeData.masterVolume * volumeData.sfxVolume;

        string path = $"Sounds/SFX/{sfxType}";
        AudioClip audioClip = GetOrAddAudioClip(path);

        if (audioClip == null)
            return;

        audioSources[(int)ENUM_SOUND_TYPE.SFX].volume = _currSfxVolume;
        audioSources[(int)ENUM_SOUND_TYPE.SFX].PlayOneShot(audioClip);
    }

    /// <summary>
    /// 특정 좌표에서 3D SFX 사운드 재생을 위한 함수 (오버로드)
    /// </summary>
    public void Play_SFX(ENUM_SFX_TYPE sfxType, Vector3 worldPosVec)
    {
        if (Get_SFXSoundMuteState())
            return;

        AudioClipVolume audioClipVolume = Get_AudioClipVolume(sfxType);

        if (audioClipVolume == null)
            return;

        OneShotAudioObject oneShotAudioObject = Managers.Resource.Instantiate($"PublicObjects/OneShotAudio").GetComponent<OneShotAudioObject>();

        oneShotAudioObject.Play_SFX(audioClipVolume, worldPosVec);
    }

    /// <summary>
    /// 호출자의 위치를 따라가는 3D SFX 사운드 재생을 위한 함수 (오버로드)
    /// </summary>
    public void PlaySFX_FollowingSound(ENUM_SFX_TYPE sfxType, Vector3 worldPosVec, Transform target)
    {
        if (Get_SFXSoundMuteState())
            return;

        AudioClipVolume audioClipVolume = Get_AudioClipVolume(sfxType);

        if (audioClipVolume == null)
            return;

        OneShotAudioObject oneShotAudioObject = Managers.Resource.Instantiate($"PublicObjects/OneShotAudio").GetComponent<OneShotAudioObject>();

        oneShotAudioObject.PlaySFX_FollowingSound(audioClipVolume, target);
    }

    public AudioClipVolume Get_AudioClipVolume(ENUM_SFX_TYPE sfxType)
    {
        float _currSfxVolume = volumeData.masterVolume * volumeData.sfxVolume;

        string path = $"Sounds/SFX/{sfxType}";
        AudioClip audioClip = GetOrAddAudioClip(path);

        if (audioClip == null)
            return null;

        AudioClipVolume audioClipVolume = new AudioClipVolume(audioClip, _currSfxVolume);

        return audioClipVolume;
    }

    public bool Get_SFXSoundMuteState()
    {
        return volumeData.isSfxMute;
    }

    private void Update_BGMAudioSource(float _currVolume)
    {
        audioSources[(int)ENUM_SOUND_TYPE.BGM].volume = volumeData.masterVolume * _currVolume;
    }

    private IEnumerator IStop_BGM()
    {
        yield return new WaitUntil(() => fadeOutInBGMCoroutine == null);

        float currVolume = audioSources[(int)ENUM_SOUND_TYPE.BGM].volume;

        if (audioSources[(int)ENUM_SOUND_TYPE.BGM].isPlaying) // 실행 중일 경우
        {
            // FadeOut
            while (currVolume > 0.05f)
            {
                currVolume -= Time.deltaTime * 0.3f;
                Update_BGMAudioSource(currVolume);

                yield return null;
            }
        }

        currVolume = 0.0f;
        Update_BGMAudioSource(currVolume);
        audioSources[(int)ENUM_SOUND_TYPE.BGM].Stop();

        bgmStopCoroutine = null;
    }

    private IEnumerator IFadeOutIn_BGM(ENUM_BGM_TYPE bgmType)
    {
        yield return new WaitUntil(() => bgmStopCoroutine == null);

        float _currBgmVolume = volumeData.masterVolume * volumeData.bgmVolume;
        float currVolume = audioSources[(int)ENUM_SOUND_TYPE.BGM].volume;

        string path = $"Sounds/BGM/{bgmType}";

        AudioClip audioClip = GetOrAddAudioClip(path);

        if (audioSources[(int)ENUM_SOUND_TYPE.BGM].isPlaying) // 실행 중일 경우
        {
            // FadeOut
            while (currVolume > 0.05f)
            {
                currVolume -= Time.deltaTime * 0.3f;
                Update_BGMAudioSource(currVolume);

                yield return null;
            }
        }

        currVolume = 0.0f;
        Update_BGMAudioSource(0.0f);

        if(audioClip != null)
            audioSources[(int)ENUM_SOUND_TYPE.BGM].clip = audioClip;

        if (bgmType != ENUM_BGM_TYPE.Unknown)
        {
            audioSources[(int)ENUM_SOUND_TYPE.BGM].Play();
            
            // FadeIn
            while (currVolume < _currBgmVolume)
            {
                currVolume += Time.deltaTime * 0.3f;
                Update_BGMAudioSource(currVolume);

                yield return null;
            }

            currVolume = _currBgmVolume;
            Update_BGMAudioSource(currVolume);
        }
        else // ENUM_BGM_TYPE.Unknown가 들어왔을 경우
        {
            currVolume = 0.0f;
            Update_BGMAudioSource(currVolume);
            audioSources[(int)ENUM_SOUND_TYPE.BGM].Stop();
        }

        fadeOutInBGMCoroutine = null;
    }
}