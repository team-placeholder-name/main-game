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
    [SerializeField]
    GameObject SwapCamDisplayPrefab;
    [SerializeField]
    Canvas targetCanvas;
    [SerializeField]
    List<SwapCamDisplay> swapCamDisplays;
    SwapCamDisplay centerDisplay;

    public void DisplayUI(PrismaticEntity controlledEntity, ReadOnlyCollection<PrismaticEntity> prismaticEntitys, List<GameObject> entityModels)
    {
        swapCamDisplays = new List<SwapCamDisplay>();
        for (int i = 0; i < prismaticEntitys.Count; i++)
        {
            if (prismaticEntitys[i] != controlledEntity)
            {
                if (entityModels[i].activeInHierarchy)
                {
                    swapCamDisplays.Add(SwapCamDisplay.CreateSwapCamDisplay(SwapCamDisplayPrefab, targetCanvas.transform, entityModels[i].GetComponentInChildren<Camera>()));
                }
            }
            else
            {
                centerDisplay = SwapCamDisplay.CreateSwapCamDisplay(SwapCamDisplayPrefab, targetCanvas.transform, entityModels[i].GetComponentInChildren<Camera>());
            }
        }


        float cellSize = 360 / (swapCamDisplays.Count);
        float startAngle = cellSize / 2;
        centerDisplay.GenerateCircleCutout(Screen.height / 4.2f);
        for (int i = 0; i < swapCamDisplays.Count; i++)
        {
            swapCamDisplays[i].GenerateCutoutSlice(startAngle + 0 + cellSize * i, startAngle + cellSize + cellSize * i, Screen.height / 4);
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
