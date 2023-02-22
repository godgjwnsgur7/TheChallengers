using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class GameStartWindowUI : MonoBehaviour
{
    [SerializeField] Text countText;
    [SerializeField] Image Image_VS; // VS 이미지

    ENUM_CHARACTER_TYPE selectedCharacterType = ENUM_CHARACTER_TYPE.Default;
    

    public void Open()
    {
        this.gameObject.SetActive(true);

    }
}
