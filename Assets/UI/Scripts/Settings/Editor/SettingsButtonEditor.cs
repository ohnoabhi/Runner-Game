using UnityEditor;
using UnityEditor.UI;

namespace Settings
{
    [CustomEditor(typeof(SettingsButton))]
    public class SettingsButtonEditor : ButtonEditor
    {
        private SerializedProperty id;
        private SerializedProperty enabledIcon;
        private SerializedProperty disabledIcon;

        protected override void OnEnable()
        {
            id = serializedObject.FindProperty("id");
            enabledIcon = serializedObject.FindProperty("enabledIcon");
            disabledIcon = serializedObject.FindProperty("disabledIcon");
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(id);
            EditorGUILayout.PropertyField(enabledIcon);
            EditorGUILayout.PropertyField(disabledIcon);
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}