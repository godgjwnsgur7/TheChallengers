using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Instance;
    static Managers Instance { get { Init(); return s_Instance; } }

    private DataMgr data = new DataMgr();
    private GameMgr game = new GameMgr();
    private InputMgr input = new InputMgr();
    private ObjectPoolMgr pool = new ObjectPoolMgr();
    private ResourceMgr resouce = new ResourceMgr();
    private SceneMgr scene = new SceneMgr();
    private SoundMgr sound = new SoundMgr();
    private UIMgr ui = new UIMgr();
    private PlayerPrefsMgr prefs = new PlayerPrefsMgr();
    private LoginSession loginSession = new LoginSession();
    private DBSession dbSession = new DBSession();
    private AdMobController adMob = new AdMobController();
    private CoffeeMachine iapController = new IAPController();

    public static DataMgr Data { get { return Instance.data; } }
    public static GameMgr Game { get { return Instance.game; } }
    public static InputMgr Input { get { return Instance.input; } }
    public static ObjectPoolMgr Pool { get { return Instance.pool; } }
    public static ResourceMgr Resource { get { return Instance.resouce; } }
    public static SceneMgr Scene { get { return Instance.scene; } }
    public static SoundMgr Sound { get { return Instance.sound; } }
    public static UIMgr UI { get { return Instance.ui; } }
    public static PlayerPrefsMgr Prefs { get { return Instance.prefs; } }
    public static LoginSession LoginSession { get { return Instance.loginSession; } }
    public static DBSession DbSession { get { return Instance.dbSession; } }
    public static AdMobController AdMob { get { return Instance.adMob; } }
    public static CoffeeMachine IAPController { get { return Instance.iapController; } }
    
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
            s_Instance.prefs.init();
            s_Instance.game.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
        Game.Clear();
    }
}
