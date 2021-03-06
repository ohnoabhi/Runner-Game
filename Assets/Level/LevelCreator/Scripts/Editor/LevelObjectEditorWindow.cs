using System;
using UnityEditor;
using UnityEngine;

public class LevelObjectEditorWindow : EditorWindow
{
    private Action<int> callback;
    private static LevelObjectEditorWindow window;
    private string title;

    public static void Show(int value, Action<int> callback, string title = "Damage")
    {
        if (!window)
            window = CreateInstance<LevelObjectEditorWindow>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 100);
        window.value = value;
        window.callback = callback;
        window.title = title;
        window.ShowPopup();
    }

    private int value;

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.Space(20);
        value = EditorGUILayout.IntField(title, value);

        GUILayout.Space(10);
        if (GUILayout.Button("Apply"))
        {
            callback?.Invoke(value);
            Close();
        }

        GUILayout.Space(20);
        EditorGUILayout.EndVertical();
    }
}