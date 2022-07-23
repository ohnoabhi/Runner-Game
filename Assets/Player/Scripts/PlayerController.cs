using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using static Utility.Input;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Running,
        Fall,
        Dead,
        Finish
    }

    [BoxGroup("Animation")] public Animator Animator;

    [BoxGroup("Movement")] [SerializeField]
    private float Speed;

    public int CoinCollected { get; private set; }

    public static int PlayerSpeed
    {
        get => PlayerPrefs.GetInt("PlayerSpeed", 12);
        set => PlayerPrefs.SetInt("PlayerSpeed", value);
    }

    public static int PlayerSlideSpeed
    {
        get => PlayerPrefs.GetInt("PlayerSlideSpeed", 15);
        set => PlayerPrefs.SetInt("PlayerSlideSpeed", value);
    }

    public static float PlayerSlideMoveSpeed
    {
        get => PlayerPrefs.GetFloat("PlayerSlideMoveSpeed", 0);
        set => PlayerPrefs.SetFloat("PlayerSlideMoveSpeed", value);
    }

    [BoxGroup("Movement")] [SerializeField]
    private float XMovementClamp;

    [BoxGroup("Movement")] [SerializeField]
    private Transform RotationTransform;

    [BoxGroup("Health")] [SerializeField] public int StartHealth = 20;

    [BoxGroup("UI")] public PlayerUIController UI;
    [BoxGroup("UI")] [SerializeField] private PlayerCashUpdateUI PlayerCashUpdateUI;
    [BoxGroup("UI")] [SerializeField] private Transform PlayerCashUpdateUITransform;

    [BoxGroup("Character Size")] [SerializeField]
    private Transform visual;

    [BoxGroup("Character Size")] [SerializeField]
    private Transform uiTransfrorm;

    [BoxGroup("Audio")] [SerializeField] private AudioSource walikingAudio;

    private Vector3 requiredUIScale;

    private new Transform transform;

    private PlayerState state;
    private Vector3 lastPosition;

    private PlayerMovement movement;


    public int Health { get; private set; }

    public PlayerState State
    {
        get => state;
        set
        {
            state = value;
            OnStateChange();
        }
    }

    public bool IsAlive;

    private void Start()
    {
        transform = gameObject.transform;
        GameManager.OnPlayerHealthChange += OnHealthChange;
        // movement = new RotationMovement(this, PlayerSpeed, PlayerSlideSpeed, PlayerSlideMoveSpeed, XMovementClamp,
        //     RotationTransform);
        movement = new TransformSmoothMovement(this, PlayerSpeed, PlayerSlideSpeed, XMovementClamp);
    }

    private void InitCharacter()
    {
        var character = Shop.GetCharacter();

        foreach (Transform child in visual)
        {
            Destroy(child.gameObject);
        }

        var characterInstance = Instantiate(character, visual);
        Animator = characterInstance.Animator;
        Animator.SetFloat("Speed", 1);

        UI = characterInstance.UI;
        uiTransfrorm = characterInstance.UITransform;
        requiredUIScale = uiTransfrorm.localScale;
        var startSize = GameManager.Instance.PlayerStartSize;
        visual.localScale = Vector3.one * startSize;
        uiTransfrorm.localScale = requiredUIScale * (1 / startSize);
    }

    private void OnDestroy()
    {
        GameManager.OnPlayerHealthChange -= OnHealthChange;
    }

    private void OnHealthChange(int health)
    {
        UI.SetPlayerHealth(health);
        var requiredForIncrement = Health <= 0
            ? 0
            : ((Health - (((StatsManager.Get(StatType.PlayerStat)) - 1) * 5)) /
                GameManager.Instance.HealthRequiredForIncrement - 1);
        var startSize = Vector3.one * GameManager.Instance.PlayerStartSize;
        var requiredSize =
            startSize + (startSize * (requiredForIncrement * GameManager.Instance.HealthIncrementPercentage));

        if (requiredSize.x > GameManager.Instance.PlayerMaxSize)
            requiredSize = Vector3.one * GameManager.Instance.PlayerMaxSize;

        visual.DOScale(requiredSize, 0.2f);
        uiTransfrorm.DOScale(requiredUIScale * (1 / requiredSize.x), 0.2f);
        // GameManager.Instance.UpdateCameraHeight((requiredForIncrement *
        //                                          GameManager.Instance.HealthIncrementPercentage));
    }

    public async void Init()
    {
        InitCharacter();
        await Task.Yield();
        Health = StartHealth + (((StatsManager.Get(StatType.PlayerStat)) - 1) * 5);
        GameManager.OnPlayerHealthChange?.Invoke(Health);
    }


    private void Update()
    {
        if (State != PlayerState.Running) return;
        movement.MoveForward();
        movement.MoveSideways();
    }


    public void GainHealth(int amount)
    {
        Health += amount;
        GameManager.OnPlayerHealthChange?.Invoke(Health);
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health < 0) Health = 0;
        GameManager.OnPlayerHealthChange?.Invoke(Health);
        if (Health == 0) Die();
    }

    private void OnStateChange()
    {
        if (State == PlayerState.Running)
        {
            Animator.SetTrigger("Run");
            // if (!walikingAudio.isPlaying)
            //     walikingAudio.Play();
        }
        else
        {
            // if (walikingAudio.isPlaying)
            //     walikingAudio.Stop();
        }

        UI.gameObject.SetActive(State == PlayerState.Running || State == PlayerState.Finish);
    }

    public async void Fall()
    {
        if (State != PlayerState.Running) return;

        State = PlayerState.Fall;
        Animator.SetTrigger("Dead");

        GameManager.Instance.SetCamera(false);

        var fallPos = new Vector3(transform.position.x, -3, transform.position.z);
        while (transform.position != fallPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, fallPos, 5 * Time.deltaTime);
            await Task.Yield();
        }

        GameManager.Instance.OnFinish(false);
    }


    public async void Die()
    {
        if (State == PlayerState.Dead) return;
        State = PlayerState.Dead;

        AudioManager.Play("PlayerDeath");
        Animator.SetTrigger("Dead");

        if (Animator.runtimeAnimatorController.animationClips.Length > 4)
        {
            var clip = Animator.runtimeAnimatorController.animationClips[4];
            if (clip)
            {
                Animator.SetFloat("Speed", clip.length / 1.5f);
            }
        }

        await Task.Delay(1600);
        GameManager.Instance.OnFinish(false);
    }

    public Vector3 GetSize()
    {
        return visual.localScale;
    }

    public void CollectCoin(int amount)
    {
        CoinCollected += amount;
        PlayerCashUpdateUI.Show(PlayerCashUpdateUITransform, GetSize().x + 1);
        GameManager.Instance.OnPlayerCoinCollected(CoinCollected);
    }

    public void SetHit(Vector3 position)
    {
        movement.Hit(position);
    }
}