using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class StepCreator : MonoBehaviour
{
    [SerializeField] private Step stepPrefab;
    [SerializeField] private int stepCount;
    [SerializeField] private Vector3 stepOffset;
    public Step[] Steps { get; private set; }

    private void Start()
    {
        CreateSteps();
    }

    [Button]
    private void CreateSteps()
    {
        foreach (Transform child in transform)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }

        var offset = stepOffset;
        offset.y = 0;
        offset.z *= 0f;
        Steps = new Step[stepCount];
        for (var i = 0; i < stepCount; i++)
        {
            var instance = Instantiate(stepPrefab, transform);
            instance.transform.localPosition = offset;

            instance.GetComponentInChildren<TextMeshPro>().text = "X" + (i + 1);
            instance.SetColor();
            Steps[i] = instance;

            offset += stepOffset;
        }
    }
}