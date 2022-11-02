using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultWindowUI : MonoBehaviour
{
    [SerializeField] GameObject winWindow;
    [SerializeField] GameObject loseWindow;
    [SerializeField] GameObject drawWindow;

    public void Open(bool isDraw, bool isWin = true)
    {
        if(isDraw)
        {
            drawWindow.SetActive(true);
        }
        else
        {
            if (isWin)
            {
                winWindow.SetActive(true);
            }
            else
            {
                loseWindow.SetActive(true);
            }
        }

       gameObject.SetActive(true);
        
    }

    public void Close()
    {
        winWindow.SetActive(false);
        loseWindow.SetActive(false);
        drawWindow.SetActive(false);
    }

    public void OnClick_Check()
    {
        Close();

        Managers.Battle.GoToLobby();
    }
}
