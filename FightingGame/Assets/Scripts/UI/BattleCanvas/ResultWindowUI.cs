using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultWindowUI : MonoBehaviour
{
    [SerializeField] Text resultText;
    
    [SerializeField] RankingScoreUI rankingScore;
    
    [SerializeField] Text notifyCountText;




    public void Open(bool isDraw, bool isWin = true)
    {
        if(isDraw)
        {
            // 무승부

        }
        else
        {
            if (isWin)
            {
               // 승리

            }
            else
            {
                // 패배

            }
        }

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
