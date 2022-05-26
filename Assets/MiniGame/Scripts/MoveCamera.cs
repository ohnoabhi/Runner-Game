using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public static MoveCamera instance;

    Vector3 startPos, endPos;

    [SerializeField]
    float swipeDist = 0.02f;

    [SerializeField]
    CreatureManager creatureManager;

    int currentPos;

    public Vector3 offset;

    [SerializeField]
    float cameraSpeed = 10;

    [SerializeField]
    bool isMoving = false;

    Vector3 finalPos;

    private void Start()
    {
        instance = this;

        getCreatureManager();

        currentPos = 0;

        foreach(var creature in creatureManager.creatureItems)
        {
            if(creature.isUnlocked == 1)
            {
                currentPos++;
            }
            else
            {
                break;
            }
        }
        if (currentPos > creatureManager.creatureItems.Length)
        {
            MapManager.instance.OnMapComplete();
        }
        else
        {
            MoveCam(currentPos);
        }

    }
    
    public void getCreatureManager()
    {
        creatureManager = GameObject.FindObjectOfType<CreatureManager>();
        creatureManager.RefreshStats();
    }

    public void GetOffset()
    {
        offset = creatureManager.creatureItems[0].gameObject.transform.position - transform.position;
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
                    if (currentPos < creatureManager.creatureItems.Length - 1)
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
       

        if (currentPos > creatureManager.creatureItems.Length - 1)
        {
            currentPos = creatureManager.creatureItems.Length - 1;
            return;
        }

        Debug.Log(currentPos);

        //transform.position = creatureManager.creatureItems[currentPos].cameraOffset;

        finalPos = creatureManager.creatureItems[currentPos].transform.position + offset;
        isMoving = true;

        //transform.position = Vector3.MoveTowards(transform.position, creatureManager.creatureItems[currentPos].cameraOffset, cameraSpeed * Time.deltaTime);
       
    }


    
}
