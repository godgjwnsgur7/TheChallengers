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

    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    // 임시로 여기에 셋팅
    private float bgmPitch = 0.7f;
    private float sfxPitch = 1.0f;

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
    }

    public void Clear()
    {
        // BGM은 페이드 인아웃을 넣어야 함.
        foreach (AudioSource audioSources in audioSources)
        {
            audioSources.clip = null;
            audioSources.Stop();
        }
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
    }


    public void Play(ENUM_SFX_TYPE sfxType, ENUM_SOUND_TYPE soundType = ENUM_SOUND_TYPE.SFX, float pitch = 0.0f)
    {
        if (pitch == 0.0f) pitch = sfxPitch;

        string path = $"Sounds/{soundType}/{sfxType}";

        AudioClip audioClip = GetOrAddAudioClip(path);
        if (audioClip == null) return;

        AudioSource audioSource = audioSources[(int)ENUM_SOUND_TYPE.SFX];
        audioSource.pitch = pitch;

        audioSource.PlayOneShot(audioClip);
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

    /* BGM 페이드 인 아웃 관련 레퍼런스 로직 (임시)

    IEnumerator FadeInBGM()
    {
        float f_time = 0f;
        float currVolume = BGM.volume;
        while (BGM.volume < 0.9f)
        {
            f_time += UnityEngine.Time.deltaTime;
            BGM.volume = Mathf.Lerp(currVolume, 1, f_time);
            yield return null;
        }
        BGM.volume = 1f;
    }

    IEnumerator FadeOutBGM()
    {
        float f_time = 0f;
        float currVolume = BGM.volume;
        BGM.volume = 1f;
        while (BGM.volume > 0)
        {
            f_time += UnityEngine.Time.deltaTime;
            BGM.volume = Mathf.Lerp(currVolume, 0, f_time);
            yield return null;
        }
        BGM.Pause();
    }

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

    */
}
