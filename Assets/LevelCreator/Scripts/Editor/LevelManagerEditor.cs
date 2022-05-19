using System;
using UnityEditor;
using UnityEngine;

namespace LevelCreator.Editor
{
    [CustomEditor(typeof(LevelEditorManager))]
    public class LevelManagerEditor : UnityEditor.Editor
    {
        public class MouseInfo
        {
            public Vector3 Position;
            public Vector3 Offset;
            public bool IsMouseDown;
            public bool IsOver;
            public int MouseOverIndex;
            public int SelectedIndex;
            public SelectionMode SelectionMode;
        }

        public enum SelectionMode
        {
            None,
            Vertical,
            HorizontalMiddle,
            HorizontalLeft,
            HorizontalRight
        }

        private LevelEditorManager _levelEditorManager;
        private bool needRepaint;
        private MouseInfo _mouseInfo;

        private void OnEnable()
        {
            _levelEditorManager = (LevelEditorManager) target;
            _mouseInfo = new MouseInfo();
            Tools.hidden = true;
        }

        private void OnDisable()
        {
            Tools.hidden = false;
        }

        private void OnSceneGUI()
        {
            var guiEvent = Event.current;
            switch (guiEvent.type)
            {
                case EventType.Repaint:
                    Draw();
                    break;
                case EventType.Layout:
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                    break;
                default:
                {
                    HandleInput(guiEvent);
                    if (needRepaint)
                    {
                        HandleUtility.Repaint();
                    }

                    break;
                }
            }
        }

        private void Draw()
        {
            if (!_levelEditorManager.Selection.HasSelection) return;

            var index = 0;

            foreach (var levelObject in _levelEditorManager.LevelObjects)
            {
                if (levelObject.IsPlatform) continue;
                var position = Vector3.zero;

                switch (levelObject.Layout)
                {
                    case LevelObject.HorizontalLayout.Single:
                        break;
                    case LevelObject.HorizontalLayout.Double:

                        Handles.color = _mouseInfo.SelectionMode == SelectionMode.HorizontalLeft
                            ? index ==
                              _mouseInfo.SelectedIndex ? Color.yellow :
                            _mouseInfo.MouseOverIndex == index ? Color.yellow * 0.75f : Color.yellow * 0.4f
                            : Color.yellow * 0.4f;

                        position = levelObject.GetHorizontalHandlePos();
                        position.x = -2.5f;

                        Handles.DrawSolidDisc(position, Vector3.up,
                            0.25f);

                        Handles.color = _mouseInfo.SelectionMode == SelectionMode.HorizontalRight
                            ? index ==
                              _mouseInfo.SelectedIndex ? Color.yellow :
                            _mouseInfo.MouseOverIndex == index ? Color.yellow * 0.75f : Color.yellow * 0.4f
                            : Color.yellow * 0.4f;

                        position = levelObject.GetHorizontalHandlePos();
                        position.x = 2.5f;

                        Handles.DrawSolidDisc(position, Vector3.up,
                            0.25f);
                        index++;

                        break;
                    case LevelObject.HorizontalLayout.Triple:
                        Handles.color = _mouseInfo.SelectionMode == SelectionMode.HorizontalLeft
                            ? index ==
                              _mouseInfo.SelectedIndex ? Color.yellow :
                            _mouseInfo.MouseOverIndex == index ? Color.yellow * 0.75f : Color.yellow * 0.4f
                            : Color.yellow * 0.4f;

                        position = levelObject.GetHorizontalHandlePos();
                        position.x = -2.5f;

                        Handles.DrawSolidDisc(position, Vector3.up,
                            0.25f);
                        index++;
                        Handles.color = _mouseInfo.SelectionMode == SelectionMode.HorizontalMiddle
                            ? index ==
                              _mouseInfo.SelectedIndex ? Color.yellow :
                            _mouseInfo.MouseOverIndex == index ? Color.yellow * 0.75f : Color.yellow * 0.4f
                            : Color.yellow * 0.4f;

                        position = levelObject.GetHorizontalHandlePos();
                        position.x = 0;

                        Handles.DrawSolidDisc(position, Vector3.up,
                            0.25f);
                        index++;
                        Handles.color = _mouseInfo.SelectionMode == SelectionMode.HorizontalRight
                            ? index ==
                              _mouseInfo.SelectedIndex ? Color.yellow :
                            _mouseInfo.MouseOverIndex == index ? Color.yellow * 0.75f : Color.yellow * 0.4f
                            : Color.yellow * 0.4f;

                        position = levelObject.GetHorizontalHandlePos();
                        position.x = 2.5f;

                        Handles.DrawSolidDisc(position, Vector3.up,
                            0.25f);
                        index++;
                        break;
                }


                Handles.color = _mouseInfo.SelectionMode == SelectionMode.Vertical
                    ? index ==
                      _mouseInfo.SelectedIndex ? Color.blue :
                    _mouseInfo.MouseOverIndex == index ? Color.blue * 0.75f : Color.blue * 0.4f
                    : Color.blue * 0.4f;
                position = levelObject.GetVerticalHandlePos();
                Handles.DrawSolidDisc(position, Vector3.up,
                    0.25f);
                index++;
            }
        }

