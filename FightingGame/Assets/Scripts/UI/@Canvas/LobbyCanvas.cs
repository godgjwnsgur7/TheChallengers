using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FGDefine;
public class LobbyCanvas : BaseCanvas
{
    [SerializeField] CustomWindow customWindow;

    public override void Init()
    {
        base.Init();

        // 마스터 서버에 접속은 로그인씬에서 체크하고,
        // 로비로 성공적으로 넘어왔다면, 여기서 로비에 접속?
    }

    public void OnClick_Activate(GameObject g) => g.SetActive(true);
    public void OnClick_Deactivate(GameObject g) => g.SetActive(false);

    public override T GetUIComponent<T>()
    {
        return default(T);
    }
}