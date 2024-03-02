using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRender : MonoBehaviour
{
    [SerializeField]
    Camera output;
    [SerializeField]
    RawImage input;

    RenderTexture renderTexture;

    Texture2D mask; 
    // Start is called before the first frame update
    void Start()
    {
        renderTexture = new RenderTexture(Screen.width, Screen.height, 0,RenderTextureFormat.ARGB32);//Try a depth buffer of 16 or 32 if masking gives
        renderTexture.Create();
        output.targetTexture = renderTexture;
        input.texture = renderTexture;
        // Release the hardware resources used by the render texture 
        renderTexture.Release();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
