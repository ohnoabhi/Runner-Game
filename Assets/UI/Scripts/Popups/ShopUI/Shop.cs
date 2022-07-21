using System.Linq;
using CharacterBase;
using Sirenix.Utilities;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    public static ShopItem[] ShopItems => instance.database.Items;
    public Character ObstacleCharacter;

    private CharacterDatabase database;

    public static string SelectedCharacterKey
    {
        get => PlayerPrefs.GetString("CharacterId", "");
        set => PlayerPrefs.SetString("CharacterId", value);
    }

    public static int PurchaseCount
    {
        get => PlayerPrefs.GetInt("CharacterPurchaseCount", 0);
        set => PlayerPrefs.SetInt("CharacterPurchaseCount", value);
    }

    private void Awake()
    {
        instance = this;
        database = CharacterDatabase.Get();
        if (SelectedCharacterKey.IsNullOrWhitespace())
        {
            ShopItems[0].Unlock();
            SelectedCharacterKey = ShopItems[0].Key;
        }
    }


    public static Character GetCharacter()
    {
        return (from item in ShopItems
            where item.Key == SelectedCharacterKey
            select item.Character).FirstOrDefault();
    }

    public static Character GetCharacter(int id)
    {
        return ShopItems[id].Character;
    }
}