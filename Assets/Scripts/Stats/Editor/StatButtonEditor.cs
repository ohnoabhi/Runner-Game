using System;
using UnityEditor;
using UnityEditor.UI;

namespace Stats
{
    [CustomEditor(typeof(StatButton))]
    public class StatButtonEditor : SelectableEditor
    {
        private SerializedProperty progressText;
        private SerializedProperty priceText;

        private void OnEnable()
        {
            progressText = serializedObject.FindProperty("progressText");
            priceText = serializedObject.FindProperty("priceText");
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();
            EditorGUILayout.PropertyField(progressText);
            EditorGUILayout.PropertyField(priceText);
            serializedObject.ApplyModifiedProperties();
        }
    }
}