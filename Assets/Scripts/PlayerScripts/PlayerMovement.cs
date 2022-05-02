using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5;

    [SerializeField] float turningSpeed = 5f;

    private Rigidbody playerRb;
    private Touch touch;

    public float MaxX = 4;

    // private void Start()
    // {
    //     playerRb = GetComponent<Rigidbody>();
    // }

    // private void FixedUpdate()
    // {
    //     Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
    //     playerRb.MovePosition(playerRb.position + forwardMove);
    // }

    void Update()
    {
        if (GameManager.instance.currentState != GameManager.GameStates.Playing) return;

        var movement = transform.position;
#if UNITY_EDITOR
        movement += Slide(Input.GetAxisRaw("Horizontal") * turningSpeed);
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                if (transform.position.x < 4 && transform.position.x > - 4)
                {
                   movement += Slide(-(touch.deltaPosition.x) * turningSpeed);
                }
            }
        }
#endif

        movement += transform.forward * speed * Time.deltaTime;
        transform.position = movement;
    }

    private Vector3 Slide(float amount)
    {
        var position = new Vector3(
            Mathf.Clamp(amount, -MaxX, MaxX),
            0,
            0
        );
        return position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "FinishLine")
        {
            UIController.OnGameEnd?.Invoke();
        }
    }
}