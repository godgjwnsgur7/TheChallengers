using Photon.Pun;

/// <summary>
/// 네트워크 통신이 필요한 MonoBehaviour 클래스는 대신 해당 클래스를 사용하시오. 
/// </summary>
/// 

public class MonoBehaviourPhoton : MonoBehaviourPun
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
}
