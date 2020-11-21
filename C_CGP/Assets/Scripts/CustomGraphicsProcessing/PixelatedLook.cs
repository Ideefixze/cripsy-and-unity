//PIXELBOY BY @WTFMIG EAT A BUTT WORLD BAHAHAHAHA POOP MY PANTS
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/PixelatedLook")]
public class PixelatedLook : MonoBehaviour
{
    public int w = 720;
    int h;

    [SerializeField]
    private Material[] pixelateEffects;
    protected void Start()
    {

    }
    void Update()
    {

        float ratio = ((float)Camera.main.pixelHeight / (float)Camera.main.pixelWidth);
        h = Mathf.RoundToInt(w * ratio);

    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        source.filterMode = FilterMode.Point;
        RenderTexture buffer = RenderTexture.GetTemporary(w, h, -1);
        buffer.filterMode = FilterMode.Point;
        Graphics.Blit(source, buffer);

        foreach(Material m in pixelateEffects)
        {
            RenderTexture buffer2 = RenderTexture.GetTemporary(w, h, -1);
            buffer2.filterMode = FilterMode.Point;
            m.SetTexture("Texture", buffer);
            Graphics.Blit(buffer, buffer2, m);
            Graphics.Blit(buffer2, buffer);
        }
        Graphics.Blit(buffer, destination);

        RenderTexture.ReleaseTemporary(buffer);
    }
}