using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharProfileUI : MonoBehaviour
{
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

}
