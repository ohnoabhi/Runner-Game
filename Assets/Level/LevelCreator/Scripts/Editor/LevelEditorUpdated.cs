using System.Linq;
using CharacterBase;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelEditorUpdated : EditorWindow
{
    public class SelectionInfo
    {
        public bool HasSelection => SelectedLevel >= 0;
        public int SelectedLevel = -1;
        public int SelectedCategory = -1;
    }

    public const string LevelsFolderPath = "Assets/Level/Levels/";
    private const string LevelScenePath = "Assets/Level/LevelCreator/LevelCreator.unity";
    private static LevelEditorUpdated _window;

    public SerializedObject LevelDatabaseSerializedObject;
    public SerializedProperty LevelsSerializedProperty;
    public SerializedObject SelectedLevelSerializedObject;
    public LevelDatabase LevelDatabase { get; private set; }

    public SelectionInfo Selection;
    public LevelViewer levelViewer;
    private LevelEditorLevelsList levelsList;
    private LevelEditorLevelObjectsList levelObjectsList;
    private LevelEditorSceneView levelEditorSceneView;

    public string[] Characters;

    [MenuItem("Level/Edit Items", priority = 1)]
    public static void EditLevelItems()
    {
        var levelObjectDatabase = LevelObjectDatabase.Get();
        if (!levelObjectDatabase) return;
        UnityEditor.Selection.activeObject = levelObjectDatabase;
    }

    [MenuItem("Level/Level Editor", priority = 0)]
    public static void Open()
    {
        if (!SceneManager.GetSceneByName("LevelCreator").isLoaded)
        {
            if (EditorUtility.DisplayDialog("Level Scene Not Loaded!",
                "Level editor requires level scene to be loaded!", "Load Scene", "cancel"))
            {
                EditorSceneManager.OpenScene(LevelScenePath);
            }
            else
            {
                return;
            }
        }

        if (!_window)
            _window = GetWindow<LevelEditorUpdated>("Level Editor");
        _window.Show();
    }

    private void OnEnable()
    {
        LevelObjectDatabase = LevelObjectDatabase.Get();
        LevelDatabase = LevelDatabase.Get();
        if (LevelDatabase == null)
        {
            Debug.LogError("LevelDatabase is missing!");
            return;
        }

        levelViewer = new LevelViewer(this);
        Selection = new SelectionInfo();
        levelsList = new LevelEditorLevelsList(this);
        levelObjectsList = new LevelEditorLevelObjectsList(this);
        levelEditorSceneView = new LevelEditorSceneView(this);

        LevelDatabaseSerializedObject = new SerializedObject(LevelDatabase);
        LevelsSerializedProperty = LevelDatabaseSerializedObject.FindProperty("Levels");

        Characters = CharacterDatabase.Get().Items.Select(shopItem => shopItem.Key).ToArray();
        SceneView.duringSceneGui += levelEditorSceneView.OnScene;
    }

    private void OnDisable()
    {
        levelViewer.ClearLevel();
        levelViewer.OnDestroy();
        SceneView.duringSceneGui -= levelEditorSceneView.OnScene;
    }

    private void OnGUI()
    {
        if (Selection.HasSelection)
            levelObjectsList.DisplayLevelDetails();
        else
        {
            levelViewer.ClearLevel();
            levelsList.DisplayLevelsTab();
        }

        LevelDatabaseSerializedObject?.ApplyModifiedProperties();
        SelectedLevelSerializedObject?.ApplyModifiedProperties();
    }


    public void RenameLevels()
    {
        var index = 1;
        foreach (var level in LevelDatabase.Levels)
        {
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(level), "Level " + index);
            index++;
        }

        AssetDatabase.Refresh();
    }


    public LevelObjectDatabase LevelObjectDatabase;
}