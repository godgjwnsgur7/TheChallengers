using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGPlatform;
using System;

public class Managers : MonoBehaviour
{
    static Managers s_Instance;
    static Managers Instance { get { Init(); return s_Instance; } }

    private BattleMgr battle = new BattleMgr();
    private DataMgr data = new DataMgr();
    private NetworkMgr network = new NetworkMgr();
    private InputMgr input = new InputMgr();
    private ObjectPoolMgr pool = new ObjectPoolMgr();
    private ObjectOrderLayerMgr orderLayer = new ObjectOrderLayerMgr();
    private ResourceMgr resouce = new ResourceMgr();
    private SceneMgr scene = new SceneMgr();
    private SoundMgr sound = new SoundMgr();
    private UIMgr ui = new UIMgr();
    private PlatformMgr platform = new PlatformMgr();
    private PlayerCharacter player = null;

    public static BattleMgr Battle { get { return Instance.battle; } }
    public static DataMgr Data { get { return Instance.data; } }
    public static NetworkMgr Network { get { return Instance.network; } }
    public static InputMgr Input { get { return Instance.input; } }
    public static ObjectPoolMgr Pool { get { return Instance.pool; } }
    public static ObjectOrderLayerMgr OrderLayer { get { return Instance.orderLayer; } }
    public static ResourceMgr Resource { get { return Instance.resouce; } }
    public static SceneMgr Scene { get { return Instance.scene; } }
    public static SoundMgr Sound { get { return Instance.sound; } }
    public static UIMgr UI { get { return Instance.ui; } }
    public static PlatformMgr Platform { get { return Instance.platform; } }
    public static PlayerCharacter Player
    { 
        get 
        {
            if (Scene.CurrSceneType != FGDefine.ENUM_SCENE_TYPE.Battle)
            {
                Debug.Log("Player is Null : 배틀 씬이 아닙니다.");
                return null;
            }
            
            if(s_Instance.player == null)
                s_Instance.player = GameObject.FindObjectOfType<PlayerCharacter>();

            return Instance.player; 
        }
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
#if UNITY_ANDROID
        UI.Update_InputBackKeyCheck();
#endif
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

            s_Instance.data.Init();
            s_Instance.network.Init();
            s_Instance.pool.Init();
            s_Instance.sound.Init();
            s_Instance.ui.Init();
            s_Instance.orderLayer.Init();
        }
    }

    /// <summary>
    /// 씬이 넘어갈때마다 호출
    /// </summary>
    public static void Clear()
    {
        if (s_Instance == null)
            return;

        Sound.Clear();
        Pool.Clear();
        Battle.Clear();
        Input.Clear();
        OrderLayer.Clear();
        UI.Clear();
    }
}
