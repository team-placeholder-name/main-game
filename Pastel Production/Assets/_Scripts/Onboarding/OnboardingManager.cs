using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnboardingManager : MonoBehaviour
{
    public List<OnboardingMessage> onboardingMessages;
    public GameObject onboardingPanel;
    private int _currentMessage;

    private void Awake()
    {
        _currentMessage = 0;   
    }

    public void ShowNextMessage()
    {
        onboardingPanel.SetActive(true);
        TextMeshProUGUI onboardingPanelText = onboardingPanel.GetComponentInChildren<TextMeshProUGUI>();
        onboardingPanelText.text = onboardingMessages[_currentMessage].messageValue;
        _currentMessage++;
        StartCoroutine(MessageTimer());
    }

    public void HideMessage()
    {
        onboardingPanel.SetActive(false);
    }

    IEnumerator MessageTimer()
    {
        yield return new WaitForSeconds(10.0f);
        HideMessage();
    }
}
