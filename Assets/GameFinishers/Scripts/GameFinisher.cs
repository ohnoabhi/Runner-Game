using UnityEngine;

public abstract class GameFinisher : MonoBehaviour
{
    protected Player player;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private bool isFollowCam = false;

    public void StartFinisher(Player player)
    {
        this.player = player;
        Finish();
    }

    protected virtual void Finish()
    {
        if (!player) return;
        GameManager.Instance.SetCamera(cameraPosition, isFollowCam);
    }
}