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
       // 졌는지 이겼는지 비겼는지 판단한 값을 받아야 함
    }

    public void Close()
    {

    }

    public void OnClick_Check()
    {
        Close();

        // 커스텀룸에서 시작한 게임이라면 돌아가야 함.
    }
}
