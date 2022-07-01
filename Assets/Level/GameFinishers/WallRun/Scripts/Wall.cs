using TMPro;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private TextMeshPro lockText;
    [SerializeField] private TextMeshPro text;
    [SerializeField] private WallObstacle obstacle;

    [SerializeField] private ParticleSystem[] particles;

    public void Init(bool locked, int multiplier, int unlockLevel, int damage)
    {
        obstacle.damage = damage;
        lockIcon.SetActive(locked);
        text.gameObject.SetActive(!locked);
        text.text = "X" + multiplier;
        lockText.text = "Lvl " + unlockLevel;
    }

    public void PlayParticles()
    {
        AudioManager.Play("ConfettiBlast");
        foreach (var particle in particles)
        {
            particle.Play();
        }
    }
}