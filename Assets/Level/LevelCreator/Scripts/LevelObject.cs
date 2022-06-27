using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class LevelObject : MonoBehaviour
{
    public static float LevelSize = 2;

    public enum HorizontalLayout
    {
        Single,
        Double,
        Triple
    }

    public enum LevelObjectType
    {
        Platform,
        Obstacle,
        Enemy,
        Multiplier,
        Collectable,
        Character
    }

    public LevelObjectType ObjectType;
    [HideIf("IsPlatform")] public HorizontalLayout Layout;
    public bool IsPlatform => ObjectType == LevelObjectType.Platform;


    [ShowIf("IsPlatform")] public Transform End;
    private LevelItemData _levelItemData;
    private bool isObstacleOrEnemy => ObjectType == LevelObjectType.Obstacle || ObjectType == LevelObjectType.Enemy || ObjectType == LevelObjectType.Character;
    public bool IsMultiplier => ObjectType == LevelObjectType.Multiplier;
    private bool showObstacle => isObstacleOrEnemy && ShowDamage;
    [ShowIf("isObstacleOrEnemy")] public bool ShowDamage;
    [ShowIf("showObstacle")] public Obstacle Obstacle;
    [SerializeField] private bool shouldClear = false;
    public bool IsCharacter => ObjectType == LevelObjectType.Character;
    public bool ShouldClear => shouldClear;

    [SerializeField] private UnityEvent resetEvents;

    public Vector3 GetHorizontalHandlePos()
    {
        return transform.position + Vector3.forward * 0.5f;
    }

    public Vector3 GetVerticalHandlePos()
    {
        return transform.position;
    }

    public LevelItemData GetData()
    {
        return _levelItemData;
    }

    public void SetData(LevelItemData levelItemData)
    {
        _levelItemData = levelItemData;
        LoadData();
    }

    private void LoadData()
    {
        transform.position = _levelItemData.Position;
        if (ShowDamage && Obstacle)
        {
            Obstacle.damage = _levelItemData.Damage;
        }

        if (IsMultiplier)
        {
            GetComponent<Multiplier>().Init(_levelItemData.MultiplierValue);
        }
        if (IsCharacter)
        {
            GetComponent<CharacterObstacle>().Init(_levelItemData.CharacterId);
        }
    }

    public void ResetObject()
    {
        resetEvents?.Invoke();
    }
}