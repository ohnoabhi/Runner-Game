using System;
using UnityEditor;
using UnityEngine;

namespace Snapshot.Editor
{
    [CustomEditor(typeof(SnapshotTaker))]
    public class SnapshotEditor : UnityEditor.Editor
    {
        private SerializedProperty input;
        private SerializedProperty position;
        private SerializedProperty rotation;
        private SerializedProperty scale;

        private SnapshotTaker _snapshotTaker;

        private void OnEnable()
        {
            input = serializedObject.FindProperty("_inputFolder");
            position = serializedObject.FindProperty("_offsetPosition");
            rotation = serializedObject.FindProperty("_offsetRotation");
            scale = serializedObject.FindProperty("_scale");

            _snapshotTaker = (SnapshotTaker) target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(input);
            EditorGUILayout.PropertyField(position);
            EditorGUILayout.PropertyField(rotation);
            EditorGUILayout.PropertyField(scale);
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Begin Capture"))
            {
                _snapshotTaker.BeginCapture();
            }

            if (GUILayout.Button("Capture Once"))
            {
                _snapshotTaker.CaptureOnce();
            }
        }
    }
}