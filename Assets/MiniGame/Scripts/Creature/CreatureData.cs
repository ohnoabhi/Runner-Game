using UnityEngine;

public class CreatureData : MonoBehaviour
{
    public static CreatureData Instance;
    public GameObject[] Creatures;
    public int RandomIndex => Random.Range(0, Creatures.Length);

    private void Awake()
    {
        Instance = this;
    }

    public GameObject ReturnCreature(int index)
    {
        if (index >= 0 && index < Creatures.Length) return Creatures[index];
        return null;
    }
}