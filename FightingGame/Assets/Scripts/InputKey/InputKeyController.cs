using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class InputKeyController : MonoBehaviour
{
    List<KeySettingData> keySettingDataList = null;

    private InputKeyManagement inputKeyManagement = null;
    private PlayerCharacter currPlayer;

    public InputPanel inputPanel = null;
    private RectTransform panelTr = null;
    private InputKey inputKey = null;
    private RectTransform inputKeyRectTr = null;

    public bool isPanelActive = false;

    public void Init()
    {
        if (inputPanel == null)
        {
            inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
            inputPanel.Init(PointerDown, PointerUp);
            panelTr = inputPanel.GetComponent<RectTransform>();
        }

        Set_keySettingDataList();
        Set_PanelActive(true);

        /*inputKeyManagement = gameObject.transform.parent.Find("InputKeyManagement").GetComponent<InputKeyManagement>();
        if (inputKeyManagement == null)
            return;

        if (inputKeyManagement.isPanelActive)
            inputKeyManagement.Set_PanelActive(false);*/
    }

    public void Connect_Player(PlayerCharacter _player) => currPlayer = _player;
    public void Disconnect_Player() => currPlayer = null;

    public void PointerDown(InputKey _inputkey)
    {
        if (currPlayer == null)
            return;

        // Attack
        if (_inputkey.name.Equals(Enum.GetName(typeof(ENUM_INPUTKEY_NAME), 2)))
        {
            int AttackObject1_Num = (int)currPlayer.activeCharacter.characterType * 10 + 1;
            CharacterAttackParam attackParam = new CharacterAttackParam((ENUM_ATTACKOBJECT_NAME)AttackObject1_Num, currPlayer.activeCharacter.reverseState);
            currPlayer.PlayerCommand(ENUM_PLAYER_STATE.Attack, attackParam);
            currPlayer.activeCharacter.Change_AttackState(true);
        }

        // Skill1
        if (_inputkey.name.Equals(Enum.GetName(typeof(ENUM_INPUTKEY_NAME), 3)))
        {
            CharacterSkillParam skillParam1 = new CharacterSkillParam(0);
            currPlayer.PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam1);
        }

        // Skill2
        if (_inputkey.name.Equals(Enum.GetName(typeof(ENUM_INPUTKEY_NAME), 4)))
        {
            CharacterSkillParam skillParam2 = new CharacterSkillParam(1);
            currPlayer.PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam2);

        }

        // Skill3
        if (_inputkey.name.Equals(Enum.GetName(typeof(ENUM_INPUTKEY_NAME), 5)))
        {
            CharacterSkillParam skillParam3 = new CharacterSkillParam(2);
            currPlayer.PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam3);
        }

        // Jump
        if (_inputkey.name.Equals(Enum.GetName(typeof(ENUM_INPUTKEY_NAME), 6)))
        {
            currPlayer.PlayerCommand(ENUM_PLAYER_STATE.Jump);
        }

        // LeftArrow
        if (_inputkey.name.Equals(Enum.GetName(typeof(ENUM_INPUTKEY_NAME), 0)))
            currPlayer.moveDir = -1.0f;

        // RightArrow
        if (_inputkey.name.Equals(Enum.GetName(typeof(ENUM_INPUTKEY_NAME), 1)))
            currPlayer.moveDir = 1.0f;

        if (currPlayer.moveDir != 0f)
        {
            currPlayer.activeCharacter.Input_MoveKey(true);
            currPlayer.PlayerCommand(ENUM_PLAYER_STATE.Move, new CharacterMoveParam(currPlayer.moveDir));
        }
    }

    public void PointerUp(InputKey _inputkey)
    {
        if(currPlayer.activeCharacter.anim.GetBool("AttackState"))
            currPlayer.activeCharacter.Change_AttackState(false);

        if (currPlayer.moveDir != 0f)
        {
            currPlayer.moveDir = 0f;
            currPlayer.activeCharacter.Input_MoveKey(false);

            if (currPlayer.activeCharacter.currState == ENUM_PLAYER_STATE.Move)
                currPlayer.PlayerCommand(ENUM_PLAYER_STATE.Idle);
        }
    }

    // PlayerPrefs 값 호출
    public void Set_keySettingDataList()
    {
        // 설정된 PlayerPrefs 호출
        keySettingDataList = PlayerPrefsManagement.Load_KeySettingData();

        // 설정된 PlayerPrefs가 없으면 초기화
        if (keySettingDataList == null)
        {
            keySettingDataList = new List<KeySettingData>();
            for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
            {
                inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)i);
                inputKeyRectTr = inputKey.GetComponent<RectTransform>();

                keySettingDataList.Insert(i, new KeySettingData(i, 50, 100, inputKeyRectTr.anchoredPosition.x, inputKeyRectTr.anchoredPosition.y));
            }
        }
        else
        {
            for (int i = 0; i < keySettingDataList.Count; i++)
            {
                inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)i);
                inputKeyRectTr = inputKey.GetComponent<RectTransform>();
                float ratio;

                // Set Size
                ratio = (50 + keySettingDataList[i].size) / 100;
                Vector3 changeVec = new Vector3(1, 1, 1) * ratio;
                inputKeyRectTr.localScale = changeVec;

                // Set Opacity
                Image inputKeyImage;
                ratio = 0.5f + (keySettingDataList[i].opacity / 200);
                Transform imageObjectTr = inputKey.transform.Find("SlotImage");
                if (imageObjectTr != null)
                {
                    inputKeyImage = imageObjectTr.GetComponent<Image>();
                    Set_ChangeColor(inputKeyImage, ratio);
                }

                imageObjectTr = inputKey.transform.Find("IconArea");
                if (imageObjectTr != null)
                {
                    inputKeyImage = imageObjectTr.GetChild(0).GetComponent<Image>();
                    Set_ChangeColor(inputKeyImage, ratio);
                }

                // Set Transform 
                changeVec = new Vector2(keySettingDataList[i].rectTrX, keySettingDataList[i].rectTrY);
                inputKeyRectTr.anchoredPosition = changeVec;
            }
        }
    }

    public void Set_ChangeColor(Image _inputKeyImage, float _opacityRatio)
    {
        Color changeColor = _inputKeyImage.color;
        changeColor.a = _opacityRatio;
        _inputKeyImage.color = changeColor;
    }

    public void Set_PanelActive(bool _binary)
    {
        this.isPanelActive = _binary;
        this.inputPanel.gameObject.SetActive(_binary);
    }
}