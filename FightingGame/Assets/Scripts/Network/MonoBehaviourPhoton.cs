using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

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

public struct AnimatorSyncParam
{
    public string parameterName;
    public MonoBehaviourPhoton.AnimParameterType parameterType;

    public AnimatorSyncParam(string parameterName, MonoBehaviourPhoton.AnimParameterType parameterType)
	{
        this.parameterName = parameterName;
        this.parameterType = parameterType;
    }
}

public static class MonoBehaviourPhotonExtension
{
    /// <summary>
    /// multiAction은 BroadCast 가능한 메소드여야 합니다. 람다식 절대 금지~~~
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mono"></param>
    /// <param name="singleAction"></param>
    /// <param name="multiAction"></param>
    public static void TrySingleOrMultiAction<T>(this T mono, Action singleAction, Action multiAction)
        where T : MonoBehaviourPhoton
    {
        if (!PhotonLogicHandler.IsConnected)
        {
            singleAction?.Invoke();
        }
        else
        {
            PhotonLogicHandler.Instance.TryBroadcastMethod(mono, multiAction, ENUM_RPC_TARGET.All);
        }
    }

    public static void TryMultiAction<T>(this T mono, Action multiAction)
        where T : MonoBehaviourPhoton
    {
        if (PhotonLogicHandler.IsConnected)
        {
            PhotonLogicHandler.Instance.TryBroadcastMethod(mono, multiAction, ENUM_RPC_TARGET.All);
        }
    }

    public static void TryMultiAction<T, TParam>(this T mono, Action<TParam> multiAction, TParam param)
        where T : MonoBehaviourPhoton
    {
        if (PhotonLogicHandler.IsConnected)
        {
            PhotonLogicHandler.Instance.TryBroadcastMethod(mono, multiAction, param, ENUM_RPC_TARGET.All);
        }
    }
}


[RequireComponent(typeof(PhotonView))]

public class MonoBehaviourPhoton : MonoBehaviourPun, IPunObservable, IPunInstantiateMagicCallback
{
    public enum AnimParameterType
    {
        Float = 1,
        Int = 3,
        Bool = 4,
        Trigger = 9,
    }

    private PhotonWriteStream writeStream = new PhotonWriteStream();
    private PhotonReadStream readStream = new PhotonReadStream();

    public int ViewID => viewID;
    protected int viewID = 0;

    public bool IsInitialized 
    {
        get
		{
            return isInitialized && viewID != 0;

        }
    }
    private bool isInitialized = false;

    private Animator syncAnim = null;
    private Rigidbody2D syncRigid = null;
    private Transform syncTransform = null;

    public virtual void Init()
    {
        if (isInitialized)
            return;

        if (photonView == null)
            gameObject.AddComponent<PhotonView>();

        photonView.ObservedComponents = new List<Component>();

        isInitialized = true;
    }

    public virtual void OnEnable()
	{
        viewID = PhotonLogicHandler.Register(photonView);
    }

    public virtual void OnDisable()
	{
        PhotonLogicHandler.Unregister(viewID);
    }

    public void SyncAnimator(Animator anim, AnimatorSyncParam[] syncParameters = null)
    {
        if (syncAnim != null)
		{
            // Debug.LogError("이미 동기화할 애니메이터가 존재합니다. : " + viewID);
            return;
		}

        syncAnim = anim;

        GameObject ownerObj = syncAnim.gameObject;

        var component = ownerObj.GetComponent<PhotonAnimatorView>();
        if (component == null)
            component = ownerObj.AddComponent<PhotonAnimatorView>();

        photonView.ObservedComponents.Add(component);

        if (syncParameters == null)
            return;

        foreach(var param in syncParameters)
		{
            PhotonAnimatorView.ParameterType type = (PhotonAnimatorView.ParameterType)param.parameterType;
            component.SetParameterSynchronized(param.parameterName, type, PhotonAnimatorView.SynchronizeType.Continuous);
		}

        component.SetLayerSynchronized(0, PhotonAnimatorView.SynchronizeType.Continuous);
    }

    public void SyncTransformView(Transform tr, bool isSyncPosition = true, bool isSyncRotation = true, bool isSyncScale = true)
    {
        if (syncTransform != null)
        {
            // Debug.LogError("이미 동기화할 트랜스폼이 존재합니다. : " + viewID);
            return;
        }

        syncTransform = tr;
        GameObject ownerObj = syncTransform.gameObject;

        var component = ownerObj.GetComponent<PhotonTransformView>();
        if (component == null)
            component = ownerObj.AddComponent<PhotonTransformView>();

        component.m_SynchronizePosition = isSyncPosition;
        component.m_SynchronizeRotation = isSyncRotation;
        component.m_SynchronizeScale = isSyncScale;

        photonView.ObservedComponents.Add(component);
    }

    public void SyncPhysics(Rigidbody2D rigid, bool isSyncAngleVelocity = true, bool isSyncVelocity = true, bool isEnableTeleport = false, float distanceForTeleport = 10.0f)
    {
        if (syncRigid != null)
        {
            // Debug.LogError("이미 동기화할 리지드바디가 존재합니다. : " + viewID);
            return;
        }

        syncRigid = rigid;

        GameObject ownerObj = syncRigid.gameObject;

        var component = ownerObj.GetComponent<PhotonRigidbody2DView>();
        if (component == null)
            component = ownerObj.AddComponent<PhotonRigidbody2DView>();

        component.m_SynchronizeAngularVelocity = isSyncAngleVelocity;
        component.m_SynchronizeVelocity = isSyncVelocity;
        component.m_TeleportEnabled = isEnableTeleport;
        component.m_TeleportIfDistanceGreaterThan = distanceForTeleport;

        photonView.ObservedComponents.Add(component);
    }

    /// <summary>
    /// 송신 클라가 다른 수신 클라들을 동기화시킬 때 사용하는 함수, OnPhotonSerializeView에 의해 자동 적용되므로 호출할 필요는 없음, 정의만
    /// </summary>
    /// <param name="info"></param>

    protected virtual void OnMineSerializeView(PhotonWriteStream writeStream)
    {
        
    }

    /// <summary>
    /// 수신 클라들이 송신 클라로부터 받은 변수로 동기화를 받을 때 사용하는 함수, OnPhotonSerializeView에 의해 자동 적용되므로 호출할 필요는 없음, 정의만
    /// </summary>
    /// <param name="info"></param>

    protected virtual void OnOtherSerializeView(PhotonReadStream readStream)
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            writeStream.SetStream(stream);
            OnMineSerializeView(writeStream);
        }
        else if(stream.IsReading)
        {
            readStream.SetStream(stream);
            OnOtherSerializeView(readStream);
        }

        Debug.Log("--- OnPhotonSerializeView ---");
        Debug.Log($"정보 보낸 이 : {info.Sender?.NickName}");
        Debug.Log($"보낸 시간 : {info.SentServerTime}");
        Debug.Log($"보낸 시간 스탬프 : {info.SentServerTimestamp}");
        Debug.Log("--- --------------------- ---");
    }

	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
        if(!isInitialized)
		{
            Init();
        }  
	}
}
