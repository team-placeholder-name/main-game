using Prismatic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[System.Serializable]
public class ShiftUIGen
{
    //TODO: These should be generated in the Simulation and referenced by the presentation
    public List<(SelectionRegion, Color)> hues;
    public List<(SelectionRegion, PrismaticEntity)> entities;


    //The use of prefabs is problematic because they serve no utility. 
    //There is no reason we would want to edit this prefab so exposing this information opens up unessesary organizational overhead
    [SerializeField]
    GameObject SwapUIPrefab;
    [SerializeField]
    GameObject SplitUIPrefab;

    [SerializeField]
    GameObject canvasPrefab;
   // [SerializeField]
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
        
        targetCanvas = GameObject.Instantiate(canvasPrefab).GetComponent<Canvas>();
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

    public void HideUI()
    {
        targetCanvas.gameObject.SetActive(false);
    }
    //TODO: Store all selection regions to a data structure for reference by player input
    public void DisplayUI(PrismaticEntity controlledEntity, ReadOnlyCollection<PrismaticEntity> prismaticEntitys, List<GameObject> entityModels)
    {
        hues = new();
        entities = new();
        List<GameObject> swapModels = new();
        targetCanvas.gameObject.SetActive(true);
        int innerRadius;
        float outerRadius;
        float splitCellSize;
        float splitStartAngle;
        float swapCellSize;
        float swapStartAngle;
        

        
        Vector2 screenCenter = new Vector2(Screen.width, Screen.height) / 2;

        //Spliting
        innerRadius = (int)(Screen.height / 4.5f);
        outerRadius = Screen.height / 3.5f;
        splitCellSize = 360 / (controlledEntity.HueMix.Colors.Count);
        splitStartAngle = splitCellSize / 3;
       
        int uiIndex;
        for (uiIndex = 0; uiIndex < controlledEntity.HueMix.Colors.Count; uiIndex++)
        {
            SelectionRegion region = new SelectionRegion(screenCenter, innerRadius, outerRadius, splitStartAngle + splitCellSize * uiIndex, splitCellSize);
            Color color = controlledEntity.HueMix.Colors[uiIndex].color;
            
            hues.Add((region, color));

            splitUIs[uiIndex].GenerateSplitRegion(color, region);
            splitUIs[uiIndex].gameObject.SetActive(true); 
        }
        for(; uiIndex < splitUIs.Count; uiIndex++)
        {
            splitUIs[uiIndex].gameObject.SetActive(false);
        }

        //swapping
        swapCellSize = 360 / (prismaticEntitys.Count-1 );
        swapStartAngle = swapCellSize / 2;

        uiIndex = 0;
        for (int i = 0; i < prismaticEntitys.Count; i++)
        {
            if (prismaticEntitys[i] == controlledEntity)
            {
                SelectionRegion circleRegion = new SelectionRegion(screenCenter, 0, innerRadius, 0, 360);
                entities.Add((circleRegion, controlledEntity));
                centerDisplay.GenerateSwapRegion(entityModels[i].GetComponentInChildren<Camera>(), circleRegion, 0);
                centerDisplay.gameObject.SetActive(true);
            }
            else
            {
                SelectionRegion region = new SelectionRegion(screenCenter, outerRadius, outerRadius * 2, swapStartAngle + 0 + swapCellSize * uiIndex, swapCellSize);
                swapModels.Add(entityModels[i]);
                entities.Add((region, prismaticEntitys[i]));
                swapUIs[uiIndex].GenerateSwapRegion(swapModels[uiIndex].GetComponentInChildren<Camera>(), region, 20);
                swapUIs[uiIndex].gameObject.SetActive(true);
                uiIndex++;
            }
        }
        for(;uiIndex < swapUIs.Count;uiIndex++)//deactivate all of the panels that arn't being used
        {
            swapUIs[uiIndex].gameObject.SetActive(false);
        }
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

        this.angle = angle;
        this.regionCenter = CalculateCenter();
    }

    
    public bool CheckRegion(Vector2 point)
    {

        return
            !CircleCheck(point, screenCenter, innerRadius)
            && CircleCheck(point, screenCenter, outerRadius)
            && (AngleCheck(point, screenCenter, angleOffset, angle / 2)
            || AngleCheck(point, screenCenter, angleOffset +(angle / 2), angle/2));
    }
    public bool CheckBoarderedRegion(Vector2 point, int boarder)
    {
        Vector2 start = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angleOffset), Mathf.Sin(Mathf.Deg2Rad * angleOffset));
        Vector2 end = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (angleOffset + angle)), Mathf.Sin(Mathf.Deg2Rad * (angleOffset + angle)));
        Vector2 centerVector = (end + start).normalized;

        Vector2 boarderOffset = centerVector * boarder;
        //float innerBoarder = boarder;
        //if(innerRadius == 0) { innerBoarder = 0; }
        return
            !CircleCheck(point, screenCenter, innerRadius+ boarder)
            && CircleCheck(point, screenCenter, outerRadius - boarder)
            && (AngleCheck(point-boarderOffset, screenCenter, angleOffset, angle / 2)
            || AngleCheck(point - boarderOffset, screenCenter, angleOffset + (angle / 2), angle / 2));
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

