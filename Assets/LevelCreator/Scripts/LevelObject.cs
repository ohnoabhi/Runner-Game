using Sirenix.OdinInspector;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
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
        Collectable
    }

    public LevelObjectType ObjectType;
    [HideIf("IsPlatform")] public HorizontalLayout Layout;
    public bool IsPlatform => ObjectType == LevelObjectType.Platform;
    [ShowIf("IsPlatform")] public Transform End;
    private LevelItemData _levelItemData;

    public Vector3 GetHorizontalHandlePos()
    {
        return transform.position + Vector3.forward * 0.5f;
    }

    public Vector3 GetVerticalHandlePos()
    {
        return transform.position + Vector3.right * 0.5f;
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
    }
}