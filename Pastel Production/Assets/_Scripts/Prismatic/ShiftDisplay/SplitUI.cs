using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplitUI : MonoBehaviour
{

    [SerializeField]
    RawImage image;
    [SerializeField]
    RawImage mask;


    //factory
    public static SplitUI CreateSplitUI(GameObject splitUIPrefab, Transform parent)
    {
        SplitUI splitUI = Instantiate(splitUIPrefab, parent).GetComponent<SplitUI>();
        return splitUI;
    }

    

    //TODO: Refactor the shared logic into a seperate function
    public void GenerateSplitRegion(Color color, SelectionRegion region)
    {
        image.color = color;


        int width = Screen.width;
        int height = Screen.height;

        Texture2D generateMask;

        generateMask = new Texture2D(width, height, TextureFormat.RGBA32, false);


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
                data[index] = region.CheckRegion(pixel) ? fillColor : alpha;
            }
        }
        // upload to the GPU
        generateMask.Apply();
        mask.texture = generateMask;
        mask.color = Color.white;
    }
}