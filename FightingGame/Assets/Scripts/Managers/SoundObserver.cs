using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class SoundObserver : MonoBehaviour
{
    private bool isSceneChange = false;
    public BaseScene currScene = null;

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

        isSceneChange = false;
    }

    public void Play_BattleSceneBGM(ENUM_BGM_TYPE _bgmType)
        => Managers.Sound.Check_Play(_bgmType, ENUM_SOUND_TYPE.BGM);
    public void Play_LobbySceneBGM(ENUM_BGM_TYPE _bgmType)
        => Managers.Sound.Check_Play(_bgmType, ENUM_SOUND_TYPE.BGM);
    public void Play_MainSceneBGM(ENUM_BGM_TYPE _bgmType)
        => Managers.Sound.Check_Play(_bgmType, ENUM_SOUND_TYPE.BGM);
    public void Play_TrainingSceneBGM(ENUM_BGM_TYPE _bgmType)
        => Managers.Sound.Check_Play(_bgmType, ENUM_SOUND_TYPE.BGM);

    public void Change_Scene(BaseScene _basescene)
    {
        currScene = _basescene;
        isSceneChange = true;
    }
}
