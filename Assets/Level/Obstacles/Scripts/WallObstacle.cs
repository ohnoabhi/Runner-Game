using Challenges;
using UnityEngine;
using UnityEngine.Events;

public class WallObstacle : Obstacle
{
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject text;
    [SerializeField] private UnityEvent onCollideEvents;
    private Vector3[] positions;
    private Quaternion[] rotations;
    [SerializeField] private bool showText = true;

    private void Awake()
    {
        positions = new Vector3[parent.transform.childCount];
        rotations = new Quaternion[parent.transform.childCount];

        var i = 0;
        foreach (Transform child in parent.transform)
        {
            positions[i] = child.localPosition;
            rotations[i] = child.localRotation;
            i++;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        text.SetActive(showText);
    }


    public void ResetObstacle()
    {
        var i = 0;
        foreach (Transform child in parent.transform)
        {
            var piece = child.GetComponent<BrakablePiece>();
            if (piece)
                piece.Sleep();
            child.localPosition = positions[i];
            child.localRotation = rotations[i];
            i++;
        }
    }

    protected override void OnCollide(PlayerController playerController)
    {
        AudioManager.Play("WallBreak");
        CollectablesManager.Add(CollectableType.Cash, 1);
        ChallengeManager.Instance.UpdateChallenge(ChallengeType.DestroyWalls);
        text.SetActive(false);
        onCollideEvents?.Invoke();
    }
}