using Prismatic;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    private PrismaticEntitySimulation dataSource;

    private SimulationData dataClone;
    [SerializeField]
    private EntityPresentation presentationLayer;


    public void Save()
    {
        presentationLayer.DisplayCheckpointMessage("Checkpoint reached!");
        dataClone = dataSource.SimulationData.Clone();
    }

    public void LoadCheckpoint()
    {
        if (dataClone != null)
        {
            presentationLayer.DisplayCheckpointMessage("Loaded last checkpoint.");
            dataSource.ReplaceData(dataClone.Clone());
        }
    }

    public void SetDataSource(PrismaticEntitySimulation source)
    {
        dataSource = source;
    }
}
