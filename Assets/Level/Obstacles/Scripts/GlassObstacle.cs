using Challenges;
using UnityEngine;

public class GlassObstacle : Obstacle
{
    [SerializeField] private GameObject unBroken;
    [SerializeField] private GameObject broken;

    private Vector3[] positions;
    private Quaternion[] rotations;

    private void Awake()
    {
        unBroken.SetActive(true);
        broken.SetActive(false);
        positions = new Vector3[broken.transform.childCount];
        rotations = new Quaternion[broken.transform.childCount];

        var i = 0;
        foreach (Transform child in broken.transform)
        {
            positions[i] = child.localPosition;
            rotations[i] = child.localRotation;
            i++;
        }
    }

    protected override void OnCollide(PlayerController playerController)
    {
        AudioManager.Play("GlassBreak");
        CollectablesManager.Add(CollectableType.Cash, 1);
        ChallengeManager.Instance.UpdateChallenge(ChallengeType.DestroyGlass);
        unBroken.SetActive(false);
        broken.SetActive(true);
    }

    public void ResetObstacle()
    {
        var i = 0;
        foreach (Transform child in broken.transform)
        {
            var piece = child.GetComponent<BrakablePiece>();
            if (piece)
                piece.Sleep();
            child.localPosition = positions[i];
            child.localRotation = rotations[i];
            i++;
        }

        unBroken.SetActive(true);
        broken.SetActive(false);
    }
}