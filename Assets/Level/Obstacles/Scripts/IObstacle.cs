using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObstacle
{
    public void Collide(PlayerController playerController, Vector3 collisionpoint);
}