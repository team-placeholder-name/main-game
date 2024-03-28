using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointObject : MonoBehaviour
{
    private bool _ReadyToTrigger = true;
    public GameSaveManager manager;
    private void OnTriggerEnter(Collider other)
    {

        if (_ReadyToTrigger && other.gameObject.CompareTag("Player"))
        {
            manager.Save();
            _ReadyToTrigger = false;
        }
    }
}
