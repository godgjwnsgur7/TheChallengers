using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : BaseCanvas
{
    [SerializeField] LoginWindow interactableUI;
    [SerializeField] ProduceUI produceUI;
    [SerializeField] Text notion;
    
    public override void Close<T>()
    {
        if (typeof(T) == typeof(TestLoginWindow)) interactableUI.Close();
        else if (typeof(T) == typeof(ProduceUI)) produceUI.Close();
        else Debug.Log("범위 벗어남");
    }

    public override void Open<T>(UIParam param = null)
    {
        if (typeof(T) == typeof(TestLoginWindow)) interactableUI.Open();
        else if (typeof(T) == typeof(ProduceUI)) produceUI.Open();
        else Debug.Log("범위 벗어남");
    }

    public override T GetUIComponent<T>()
    {

        return default(T);
    }

    public void OnClickStart()
    {
        if (PlayerPrefs.HasKey("LoginUser")) // 로그인 중 일 때로 변경해야함
        {
            bool a = PhotonLogicHandler.Instance.TryConnectToMaster(
            () => { Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.Lobby); },
            SetStatus);
        }
        else
        {
            if (interactableUI.gameObject.activeSelf)
            {
                Managers.UI.CloseUI<TestLoginWindow>();
            }
            else
            {
                Managers.UI.OpenUI<TestLoginWindow>();
            }
        }
    }

    public void OnClickProduce() 
    {
        if (produceUI.gameObject.activeSelf)
            Managers.UI.CloseUI<ProduceUI>();
        else
            Managers.UI.OpenUI<ProduceUI>();
    }

    public void SetStatus(string txt)
    {
        notion.text = txt;
    }
}
