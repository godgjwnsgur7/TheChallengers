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
    #region soundPool
    class SoundPool
    {
        public Transform Root { get; set; }
        Stack<OneShotAudioObject> soundPoolStack = new Stack<OneShotAudioObject>();

        public void Init(Transform root, int poolCount)
        {
            Root = root;

            for (int i = 0; i < poolCount; i++)
                Push(Create());
        }

        OneShotAudioObject Create()
        {
            OneShotAudioObject obj = Managers.Resource.Instantiate($"PublicObjects/OneShotAudio").GetComponent<OneShotAudioObject>();
            if(obj.gameObject.activeSelf)
                obj.gameObject.SetActive(false);
            return obj;
        }

        public void Push(OneShotAudioObject oneShotAudioObject)
        {
            if (oneShotAudioObject == null) return;

            oneShotAudioObject.transform.parent = Root;
            oneShotAudioObject.gameObject.SetActive(false);
            oneShotAudioObject.isUsing = false;
            oneShotAudioObject.Init();

            soundPoolStack.Push(oneShotAudioObject);
        }

        public OneShotAudioObject Pop()
        {
            OneShotAudioObject oneShotAudioObject;

            if (soundPoolStack.Count == 0)
                oneShotAudioObject = Create();
            else
                oneShotAudioObject = soundPoolStack.Pop();

            oneShotAudioObject.gameObject.SetActive(true);
            oneShotAudioObject.isUsing = true;
            return oneShotAudioObject;
        }
    }
    #endregion

    SoundPool soundPool = new SoundPool();

    AudioSource[] audioSources = new AudioSource[(int)ENUM_SOUND_TYPE.MASTER]; // BGM, SFX
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>(); // 키 : 파일경로

    Coroutine fadeOutInBGMCoroutine;
    Coroutine bgmStopCoroutine;

    VolumeData volumeData = null;

    AudioListener audioListener = null;

    readonly float fadeSoundTime = 0.75f;

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

            soundPool.Init(root.transform, 10);
        }
    }
    
    public void Clear()
    {
        audioListener = null;
    }

    public void Push_SoundPool(OneShotAudioObject oneShotAudioObject)
    {
        soundPool.Push(oneShotAudioObject);
    }

    public VolumeData Get_CurrVolumeData()
    {
        volumeData = PlayerPrefsManagement.Load_VolumeData();

        return volumeData;
    }

    public Vector2 Get_AudioListenerWorldPosVec()
    {
        if(audioListener == null)
            audioListener = MonoBehaviour.FindObjectOfType<AudioListener>();

        if (audioListener == null)
            return Vector2.zero;

        return audioListener.gameObject.transform.position;
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
    public void Play_SFX(ENUM_SFX_TYPE sfxType,ENUM_TEAM_TYPE teamType, Vector3 worldPosVec)
    {
        if (Get_SFXSoundMuteState())
            return;

        AudioClipVolume audioClipVolume = Get_AudioClipVolume(sfxType, teamType);

        if (audioClipVolume == null)
            return;

        OneShotAudioObject oneShotAudioObject = soundPool.Pop();

        oneShotAudioObject.Play_SFX(audioClipVolume, worldPosVec);
    }

    /// <summary>
    /// 호출자의 위치를 따라가는 3D SFX 사운드 재생을 위한 함수 (오버로드)
    /// </summary>
    public void PlaySFX_FollowingSound(ENUM_SFX_TYPE sfxType, ENUM_TEAM_TYPE teamType, Vector3 worldPosVec, Transform target)
    {
        if (Get_SFXSoundMuteState())
            return;

        AudioClipVolume audioClipVolume = Get_AudioClipVolume(sfxType, teamType);

        if (audioClipVolume == null)
            return;

        OneShotAudioObject oneShotAudioObject = soundPool.Pop();

        oneShotAudioObject.PlaySFX_FollowingSound(audioClipVolume, target);
    }

    public AudioClipVolume Get_AudioClipVolume(ENUM_SFX_TYPE sfxType, ENUM_TEAM_TYPE teamType)
    {
        float _currSfxVolume = volumeData.masterVolume * volumeData.sfxVolume;

        if (Managers.Network.IsServerSyncState)
        {
            if (PhotonLogicHandler.IsMasterClient != (teamType == ENUM_TEAM_TYPE.Blue))
                _currSfxVolume *= 0.6f;
        }
        else if (teamType == ENUM_TEAM_TYPE.Red)
        {
            _currSfxVolume *= 0.6f;
        }
            

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

        float time = 0f;
        float currVolume = audioSources[(int)ENUM_SOUND_TYPE.BGM].volume;

        if (audioSources[(int)ENUM_SOUND_TYPE.BGM].isPlaying) // 실행 중일 경우
        {
            float volume = currVolume;
            // FadeOut
            while (volume > 0f)
            {
                time += Time.deltaTime / fadeSoundTime;
                volume = Mathf.Lerp(currVolume, 0, time);
                Update_BGMAudioSource(volume);

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

        float time = 0f;
        float _currBgmVolume = volumeData.masterVolume * volumeData.bgmVolume;
        float currVolume = audioSources[(int)ENUM_SOUND_TYPE.BGM].volume;

        string path = $"Sounds/BGM/{bgmType}";

        AudioClip audioClip = GetOrAddAudioClip(path);

        if (audioSources[(int)ENUM_SOUND_TYPE.BGM].isPlaying) // 실행 중일 경우
        {
            float volume = currVolume;
            // FadeOut
            while (volume > 0)
            {
                time += Time.deltaTime / fadeSoundTime;
                volume = Mathf.Lerp(currVolume, 0, time);
                Update_BGMAudioSource(volume);

                yield return null;
            }
        }

        time = 0f;
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
                time += Time.deltaTime / fadeSoundTime;
                currVolume = Mathf.Lerp(0, _currBgmVolume, time);
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