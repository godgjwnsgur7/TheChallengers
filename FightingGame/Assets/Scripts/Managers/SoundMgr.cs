using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public enum ENUM_SOUND_TYPE
{
    BGM,
    SFX,

    Max
}

public class SoundMgr
{
    List<VolumeData> volumeDataList = null;

    AudioSource[] audioSources = new AudioSource[(int)ENUM_SOUND_TYPE.Max];
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    Coroutine bgmCoroutine;

    // 임시로 여기에 셋팅
    private float bgmPitch = 0.7f;
    private float sfxPitch = 1.0f;
    private float fade_time = 0f;

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(ENUM_SOUND_TYPE));
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }
        }

        audioSources[(int)ENUM_SOUND_TYPE.BGM].loop = true;
        audioSources[(int)ENUM_SOUND_TYPE.BGM].volume = 0f;

        volumeDataList = PlayerPrefsManagement.Load_VolumeData();
        if (volumeDataList == null)
        {
            volumeDataList = new List<VolumeData>();
            for (int i = 0; i < (int)ENUM_SOUND_TYPE.Max; i++)
                volumeDataList.Insert(i, new VolumeData((ENUM_SOUND_TYPE)i, 0.5f));
        }
    }

    public void Clear()
    {
        /*foreach(AudioSource audios in audioSources)
        {
            audios.clip = null;
        }*/
        PauseBGM();

        audioClips.Clear();
    }

    public void Play(ENUM_BGM_TYPE bgmType, ENUM_SOUND_TYPE soundType = ENUM_SOUND_TYPE.BGM, float pitch = 0.0f)
    {
        if (pitch == 0.0f) pitch = bgmPitch;

        string path = $"Sounds/{soundType}/{bgmType}";

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
        if (pitch == 0.0f) pitch = sfxPitch;

        string path = $"Sounds/{soundType}/{sfxType}";

        AudioClip audioClip = GetOrAddAudioClip(path);
        if (audioClip == null) return;

        AudioSource audioSource = audioSources[(int)ENUM_SOUND_TYPE.SFX];

        audioSource.volume = 0f;
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(audioClip);
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
