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
    AudioSource[] audioSources = new AudioSource[(int)ENUM_SOUND_TYPE.Max];
    SoundObserver sceneObserver;
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    List<VolumeData> volumeDataList = null;

    // 임시로 여기에 셋팅
    private float bgmPitch = 0.7f;
    private float sfxPitch = 1.0f;

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            sceneObserver = root.AddComponent<SoundObserver>();
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

        volumeDataList = PlayerPrefsManagement.Load_VolumeData();
        if(volumeDataList == null)
        {
            volumeDataList = new List<VolumeData>();
            for (int i = 0; i < (int)ENUM_SOUND_TYPE.Max; i++)
                volumeDataList.Insert(i, new VolumeData((ENUM_SOUND_TYPE)i, 0.5f));
        }
    }

    public void Clear()
    {
        audioSources[1].clip = null;
        audioSources[1].Stop();

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

        audioSource.volume = 0f;
        audioSource.pitch = pitch;
        audioSource.clip = audioClip;
        audioSource.Play();
        CoroutineHelper.StartCoroutine(FadeInBGM());
    }

    public void Check_Play(ENUM_BGM_TYPE bgmType, ENUM_SOUND_TYPE soundType = ENUM_SOUND_TYPE.BGM, float pitch = 0.0f)
    {
        if (audioSources[0].isPlaying)
            CoroutineHelper.StartCoroutine(FadeOutBGM(bgmType));
        else
            Play(bgmType, soundType, pitch);
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
        CoroutineHelper.StartCoroutine(FadeInSFX());
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

    public void Change_ObserverScene(BaseScene _sceneType) => sceneObserver.Change_Scene(_sceneType);
    public void OnValueChanged_BGMVolume(float _volume) => audioSources[0].volume = _volume;
    public void OnValueChanged_SFXVolume(float _volume) => audioSources[1].volume = _volume;
    public void Save_SoundData() => PlayerPrefsManagement.Save_VolumeData(volumeDataList);

    IEnumerator FadeInBGM()
    {
        float f_time = 0f;
        float currVolume = audioSources[0].volume;
        while (audioSources[0].volume < volumeDataList[0].volume)
        {
            f_time += Time.deltaTime;
            OnValueChanged_BGMVolume(Mathf.Lerp(currVolume, volumeDataList[0].volume, f_time));
            yield return null;
        }
        audioSources[0].volume = volumeDataList[0].volume;
    }
    
    IEnumerator FadeOutBGM(ENUM_BGM_TYPE _bgmType)
    {
        float f_time = 0f;
        float currVolume = audioSources[0].volume;
        while (audioSources[0].volume > 0)
        {
            f_time += Time.deltaTime;
            OnValueChanged_BGMVolume(Mathf.Lerp(currVolume, 0, f_time));
            yield return null;
        }
        audioSources[0].Stop();
        Play(_bgmType);
    }

    IEnumerator FadeInSFX()
    {
        float f_time = 0f;
        float currVolume = audioSources[1].volume;
        while (audioSources[1].volume < 0.9f)
        {
            f_time += Time.deltaTime;
            OnValueChanged_SFXVolume(Mathf.Lerp(currVolume, 1, f_time));
            yield return null;
        }
        audioSources[1].volume = 1f;
    }

    /* BGM 페이드 인 아웃 관련 레퍼런스 로직 (임시)
    IEnumerator FadeOutInBGM()
    {
        float f_time = 0f;
        BGM.volume = 1f;
        while (BGM.volume > 0.3f)
        {
            f_time += UnityEngine.Time.deltaTime;
            BGM.volume = Mathf.Lerp(1, 0, f_time);
            yield return null;
        }
        if (nowIndex != selectedIndex)
        {
            selectedIndex = nowIndex;
            BGM.Pause();
            Set_BGM(nowIndex);
        }
        StartCoroutine(FadeIn());
    }
    
    foreach (AudioSource audioSources in audioSources)
    { 
        audioSources.clip = null;
        audioSources.Stop();
    }
     */
}
