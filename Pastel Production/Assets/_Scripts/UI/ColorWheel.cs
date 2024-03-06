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

    public Color ChosenColor { get; private set; } = Color.clear;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        ChosenColor = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        PrismaticEntity previousEntity = currentEntity;
        currentEntity = simulationTarget.SimulationData.CurrentEntity;

        if (currentEntity != previousEntity || previousEntity == null)
        {
            ReformColorWheel();
        }
    }

    public void ReformColorWheel()
    {
        // Destroy all color selectors on the colorwheel
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        List<Color> currentColors = currentEntity.HueMix.GetColors();
        int numColors = currentColors.Count;

        for(int i = 0; i < numColors; i++)
        {
            Color color = currentColors[i];
            Button colorSelect = Instantiate(colorSelectPrefab, transform);
            colorSelect.GetComponent<Image>().color = color;

            colorSelect.onClick.AddListener(() => ChosenColor = color);

            float angle = 2 * Mathf.PI * i / (float)numColors;
            float radius = Camera.main.pixelHeight / 5;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            colorSelect.transform.position = new Vector3(x, y, 0) + transform.position;
        }
    }
}
