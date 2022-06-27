using System;
using UnityEditor;
using UnityEditor.UI;

namespace Stats
{
    [CustomEditor(typeof(StatButton))]
    public class StatButtonEditor : SelectableEditor
    {
        private SerializedProperty type;
        private SerializedProperty progressText;
        private SerializedProperty priceText;
        private SerializedProperty priceIcon;

        protected override void OnEnable()
        {
            base.OnEnable();
            progressText = serializedObject.FindProperty("progressText");
            priceText = serializedObject.FindProperty("priceText");
            priceIcon = serializedObject.FindProperty("priceIcon");
            type = serializedObject.FindProperty("type");
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();
            EditorGUILayout.PropertyField(type);
            EditorGUILayout.PropertyField(progressText);
            EditorGUILayout.PropertyField(priceText);
            EditorGUILayout.PropertyField(priceIcon);
            serializedObject.ApplyModifiedProperties();
        }
    }
}