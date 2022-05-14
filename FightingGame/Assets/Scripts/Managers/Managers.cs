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

    // 아래 네트워크 관련 세션, 핸들러는 우선 마땅히 둘 곳이 없어 이 곳에 배치하였음
    // 클라 편한 대로 구워 삶으면 될 듯함
    private LoginSession loginSession = new LoginSession(); 
    private PhotonLogicHandler logicHandler = new PhotonLogicHandler(); 

    public static InputMgr Input { get { return Instance.input; } }
    public static ResourceMgr Resource { get { return Instance.resouce; } }
    public static SceneMgr Scene { get { return Instance.scene; } }
    public static UIMgr UIMgr { get { return Instance.ui; } }

    public static PhotonLogicHandler LogicHandler { get { return Instance.logicHandler; } }
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
