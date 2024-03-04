using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;


public class SwapUI : MonoBehaviour
{
    [SerializeField]
    Camera rtInput;
    [SerializeField]
    RawImage rtOutput;
    [SerializeField]
    RawImage mask;


    //factory
    public static SwapUI CreateSwapUI(GameObject swapCamDisplayPrefab, Transform parent, Camera input)
    {
        SwapUI swapCamDisplay = Instantiate(swapCamDisplayPrefab, parent).GetComponent<SwapUI>();
        swapCamDisplay.rtInput = input;
        return swapCamDisplay;
    }

    //TODO: Refactor the shared logic into a seperate function
    public void GenerateCutoutSlice(float angle1, float angle2, float centerRadius)
    {
        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
        rtOutput.transform.position = ShiftUIGen.GetCenterOfSlice(center, angle1, angle2, 3);
        RenderTexture renderTexture;
        Texture2D generateMask;
        //apply the render texture
        renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);//Try a depth buffer of 16 or 32 if masking gives
        renderTexture.Create();
        rtInput.targetTexture = renderTexture;
        rtOutput.texture = renderTexture;
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


        for (int y = 0; y < generateMask.height; y++)
        {
            for (int x = 0; x < generateMask.width; x++)
            {
                Vector2 pixel = new Vector2(x, y);
                data[index++] = OuterSliceCheck(pixel,center, centerRadius,angle1, angle2) ? fillColor : alpha;
            }
        }
        // upload to the GPU
        generateMask.Apply();
    }
    public void GenerateCircleCutout(float centerRadius)
    {
        RenderTexture renderTexture;
        Texture2D generateMask;
        //apply the render texture
        renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);//Try a depth buffer of 16 or 32 if masking gives
        renderTexture.Create();
        rtInput.targetTexture = renderTexture;
        rtOutput.texture = renderTexture;
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
        for (int y = 0; y < generateMask.height; y++)
        {
            for (int x = 0; x < generateMask.width; x++)
            {
                Vector2 pixel = new Vector2(x, y);
                data[index++] = ShiftUIGen.CircleCheck(pixel, center, centerRadius) ? fillColor : alpha;
            }
        }
        // upload to the GPU
        generateMask.Apply();
    }

    public bool OuterSliceCheck(Vector2 pixel, Vector2 center, float centerRadius, float angle1, float angle2)
    {
        return !ShiftUIGen.CircleCheck(pixel, center, centerRadius) && ShiftUIGen.AngleCheck(pixel, center, angle1, angle2);
    }

    
}
