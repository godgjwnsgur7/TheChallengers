using Photon.Pun;

/// <summary>
/// 네트워크 통신이 필요한 MonoBehaviour 클래스는 대신 해당 클래스를 사용하시오. 
/// </summary>
/// 

public class MonoBehaviourPhoton : MonoBehaviourPun, IPunObservable
{
    protected virtual void Awake()
    {
        gameObject.AddComponent<PhotonView>();
    }

    protected void Init(bool isAnimatingSync, bool isTransformSync, bool isPhysicsSync)
    {
        if (isAnimatingSync)
            SyncAnimatorView();

        if (isTransformSync)
            SyncTransformView();

        if (isPhysicsSync)
            SyncPhysics();
    }

    private void SyncAnimatorView()
    {
        var component = GetOrAddComponent<PhotonAnimatorView>();
    }

    private void SyncTransformView()
    {
        var component = GetOrAddComponent<PhotonTransformView>();
    }

    private void SyncPhysics()
    {
        var component = GetOrAddComponent<PhotonRigidbody2DView>();
    }

    private T GetOrAddComponent<T>() where T : MonoBehaviourPun
    {
        T component = GetComponent<T>();

        if (component == null)
            component = gameObject.AddComponent<T>();

        return component;
    }

    protected virtual void OnEnable()
    {
        PhotonLogicHandler.Register(this);
    }

    protected virtual void OnDisable()
    {
        PhotonLogicHandler.Unregister(this);
    }

    /// <summary>
    /// 마스터 클라이언트가 모든 슬레이브들을 동기화시킬 때 사용하는 함수, OnPhotonSerializeView에 의해 자동 적용되므로 호출할 필요는 없음, 정의만
    /// </summary>
    /// <param name="info"></param>

    protected virtual void OnMasterSerializeView(PhotonMessageInfo info)
    {
        
    }

    /// <summary>
    /// 슬레이브 클라이언트가 마스터로부터 받은 변수로 동기화를 받을 때 사용하는 함수, OnPhotonSerializeView에 의해 자동 적용되므로 호출할 필요는 없음, 정의만
    /// </summary>
    /// <param name="info"></param>

    protected virtual void OnSlaveSerializeView(PhotonMessageInfo info)
    {

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            OnMasterSerializeView(info);
        }
        else if(stream.IsReading)
        {
            OnSlaveSerializeView(info);
        }
    }
}
