using System;
using UnityEditor;
using UnityEngine;

public class ZPositionHandle
{
    private static float zOffset = 0;

    public static Vector3 Move(int controlId, Vector3 position)
    {
        var movement = position;

        var currentEvent = Event.current;
        var mouseRay = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
        var mousePosition =
            mouseRay.GetPoint((0 - mouseRay.origin.y) /
                              mouseRay.direction.y);
        switch (currentEvent.GetTypeForControl(controlId))
        {
            case EventType.MouseDown:
                if (HandleUtility.nearestControl == controlId && currentEvent.button == 0)
                {
                    GUIUtility.hotControl = controlId;
                    zOffset = position.z - mousePosition.z;
                    currentEvent.Use();
                    EditorGUIUtility.SetWantsMouseJumping(1);
                }

                break;
            case EventType.MouseUp:
                if (GUIUtility.hotControl == controlId && (currentEvent.button == 0 || currentEvent.button == 2))
                {
                    zOffset = 0;
                    GUIUtility.hotControl = 0;
                    currentEvent.Use();
                    EditorGUIUtility.SetWantsMouseJumping(0);
                }

                break;
            case EventType.MouseMove:
                if (HandleUtility.nearestControl == controlId)
                    HandleUtility.Repaint();
                break;
            case EventType.MouseDrag:
                if (GUIUtility.hotControl == controlId)
                {
                    movement.z = mousePosition.z + zOffset;
                    GUI.changed = true;
                    currentEvent.Use();
                }

                break;
            case EventType.Repaint:
                if (controlId == GUIUtility.hotControl)
                {
                    Handles.color = Color.blue * 1f;
                }
                else if (controlId == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
                {
                    Handles.color = Color.blue * 0.8f;
                }
                else
                {
                    Handles.color = Color.blue * 0.6f;
                }

                Handles.ArrowHandleCap(controlId, position, Quaternion.LookRotation(Vector3.forward, Vector3.up), 1,
                    EventType.Repaint);
                if (controlId == GUIUtility.hotControl ||
                    (controlId == HandleUtility.nearestControl && GUIUtility.hotControl == 0))
                {
                    Handles.color = Color.blue * 0.6f;
                }

                break;
            case EventType.Layout:
                Handles.ArrowHandleCap(controlId, position, Quaternion.identity, 1, EventType.Layout);
                break;
        }

        return movement;
    }
}