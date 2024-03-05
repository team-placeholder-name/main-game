using Prismatic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

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

    int splitCount = 10;
    int swapCount = 10;

    //Find a way to make it easier to iterate over all these values.
    //Use these parameters to find what section of teh screen has been selected

    public void SetUp()
    {
        //Split is the color slices around the center circle
        splitUIs = new List<SplitUI>();
        for (int i = 0; i < splitCount; i++)
        {
            SplitUI ui = SplitUI.CreateSplitUI(SplitUIPrefab, targetCanvas.transform);
            ui.gameObject.SetActive(false);
            splitUIs.Add(ui);
        }
        //Swap is the cameras around the splits
        swapUIs = new List<SwapUI>();
        for (int i = 0; i < swapCount; i++)
        {
            SwapUI ui = SwapUI.CreateSwapUI(SwapUIPrefab, targetCanvas.transform);
            swapUIs.Add(ui);
            ui.gameObject.SetActive(false);
        }
        centerDisplay = SwapUI.CreateSwapUI(SwapUIPrefab, targetCanvas.transform);
        centerDisplay.gameObject.SetActive(false);
    }

    //TODO: Store all selection regions to a data structure for reference by player input
    public void DisplayUI(PrismaticEntity controlledEntity, ReadOnlyCollection<PrismaticEntity> prismaticEntitys, List<GameObject> entityModels)
    {
        int innerRadius;
        float outerRadius;
        float splitCellSize;
        float splitStartAngle;
        float swapCellSize;
        float swapStartAngle;


        GameObject controlledModel = null;
        List<GameObject> swapModels = new();
        //Spliting
        innerRadius = (int)(Screen.height / 4.5f);
        outerRadius = Screen.height / 3.5f;
        splitCellSize = 360 / (controlledEntity.HueMix.Colors.Count);
        splitStartAngle = splitCellSize / 3;
       
        int uiIndex;
        for (uiIndex = 0; uiIndex < controlledEntity.HueMix.Colors.Count; uiIndex++)
        {
            SelectionRegion region = new SelectionRegion(new Vector2(Screen.width, Screen.height) / 2, innerRadius, outerRadius, splitStartAngle + 0 + splitCellSize * uiIndex, splitCellSize);
            splitUIs[uiIndex].GenerateCutoutSlice(controlledEntity.HueMix.Colors[uiIndex].color, region);
            splitUIs[uiIndex].gameObject.SetActive(true); 
        }
        for(; uiIndex < splitUIs.Count; uiIndex++)
        {
            splitUIs[uiIndex].gameObject.SetActive(false);
        }


        swapCellSize = 360 / (prismaticEntitys.Count-1 );
        swapStartAngle = swapCellSize / 2;


        for (int i = 0; i < prismaticEntitys.Count; i++)
        {
            if (prismaticEntitys[i] == controlledEntity)
            {
                controlledModel = entityModels[i];
            }
            else
            {
                swapModels.Add(entityModels[i]);
            }
        }
        SelectionRegion circleRegion = new SelectionRegion(new Vector2(Screen.width, Screen.height) / 2, 0, innerRadius, 0, 360);
        //centerDisplay.GenerateCircleCutout(controlledModel.GetComponentInChildren<Camera>(), centerRadius);
        centerDisplay.GenerateSwapRegion(controlledModel.GetComponentInChildren<Camera>(), circleRegion);
        centerDisplay.gameObject.SetActive(true);
        for(uiIndex = 0;uiIndex < swapModels.Count;uiIndex++)
        {
            SelectionRegion region = new SelectionRegion(new Vector2(Screen.width,Screen.height)/2,outerRadius,outerRadius*2,swapStartAngle + 0 + swapCellSize * uiIndex,swapCellSize);
            swapUIs[uiIndex].GenerateSwapRegion(swapModels[uiIndex].GetComponentInChildren<Camera>(), region);
            swapUIs[uiIndex].gameObject.SetActive(true);
        }
        for(;uiIndex < swapUIs.Count;uiIndex++)//deactivate all of the panels that arn't being used
        {
            splitUIs[uiIndex].gameObject.SetActive(false);
        }
    }
    

    public Vector2 CircleSize(float radius)
    {
        return new Vector2(radius*2, radius*2);
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

    public static bool OuterSliceCheck(Vector2 pixel, Vector2 center, float centerRadius, float angle1, float angle2)
    {
        return !ShiftUIGen.CircleCheck(pixel, center, centerRadius) && ShiftUIGen.AngleCheck(pixel, center, angle1, angle2);
    }
    public bool InnerSliceCheck(Vector2 pixel, Vector2 center, float centerRadius, float outerRadius, float angle1, float angle2)
    {
        //check if it's not in the middle, within range of the outer,and within the proper angle
        return !ShiftUIGen.CircleCheck(pixel, center, centerRadius) && ShiftUIGen.CircleCheck(pixel, center, outerRadius) && ShiftUIGen.AngleCheck(pixel, center, angle1, angle2);
    }

}

