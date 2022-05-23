using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    Vector3 startPos, endPos;

    [SerializeField]
    float swipeDist = 0.02f;

    [SerializeField]
    CreatureManager creatureManager;

    int currentPos;

    [SerializeField]
    float Xoffset;

    [SerializeField]
    float Zoffset;

    Vector3 offset;


    private void Start()
    {
        currentPos = 0;

        
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
                    currentPos++;
                    MoveCam(currentPos);
                    //Debug.Log("@@@@@@@@@@@@");
                }

                else if (startPos.x <= endPos.x)
                {
                    currentPos--;
                    MoveCam(currentPos);
                    //Debug.Log("@@@@@@@@@@@@");
                }
            }
        }

        
    }

    private void MoveCam(int currentPos)
    {
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

        transform.position = creatureManager.creatureItems[currentPos].cameraOffset;
       // Debug.Log("%%%%%%%%%%%%");
    }
}
