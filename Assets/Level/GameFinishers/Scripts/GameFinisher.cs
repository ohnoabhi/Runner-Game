using UnityEngine;

public abstract class GameFinisher : MonoBehaviour
{
    protected PlayerController PlayerController;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private bool isFollowCam = false;

    public void StartFinisher(PlayerController playerController)
    {
        this.PlayerController = playerController;
        playerController.transform.forward = Vector3.forward;

        Finish();
    }

    protected virtual void Finish()
    {
        if (!PlayerController) return;
        GameManager.Instance.SetCamera(cameraPosition, isFollowCam);
    }
}