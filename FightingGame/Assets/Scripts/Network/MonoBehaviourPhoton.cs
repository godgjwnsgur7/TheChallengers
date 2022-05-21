using Photon.Pun;
using System;

/// <summary>
/// 네트워크 통신이 필요한 MonoBehaviour 클래스는 대신 해당 클래스를 사용하시오. 
/// </summary>
/// 

public class PhotonCustomStreamBase
{
    protected PhotonStream stream;

    public void SetStream(PhotonStream stream)
    {
        this.stream = stream;
    }
}

public class PhotonWriteStream : PhotonCustomStreamBase
{
    public void Write(object data)
    {
        stream.SendNext(data);
    }
}

public class PhotonReadStream : PhotonCustomStreamBase
{
    public object Read()
    {
        return stream.ReceiveNext();
    }
}

public class MonoBehaviourPhoton : MonoBehaviourPun, IPunObservable
{
    private PhotonWriteStream writeStream = new PhotonWriteStream();
    private PhotonReadStream readStream = new PhotonReadStream();

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
        photonView.ObservedComponents.Add(component);
    }

    private void SyncTransformView()
    {
        var component = GetOrAddComponent<PhotonTransformView>();
        photonView.ObservedComponents.Add(component);
    }

    private void SyncPhysics()
    {
        var component = GetOrAddComponent<PhotonRigidbody2DView>();
        photonView.ObservedComponents.Add(component);
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

    protected virtual void OnMasterSerializeView(PhotonWriteStream writeStream, PhotonMessageInfo info)
    {
        
    }

    /// <summary>
    /// 슬레이브 클라이언트가 마스터로부터 받은 변수로 동기화를 받을 때 사용하는 함수, OnPhotonSerializeView에 의해 자동 적용되므로 호출할 필요는 없음, 정의만
    /// </summary>
    /// <param name="info"></param>

    protected virtual void OnSlaveSerializeView(PhotonReadStream readStream, PhotonMessageInfo info)
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            writeStream.SetStream(stream);
            OnMasterSerializeView(writeStream, info);
        }
        else if(stream.IsReading)
        {
            readStream.SetStream(stream);
            OnSlaveSerializeView(readStream, info);
        }
    }
}
