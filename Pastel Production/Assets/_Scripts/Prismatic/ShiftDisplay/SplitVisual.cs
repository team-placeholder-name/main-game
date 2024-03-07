using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplitVisual : MonoBehaviour
{

    [SerializeField]
    RawImage image;
    [SerializeField]
    RawImage mask;


    //factory
    public static SplitVisual CreateSplitUI(GameObject splitUIPrefab, Transform parent)
    {
        SplitVisual splitUI = Instantiate(splitUIPrefab, parent).GetComponent<SplitVisual>();
        return splitUI;
    }

    

    //TODO: Refactor the shared logic into a seperate function
    public void Generate(Color color, SelectionRegion region)
    {
        Texture2D generateMask = region.GenerateMask(0);
        mask.texture = generateMask;
        mask.color = Color.white;

        image.color = color;
    }
}