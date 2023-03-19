using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public enum ENUM_SOUND_TYPE
{
    BGM = 0,
    SFX = 1,
    Max = 2,
}

public class SoundMgr
{
    AudioSource[] audioSources = new AudioSource[(int)ENUM_SOUND_TYPE.Max]; // BGM, SFX
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

    public void Update_VolumeData(VolumeData _volumeData)
    {
        volumeData = _volumeData;
        audioSources[(int)ENUM_SOUND_TYPE.BGM].volume = volumeData.wholeVolume * volumeData.bgmVolume;
        audioSources[(int)ENUM_SOUND_TYPE.SFX].volume = volumeData.wholeVolume * volumeData.sfxVolume;
    }

    public void Update_BGMVolumeData(float _volumeValue)
    {
        volumeData.bgmVolume = _volumeValue;
        audioSources[(int)ENUM_SOUND_TYPE.BGM].volume = volumeData.wholeVolume * volumeData.bgmVolume;
    }
    public void Update_SFXVolumeData(float _volumeValue)
    {
        volumeData.sfxVolume = _volumeValue;
        audioSources[(int)ENUM_SOUND_TYPE.SFX].volume = volumeData.wholeVolume * volumeData.sfxVolume;
    }
    public void Update_WholeVolumeData(float _volumeValue)
    {
        volumeData.wholeVolume = _volumeValue;
        audioSources[(int)ENUM_SOUND_TYPE.BGM].volume = volumeData.wholeVolume * volumeData.bgmVolume;
        audioSources[(int)ENUM_SOUND_TYPE.SFX].volume = volumeData.wholeVolume * volumeData.sfxVolume;
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
        float currSfxVolume = volumeData.wholeVolume * volumeData.sfxVolume;

        string path = $"Sounds/SFX/{sfxType}";
        AudioClip audioClip = GetOrAddAudioClip(path);

        if (audioClip == null || currSfxVolume == 0)
            return;

        audioSources[(int)ENUM_SOUND_TYPE.SFX].volume = currSfxVolume;
        audioSources[(int)ENUM_SOUND_TYPE.SFX].PlayOneShot(audioClip);
    }

    private void Update_BGMAudioSource(float _currVolume)
    {
        audioSources[(int)ENUM_SOUND_TYPE.BGM].volume = volumeData.wholeVolume * _currVolume;
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

public class SoundMgr_Reference
{
    List<VolumeData> volumeDataList = null;

    AudioSource[] audioSources = new AudioSource[(int)ENUM_SOUND_TYPE.Max];
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    Coroutine bgmCoroutine;

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(ENUM_SOUND_TYPE));
            for (int i = 0; i <= (int)ENUM_SOUND_TYPE.SFX; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }
        }

        audioSources[(int)ENUM_SOUND_TYPE.BGM].loop = true;
        
        
        audioSources[(int)ENUM_SOUND_TYPE.BGM].volume = 0f;

       //  volumeDataList = PlayerPrefsManagement.Load_VolumeData();
        if (volumeDataList == null)
        {
            volumeDataList = new List<VolumeData>();
            // for (int i = 0; i <= (int)ENUM_SOUND_TYPE.SFX; i++)
                // volumeDataList.Insert(i, new VolumeData((ENUM_SOUND_TYPE)i, 0.5f, 1f));
        }
    }

    public void Clear()
    {
        PauseBGM();

        audioClips.Clear();
    }

    public void Play(ENUM_BGM_TYPE bgmType, ENUM_SOUND_TYPE soundType = ENUM_SOUND_TYPE.BGM, float pitch = 0.0f)
    {
        // if (pitch == 0.0f) pitch = volumeDataList[0].pitch;

        string path = $"Sounds/BGM/{bgmType}";

        AudioClip audioClip = GetOrAddAudioClip(path);

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        AudioSource audioSource = audioSources[(int)ENUM_SOUND_TYPE.BGM];

        if (audioSource.isPlaying) audioSource.Stop();

        audioSource.pitch = pitch;
        audioSource.clip = audioClip;
        audioSource.Play();

        if (bgmCoroutine != null)
            CoroutineHelper.StopCoroutine(bgmCoroutine);

        // bgmCoroutine = CoroutineHelper.StartCoroutine(FadeInBGM());
    }

    public void Play(ENUM_SFX_TYPE sfxType, ENUM_SOUND_TYPE soundType = ENUM_SOUND_TYPE.SFX, float pitch = 0.0f)
    {
        // if (pitch == 0.0f) pitch = volumeDataList[1].pitch;

        string path = $"Sounds/SFX/{sfxType}";

        AudioClip audioClip = GetOrAddAudioClip(path);
        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        AudioSource audioSource = audioSources[(int)soundType];

        audioSource.pitch = pitch;
        // audioSource.volume = volumeDataList[1].volume;
        audioSource.PlayOneShot(audioClip);
    }

    public void PauseBGM()
    {
        if (bgmCoroutine != null)
            CoroutineHelper.StopCoroutine(bgmCoroutine);

        // bgmCoroutine = CoroutineHelper.StartCoroutine(FadeOutBGM());
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

    public void OnValueChanged_BGMVolume(float _volume) => audioSources[0].volume = _volume;
    public void OnValueChanged_SFXVolume(float _volume) => audioSources[1].volume = _volume;
    public void Save_SoundData()
    {
        // volumeDataList[0].volume = audioSources[0].volume;
        // volumeDataList[1].volume = audioSources[1].volume;

        // PlayerPrefsManagement.Save_VolumeData(volumeDataList);
    }

    public List<VolumeData> Get_VolumeDatas() => volumeDataList;
}