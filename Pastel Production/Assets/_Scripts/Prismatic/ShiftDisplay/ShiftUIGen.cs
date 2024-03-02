using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ShiftUIGen : MonoBehaviour
{
    [SerializeField]
    Camera output;
    [SerializeField]
    RawImage input;
    [SerializeField]
    RawImage mask;

    RenderTexture renderTexture;

    Texture2D generateMask; 
    // Start is called before the first frame update
    void Start()
    {
        //apply the render texture
        renderTexture = new RenderTexture(Screen.width, Screen.height, 0,RenderTextureFormat.ARGB32);//Try a depth buffer of 16 or 32 if masking gives
        renderTexture.Create();
        output.targetTexture = renderTexture;
        input.texture = renderTexture;
        // Release the hardware resources used by the render texture 
        renderTexture.Release();


        generateMask = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        mask.texture = generateMask;
        mask.color = Color.white;

        // RGBA32 texture format data layout exactly matches Color32 struct
        NativeArray<Color32> data = generateMask.GetRawTextureData<Color32>();

        // fill texture data
        Color32 alpha = new Color32(0, 0, 0, 0);
        Color32 fillColor = new Color32(255, 255, 255, 255);
        int index = 0;

        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
        float radius = Screen.height / 3;
        for (int y = 0; y < generateMask.height; y++)
        {
            for (int x = 0; x < generateMask.width; x++)
            {
                Vector2 pixel = new Vector2(x, y);
                data[index++] = !CircleCheck(pixel,center,Screen.height/4f)&&AngleCheck(pixel,center,60+180,0+180) ? fillColor : alpha;
                
            }
        }
        // upload to the GPU
        generateMask.Apply();
        
    }
    private bool CircleCheck(Vector2 point, Vector2 center, float radius)
    {
        return Vector2.Distance(point, center) < radius;
    }
    private bool AngleCheck(Vector2 point, Vector2 center, float angleStart, float angleEnd)
    {
        Vector2 dir = (point - center).normalized;
        Vector2  start = new Vector2(Mathf.Sin(Mathf.Deg2Rad*angleStart), Mathf.Cos(Mathf.Deg2Rad*angleStart));
        Vector2 end = new Vector2(Mathf.Sin(Mathf.Deg2Rad * angleEnd), Mathf.Cos(Mathf.Deg2Rad * angleEnd));
        float angleDif = (Vector2.Angle(dir, start) + Vector2.Angle(dir, end)) - Vector2.Angle(start, end);
        return angleDif < 0.1f&& angleDif>-0.1f;
    }

}
