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
    bool isLock = false;

    protected override void OnDisable()
    {
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
        isLock = false;

        userInputField.text = "";
        userInputField.characterLimit = Managers.Data.nameTextLimit;
        inputFieldBorderImage.color = Managers.Data.Get_DeselectColor();

        if (errorText.gameObject.activeSelf)
            errorText.gameObject.SetActive(false);

        userInputField.onValueChanged.RemoveAllListeners(); 
        userInputField.onValueChanged.AddListener(
            (word) => userInputField.text = Regex.Replace(word, @"[^0-9a-zA-Zㄱ-ㅎ가-힣\!\?\~\.\,)]", "")
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
        if(userInputField.text.Trim() == "")
        {
            ErrorTextShakeEffect("방이름을 입력해주세요.");
            return;
        }
        if (userInputField.text.Length < 4)
        {
            ErrorTextShakeEffect("방이름은 4글자 이상이여야 합니다.");
            return;
        }
        else if(Managers.Data.BadWord_Discriminator(userInputField.text))
        {
            ErrorTextShakeEffect("사용할 수 없는 방이름입니다.");
            return;
        }

        isLock = true;
        Managers.UI.popupCanvas.Open_LoadingPopup();
        PhotonLogicHandler.Instance.TryCreateRoom(userInputField.text, CreateRoomSuccessCallBack
            , null, true, 2, currMap);
    }

    private void CreateRoomSuccessCallBack()
    {
        Managers.UI.popupCanvas.Play_FadeOutEffect(Open_CustomRoom);
    }

    private void ErrorTextShakeEffect(string _errorMessage = "")
    {
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
        float shakePower = 0.05f;
        Vector2 originVec = errorText.gameObject.transform.position;

        float realTime = 0f;

        while(realTime < shakeTime)
        {
            realTime += Time.deltaTime;
            
            errorText.gameObject.transform.position = originVec + (Vector2)Random.insideUnitCircle * shakePower;

            yield return null;
        }

        errorText.gameObject.transform.position = originVec;
        errorTextShakeEffectCoroutine = null;
    }
}
