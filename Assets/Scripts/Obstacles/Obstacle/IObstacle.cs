using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObstacle
{
    public void Collide(Player player);
}
