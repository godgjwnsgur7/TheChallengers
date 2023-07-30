using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public class CharSelectPopup : PopupUI
{
    [SerializeField] Image[] characterImages;
    [SerializeField] RectTransform selectionEffectRectTr;
    [SerializeField] Text titleText;
    [SerializeField] Text charDescriptionText;

    Action<ENUM_CHARACTER_TYPE> onSelectionCharacter;

    ENUM_CHARACTER_TYPE selectedCharType = ENUM_CHARACTER_TYPE.Default;

    protected override void OnEnable()
    {
        isUsing = true;
        Managers.UI.Push_WindowExitStack(OnClick_Exit);
    }

    protected override void OnDisable()
    {
        isUsing = false;
        Managers.UI.Pop_WindowExitStack();

    }
    public void Open(Action<ENUM_CHARACTER_TYPE> _onSelectionCharacter, bool isMine)
    {
        if (_onSelectionCharacter == null)
        {
            Debug.LogError("SelectionCharacterCallBack is Null!");
            return;
        }

        if (isMine) titleText.text = "내 캐릭터 소환";
        else titleText.text = "상대 캐릭터 소환";

        onSelectionCharacter = _onSelectionCharacter;
        selectedCharType = ENUM_CHARACTER_TYPE.Default;
        charDescriptionText.text = "캐릭터를 선택해주세요.";

        if (selectionEffectRectTr.gameObject.activeSelf)
            selectionEffectRectTr.gameObject.SetActive(false);

        gameObject.SetActive(true);
    }

    public void Close()
    {
        onSelectionCharacter = null;
        selectedCharType = ENUM_CHARACTER_TYPE.Default;
        gameObject.SetActive(false);
    }

    public void OnClick_CharacterSelectImage(int _charTypeNum)
    {
        if (selectedCharType == (ENUM_CHARACTER_TYPE)_charTypeNum)
            return;

        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Cilck_Heavy2);
        selectedCharType = (ENUM_CHARACTER_TYPE)_charTypeNum;

        RectTransform rectTr = characterImages[_charTypeNum - 1].GetComponent<RectTransform>();
        selectionEffectRectTr.position = rectTr.position;

        charDescriptionText.text = Managers.Data.Get_CharExplanationDict(selectedCharType);

        if (!selectionEffectRectTr.gameObject.activeSelf)
            selectionEffectRectTr.gameObject.SetActive(true);
    }

    public void OnClick_SelectCompletion()
    {
        if (selectedCharType == ENUM_CHARACTER_TYPE.Default || selectedCharType == ENUM_CHARACTER_TYPE.Max)
        {
            Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Error);
            return;
        }

        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Enter);
        onSelectionCharacter?.Invoke(selectedCharType);
        Close();
    }

    public override void OnClick_Exit()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Cancel);

        base.OnClick_Exit();

        Close();
    }
}
