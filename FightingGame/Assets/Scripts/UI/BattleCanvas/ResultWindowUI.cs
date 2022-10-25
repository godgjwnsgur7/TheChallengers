using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultWindowUI : MonoBehaviour
{
    [SerializeField] GameObject winWindow;
    [SerializeField] GameObject loseWindow;
    [SerializeField] GameObject drawWindow;

    public void Open()
    {
       // 임시로 갈겨
       gameObject.SetActive(true);
        
    }

    public void Close()
    {

    }

    public void OnClick_Check()
    {
        Close();

        Managers.Battle.GoToLobby();
    }
}
