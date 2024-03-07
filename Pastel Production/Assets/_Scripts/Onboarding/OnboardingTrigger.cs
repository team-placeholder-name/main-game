using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingTrigger : MonoBehaviour
{
    public OnboardingManager manager;
    private bool _ReadyToTrigger;

    private void Awake()
    {
        _ReadyToTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        if (_ReadyToTrigger && other.gameObject.CompareTag("Player"))
        {
            manager.ShowNextMessage();
            _ReadyToTrigger = false;
        }
    }
}