public class SelectionRegion
{
    Vector2 screenCenter;
    float innerRadius;
    float outerRadius;
    float angleOffset;
    float angle;

    Vector2 regionCenter;
    public SelectionRegion(Vector2 center, float innerRadius, float outerRadius, float angleOffset, float angle) 
    { 
        this.screenCenter= center;
        this.innerRadius= innerRadius;
        this.outerRadius= outerRadius;
        this.angleOffset= angleOffset;
        if(angle == 360) 
        {
            this.angle = 180;
        }
        else if (angle == 180)
        {
            this.angle = 179;
        }
        else
        {
            this.angle = angle;
        }
        this.regionCenter = CalculateCenter();
    }

    
    public bool CheckRegion(Vector2 point)
    {

        return
            !CircleCheck(point, screenCenter, innerRadius)
            && CircleCheck(point, screenCenter, outerRadius)
            && AngleCheck(point,screenCenter, angleOffset, angle);
    }
    public bool CheckBoarderedRegion(Vector2 point, float boarder)
    {
        Vector2 start = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angleOffset), Mathf.Sin(Mathf.Deg2Rad * angleOffset));
        Vector2 end = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (angleOffset + angle)), Mathf.Sin(Mathf.Deg2Rad * (angleOffset + angle)));
        Vector2 centerVector = (end + start).normalized;

        Vector2 boarderOffset = centerVector * boarder;


        return
            !CircleCheck(point - boarderOffset, screenCenter, innerRadius)
            && CircleCheck(point, screenCenter, outerRadius - boarder)
            && AngleCheck(point - boarderOffset, screenCenter, angleOffset, angle);
    }
    public Vector2 GetCenter()
    {
        return regionCenter;
    }

    private Vector2 CalculateCenter()
    {

        Vector2 start = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angleOffset), Mathf.Sin(Mathf.Deg2Rad * angleOffset));
        Vector2 end = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (angleOffset+angle)), Mathf.Sin(Mathf.Deg2Rad * (angleOffset+angle)));
        Vector2 centerVector = (end + start).normalized;
        float max = Vector2.Scale(centerVector, new Vector2(Screen.width, Screen.height)).magnitude;
        float outerDistance = Mathf.Min(outerRadius, max);

        return centerVector * ((outerDistance+innerRadius)/2) + screenCenter;
    }

    private bool CircleCheck(Vector2 point, Vector2 center, float radius)
    {
        return Vector2.Distance(point, center) < radius;
    }
    private bool AngleCheck(Vector2 point, Vector2 center, float angleOffset, float angle)
    {
        Vector2 dir = (point - center).normalized;
        Vector2 start = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angleOffset), Mathf.Sin(Mathf.Deg2Rad * angleOffset));
        Vector2 end = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (angleOffset+angle)), Mathf.Sin(Mathf.Deg2Rad * (angleOffset+angle)));
        float angleDif = (Vector2.Angle(dir, start) + Vector2.Angle(dir, end)) - Vector2.Angle(start, end);
        return angleDif < 0.1f && angleDif > -0.1f;
    }




}

