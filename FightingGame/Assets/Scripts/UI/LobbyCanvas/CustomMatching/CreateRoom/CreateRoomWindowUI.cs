using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using FGDefine;

public class CreateRoomWindowUI : UIElement
{
    [SerializeField] InputField userInputField;

    [SerializeField] Image inputFieldBorderImage;
    [SerializeField] Text errorText;

    [SerializeField] CustormRoom_MapInfo mapInfo;

    ENUM_MAP_TYPE currMap = ENUM_MAP_TYPE.CaveMap;

    Coroutine errorTextShakeEffectCoroutine;

    protected override void OnEnable()
    {
        if(!Managers.UI.popupCanvas.isFadeObjActiveState)
            Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Enter);

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        if (!Managers.UI.popupCanvas.isFadeObjActiveState)
            Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Cancel);

        base.OnDisable();

        if (errorTextShakeEffectCoroutine != null)
            StopCoroutine(errorTextShakeEffectCoroutine);
    }

    private void Open_CustomRoom()
    {
        Managers.UI.popupCanvas.Close_LoadingPopup();
        Managers.UI.currCanvas.GetComponent<LobbyCanvas>().Open_CustomRoomWindow();
    }

    private void Init()
    {
        userInputField.text = "";
        userInputField.characterLimit = Managers.Data.nameTextLimit;
        inputFieldBorderImage.color = Managers.Data.Get_DeselectColor();

        if (errorText.gameObject.activeSelf)
            errorText.gameObject.SetActive(false);

        userInputField.onValueChanged.RemoveAllListeners(); 
        userInputField.onValueChanged.AddListener(
            (word) => userInputField.text = Regex.Replace(word, @"[^0-9a-zA-Zㄱ-ㅎ가-힣\!\?\~\.\,ㆍᆢ]", "")
        );
    }

    public void Open()
    {
        Init();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        errorText.gameObject.SetActive(false);
    }
    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        Close();
    }

    public void OnClick_ChangeMap(int _mapTypeNum)
    {
        currMap = (ENUM_MAP_TYPE)_mapTypeNum;
        mapInfo.Set_CurrMapInfo(currMap);
    }

    public void OnClick_CreatRoom()
    {
        string roomNameStr = userInputField.text.Trim();

        if(roomNameStr == null || roomNameStr == "")
        {
            ErrorTextShakeEffect("방 이름을 입력해주세요.");
            return;
        }
        else if (roomNameStr.Length < 4)
        {
            ErrorTextShakeEffect("방 이름은 4글자 이상이여야 합니다.");
            return;
        }
        else if(Managers.Data.BadWord_Discriminator(roomNameStr))
        {
            ErrorTextShakeEffect("사용할 수 없는 방 이름입니다.");
            return;
        }

        Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Enter);
        Managers.UI.popupCanvas.Open_LoadingPopup();
        PhotonLogicHandler.Instance.TryCreateRoom(roomNameStr, CreateRoomSuccessCallBack
            , null, true, 2, currMap);
    }

    private void CreateRoomSuccessCallBack()
    {
        Managers.UI.popupCanvas.Play_FadeOutEffect(Open_CustomRoom);
    }

    private void ErrorTextShakeEffect(string _errorMessage = "")
    {
        Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Error);

        if (errorTextShakeEffectCoroutine != null)
            StopCoroutine(errorTextShakeEffectCoroutine);

        ColorUtility.TryParseHtmlString("#E74341", out Color color);
        inputFieldBorderImage.color = color;

        errorText.text = "* " + _errorMessage;

        if (errorText.gameObject.activeSelf == false)
            errorText.gameObject.SetActive(true);

        errorTextShakeEffectCoroutine = StartCoroutine(IErrorTextShakeEffect());
    }

    public void InputField_ValueChange()
    {
        if (errorTextShakeEffectCoroutine != null)
            StopCoroutine(errorTextShakeEffectCoroutine);

        inputFieldBorderImage.color = Managers.Data.Get_SelectColor();

        if (errorText.gameObject.activeSelf)
            errorText.gameObject.SetActive(false);
    }

    public void InputField_EndEdit()
    {
        if (errorTextShakeEffectCoroutine != null)
            StopCoroutine(errorTextShakeEffectCoroutine);

        inputFieldBorderImage.color = Managers.Data.Get_DeselectColor();
    }

    protected IEnumerator IErrorTextShakeEffect()
    {
        float shakeTime = 0.5f;
        float shakePower = 0.04f;
        Vector2 originVec = errorText.gameObject.transform.position;

        float realTime = 0f;

        while(realTime < shakeTime)
        {
            realTime += Time.deltaTime;
            
            errorText.gameObject.transform.position = originVec + (Vector2)UnityEngine.Random.insideUnitCircle * shakePower;
            errorText.gameObject.transform.position = new Vector2(errorText.gameObject.transform.position.x, originVec.y);

            yield return null;
        }

        errorText.gameObject.transform.position = originVec;
        errorTextShakeEffectCoroutine = null;
    }
}
