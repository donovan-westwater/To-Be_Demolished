using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Shader FogEffect;
    public static int enableFog = 0;

    public Material fogMaterial;
    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (fogMaterial == null)
        {
            fogMaterial = new Material(FogEffect);
        }
        fogMaterial.SetInt("_EnableFog", enableFog);
        Graphics.Blit(source, destination,fogMaterial);
    }
}
