using UnityEngine;

public class TreeObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] int damage;


    public void Collide(Player player)
    {
        player.GetComponent<PlayerHealth>().TakeDamage(damage);
    }
}