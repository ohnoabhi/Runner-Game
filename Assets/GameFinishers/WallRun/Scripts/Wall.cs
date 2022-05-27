using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject lockIcon;

    public void SetLock(bool locked)
    {
        lockIcon.SetActive(locked);
    }
}