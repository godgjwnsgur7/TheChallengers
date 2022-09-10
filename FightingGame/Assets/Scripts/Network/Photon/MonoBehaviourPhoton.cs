using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public T Read<T>()
    {
        return (T)stream.ReceiveNext();
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

    public static void TryMineAction<T>(this T mono, Action mineAction)
    where T : MonoBehaviourPhoton
    {
        if (PhotonLogicHandler.IsMine(mono.ViewID))
        {
            mineAction?.Invoke();
        }
    }

    public static void TryMineMultiAction<T>(this T mono, Action multiAction)
    where T : MonoBehaviourPhoton
    {
        if (!PhotonLogicHandler.IsConnected)
            return;

        if (!PhotonLogicHandler.IsMine(mono.ViewID))
            return;

        PhotonLogicHandler.Instance.TryBroadcastMethod(mono, multiAction, ENUM_RPC_TARGET.All);
    }

    public static void TryMineMultiAction<T, TParam>(this T mono, Action<TParam> multiAction, TParam param)
        where T : MonoBehaviourPhoton
    {
        if (!PhotonLogicHandler.IsConnected)
            return;

        if (!PhotonLogicHandler.IsMine(mono.ViewID))
            return;

        PhotonLogicHandler.Instance.TryBroadcastMethod(mono, multiAction, param, ENUM_RPC_TARGET.All);
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

    Dictionary<string, bool> animTriggerDictionary = new Dictionary<string, bool>();
    Dictionary<string, bool> animBoolDictionary = new Dictionary<string, bool>();
    Dictionary<string, int> animIntDictionary = new Dictionary<string, int>();
    Dictionary<string, float> animFloatDictionary = new Dictionary<string, float>();

    protected Action<string, bool> OnBoolParameter = null;
    protected Action<string> OnTriggerParameter = null;
    protected Action<string, int> OnIntParameter = null;
    protected Action<string, float> OnFloatParameter = null;

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

        photonView.ObservedComponents = new List<Component>() { this };

        if (viewID == 0)
            viewID = PhotonLogicHandler.Register(photonView);

        isInitialized = true;
    }

    private void SetAnimParameter()
    {
        OnTriggerParameter = (sender) =>
        {
            animTriggerDictionary[sender] = true;
        };

        OnBoolParameter = (sender, arg) =>
        {
            animBoolDictionary[sender] = arg;
        };

        OnIntParameter = (sender, arg) =>
        {
            animIntDictionary[sender] = arg;
        };

        OnFloatParameter = (sender, arg) =>
        {
            animFloatDictionary[sender] = arg;
        };
    }

    private void UnsetAnimParameter()
    {
        OnTriggerParameter = null;
        OnBoolParameter = null;
        OnIntParameter = null;
        OnFloatParameter = null;
    }
    
    public void SetAnimBool(string paramName, bool value)
    {
        if(!PhotonLogicHandler.IsConnected || !PhotonLogicHandler.IsFullRoom)
        {
            syncAnim.SetBool(paramName, value);
            return;
        }

        OnBoolParameter?.Invoke(paramName, value);
    }

    public void SetAnimTrigger(string paramName)
    {
        if (!PhotonLogicHandler.IsConnected || !PhotonLogicHandler.IsFullRoom)
        {
            syncAnim.SetTrigger(paramName);
            return;
        }

        OnTriggerParameter?.Invoke(paramName);
    }

    public void SetAnimInt(string paramName, int value)
    {
        if (!PhotonLogicHandler.IsConnected || !PhotonLogicHandler.IsFullRoom)
        {
            syncAnim.SetInteger(paramName, value);
            return;
        }

        OnIntParameter?.Invoke(paramName, value);
    }

    public void SetAnimFloat(string paramName, float value)
    {
        if (!PhotonLogicHandler.IsConnected || !PhotonLogicHandler.IsFullRoom)
        {
            syncAnim.SetFloat(paramName, value);
            return;
        }

        OnFloatParameter?.Invoke(paramName, value);
    }

    protected virtual void OnDestroy()
    {
        UnsetAnimParameter();
    }

    public virtual void OnEnable()
	{
        if(viewID == 0)
            viewID = PhotonLogicHandler.Register(photonView);
    }

    public virtual void OnDisable()
	{
        viewID = PhotonLogicHandler.Unregister(viewID);
    }

    public override sealed void Start()
	{
        // Start문은 사용하지 마시오.
	}

    public void SyncAnimator(Animator anim, AnimatorSyncParam[] syncParameters = null)
    {
        if (syncAnim != null)
		{
            // Debug.LogError("이미 동기화할 애니메이터가 존재합니다. : " + viewID);
            return;
		}

        syncAnim = anim;

        if (syncParameters == null)
            return;

        SetAnimParameter();

        foreach (var param in syncParameters)
		{
            PhotonAnimatorView.ParameterType type = (PhotonAnimatorView.ParameterType)param.parameterType;

            switch(type)
            {
                case PhotonAnimatorView.ParameterType.Bool:
                    animBoolDictionary.Add(param.parameterName, false);
                    break;

                case PhotonAnimatorView.ParameterType.Trigger:
                    animTriggerDictionary.Add(param.parameterName, false);
                    break;

                case PhotonAnimatorView.ParameterType.Int:
                    animIntDictionary.Add(param.parameterName, 0);
                    break;

                case PhotonAnimatorView.ParameterType.Float:
                    animFloatDictionary.Add(param.parameterName, 0.0f);
                    break;
            }
           
		}
    }

    private void SyncAnimationState()
    {
        if (!IsInitialized)
            return;

        foreach (var boolSet in animBoolDictionary)
        {
            if (syncAnim.GetBool(boolSet.Key) != boolSet.Value)
            {
                syncAnim.SetBool(boolSet.Key, boolSet.Value);
            }
        }

        foreach (var intSet in animIntDictionary)
        {
            if (syncAnim.GetInteger(intSet.Key) != intSet.Value)
            {
                syncAnim.SetInteger(intSet.Key, intSet.Value);
            }
        }
        foreach (var floatSet in animFloatDictionary)
        {
            if (syncAnim.GetFloat(floatSet.Key) != floatSet.Value)
            {
                syncAnim.SetFloat(floatSet.Key, floatSet.Value);
            }
        }
        List<string> changedKeys = new List<string>();
        foreach (var triggerSet in animTriggerDictionary)
        {
            if (triggerSet.Value == true)
            {
                syncAnim.SetTrigger(triggerSet.Key);
                changedKeys.Add(triggerSet.Key);
            }
        }

        foreach (var key in changedKeys)
        {
            animTriggerDictionary[key] = false;
        }
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
        if (syncAnim == null)
            return;

        foreach(var set in animBoolDictionary)
        {
            writeStream.Write(set.Key);
            writeStream.Write(set.Value);
        }
        foreach (var set in animTriggerDictionary)
        {
            writeStream.Write(set.Key);
            writeStream.Write(set.Value);
        }
        foreach (var set in animIntDictionary)
        {
            writeStream.Write(set.Key);
            writeStream.Write(set.Value);
        }
        foreach (var set in animFloatDictionary)
        {
            writeStream.Write(set.Key);
            writeStream.Write(set.Value);
        }

        SyncAnimationState();
    }

    /// <summary>
    /// 수신 클라들이 송신 클라로부터 받은 변수로 동기화를 받을 때 사용하는 함수, OnPhotonSerializeView에 의해 자동 적용되므로 호출할 필요는 없음, 정의만
    /// </summary>
    /// <param name="info"></param>

    protected virtual void OnOtherSerializeView(PhotonReadStream readStream)
    {
        if (syncAnim == null)
            return;

        for (int i = 0; i < animBoolDictionary.Count; i++)
        {
            var key = readStream.Read<string>();
            animBoolDictionary[key] = readStream.Read<bool>();
        }
        for (int i = 0; i < animTriggerDictionary.Count; i++)
        {
            var key = readStream.Read<string>();
            animTriggerDictionary[key] = readStream.Read<bool>();
        }
        for (int i = 0; i < animIntDictionary.Count; i++)
        {
            var key = readStream.Read<string>();
            animIntDictionary[key] = readStream.Read<int>();
        }
        for (int i = 0; i < animFloatDictionary.Count; i++)
        {
            var key = readStream.Read<string>();
            animFloatDictionary[key] = readStream.Read<float>();
        }

        SyncAnimationState();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            //if (!PhotonLogicHandler.IsMine(viewID))
                //return;

            writeStream.SetStream(stream);
            OnMineSerializeView(writeStream);
        }
        else if(stream.IsReading)
        {
            //if (PhotonLogicHandler.IsMine(viewID))
                //return;

            readStream.SetStream(stream);
            OnOtherSerializeView(readStream);
        }

        // Debug.Log("--- OnPhotonSerializeView ---");
        // Debug.Log($"정보 보낸 이 : {info.Sender?.NickName}");
        // Debug.Log($"보낸 시간 : {info.SentServerTime}");
        // Debug.Log($"보낸 시간 스탬프 : {info.SentServerTimestamp}");
        // Debug.Log("--- --------------------- ---");
    }

	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
        if(!isInitialized)
		{
            Init();
        }  
	}
}
