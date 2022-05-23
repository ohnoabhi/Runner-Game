using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private PlayerCharacterManager playerCharacterManager;
    private Touch touch;

    private void Start()
    {
        player = GetComponent<Player>();
        playerCharacterManager = GetComponent<PlayerCharacterManager>();
    }

    private void Update()
    {
        if (player.State != Player.PlayerState.Running) return;

        var movement = transform.position;
        var slide = Vector3.zero;
#if UNITY_EDITOR
        slide += Slide(Input.GetAxisRaw("Horizontal") * player.SlideSpeed);
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                if (transform.position.x < 4 && transform.position.x > - 4)
                {
                   slide += Slide(-(touch.deltaPosition.x) * playerStats.turningSpeed);
                }
            }
        }
#endif

        movement += Vector3.forward * player.Speed * Time.deltaTime;
        movement += slide;
        movement.x = Mathf.Clamp(movement.x, -player.XMovementClamp, player.XMovementClamp);

        if (playerCharacterManager.Character)
            playerCharacterManager.Character.Animator.SetBool("Running", true);
        transform.position = movement;
        var quaternion = Quaternion.Euler(0, slide.x != 0 ? (slide.x > 0 ? 20 : -20) : 0, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            quaternion, 150 * Time.deltaTime);
    }

    public async void Fall()
    {
        if (player.State != Player.PlayerState.Running) return;

        player.State = Player.PlayerState.Fall;

        var fallPos = new Vector3(transform.position.x, -15, transform.position.z);
        while (transform.position != fallPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, fallPos, 5 * Time.deltaTime);
            await Task.Yield();
        }

        GameManager.Instance.OnFinish(false);
    }

    private Vector3 Slide(float amount)
    {
        var position = new Vector3(
            amount,
            0,
            0
        );
        return position;
    }
}