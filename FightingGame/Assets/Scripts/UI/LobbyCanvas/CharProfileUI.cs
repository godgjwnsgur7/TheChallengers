using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class CharProfileUI : MonoBehaviour
{
    public bool isInit = false;

    [Header ("Set In Editor")]
    [SerializeField] Image charImage;
    [SerializeField] Image readyStateImage;

    [SerializeField] Text charNameText;
    [SerializeField] Text userNicknameText;

    [Header("Setting Resources With Editor")]
    [SerializeField] Sprite readySprite;
    [SerializeField] Sprite unreadySprite;


    public void Set()
    {

    }

    // 이런 느낌으로 캐릭터를 선택해서 이미지를 가져오는 아이들이
    // 생각보다 많아지고 있으므로, 스크립트를 분할해 따로 묶어놓고
    // 해당하는 스크립트에서 받아와서 세팅하는 것도 괜찮을지도 모름
    // 결론 : 고민중
    public void Select_Char(ENUM_CHARACTER_TYPE _charType)
    {
        switch(_charType)
        {
            case ENUM_CHARACTER_TYPE.Default:

                break;
            case ENUM_CHARACTER_TYPE.Wizard:

                break;

            case ENUM_CHARACTER_TYPE.Knight:

                break;
            default:
                Debug.LogError("알 수 없는 캐릭터입니다.");
                break;
        }
    }
    

}
