using Prismatic;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    private PrismaticEntitySimulation dataSource;

    private SimulationData dataClone;


    public void Save()
    {
        dataClone = dataSource.SimulationData.Clone();
    }

    public void LoadCheckpoint()
    {
        if (dataClone != null)
        {
            Debug.Log(dataClone.entities[0].Position);
            dataSource.ReplaceData(dataClone.Clone());
        }
    }

    public void SetDataSource(PrismaticEntitySimulation source)
    {
        dataSource = source;
    }
}
