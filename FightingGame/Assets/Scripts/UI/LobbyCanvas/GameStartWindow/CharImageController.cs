using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

/// <summary>
/// 이 친구는 일단 보류.
/// </summary>
public class CharImageController : MonoBehaviour
{
    private ENUM_CHARACTER_TYPE currCharType = ENUM_CHARACTER_TYPE.Default;

    Image CharacterImage = null;

    Coroutine charImageChangeCoroutine = null;

    public void Change_CharacterImage()
    {
        if(currCharType == ENUM_CHARACTER_TYPE.Default)
        {

        }

    }

    protected IEnumerator ICharImageChange(ENUM_CHARACTER_TYPE _charType)
    {
        if (currCharType == _charType)
        {
            charImageChangeCoroutine = null;
            yield break;
        }

        if(CharacterImage != null)
        {
            // 기존에 가지고 있는 요소를 지워지는 로직 수행
        }

        // 이새끼는 정해야 해.
    }
}

