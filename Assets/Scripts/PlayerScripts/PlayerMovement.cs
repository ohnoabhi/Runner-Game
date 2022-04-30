using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 5;

    [SerializeField]
    float turningSpeed = 5f;

    private Rigidbody playerRb;
    private Touch touch;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        playerRb.MovePosition(playerRb.position + forwardMove);
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                if (transform.position.x < 4 && transform.position.x > - 4)
                {
                    transform.position = new Vector3(
                        transform.position.x + -(touch.deltaPosition.x) * turningSpeed,
                        transform.position.y,
                        transform.position.z
                        );
                }
            }
        }
    }

}
