using UnityEngine;

public class ButtonObstacleDetector : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        var button = other.GetComponent<TriggerButton>();
        if (button) button.Trigger();
    }
}