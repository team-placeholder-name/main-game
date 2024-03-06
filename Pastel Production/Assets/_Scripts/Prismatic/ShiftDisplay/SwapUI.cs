using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;


public class SwapUI : MonoBehaviour
{
    [SerializeField]
    RawImage rtOutput;
    [SerializeField]
    RawImage mask;


    //factory
    public static SwapUI CreateSwapUI(GameObject swapCamDisplayPrefab, Transform parent)
    {
        SwapUI swapCamDisplay = Instantiate(swapCamDisplayPrefab, parent).GetComponent<SwapUI>();
        return swapCamDisplay;
    }

    //TODO: Refactor the shared logic into a seperate function
    public void GenerateSwapRegion(Camera rtInput, SelectionRegion region, int boarder)
    {
        int width= Screen.width;
        int height = Screen.height;


        Texture2D generateMask = new Texture2D(width, height, TextureFormat.RGBA32, false);


        // RGBA32 texture format data layout exactly matches Color32 struct
        NativeArray<Color32> data = generateMask.GetRawTextureData<Color32>();

        // fill texture data
        Color32 alpha = new Color32(0, 0, 0, 0);
        Color32 fillColor = new Color32(255, 255, 255, 255);

        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 pixel = new Vector2(x, y);
                int index = y * width + x;
                data[index++] = region.CheckBoarderedRegion(pixel,boarder) ? fillColor : alpha;
            }
        }
        // upload to the GPU
        generateMask.Apply();



        //This stuff stays here?
        rtOutput.transform.position = region.GetCenter();
        RenderTexture renderTexture;

        //apply the render texture
        renderTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);//Try a depth buffer of 16 or 32 if masking gives
        renderTexture.Create();
        rtInput.targetTexture = renderTexture;
        rtOutput.texture = renderTexture;
        // Release the hardware resources used by the render texture 
        renderTexture.Release();
        mask.texture = generateMask;
        mask.color = Color.white;
    }
}
