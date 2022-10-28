using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKeyController : MonoBehaviour
{
    List<KeySettingData> keySettingDataList = null;

    public bool isActive = false;
    public InputPanel inputPanel = null;

    public void Init()
    {
        if(inputPanel == null)
        {
            inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
        }

        // 설정된 PlayerPrefs 호출
        keySettingDataList = PlayerPrefsManagement.Load_KeySettingData();
        // 이제 생각해보니 Controller가 Init될때마다 Management에서 수정된 값으로 InputKey들의 크기등을 바꿔야하는데
        // 수정은 Management에서만 하네..? 수정을 어케하지...

        if (!this.inputPanel.gameObject.activeSelf)
        {
            Set_PanelActive(true);
            this.isActive = true;
        }
    }

    public void Set_PanelActive(bool _binary)
    {
        this.inputPanel.gameObject.SetActive(_binary);
    }
}
