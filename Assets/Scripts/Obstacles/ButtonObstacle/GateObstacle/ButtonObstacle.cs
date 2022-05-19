using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObstacle : MonoBehaviour,IbuttonObstacle
{
    public GameObject greenButton;
    //public GameObject[] spikes;

    public void Collide(Player player)
    {
        player.GetComponent<PlayerHealth>().TakeDamage(999999);
    }

    public void OnGreenButtonClick()
    {
        GetComponent<Animator>().SetBool("Play",true);
    }

    
}
