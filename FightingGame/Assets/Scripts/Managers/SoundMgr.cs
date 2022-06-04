using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class SoundMgr
{
    AudioSource[] audioSources = new AudioSource[(int)ENUM_SOUND_TYPE.Max];

    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

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
        foreach(AudioSource audioSources in audioSources)
        {
            audioSources.clip = null;
            audioSources.Stop();
        }
        audioClips.Clear();
    }

    public void Play(ENUM_BGM_TYPE bgmType, ENUM_SOUND_TYPE soundType = ENUM_SOUND_TYPE.BGM, float pitch = 1.0f)
    {
        string path = $"Sounds/{soundType}/{bgmType}";

        AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);

        if (audioClip == null)
        {
            Debug.Log($"AudioClip Missing ! {path}");
            return;
        }

        AudioSource audioSource = audioSources[(int)ENUM_SOUND_TYPE.BGM];

        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.pitch = pitch;
        audioSource.clip = audioClip;
        audioSource.Play();
        
    }

    public void Play(ENUM_SFX_TYPE sfxType, ENUM_SOUND_TYPE soundType = ENUM_SOUND_TYPE.SFX, float pitch = 1.0f)
    {
        string path = $"Sounds/{soundType}/{sfxType}";

        AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);

        if (audioClip == null)
        {
            Debug.Log($"AudioClip Missing ! {path}");
            return;
        }

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
        return audioClip;
    }
}
