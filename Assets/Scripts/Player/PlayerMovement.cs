using System;
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

    void Update()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameStates.Playing) return;

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