using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
        Vector3 startPos, endPos;

    [SerializeField]
    float swipeDist = 0.02f;


    int currentPos;

    public Vector3 offset;

    [SerializeField]
    float cameraSpeed = 10;

    [SerializeField]
    bool isMoving = false;

    Vector3 finalPos;

    MapManager mapManager;

    private void Start()
    {
        currentPos = 0;
        mapManager = MapManager.instance;

        foreach(var creature in mapManager.GetCurrentMap().Creatures)
        {
            if(creature.IsUnlocked)
            {
                currentPos++;
            }
            else
            {
                break;
            }
        }
           
        MoveCam(currentPos);

    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;

            if ((startPos - endPos).magnitude >= swipeDist)
            {
                if (startPos.x >= endPos.x)
                {
                    if (currentPos < mapManager.GetCurrentMap().Creatures.Length - 1)
                    {
                        currentPos++;
                        MoveCam(currentPos);
                    }
                    //Debug.Log("@@@@@@@@@@@@");
                }

                else if (startPos.x <= endPos.x)
                {
                    if (currentPos > 0)
                    {
                        currentPos--;
                        MoveCam(currentPos);
                        //Debug.Log("@@@@@@@@@@@@");
                    }
                }
            }
        }

        if(isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, finalPos, cameraSpeed * Time.deltaTime);

            if (transform.position == finalPos)
            {
                isMoving = false;
            }
        }

        
    }

    private void MoveCam(int currentPos)
    {
        if (isMoving) return;

        //Debug.Log("############");
        if (currentPos < 0)
        {
            currentPos = 0;
            return;
        }
       

        if (currentPos > mapManager.GetCurrentMap().Creatures.Length - 1)
        {
            currentPos = mapManager.GetCurrentMap().Creatures.Length - 1;
            return;
        }


        //transform.position = creatureManager.creatureItems[currentPos].cameraOffset;

        finalPos = mapManager.GetCurrentMap().Creatures[currentPos].transform.position + offset;
        isMoving = true;

        //transform.position = Vector3.MoveTowards(transform.position, creatureManager.creatureItems[currentPos].cameraOffset, cameraSpeed * Time.deltaTime);
       
    }


    
}
