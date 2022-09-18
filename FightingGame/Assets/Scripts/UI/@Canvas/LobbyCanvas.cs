using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FGDefine;
public class LobbyCanvas : BaseCanvas
{
    [SerializeField] CustomWindow customWindow;

    public void OnClick_Activate(GameObject g) => g.SetActive(true);
    public void OnClick_Deactivate(GameObject g) => g.SetActive(false);

    public override T GetUIComponent<T>()
    {

        return default(T);
    }
}
