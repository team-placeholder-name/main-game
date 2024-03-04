using JetBrains.Annotations;
using Prismatic;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
[System.Serializable]
public class ShiftUIGen
{
    //The use of prefabs is problematic because they serve no utility. 
    //There is no reason we would want to edit this prefab so exposing this information opens up unessesary organizational overhead
    [SerializeField]
    GameObject SwapUIPrefab;
    [SerializeField]
    GameObject SplitUIPrefab;

    [SerializeField]
    Canvas targetCanvas;

    List<SwapUI> swapUIs;
    List<SplitUI> splitUIs;
    SwapUI centerDisplay;

    public void DisplayUI(PrismaticEntity controlledEntity, ReadOnlyCollection<PrismaticEntity> prismaticEntitys, List<GameObject> entityModels)
    {
        float centerRadius = Screen.height / 4.5f;
        float outerRadius = Screen.height / 3.5f;
        //Split is the color slices around the center circle
        splitUIs = new List<SplitUI>();
        for(int i = 0; i< controlledEntity.HueMix.Colors.Count; i++)
        {
            splitUIs.Add(SplitUI.CreateSplitUI(SplitUIPrefab, targetCanvas.transform, controlledEntity.HueMix.Colors[i].color));
        }

        float cellSize = 360 / (splitUIs.Count);
        float startAngle = cellSize / 3;
        for (int i = 0; i < splitUIs.Count; i++)
        {
            splitUIs[i].GenerateCutoutSlice(startAngle + 0 + cellSize * i, startAngle + cellSize + cellSize * i, centerRadius, outerRadius);
        }

        //Swap is the cameras around the splits
        swapUIs = new List<SwapUI>();
        for (int i = 0; i < prismaticEntitys.Count; i++)
        {
            if (prismaticEntitys[i] != controlledEntity)
            {
                if (entityModels[i].activeInHierarchy)
                {
                    swapUIs.Add(SwapUI.CreateSwapUI(SwapUIPrefab, targetCanvas.transform, entityModels[i].GetComponentInChildren<Camera>()));
                }
            }
            else
            {
                centerDisplay = SwapUI.CreateSwapUI(SwapUIPrefab, targetCanvas.transform, entityModels[i].GetComponentInChildren<Camera>());
            }
        }


        cellSize = 360 / (swapUIs.Count);
        startAngle = cellSize / 2;
        centerDisplay.GenerateCircleCutout(centerRadius);
        for (int i = 0; i < swapUIs.Count; i++)
        {
            swapUIs[i].GenerateCutoutSlice(startAngle + 0 + cellSize * i, startAngle + cellSize + cellSize * i, outerRadius);
        }
    }


    public static bool CircleCheck(Vector2 point, Vector2 center, float radius)
    { 
        return Vector2.Distance(point, center) < radius;
    }
    public static bool AngleCheck(Vector2 point, Vector2 center, float angleStart, float angleEnd)
    {
        Vector2 dir = (point - center).normalized;
        Vector2  start = new Vector2(Mathf.Cos(Mathf.Deg2Rad*angleStart), Mathf.Sin(Mathf.Deg2Rad*angleStart));
        Vector2 end = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angleEnd), Mathf.Sin(Mathf.Deg2Rad * angleEnd));
        float angleDif = (Vector2.Angle(dir, start) + Vector2.Angle(dir, end)) - Vector2.Angle(start, end);
        return angleDif < 0.1f&& angleDif>-0.1f;
    }
    public static Vector2 GetCenterOfSlice(Vector2 center,float angleStart, float angleEnd, float fraction)
    {
        Vector2 start = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angleStart), Mathf.Sin(Mathf.Deg2Rad * angleStart));
        Vector2 end = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angleEnd), Mathf.Sin(Mathf.Deg2Rad * angleEnd));
        Vector2 edge = Vector2.Scale((end + start).normalized, new Vector2(Screen.width, Screen.height));
        return edge/fraction+center;
    }

}
