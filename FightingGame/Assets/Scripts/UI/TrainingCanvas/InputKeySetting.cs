using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKeySetting : MonoBehaviour
{
    [SerializeField] InputKeyManagement inputKeyManagement;
    [SerializeField] SettingPanel settingPanel;

    private void OnEnable()
    {
        inputKeyManagement.Init();
        settingPanel.Open();
    }
}