        private void HandleInput(Event guiEvent)
        {
            var mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
            _mouseInfo.Position =
                mouseRay.GetPoint((0 - mouseRay.origin.y) /
                                  mouseRay.direction.y);

            switch (guiEvent.type)
            {
                case EventType.MouseDown when guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None:
                    HandleLeftMouseDown();
                    break;
                case EventType.MouseUp when guiEvent.button == 0:
                    HandleLeftMouseUp();
                    break;
                case EventType.MouseDrag when guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None:
                    HandleLeftMouseDrag();
                    break;
            }

            if (!_mouseInfo.IsMouseDown)
            {
                UpdateMouseOverInfo();
            }
        }

        void HandleLeftMouseDown()
        {
            if (_mouseInfo.IsOver)
            {
                _mouseInfo.SelectedIndex = _mouseInfo.MouseOverIndex;
                _mouseInfo.IsMouseDown = true;
            }
        }

        private void HandleLeftMouseUp()
        {
            if (_mouseInfo.SelectedIndex >= 0)
                switch (_mouseInfo.SelectionMode)
                {
                    case SelectionMode.HorizontalLeft:
                    {
                        var levelItemData = _levelEditorManager.LevelObjects[_mouseInfo.SelectedIndex].GetData();
                        levelItemData.Position =
                            new Vector3(-2.5f, levelItemData.Position.y, levelItemData.Position.z);

                        _levelEditorManager.LevelObjects[_mouseInfo.SelectedIndex].SetData(levelItemData);

                        var levelData =
                            _levelEditorManager.LevelDatabase.Levels[_levelEditorManager.Selection.SelectedLevel];
                        levelData.LevelItems[_mouseInfo.SelectedIndex] = levelItemData;
                        EditorUtility.SetDirty(levelData);
                        break;
                    }
                    case SelectionMode.HorizontalMiddle:
                    {
                        var levelItemData = _levelEditorManager.LevelObjects[_mouseInfo.SelectedIndex].GetData();
                        levelItemData.Position =
                            new Vector3(0, levelItemData.Position.y, levelItemData.Position.z);

                        _levelEditorManager.LevelObjects[_mouseInfo.SelectedIndex].SetData(levelItemData);

                        var levelData =
                            _levelEditorManager.LevelDatabase.Levels[_levelEditorManager.Selection.SelectedLevel];
                        levelData.LevelItems[_mouseInfo.SelectedIndex] = levelItemData;
                        EditorUtility.SetDirty(levelData);
                        break;
                    }
                    case SelectionMode.HorizontalRight:
                    {
                        var levelItemData = _levelEditorManager.LevelObjects[_mouseInfo.SelectedIndex].GetData();
                        levelItemData.Position =
                            new Vector3(2.5f, levelItemData.Position.y, levelItemData.Position.z);

                        _levelEditorManager.LevelObjects[_mouseInfo.SelectedIndex].SetData(levelItemData);

                        var levelData =
                            _levelEditorManager.LevelDatabase.Levels[_levelEditorManager.Selection.SelectedLevel];
                        levelData.LevelItems[_mouseInfo.SelectedIndex] = levelItemData;
                        EditorUtility.SetDirty(levelData);
                        break;
                    }
                }

            _mouseInfo.IsMouseDown = false;
            _mouseInfo.IsOver = false;
        }

