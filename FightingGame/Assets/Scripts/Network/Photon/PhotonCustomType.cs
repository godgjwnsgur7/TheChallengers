using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

internal class PhotonCustomTypeManagement 
{ 
	public static void Register()
	{
        SetPhotonPeerParameterType();
    }

    private static void SetPhotonPeerParameterType()
    {
        var types = Assembly.GetAssembly(typeof(PhotonCustomType))
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(PhotonCustomType)))
            ?.ToArray();

        if (types == null || types.Length > 255)
		{
            Debug.LogError("등록할 수 있는 커스텀 타입의 갯수는 255개를 넘을 수 없습니다.");
            return;
        }
            
        byte code = 0;

        Type[] serialParamTypes = new Type[] { typeof(object) };
        Type[] deserialParamTypes = new Type[] { typeof(byte[] )};

        foreach (var type in types)
        {
            if(type.IsAbstract)
                continue;

            var serializeMethod = type.GetMethod("Serialize", serialParamTypes);
            var deserializeMethod = type.GetMethod("Deserialize", deserialParamTypes);

            if (serializeMethod == null || deserializeMethod == null)
                continue;

            var sDel = serializeMethod.CreateDelegate(typeof(SerializeMethod));
            var dsDel = deserializeMethod.CreateDelegate(typeof(DeserializeMethod));

            PhotonPeer.RegisterType(type, ++code, sDel as SerializeMethod, dsDel as DeserializeMethod);
        }
    }
}

/// <summary>
/// 반드시 
/// public static object Deserialize(byte[] data);
/// public static byte[] Serialize(object customObject);
/// 두 함수를 정의해야 동작합니다.
/// 
/// 커스텀 타입 클래스의 모든 멤버 변수를 byte의 형태로 나타낼 수 있다면, (재량껏...)
/// 우선 참조의 참조 형식(클래스 타입 변수)은 무조건 안됩니다
/// 해당 타입을 상속하여 사용할 수 있습니다. 
/// </summary>
public abstract class PhotonCustomType
{

}

public class PhotonCustomType<T> : PhotonCustomType where T : new()
{
    public static object Deserialize(byte[] data)
    {
        if (!typeof(T).IsSerializable)
		{
            Debug.LogError($"{typeof(T)} 해당 타입은 직렬화가 불가능합니다.");
            return null;
        }

        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }

    public static byte[] Serialize(object customObject)
    {
        if (!typeof(T).IsSerializable)
        {
            Debug.LogError($"{typeof(T)} 해당 타입은 직렬화가 불가능합니다.");
            return null;
        }

        T param = (T)customObject;
        string jsonData = JsonUtility.ToJson(param);
        return Encoding.UTF8.GetBytes(jsonData);
    }
}