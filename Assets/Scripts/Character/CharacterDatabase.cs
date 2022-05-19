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
        public List<CharacterList> CharacterLists;

        public static int CurrentSkinIndex
        {
            get => PlayerPrefs.GetInt("PLAYER_SKIN", 0);
            set => PlayerPrefs.SetInt("PLAYER_SKIN", value);
        }

        public CharacterList CurrentCharacterList => CharacterLists[CurrentSkinIndex];

        public static CharacterDatabase Get()
        {
            var database = Resources.Load<CharacterDatabase>("Character Database");
            if (database) return database;
            database = CreateInstance<CharacterDatabase>();

            var path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Character Database.asset");
            AssetDatabase.CreateAsset(database, path);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            return database;
        }
    }

    [Serializable]
    public class CharacterList
    {
        public string Name;
        public List<CharacterItem> Characters;

        [CanBeNull]
        public Character GetSkin(int index)
        {
            if (Characters != null && Characters.Count > index) return Characters[index].Character;
            return null;
        }
    }

    [Serializable]
    public class CharacterItem
    {
        public Sprite Icon;
        public Character Character;
    }
}