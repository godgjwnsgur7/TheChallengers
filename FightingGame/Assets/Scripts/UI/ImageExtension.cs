using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ImageExtension
{
    private static Material blurMaterial = null;

    public static void SetBlur(this Image image, bool isOn)
    {
        if (image == null)
        {
            Debug.LogError("Blur 처리할 이미지 없음");
            return;
        }

        image.material = isOn ? LoadBlurMaterial() : null;
    }

    private static Material LoadBlurMaterial()
    {
        string blurPath = "Materials/Blur";

        if (blurMaterial == null)
            blurMaterial = Resources.Load<Material>(blurPath);

        return blurMaterial;
    }
}
