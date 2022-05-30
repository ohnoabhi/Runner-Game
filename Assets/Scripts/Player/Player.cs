using System;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Running,
        Fall,
        Dead,
        Finish
    }

    public float Speed;
    public float SlideSpeed;
    public float XMovementClamp;

    private PlayerState state;

    public PlayerState State
    {
        get => state;
        set
        {
            state = value;
            OnStateChange();
        }
    }

    private void OnStateChange()
    {
        UI.gameObject.SetActive(state == PlayerState.Running);
    }

    public PlayerUIController UI;

    public async void Die()
    {
        state = PlayerState.Dead;
        GetComponent<PlayerCharacterManager>().Character.Animator.SetTrigger("Dead");
        await Task.Delay(1600);
        GameManager.Instance.OnFinish(false);
    }
}