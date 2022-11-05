using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class SoundObserver : MonoBehaviour
{
    private bool isSceneChange = false;

    public BaseScene currScene = null;
    Coroutine runningCoroutine;
    AudioSource bgmSource;

    public void Init()
    {
        bgmSource = this.transform.Find("BGM").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (currScene == null)
            return;

        if (isSceneChange)
        {
            ENUM_SCENE_TYPE currSound = currScene.SceneType;
            PlayBGM(currSound);
        }
    }

    public void PlayBGM(ENUM_SCENE_TYPE currSound)
    {
        if (currSound == ENUM_SCENE_TYPE.Unknown)
            return;

        if (currSound == ENUM_SCENE_TYPE.Battle)
            Play_BattleSceneBGM(ENUM_BGM_TYPE.TestBGM);
        else if (currSound == ENUM_SCENE_TYPE.Lobby)
            Play_LobbySceneBGM(ENUM_BGM_TYPE.TestBGM);
        else if (currSound == ENUM_SCENE_TYPE.Main)
            Play_MainSceneBGM(ENUM_BGM_TYPE.TestBGM);
        else if (currSound == ENUM_SCENE_TYPE.Training)
            Play_TrainingSceneBGM(ENUM_BGM_TYPE.TestBGM);
        else
        {
            Debug.Log("Error : 잘못된 Scene 범위");
            return;
        }

        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);

        runningCoroutine = StartCoroutine(FadeInBGM());
        isSceneChange = false;
    }

    public void Play_BattleSceneBGM(ENUM_BGM_TYPE _bgmType)
        => Managers.Sound.Play(_bgmType, ENUM_SOUND_TYPE.BGM);
    public void Play_LobbySceneBGM(ENUM_BGM_TYPE _bgmType)
        => Managers.Sound.Play(_bgmType, ENUM_SOUND_TYPE.BGM);
    public void Play_MainSceneBGM(ENUM_BGM_TYPE _bgmType)
        => Managers.Sound.Play(_bgmType, ENUM_SOUND_TYPE.BGM);
    public void Play_TrainingSceneBGM(ENUM_BGM_TYPE _bgmType)
        => Managers.Sound.Play(_bgmType, ENUM_SOUND_TYPE.BGM);

    public void Pause_SceneBGM()
    {
        if (!bgmSource.isPlaying)
            return;

        runningCoroutine = StartCoroutine(FadeOutBGM());
    }

    public void Change_Scene(BaseScene _basescene)
    {
        currScene = _basescene;
        isSceneChange = true;
    }

    IEnumerator FadeInBGM()
    {
        float f_time = 0f;
        float currVolume = bgmSource.volume;
        while (bgmSource.volume < 0.9f)
        {
            f_time += Time.deltaTime / 3;
            bgmSource.volume = Mathf.Lerp(currVolume, 1, f_time);
            yield return null;
        }
        bgmSource.volume = 1f;
    }

    IEnumerator FadeOutBGM()
    {
        float f_time = 0f;
        float currVolume = bgmSource.volume;
        bgmSource.volume = 1f;
        while (bgmSource.volume > 0)
        {
            f_time += UnityEngine.Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(currVolume, 0, f_time);
            yield return null;
        }
        bgmSource.Pause();
    }
}
