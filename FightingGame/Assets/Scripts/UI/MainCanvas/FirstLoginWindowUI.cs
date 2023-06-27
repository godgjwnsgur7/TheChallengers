using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class FirstLoginWindowUI : UIElement
{
    [SerializeField] InputField userNicknameInputField;

    [SerializeField] Image inputFieldBorderImage;
    [SerializeField] Text errorText;

    Action<string> nickNameCallBack = null;

    Coroutine errorTextShakeEffectCoroutine;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private void Init()
    {
        userNicknameInputField.text = "";
        userNicknameInputField.characterLimit = Managers.Data.nameTextLimit;
        inputFieldBorderImage.color = Managers.Data.Get_DeselectColor();

        if (errorText.gameObject.activeSelf)
            errorText.gameObject.SetActive(false);

        userNicknameInputField.onValueChanged.RemoveAllListeners();
        userNicknameInputField.onValueChanged.AddListener(
            (word) => userNicknameInputField.text = Regex.Replace(word, @"[^0-9a-zA-Zㄱ-ㅎ가-힣]", "")
        );
    }

    public void Open(Action<string> _nickNameCallBack)
    {
        Init();
        nickNameCallBack = _nickNameCallBack;
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        nickNameCallBack = null;
        this.gameObject.SetActive(false);
    }

    public void OnClick_Check()
    {
        if (userNicknameInputField.text.Trim() == "")
        {
            ErrorTextShakeEffect("닉네임을 입력해주세요.");
            return;
        }
        else if (userNicknameInputField.text.Length < 2)
        {
            ErrorTextShakeEffect("닉네임은 2글자 이상이여야 합니다.");
            return;
        }
        else if (Managers.Data.BadWord_Discriminator(userNicknameInputField.text))
        {
            ErrorTextShakeEffect("사용할 수 없는 닉네임입니다.");
            return;
        }

        // 중복 닉네임 체크해야 함
         
        Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Enter);

        nickNameCallBack(userNicknameInputField.text);

        Close();
    }

    public override void OnClick_Exit()
    {
        Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Error);

        base.OnClick_Exit();

        Close();
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

    protected IEnumerator IErrorTextShakeEffect()
    {
        float shakeTime = 0.5f;
        float shakePower = 0.04f;
        Vector2 originVec = errorText.gameObject.transform.position;

        float realTime = 0f;

        while (realTime < shakeTime)
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
