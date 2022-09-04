using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        Type[] serialParamTypes = new Type[2] { typeof(StreamBuffer), typeof(object) };
        Type[] deserialParamTypes = new Type[2] { typeof(StreamBuffer), typeof(short) };

        foreach (var type in types)
        {
            if(type.IsAbstract)
                continue;

            var serializeMethod = type.GetMethod("Serialize", serialParamTypes);
            var deserializeMethod = type.GetMethod("Deserialize", deserialParamTypes);

            // 객체 참조가 필요하다.
            var sDel = serializeMethod.CreateDelegate(typeof(SerializeStreamMethod)) as SerializeStreamMethod;
            var dsDel = deserializeMethod.CreateDelegate(typeof(DeserializeStreamMethod)) as DeserializeStreamMethod;

            PhotonPeer.RegisterType(type, ++code, sDel, dsDel);
        }
    }
}

public abstract class PhotonCustomType
{
    
}