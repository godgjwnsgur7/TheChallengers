using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKeyController : MonoBehaviour
{
    InputPanel inputPanel = null;

    public void Init()
    {
        if(inputPanel == null)
        {
            inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
        }
    }
}
