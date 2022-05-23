using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad()]
public static class ToolbarExtension
{
    private static string[] paths;
    private static string[] names;

    static ToolbarExtension()
    {
        var guids = AssetDatabase.FindAssets("t:scene", null);
        if (guids.Length == 0)
        {
            Debug.LogWarning("Couldn't find scene file");
            return;
        }

        paths = new string[guids.Length];
        names = new string[guids.Length];
        for (var i = 0; i < guids.Length; i++)
        {
            paths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            names[i] = AssetDatabase.LoadAssetAtPath<Object>(paths[i]).name;
        }

        ToolbarExtender.RightToolbarGUI.Add(DrawSceneButton);
    }

    private static int selected = 0;

    private static void DrawSceneButton()
    {
        GUILayout.FlexibleSpace();

        var temp = EditorGUILayout.Popup("", selected, names, GUILayout.Width(100));
        if (temp != selected)
        {
            selected = temp;
            LoadScene(paths[selected]);
        }
    }

    private static void LoadScene(string path)
    {
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorUtility.DisplayDialog("Save", "Do you want to save scene?", "Save", "Cancel");
        }

        EditorSceneManager.OpenScene(paths[selected]);
    }
}


static class ToolbarStyles
{
    public static readonly GUIStyle commandButtonStyle;

    static ToolbarStyles()
    {
        commandButtonStyle = new GUIStyle("Command")
        {
            fontSize = 16,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageAbove,
            fontStyle = FontStyle.Bold
        };
    }
}


public static class Utility
{
}