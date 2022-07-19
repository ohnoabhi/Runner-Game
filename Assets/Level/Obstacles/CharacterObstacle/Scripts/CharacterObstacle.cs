using System.Threading.Tasks;
using UnityEngine;

public class CharacterObstacle : Obstacle
{
    [SerializeField] private Transform visual;
    private Character character;


    public void Init(int id)
    {
        if(!Application.isPlaying) return;
        
        foreach (Transform child in visual)
        {
            Destroy(child.gameObject);
        }

        character = Instantiate(Shop.GetCharacter(id), visual);
        character.Animator.SetTrigger("Idle");
        character.UI.SetPlayerHealth(damage);

        var requiredForIncrement = damage / (GameManager.Instance.HealthRequiredForIncrement - 1);
        var startSize = Vector3.one * GameManager.Instance.PlayerStartSize;
        var requiredSize =
            startSize + (startSize * (requiredForIncrement * GameManager.Instance.HealthIncrementPercentage));

        if (requiredSize.x > GameManager.Instance.PlayerMaxSize)
            requiredSize = Vector3.one * GameManager.Instance.PlayerMaxSize;
        visual.localScale = requiredSize;
        character.UI.SetTextColor(Color.red);
        character.UITransform.localScale = Vector3.one * (1 / (requiredSize.x * 0.5f));
        character.transform.rotation = Quaternion.Euler(0, 180, 0);
        character.UITransform.rotation = Quaternion.Euler(0, 0, 0);
        character.Animator.SetFloat("Speed", 1f);
    }

    protected override async void OnCollide(PlayerController playerController, Vector3 collisionPoint)
    {
        AudioManager.Play("CharacterObstacle");
        if (playerController.State != PlayerController.PlayerState.Dead)
        {
            await Task.Delay(10);
            character.Animator.SetTrigger("Dead");
            // if (character.Animator.runtimeAnimatorController.animationClips.Length > 4)
            // {
            //     var clip = character.Animator.runtimeAnimatorController.animationClips[4];
            //     if (clip)
            //     {
            //         character.Animator.SetFloat("Speed", clip.length / 1.5f);
            //     }
            // }
            character.Animator.SetFloat("Speed", 3f);
        }
        else
        {
            character.Animator.SetTrigger("Attack");
            await Task.Delay(500);
            character.Animator.SetTrigger("Idle");
        }
    }
}