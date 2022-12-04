using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public enum ENUM_SOUND_TYPE
{
    BGM,
    SFX,
    SFX_Player,
    SFX_Enemy,

    Max
}

public class SoundMgr
{
    List<VolumeData> volumeDataList = null;

    AudioSource[] audioSources = new AudioSource[(int)ENUM_SOUND_TYPE.Max];
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    Coroutine bgmCoroutine;

    private float fade_time = 0f;

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

        Set_BGMValue();

        volumeDataList = PlayerPrefsManagement.Load_VolumeData();
        if (volumeDataList == null)
        {
            volumeDataList = new List<VolumeData>();
            for (int i = 0; i <= (int)ENUM_SOUND_TYPE.SFX; i++)
                volumeDataList.Insert(i, new VolumeData((ENUM_SOUND_TYPE)i, 0.5f, 0.7f));
        }
    }

    private void Set_BGMValue()
    {
        audioSources[(int)ENUM_SOUND_TYPE.BGM].loop = true;
        audioSources[(int)ENUM_SOUND_TYPE.BGM].volume = 0f;
    }

    public void Clear()
    {
        PauseBGM();

        audioSources[(int)ENUM_SOUND_TYPE.SFX_Player] = null;
        audioSources[(int)ENUM_SOUND_TYPE.SFX_Enemy] = null;
        audioClips.Clear();
    }

    public void Play(ENUM_BGM_TYPE bgmType, ENUM_SOUND_TYPE soundType = ENUM_SOUND_TYPE.BGM, float pitch = 0.0f)
    {
        if (pitch == 0.0f) pitch = volumeDataList[0].pitch;

        string path = $"Sounds/{ENUM_SOUND_TYPE.BGM.ToString()}/{bgmType}";

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

        bgmCoroutine = CoroutineHelper.StartCoroutine(FadeInBGM());
    }

    public void Play(ENUM_SFX_TYPE sfxType, ENUM_SOUND_TYPE soundType = ENUM_SOUND_TYPE.SFX, float pitch = 0.0f)
    {
        if (pitch == 0.0f) pitch = volumeDataList[1].pitch;

        string path = $"Sounds/{ENUM_SOUND_TYPE.SFX.ToString()}/{sfxType}";

        AudioClip audioClip = GetOrAddAudioClip(path);
        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        AudioSource audioSource = audioSources[(int)soundType];

        audioSource.pitch = pitch;
        audioSource.volume = volumeDataList[1].volume;
        audioSource.PlayOneShot(audioClip);
    }

    public void Set_AudioSource(AudioSource audioSource, ENUM_SOUND_TYPE soundType)
    {
        if (soundType < ENUM_SOUND_TYPE.SFX_Player)
            return;

        audioSources[(int)soundType] = audioSource;
        audioSources[(int)soundType].volume = volumeDataList[1].volume;
        audioSources[(int)soundType].pitch = volumeDataList[1].pitch;
        audioSources[(int)soundType].minDistance = 2f;
        audioSources[(int)soundType].maxDistance = Managers.Battle.Get_playerCamSizeDict(ENUM_MAP_TYPE.BasicMap) * Screen.width / Screen.height; ;
        audioSources[(int)soundType].spatialBlend = 1f;
        audioSources[(int)soundType].rolloffMode = AudioRolloffMode.Linear;
    }

    public void PauseBGM()
    {
        if (bgmCoroutine != null)
            CoroutineHelper.StopCoroutine(bgmCoroutine);

        bgmCoroutine = CoroutineHelper.StartCoroutine(FadeOutBGM());
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
        volumeDataList[0].volume = audioSources[0].volume;
        volumeDataList[1].volume = audioSources[1].volume;

        PlayerPrefsManagement.Save_VolumeData(volumeDataList);
    }

    IEnumerator FadeInBGM()
    {
        float currVolume = audioSources[0].volume;
        while (audioSources[0].volume < volumeDataList[0].volume)
        {
            fade_time += Time.deltaTime;
            OnValueChanged_BGMVolume(Mathf.Lerp(currVolume, volumeDataList[0].volume, fade_time));
            yield return null;
        }
        audioSources[0].volume = volumeDataList[0].volume;
        fade_time = 0f;
    }

    IEnumerator FadeOutBGM()
    {
        float currVolume = audioSources[0].volume;
        while (audioSources[0].volume > 0)
        {
            fade_time += Time.deltaTime;
            OnValueChanged_BGMVolume(Mathf.Lerp(currVolume, 0, fade_time));
            yield return null;
        }
        audioSources[0].Pause();
        fade_time = 0f;
    }

    public List<VolumeData> Get_VolumeDatas() => volumeDataList;
}

/*
 public void Set_SFXCharacterAudio(ENUM_SOUND_TYPE soundType, Transform parentTr)
    {
        if (soundType < ENUM_SOUND_TYPE.SFX_Player)
        {
            Debug.Log("캐릭터 오디오 생성 범위 벗어남");
            return;
        }

        GameObject go = new GameObject { name = soundType.ToString() };
        audioSources[(int)soundType] = go.AddComponent<AudioSource>();
        go.transform.parent = parentTr;

        audioSources[(int)soundType].volume = volumeDataList[1].volume;
        audioSources[(int)soundType].pitch = volumeDataList[1].pitch;
        audioSources[(int)soundType].minDistance = 2f;
        audioSources[(int)soundType].maxDistance = Managers.Battle.Get_playerCamSizeDict(ENUM_MAP_TYPE.BasicMap) * Screen.width / Screen.height; ;
        audioSources[(int)soundType].spatialBlend = 1f;
        audioSources[(int)soundType].rolloffMode = AudioRolloffMode.Linear;
    }
 */