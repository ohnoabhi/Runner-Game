using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRb;
    private Touch touch;

    Player player;

    private void Start()
    {
        player = Player.Instance;
    }

    void Update()
    {
        if (GameManager.instance.currentState != GameManager.GameStates.Playing) return;

        var movement = transform.position;
#if UNITY_EDITOR
        movement += Slide(Input.GetAxisRaw("Horizontal") * player.SlideSpeed);
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                if (transform.position.x < 4 && transform.position.x > - 4)
                {
                   movement += Slide(-(touch.deltaPosition.x) * playerStats.turningSpeed);
                }
            }
        }
#endif

        movement += transform.forward * player.Speed * Time.deltaTime;
        movement.x = Mathf.Clamp(movement.x, -player.XMovementClamp, player.XMovementClamp);
        transform.position = movement;
    }

    private Vector3 Slide(float amount)
    {
        var position = new Vector3(
            amount,
            0,
            0
        );
        Debug.Log(position.x);
        return position;
    }
}