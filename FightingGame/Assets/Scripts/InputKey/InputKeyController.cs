using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKeyController : MonoBehaviour
{
    public bool isActive = false;
    public InputPanel inputPanel = null;

    public void Init()
    {
        if(inputPanel == null)
        {
            inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
        }

        if(!this.inputPanel.gameObject.activeSelf)
            Set_Active(true);
    }

    public void Set_Active(bool _binary)
    {
        this.isActive = _binary;
        this.inputPanel.gameObject.SetActive(_binary);
    }
}
