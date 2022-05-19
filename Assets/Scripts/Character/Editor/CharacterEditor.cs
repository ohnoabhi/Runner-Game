using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CharacterBase
{
    public class CharacterEditor : EditorWindow
    {
        class Options
        {
            public ViewType ViewType = ViewType.List;
            public int Selected;
        }

        private enum ViewType
        {
            List,
            Edit
        }

        private CharacterDatabase database;
        private Options options;

        [MenuItem("Tools/Character")]
        private static void ShowWindow()
        {
            var window = GetWindow<CharacterEditor>();
            window.titleContent = new GUIContent("Character");
            window.Show();
        }

        private void OnEnable()
        {
            database = CharacterDatabase.Get();
            options = new Options();
        }

        private void OnGUI()
        {
            if (options.ViewType == ViewType.List)
                ListView();
            else if (options.ViewType == ViewType.Edit)
                EditView();
        }

        private void EditView()
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            {
                if (GUILayout.Button("<", GUILayout.Width(30))) options.ViewType = ViewType.List;
                EditorGUILayout.LabelField("Characters");
            }
            EditorGUILayout.EndHorizontal();
            var list = database.CharacterLists[options.Selected];
            if (list.Characters == null || list.Characters.Count <= 0)
            {
                EditorGUILayout.LabelField("Empty");
            }
            else
            {
                var index = 0;

                foreach (var character in list.Characters)
                {
                    character.Character =
                        (Character) EditorGUILayout.ObjectField(character.Character, typeof(Character), false);
                    index++;
                }
            }

            if (GUILayout.Button("Add"))
            {
                list.Characters ??= new List<CharacterItem>();
                list.Characters.Add(new CharacterItem());
                EditorUtility.SetDirty(database);
            }
        }

        private void ListView()
        {
            EditorGUILayout.LabelField("Character Lists");
            if (database.CharacterLists == null || database.CharacterLists.Count <= 0)
            {
                EditorGUILayout.LabelField("Empty");
            }
            else
            {
                var index = 0;
                foreach (var characterList in database.CharacterLists)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        characterList.Name = EditorGUILayout.TextField(characterList.Name);
                        if (GUILayout.Button("Edit"))
                        {
                            options.Selected = index;
                            options.ViewType = ViewType.Edit;

                            // selectedList = new SerializedObject(characterList);
                        }

                        index++;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            if (GUILayout.Button("Add"))
            {
                database.CharacterLists ??= new List<CharacterList>();
                database.CharacterLists.Add(new CharacterList() {Name = "New Skin Type"});
                EditorUtility.SetDirty(database);
            }
        }
    }
}