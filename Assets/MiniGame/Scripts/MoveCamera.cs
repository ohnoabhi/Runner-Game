using System;
using System.Threading.Tasks;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] float swipeDist = 0.02f;

    [SerializeField] float cameraSpeed = 10;
    private int currentIndex;

    public Vector3 offset;


    private bool isMoving;
    private Vector3 mouseStartPosition;

    private void Awake()
    {
        MapManager.OnMapLoaded += OnMapLoaded;
    }

    private void Start()
    {
        currentIndex = 0;
    }

    private void OnDestroy()
    {
        MapManager.OnMapLoaded -= OnMapLoaded;
    }

    private void OnMapLoaded(int i)
    {
        currentIndex = i;
        Move();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseStartPosition = Input.mousePosition;
        }

        if (!Input.GetMouseButtonUp(0)) return;
        var endPos = Input.mousePosition;

        if (!((mouseStartPosition - endPos).magnitude >= swipeDist)) return;
        if (mouseStartPosition.x >= endPos.x)
        {
            if (currentIndex >= MapManager.instance.CurrentMap.Creatures.Length - 1) return;
            currentIndex++;
            Move();
        }
        else if (mouseStartPosition.x <= endPos.x)
        {
            if (currentIndex <= 0) return;
            currentIndex--;
            Move();
        }
    }

    private async void Move()
    {
        if (isMoving) return;

        if (currentIndex < 0)
        {
            currentIndex = 0;
            return;
        }


        var currentMap = MapManager.instance.CurrentMap;
        if (currentIndex > currentMap.Creatures.Length - 1)
        {
            currentIndex = currentMap.Creatures.Length - 1;
            return;
        }


        isMoving = true;

        var targetPosition = currentMap.Creatures[currentIndex].transform.position + offset;

        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, cameraSpeed * Time.deltaTime);
            await Task.Yield();
        }

        isMoving = false;
    }
}