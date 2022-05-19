using UnityEngine;

public class NormalObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] private int damage;

    public void Collide(Player player)
    {
        player.GetComponent<PlayerHealth>().TakeDamage(damage);
    }
}