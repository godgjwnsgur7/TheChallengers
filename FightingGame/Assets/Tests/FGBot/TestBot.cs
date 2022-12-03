using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBot : FGBot
{
    bool fuckYou = true;

    public override IEnumerator CoProcess()
    {
        Initialize();

        Debug.Log("테스트 1");

        yield return null;

        Debug.Log("테스트 2");
        
        if (fuckYou)
        {
            Fail();
        }
        else
        {
            Success();
        }
            
        End();
    }
}
