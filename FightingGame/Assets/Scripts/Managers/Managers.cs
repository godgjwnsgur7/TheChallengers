using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGPlatform;

public class Managers : MonoBehaviour
{
    static Managers s_Instance;
    static Managers Instance { get { Init(); return s_Instance; } }

    private DataMgr data = new DataMgr();
    private BattleMgr battle = new BattleMgr();
    private InputMgr input = new InputMgr();
    private ObjectPoolMgr pool = new ObjectPoolMgr();
    private ResourceMgr resouce = new ResourceMgr();
    private SceneMgr scene = new SceneMgr();
    private SoundMgr sound = new SoundMgr();
    private UIMgr ui = new UIMgr();
    private PlatformMgr platform = new PlatformMgr();


    public static DataMgr Data { get { return Instance.data; } }
    public static BattleMgr Battle { get { return Instance.battle; } }
    public static InputMgr Input { get { return Instance.input; } }
    public static ObjectPoolMgr Pool { get { return Instance.pool; } }
    public static ResourceMgr Resource { get { return Instance.resouce; } }
    public static SceneMgr Scene { get { return Instance.scene; } }
    public static SoundMgr Sound { get { return Instance.sound; } }
    public static UIMgr UI { get { return Instance.ui; } }
    public static PlatformMgr Platform { get { return Instance.platform; } }
    
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        // input.OnUpdate();
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
            s_Instance.pool.Init();
            s_Instance.sound.Init();
            s_Instance.ui.Init();

            s_Instance.battle.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }
}
