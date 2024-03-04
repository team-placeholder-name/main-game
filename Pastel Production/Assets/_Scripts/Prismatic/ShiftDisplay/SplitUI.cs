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
    public static SplitUI CreateSplitUI(GameObject splitUIPrefab, Transform parent, Color color)
    {
        SplitUI splitUI = Instantiate(splitUIPrefab, parent).GetComponent<SplitUI>();
        splitUI.image.color = color;
        return splitUI;
    }

    //TODO: Refactor the shared logic into a seperate function
    public void GenerateCutoutSlice(float angle1, float angle2, float centerRadius, float outerRadius)
    {
        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);

        Texture2D generateMask;

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
                data[index++] = OuterSliceCheck(pixel, center, centerRadius, outerRadius, angle1, angle2) ? fillColor : alpha;
            }
        }
        // upload to the GPU
        generateMask.Apply();
    }


    public bool OuterSliceCheck(Vector2 pixel, Vector2 center, float centerRadius, float outerRadius, float angle1, float angle2)
    {
        //check if it's not in the middle, within range of the outer,and within the proper angle
        return !ShiftUIGen.CircleCheck(pixel, center, centerRadius)&& ShiftUIGen.CircleCheck(pixel, center,outerRadius) && ShiftUIGen.AngleCheck(pixel, center, angle1, angle2);
    }


}