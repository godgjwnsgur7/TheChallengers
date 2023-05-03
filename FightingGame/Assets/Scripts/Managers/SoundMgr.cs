using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public enum ENUM_SOUND_TYPE
{
    BGM = 0,
    SFX = 1,
    MASTER = 2,
}

public class AudioClipData
{
    public AudioClip audioClip;
    public SfxSound sfxSound;
    public float volume;

    public AudioClipData(AudioClip _audioClip, SfxSound _sfxSound, float _volume)
    {
        audioClip = _audioClip;
        sfxSound = _sfxSound;
        volume = _volume;
    }
}

public class SoundMgr
{
    AudioSource[] audioSources = new AudioSource[(int)ENUM_SOUND_TYPE.MASTER]; // BGM, SFX
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>(); // 키 : 파일경로

    Coroutine fadeOutInBGMCoroutine;

    VolumeData volumeData = null;

    ENUM_BGM_TYPE currBGM = ENUM_BGM_TYPE.Unknown;

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(ENUM_SOUND_TYPE)); // BGM, SFX
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            audioSources[(int)ENUM_SOUND_TYPE.BGM].loop = true; // BGM은 반복 무한 재생

            // SFX 3D사운드 기본 값
            Set_SFXSoundSetting(audioSources[(int)ENUM_SOUND_TYPE.SFX]);

            volumeData = PlayerPrefsManagement.Load_VolumeData();
            audioSources[(int)ENUM_SOUND_TYPE.BGM].mute = volumeData.isBgmMute;
            audioSources[(int)ENUM_SOUND_TYPE.SFX].mute = volumeData.isSfxMute;
        }
    }
    
    public void Clear()
    {

    }

    public void Set_SFXSoundSetting(AudioSource audioSource)
    {
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.spatialBlend = 1.0f;

        SoundSettingInfo soundSettingInfo = Managers.Data.soundSettingInfo;
        audioSource.dopplerLevel = soundSettingInfo.dopplerLevel;
        audioSource.spread = soundSettingInfo.spread;
        audioSource.minDistance = soundSettingInfo.minDistance;
        audioSource.maxDistance = soundSettingInfo.maxDistance;
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


    public void Play_BGM(ENUM_BGM_TYPE bgmType)
    {
        if (fadeOutInBGMCoroutine != null)
            CoroutineHelper.StopCoroutine(fadeOutInBGMCoroutine);

        // BGM 사운드 구현부는 코루틴 안에서 수행
        fadeOutInBGMCoroutine = CoroutineHelper.StartCoroutine(IFadeOutIn_BGM(bgmType));
    }

    private void Set_BgmAudioSource(BgmSound _bgmSoundData)
    {
        audioSources[(int)ENUM_SOUND_TYPE.BGM].priority = _bgmSoundData.priority;
        audioSources[(int)ENUM_SOUND_TYPE.BGM].pitch = _bgmSoundData.pitch;
        audioSources[(int)ENUM_SOUND_TYPE.BGM].panStereo = _bgmSoundData.stereoPan;
        audioSources[(int)ENUM_SOUND_TYPE.BGM].spatialBlend = _bgmSoundData.spatialBlend;
        audioSources[(int)ENUM_SOUND_TYPE.BGM].reverbZoneMix = _bgmSoundData.reverbZoneMix;
    }

    private void Set_SfxAudioSource(SfxSound _sfxSoundData)
    {
        audioSources[(int)ENUM_SOUND_TYPE.SFX].priority = _sfxSoundData.priority;
        audioSources[(int)ENUM_SOUND_TYPE.SFX].pitch = _sfxSoundData.pitch;
        audioSources[(int)ENUM_SOUND_TYPE.SFX].panStereo = _sfxSoundData.stereoPan;
        audioSources[(int)ENUM_SOUND_TYPE.SFX].spatialBlend = _sfxSoundData.spatialBlend;
        audioSources[(int)ENUM_SOUND_TYPE.SFX].reverbZoneMix = _sfxSoundData.reverbZoneMix;
    }

    public void Play_SFX(ENUM_SFX_TYPE sfxType)
    {
        float _currSfxVolume = volumeData.masterVolume * volumeData.sfxVolume;

        string path = $"Sounds/SFX/{sfxType}";
        AudioClip audioClip = GetOrAddAudioClip(path);

        if (audioClip == null)
            return;

        SfxSound sfxSoundData = null;

        if (Managers.Data.SfxSoundDict.ContainsKey((int)sfxType))
            sfxSoundData = Managers.Data.SfxSoundDict[(int)sfxType];

        if (sfxSoundData != null)
        {
            _currSfxVolume *= sfxSoundData.volume;
            Set_SfxAudioSource(sfxSoundData);
        }

        audioSources[(int)ENUM_SOUND_TYPE.SFX].volume = _currSfxVolume;
        audioSources[(int)ENUM_SOUND_TYPE.SFX].PlayOneShot(audioClip);
    }

    public AudioClipData Get_SFXAudioClipData(ENUM_SFX_TYPE sfxType)
    {
        float _currSfxVolume = volumeData.masterVolume * volumeData.sfxVolume;

        string path = $"Sounds/SFX/{sfxType}";
        AudioClip audioClip = GetOrAddAudioClip(path);

        if (audioClip == null)
            return null;

        SfxSound sfxSoundData = null;

        if (Managers.Data.SfxSoundDict.ContainsKey((int)sfxType))
            sfxSoundData = Managers.Data.SfxSoundDict[(int)sfxType];

        if (sfxSoundData != null)
        {
            _currSfxVolume *= sfxSoundData.volume;
            Set_SfxAudioSource(sfxSoundData);
        }

        AudioClipData audioClipData = new AudioClipData(audioClip, sfxSoundData, _currSfxVolume);
        return audioClipData;
    }

    private void Update_BGMAudioSource(float _currVolume)
    {
        audioSources[(int)ENUM_SOUND_TYPE.BGM].volume = volumeData.masterVolume * _currVolume;
    }

    private IEnumerator IFadeOutIn_BGM(ENUM_BGM_TYPE bgmType)
    {
        float _currBgmVolume = volumeData.masterVolume * volumeData.bgmVolume;
        float currVolume = audioSources[(int)ENUM_SOUND_TYPE.BGM].volume;

        string path = $"Sounds/BGM/{bgmType}";

        AudioClip audioClip = GetOrAddAudioClip(path);

        if (audioSources[(int)ENUM_SOUND_TYPE.BGM].isPlaying) // 실행 중일 경우
        {
            // FadeOut
            while (currVolume > 0.1f)
            {
                currVolume -= Time.deltaTime;
                Update_BGMAudioSource(currVolume);

                yield return null;
            }
        }

        currVolume = 0.0f;
        Update_BGMAudioSource(currVolume);

        currBGM = bgmType;
        if(audioClip != null)
            audioSources[(int)ENUM_SOUND_TYPE.BGM].clip = audioClip;

        if (bgmType != ENUM_BGM_TYPE.Unknown)
        {
            BgmSound bgmSoundData = null;
            if (Managers.Data.BgmSoundDict.ContainsKey((int)bgmType))
                bgmSoundData = Managers.Data.BgmSoundDict[(int)bgmType];

            if (bgmSoundData != null)
            {
                _currBgmVolume *= bgmSoundData.volume;
                Set_BgmAudioSource(bgmSoundData);
            }

            audioSources[(int)ENUM_SOUND_TYPE.BGM].Play();

            // FadeIn
            while (currVolume < _currBgmVolume)
            {
                currVolume += Time.deltaTime;
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