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

public class SoundMgr
{
    AudioSource[] audioSources = new AudioSource[(int)ENUM_SOUND_TYPE.MASTER]; // BGM, SFX
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>(); // 키 : 파일경로

    Coroutine fadeOutInBGMCoroutine;

    VolumeData volumeData;

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

            volumeData = PlayerPrefsManagement.Load_VolumeData();
        }
    }
    
    public void Clear()
    {
        Play_BGM(ENUM_BGM_TYPE.Unknown);
    }

    public VolumeData Get_CurrVolumeData()
    {
        if (volumeData == null)
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
        if (currBGM == bgmType)
            return;

        if (fadeOutInBGMCoroutine != null)
            CoroutineHelper.StopCoroutine(fadeOutInBGMCoroutine);

        fadeOutInBGMCoroutine = CoroutineHelper.StartCoroutine(IFadeOutIn_BGM(bgmType));
    }

    public void Play_SFX(ENUM_SFX_TYPE sfxType)
    {
        float currSfxVolume = volumeData.masterVolume * volumeData.sfxVolume;

        string path = $"Sounds/SFX/{sfxType}";
        AudioClip audioClip = GetOrAddAudioClip(path);

        if (audioClip == null || currSfxVolume == 0)
            return;

        audioSources[(int)ENUM_SOUND_TYPE.SFX].volume = currSfxVolume;
        audioSources[(int)ENUM_SOUND_TYPE.SFX].PlayOneShot(audioClip);
    }

    private void Update_BGMAudioSource(float _currVolume)
    {
        audioSources[(int)ENUM_SOUND_TYPE.BGM].volume = volumeData.masterVolume * _currVolume;
    }

    private IEnumerator IFadeOutIn_BGM(ENUM_BGM_TYPE bgmType)
    {
        string path = $"Sounds/BGM/{bgmType}";

        AudioClip audioClip = GetOrAddAudioClip(path);

        float currVolume = audioSources[(int)ENUM_SOUND_TYPE.BGM].volume;

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
            audioSources[(int)ENUM_SOUND_TYPE.BGM].Play();

            // FadeIn
            while (currVolume < 0.9f)
            {
                currVolume += Time.deltaTime;
                Update_BGMAudioSource(currVolume);

                yield return null;
            }

            currVolume = 1.0f;
            Update_BGMAudioSource(currVolume);

        }
        else // BGM을 끌 경우
        {
            currVolume = 0.0f;
            Update_BGMAudioSource(currVolume);
            audioSources[(int)ENUM_SOUND_TYPE.BGM].Stop();
        }

        fadeOutInBGMCoroutine = null;
    }
}