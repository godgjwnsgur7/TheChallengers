using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class TrainingCanvas : BaseCanvas
{
    [Header("Set In Editor")]
    [SerializeField] SelectWindow selectWindow;
    [SerializeField] ButtonPanel buttonPanel;
    [SerializeField] SettingPanel settingPanel;
    [SerializeField] KeyPanelAreaEdit keyPanelAreaEdit;
    [SerializeField] KeyPanelArea keyPanelArea;
    [SerializeField] BottomPanel bottomPanel;
    [SerializeField] NotionPopup notionPopup;
    [SerializeField] Text notion;
    [SerializeField] Button sliderBtn;

    Coroutine runCorutine;

    public string ChangeCharacter;
    public bool isCallPlayer = false;

    public override void Init()
    {
        base.Init();
        buttonPanel.Init();
    }

    public override void Open<T>(UIParam param = null)
    {
        if (typeof(T) == typeof(SelectWindow)) selectWindow.Open();
        else if (typeof(T) == typeof(SettingPanel)) settingPanel.Open();
        else if (typeof(T) == typeof(KeyPanelArea)) keyPanelArea.Open();
        else if (typeof(T) == typeof(KeyPanelAreaEdit)) keyPanelAreaEdit.Open();
        else if (typeof(T) == typeof(NotionPopup)) notionPopup.Open();
        else if (typeof(T) == typeof(BottomPanel)) bottomPanel.Open();
        else Debug.Log("범위 벗어남");
    }

    public override void Close<T>()
    {
        if (typeof(T) == typeof(SelectWindow)) selectWindow.Close();
        else if (typeof(T) == typeof(SettingPanel)) settingPanel.Close();
        else if (typeof(T) == typeof(KeyPanelArea)) keyPanelArea.Close();
        else if (typeof(T) == typeof(KeyPanelAreaEdit)) keyPanelAreaEdit.Close();
        else if (typeof(T) == typeof(NotionPopup)) notionPopup.Close();
        else if (typeof(T) == typeof(BottomPanel)) bottomPanel.Close();
        else Debug.Log("범위 벗어남");
    }
    public override T GetUIComponent<T>()
    {

        return default(T);
    }

    // 캐릭터 UI 세팅 패널 open,close
    public void OnOffSettingPanel()
    {
        if (keyPanelAreaEdit.isOpen == false) 
        {
            //keyPanelAreaEdit.playerType = FGDefine.ENUM_CHARACTER_TYPE.Knight;
            Managers.UI.OpenUI<KeyPanelAreaEdit>();
        }

        if (settingPanel.isOpen)
        {
            Managers.UI.CloseUI<SettingPanel>();
            Managers.UI.CloseUI<BottomPanel>();
            if (!isCallPlayer)
                Managers.UI.CloseUI<KeyPanelAreaEdit>();
        }
        else
        {
            Managers.UI.OpenUI<SettingPanel>();
        }

        buttonPanel.SlidePanel();
        buttonPanel.InteractableBtn();
    }

    // 캐릭터 선택
    public void SelectCharacter(int charType)
    {
        if (notion.gameObject.activeSelf)
            notion.gameObject.SetActive(false);

        switch (ChangeCharacter)
        {
            case "Player":

                break;

            case "Enemy":

                break;
        }

        CloseSelectWindow();
    }

    // 캐릭터 변경창 open, close
    public void OpenSelectWindow(string characterType)
    {
        ChangeCharacter = characterType;
        Managers.UI.OpenUI<SelectWindow>();
    }

    public void CloseSelectWindow()
    {
        Managers.UI.CloseUI<SelectWindow>();
    }

    // 알림 설정
    public void SetNotionText(string text)
    {
        if (runCorutine != null)
            StopCoroutine(runCorutine);

        runCorutine = StartCoroutine(ShowNotion(text));
    }

    IEnumerator ShowNotion(string text)
    {
        notion.gameObject.SetActive(true);
        notion.text = text;

        yield return new WaitForSeconds(1.5f);

        notion.gameObject.SetActive(false);
        runCorutine = null;
    }

    // 로비로 귀환
    public void LoadLobby() 
    {
        Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.Lobby);
    }
}
