using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CharacterBase
{
    public class CharacterDatabase : ScriptableObject
    {
        public ShopItem[] Items;
        
        public static CharacterDatabase Get()
        {
            var database = Resources.Load<CharacterDatabase>("Character Database");
            if (database) return database;
#if UNITY_EDITOR
            database = CreateInstance<CharacterDatabase>();

            var path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Character Database.asset");
            AssetDatabase.CreateAsset(database, path);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

#endif
            return database;
        }
    }
    //
    // [Serializable]
    // public class CharacterList
    // {
    //     public string Name;
    //     public List<CharacterItem> Characters;
    //
    //     [CanBeNull]
    //     public Character GetSkin(int index)
    //     {
    //         if (Characters == null) return null;
    //         return Characters.Count > index ? Characters[index].Character : Characters[Characters.Count - 1].Character;
    //     }
    // }
    //
    // [Serializable]
    // public class CharacterItem
    // {
    //     public Sprite Icon;
    //     public Character Character;
    // }
}