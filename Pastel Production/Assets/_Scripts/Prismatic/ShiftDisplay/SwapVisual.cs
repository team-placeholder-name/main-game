using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;


public class SwapVisual : MonoBehaviour
{
    [SerializeField]
    RawImage rtOutput;
    [SerializeField]
    RawImage mask;


    //factory
    public static SwapVisual CreateSwapUI(GameObject swapCamDisplayPrefab, Transform parent)
    {
        SwapVisual swapCamDisplay = Instantiate(swapCamDisplayPrefab, parent).GetComponent<SwapVisual>();
        return swapCamDisplay;
    }

    //TODO: Refactor the shared logic into a seperate function
    public void Generate(Camera rtInput, SelectionRegion region, int boarder)
    {

        Texture2D generateMask = region.GenerateMask(boarder);
        mask.texture = generateMask;
        mask.color = Color.white;


        //This stuff stays here?
        rtOutput.transform.position = region.GetCenter();
        RenderTexture renderTexture;

        //apply the render texture
        renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);//Try a depth buffer of 16 or 32 if masking gives
        renderTexture.Create();
        rtInput.targetTexture = renderTexture;
        rtOutput.texture = renderTexture;
        // Release the hardware resources used by the render texture 
        renderTexture.Release();

    }
}
