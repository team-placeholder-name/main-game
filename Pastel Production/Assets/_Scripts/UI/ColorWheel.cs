using Prismatic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorWheel : MonoBehaviour
{
    [SerializeField]
    private Button colorSelectPrefab;
    [SerializeField]
    private PrismaticEntitySimulation simulationTarget;

    private PrismaticEntity currentEntity;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PrismaticEntity previousEntity = currentEntity;
        currentEntity = simulationTarget.SimulationData.Entities[simulationTarget.SimulationData.currentIndex];

        if (currentEntity != previousEntity || previousEntity == null)
        {
            ReformColorWheel();
        }
    }

    private void ReformColorWheel()
    {
        // Destroy all color selectors on the colorwheel
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        List<Color> currentColors = currentEntity.HueMix.Colors;
        int numColors = currentColors.Count;

        for(int i = 0; i < numColors; i++)
        {
            Color color = currentColors[i];
            Button colorSelect = Instantiate(colorSelectPrefab, transform);
            colorSelect.GetComponent<Image>().color = color;

            float angle = 2 * Mathf.PI * i / (float)numColors;
            float radius = 100;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            Debug.Log(angle + " " + radius + " -> " + x + " " + y);

            colorSelect.transform.position = new Vector3(x, y, 0) + transform.position;
        }
    }
}
