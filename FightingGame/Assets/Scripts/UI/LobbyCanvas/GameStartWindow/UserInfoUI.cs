using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class UserInfoUI : MonoBehaviour
{
    [SerializeField] Image charImage;
    [SerializeField] Image ratingEmblemImage;

    [SerializeField] Text userNicknameText;
    [SerializeField] Text ratingPointText;
    [SerializeField] Text battleRecordText;

    Coroutine CharImageChangeCoroutine = null;

    ENUM_CHARACTER_TYPE currCharType = ENUM_CHARACTER_TYPE.Default;

    public void Init(DBUserData _userData)
    {
        if(_userData.victoryPoint == 0 && _userData.defeatPoint == 0)
        {
            // ratingEmblemImage를 Unknown으로 셋팅해야 함 (임시)
            ratingPointText.text = "Unknown";
        }
        else
        {
            char rank = RankingScoreOperator.Get_RankingEmblemChar(_userData.ratingPoint);
            ratingEmblemImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/RankEmblem/RankEmblem_{rank}");
            
            long winningRate = _userData.victoryPoint / (_userData.victoryPoint + _userData.defeatPoint) * 100;
            battleRecordText.text = $"{_userData.victoryPoint}승 {_userData.defeatPoint}패 ({winningRate}%)";
            ratingPointText.text = $"{_userData.ratingPoint}점";
        }
        
        userNicknameText.text = _userData.nickname;
    }

    public void Update_SelectCharacter(ENUM_CHARACTER_TYPE _selectCharType)
    {
        
    }


    protected IEnumerator ICharImageChange(ENUM_CHARACTER_TYPE _selectCharType)
    {
        if(currCharType != ENUM_CHARACTER_TYPE.Default)
        {
            // 기존 캐릭터를 지우는 작업이 필요할 거고요~

        }

        yield return null;


        CharImageChangeCoroutine = null;
    }
}
