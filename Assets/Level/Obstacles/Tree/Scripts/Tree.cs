using UnityEngine;

public class Tree : Obstacle
{
    [SerializeField] private GameObject logPrefab;
    [SerializeField] private GameObject trunkPrefab;
    [SerializeField] private GameObject tree;
    [SerializeField] private Transform parent;
    [SerializeField] private Forest forest;

    protected override void OnCollide(PlayerController playerController)
    {
        forest.PlaySound();
        var transformPosition = transform.position;
        var rigidBody = Instantiate(logPrefab, transformPosition,
                Quaternion.Euler(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f)), parent)
            .GetComponent<Rigidbody>();
        if (rigidBody)
        {
            rigidBody.AddExplosionForce(400, transformPosition, 2f);
        }

        // Instantiate(trunkPrefab, transformPosition, transform.rotation, parent);
        tree.SetActive(false);
    }

    public void ResetObstacle()
    {
        tree.SetActive(true);
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}