using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FindMissingScriptsRecursively : EditorWindow
{
    static int _goCount = 0, _componentsCount = 0, _missingCount = 0;

    [MenuItem("Window/FindMissingScriptsRecursively")]
    public static void ShowWindow()
    {
        GetWindow(typeof(FindMissingScriptsRecursively));
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Find Missing Scripts in selected GameObjects"))
        {
            FindInSelected();
        }

        if (GUILayout.Button("Find Missing Scripts"))
        {
            FindAll();
        }

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Component Scanned:");
            EditorGUILayout.LabelField("" + (_componentsCount == -1 ? "---" : _componentsCount.ToString()));
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Object Scanned:");
            EditorGUILayout.LabelField("" + (_goCount == -1 ? "---" : _goCount.ToString()));
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Possible Missing Scripts:");
            EditorGUILayout.LabelField("" + (_missingCount == -1 ? "---" : _missingCount.ToString()));
        }
        EditorGUILayout.EndHorizontal();
    }

    private static void FindAll()
    {
        _componentsCount = 0;
        _goCount = 0;
        _missingCount = 0;

        string[] assetsPaths = AssetDatabase.GetAllAssetPaths();

        var toRemove = new List<Component>();
        foreach (string assetPath in assetsPaths)
        {
            Object[] data = LoadAllAssetsAtPath(assetPath);
            foreach (Object o in data)
            {
                if (o != null)
                {
                    if (o is GameObject)
                    {
                        toRemove.AddRange(FindInGO((GameObject) o));
                    }
                }
            }
        }

        Debug.Log($"Searched {_goCount} GameObjects, {_componentsCount} components, found {_missingCount} missing");
        foreach (var component in toRemove)
        {
            Debug.Log("Removing: " + component.GetType());
            DestroyImmediate(component);
        }
    }

    public static Object[] LoadAllAssetsAtPath(string assetPath)
    {
        return typeof(SceneAsset).Equals(AssetDatabase.GetMainAssetTypeAtPath(assetPath))
            ?
            // prevent error "Do not use readobjectthreaded on scene objects!"
            new[] {AssetDatabase.LoadMainAssetAtPath(assetPath)}
            : AssetDatabase.LoadAllAssetsAtPath(assetPath);
    }

    private static void FindInSelected()
    {
        GameObject[] go = Selection.gameObjects;
        _goCount = 0;
        _componentsCount = 0;
        _missingCount = 0;
        foreach (GameObject g in go)
        {
            FindInGO(g);
        }

        Debug.Log($"Searched {_goCount} GameObjects, {_componentsCount} components, found {_missingCount} missing");
    }

    private static List<Component> FindInGO(GameObject g)
    {
        _goCount++;
        var toRemove = new List<Component>();
        Component[] components = g.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            _componentsCount++;
            if (components[i] == null)
            {
                _missingCount++;
                string s = g.name;
                Transform t = g.transform;
                while (t.parent != null)
                {
                    var parent = t.parent;
                    s = parent.name + "/" + s;
                    t = parent;
                }

                Debug.Log(s + " has an empty script attached in position: " + i, g);
                toRemove.Add(components[i]);
            }
        }

        // Now recurse through each child GO (if there are any):
        foreach (Transform childT in g.transform)
        {
            //Debug.Log("Searching " + childT.name  + " " );
            toRemove.AddRange(FindInGO(childT.gameObject));
        }

        return toRemove;
    }
}