        private void HandleLeftMouseDrag()
        {
            if (!_mouseInfo.IsMouseDown) return;
            var position = _mouseInfo.Position + _mouseInfo.Offset;

            switch (_mouseInfo.SelectionMode)
            {
                case SelectionMode.Vertical:
                {
                    var levelItemData = _levelEditorManager.LevelObjects[_mouseInfo.SelectedIndex].GetData();
                    levelItemData.Position =
                        new Vector3(levelItemData.Position.x, levelItemData.Position.y, position.z);

                    _levelEditorManager.LevelObjects[_mouseInfo.SelectedIndex].SetData(levelItemData);

                    var levelData =
                        _levelEditorManager.LevelDatabase.Levels[_levelEditorManager.Selection.SelectedLevel];
                    levelData.LevelItems[_mouseInfo.SelectedIndex] = levelItemData;
                    EditorUtility.SetDirty(levelData);
                    break;
                }
            }
        }

        private void UpdateMouseOverInfo()
        {
            _mouseInfo.IsMouseDown = false;
            _mouseInfo.IsOver = false;
            _mouseInfo.MouseOverIndex = -1;
            _mouseInfo.SelectedIndex = -1;

            var index = 0;
            foreach (var levelObject in _levelEditorManager.LevelObjects)
            {
                if (levelObject.IsPlatform)
                {
                    index++;
                    continue;
                }

                var horizontalHandlePos = levelObject.GetHorizontalHandlePos();
                horizontalHandlePos.x = 0;
                if (Vector3.Distance(_mouseInfo.Position,
                    horizontalHandlePos) <= 0.25f)
                {
                    _mouseInfo.SelectionMode = SelectionMode.HorizontalMiddle;
                    _mouseInfo.MouseOverIndex = index;
                    _mouseInfo.Offset = horizontalHandlePos - _mouseInfo.Position;
                    _mouseInfo.IsOver = true;
                    break;
                }

                horizontalHandlePos.x = -2.5f;
                if (Vector3.Distance(_mouseInfo.Position,
                    horizontalHandlePos) <= 0.25f)
                {
                    _mouseInfo.SelectionMode = SelectionMode.HorizontalLeft;
                    _mouseInfo.MouseOverIndex = index;
                    _mouseInfo.Offset = horizontalHandlePos - _mouseInfo.Position;
                    _mouseInfo.IsOver = true;
                    break;
                }

                horizontalHandlePos.x = 2.5f;
                if (Vector3.Distance(_mouseInfo.Position,
                    horizontalHandlePos) <= 0.25f)
                {
                    _mouseInfo.SelectionMode = SelectionMode.HorizontalRight;
                    _mouseInfo.MouseOverIndex = index;
                    _mouseInfo.Offset = horizontalHandlePos - _mouseInfo.Position;
                    _mouseInfo.IsOver = true;
                    break;
                }

                if (Vector3.Distance(_mouseInfo.Position,
                    levelObject.GetVerticalHandlePos()) <= 0.25f)
                {
                    _mouseInfo.SelectionMode = SelectionMode.Vertical;
                    _mouseInfo.MouseOverIndex = index;
                    _mouseInfo.Offset = levelObject.GetVerticalHandlePos() - _mouseInfo.Position;
                    _mouseInfo.IsOver = true;
                    break;
                }

                index++;
            }
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();
        }
    }
}