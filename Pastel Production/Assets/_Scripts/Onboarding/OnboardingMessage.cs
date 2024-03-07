using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/OnboardingMessage")]
public class OnboardingMessage : ScriptableObject
{
    [TextArea]
    public string messageValue;
}
