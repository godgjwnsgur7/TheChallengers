using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


enum ENUM_Notion_TYPE
{
    Default = 0,
    Close = 1,
    Reset = 2,
    Save = 3,
}

public class NotionPopup : UIElement
{
    [SerializeField] Text NotionText;
    [SerializeField] TrainingCanvas trainingCanvas;
    [SerializeField] KeyPanelArea keyPanelArea;
    [SerializeField] BottomPanel bottomPanel;

    private ENUM_Notion_TYPE CurrentPopupNum;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        CurrentPopupNum = ENUM_Notion_TYPE.Default;
        base.Close();
    }

    public void SetNotion(int BtnType)
    {
        switch (BtnType) 
        {
            case 1:
                NotionText.text = "버튼 설정을 종료하겠습니까?";
                CurrentPopupNum = (ENUM_Notion_TYPE)BtnType;
                break;
            case 2:
                NotionText.text = "값을 초기화 하시겠습니까?";
                CurrentPopupNum = (ENUM_Notion_TYPE)BtnType;
                break;
            case 3:
                NotionText.text = "값을 저장하시겠습니까?";
                CurrentPopupNum = (ENUM_Notion_TYPE)BtnType;
                break;
            default:
                Debug.Log("범위 벗어남");
                break;
        }
    }

    public void CheckBtn(bool result)
    {
        if (!result)
        {
            Managers.UI.CloseUI<NotionPopup>();
            return;
        }

        switch (CurrentPopupNum)
        {
            case ENUM_Notion_TYPE.Default :
                return;
            case ENUM_Notion_TYPE.Close:
                trainingCanvas.OnOffSettingPanel();
                trainingCanvas.SetNotionText("버튼 설정 종료");
                break;
            case ENUM_Notion_TYPE.Reset:
                keyPanelArea.SliderResetAll();
                trainingCanvas.SetNotionText("값을 초기화 했습니다.");
                break;
            case ENUM_Notion_TYPE.Save:
                if (!bottomPanel.isUpdatable)
                {
                    trainingCanvas.SetNotionText("선택한 UI와 겹치는 UI가 있어 저장할 수 없습니다.");
                }
                else if (bottomPanel.isOpen)
                {
                    bottomPanel.SaveSliderValue();
                    trainingCanvas.SetNotionText("저장을 완료했습니다.");
                }
                else
                    trainingCanvas.SetNotionText("설정 변경할 UI를 먼저 선택해주세요.");
                break;
        }

        Managers.UI.CloseUI<NotionPopup>();
    }
}
