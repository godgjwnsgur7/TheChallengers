using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Instance;
    static Managers Instance { get { Init(); return s_Instance; } }

    private InputMgr input = new InputMgr();
    private ResourceMgr resouce = new ResourceMgr();
    private SceneMgr scene = new SceneMgr();
    private UIMgr ui = new UIMgr();

    private LoginSession loginSession = new LoginSession(); // 이 곳에 우선 세션을 추가하였으나, 클라가 검토해서 사용하는 것을 추천

    public static InputMgr Input { get { return Instance.input; } }
    public static ResourceMgr Resource { get { return Instance.resouce; } }
    public static SceneMgr Scene { get { return Instance.scene; } }
    public static UIMgr UIMgr { get { return Instance.ui; } }

    public static LoginSession LoginSession { get { return Instance.loginSession; } }


    private void Start()
    {
        Init();
    }

    private void Update()
    {
        input.OnUpdate();
    }

    private static void Init()
    {
        if (s_Instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<Managers>();
        }
    }
}
