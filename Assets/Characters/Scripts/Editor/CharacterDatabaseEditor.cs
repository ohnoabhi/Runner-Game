// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
//
// namespace CharacterBase
// {
//     [CustomEditor(typeof(CharacterDatabase))]
//     public class CharacterDatabaseEditor : Editor
//     {
//         class Options
//         {
//             public ViewType ViewType = ViewType.List;
//             public int Selected;
//         }
//
//         private enum ViewType
//         {
//             List,
//             Edit
//         }
//
//         private CharacterDatabase _database;
//         private Options _options;
//
//         private void OnEnable()
//         {
//             _database = (CharacterDatabase) target;
//             _options = new Options();
//         }
//
//         public override void OnInspectorGUI()
//         {
//             if (_options.ViewType == ViewType.List)
//                 ListView();
//             else if (_options.ViewType == ViewType.Edit)
//                 EditView();
//
//             serializedObject.ApplyModifiedProperties();
//         }
//
//         private void EditView()
//         {
//             EditorGUILayout.BeginHorizontal(GUI.skin.box);
//             {
//                 if (GUILayout.Button("<", GUILayout.Width(30))) _options.ViewType = ViewType.List;
//                 EditorGUILayout.LabelField("Characters");
//             }
//             EditorGUILayout.EndHorizontal();
//             var list = _database.CharacterLists[_options.Selected];
//             if (list.Characters == null || list.Characters.Count <= 0)
//             {
//                 EditorGUILayout.LabelField("Empty");
//             }
//             else
//             {
//                 var index = 0;
//
//                 foreach (var character in list.Characters)
//                 {
//                     character.Skin =
//                         (Character) EditorGUILayout.ObjectField(character.Skin, typeof(GameObject), false);
//                     index++;
//                 }
//             }
//
//             if (GUILayout.Button("Add"))
//             {
//                 list.Characters ??= new List<CharacterItem>();
//                 list.Characters.Add(new CharacterItem());
//             }
//         }
//
//         private void ListView()
//         {
//             EditorGUILayout.LabelField("Character Lists");
//             if (_database.CharacterLists == null || _database.CharacterLists.Count <= 0)
//             {
//                 EditorGUILayout.LabelField("Empty");
//             }
//             else
//             {
//                 var index = 0;
//                 foreach (var characterList in _database.CharacterLists)
//                 {
//                     EditorGUILayout.BeginHorizontal(GUI.skin.box);
//                     {
//                         characterList.Name = EditorGUILayout.TextField(characterList.Name);
//                         if (GUILayout.Button("Edit"))
//                         {
//                             _options.Selected = index;
//                             _options.ViewType = ViewType.Edit;
//                         }
//
//                         index++;
//                     }
//                     EditorGUILayout.EndHorizontal();
//                 }
//             }
//
//             if (GUILayout.Button("Add"))
//             {
//                 _database.CharacterLists ??= new List<CharacterList>();
//                 _database.CharacterLists.Add(new CharacterList() {Name = "New Skin Type"});
//                 AssetDatabase.SaveAssets();
//                 AssetDatabase.Refresh();
//             }
//         }
//
//         [MenuItem("Tools/Character Database")]
//         public static void OpenEditor()
//         {
//             var database = Resources.Load<CharacterDatabase>("Character Database");
//             if (!database)
//             {
//                 database = CreateInstance<CharacterDatabase>();
//
//                 var path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Character Database.asset");
//                 AssetDatabase.CreateAsset(database, path);
//                 AssetDatabase.SaveAssets();
//
//                 EditorUtility.FocusProjectWindow();
//             }
//
//             Selection.activeObject = database;
//         }
//     }
// }

