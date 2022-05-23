using UnityEngine;

public class Spike : MonoBehaviour, IObstacle
{
    public void Collide(Player player)
    {
        player.GetComponent<PlayerHealth>().TakeDamage(999999);
    }
}
