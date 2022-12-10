using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AttributeUtil
{
    public static string GetResourcePath<T>() where T : UnityEngine.Object
    {
        Type type = typeof(T);

        Attribute[] attributes = Attribute.GetCustomAttributes(type);

        foreach (Attribute attr in attributes)
        {
            ResourceAttribute resourceAttr = attr as ResourceAttribute;

            if (resourceAttr != null)
            {
                return resourceAttr.GetPath();
            }
        }

        return string.Empty;
    }

    public static ResourceType GetResourceType<T>() where T : UnityEngine.Object
    {
        Type type = typeof(T);

        Attribute[] attributes = Attribute.GetCustomAttributes(type);

        foreach (Attribute attr in attributes)
        {
            ResourceAttribute resourceAttr = attr as ResourceAttribute;

            if (resourceAttr != null)
            {
                return resourceAttr.GetResourceType();
            }
        }

        return ResourceType.None;
    }

    public static string GetResourcePath(Type type)
    {
        Attribute[] attributes = Attribute.GetCustomAttributes(type);

        foreach (Attribute attr in attributes)
        {
            ResourceAttribute resourceAttr = attr as ResourceAttribute;

            if (resourceAttr != null)
            {
                return resourceAttr.GetPath();
            }
        }

        return string.Empty;
    }
}
