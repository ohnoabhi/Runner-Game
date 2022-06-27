#if UNITY_EDITOR
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Snapshot
{
    public class SnapshotTaker : MonoBehaviour
    {
        [SerializeField] private Object _inputFolder;
        [SerializeField] private Vector3 _offsetPosition;
        [SerializeField] private Vector3 _offsetRotation;
        [SerializeField] private float _scale;

        [ContextMenu("Clear Progress")]
        public void ClearProgress()
        {
            EditorUtility.ClearProgressBar();
        }

        public async void BeginCapture()
        {
            if (!Application.isPlaying)
            {
                EditorApplication.isPlaying = true;
                return;
            }


            var t = 0f;
            while (!Application.isPlaying)
            {
                EditorUtility.DisplayProgressBar("Waiting..", "Waiting for play mode", t);
                t += Time.deltaTime;
                await Task.Yield();
            }

            EditorUtility.ClearProgressBar();

            var assetPath = AssetDatabase.GetAssetPath(_inputFolder);
            var dirInfo = new DirectoryInfo(assetPath);
            var fileInf = dirInfo.GetFiles("*.prefab");

            //loop through directory loading the game object and checking if it has the component you want
            var objectsToCapture = (from fileInfo in fileInf
                select fileInfo.FullName.Replace(@"\", "/")
                into fullPath
                select "Assets" + fullPath.Replace(Application.dataPath, "")
                into path
                select AssetDatabase.LoadAssetAtPath(path, typeof(GameObject))).OfType<GameObject>().ToList();

            // var objectsToCapture =
            //     Resources.LoadAll<GameObject>(assetPath);

            if (objectsToCapture == null || objectsToCapture.Count == 0)
            {
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("Failed!", "No objects found!", "OK");
#endif
                return;
            }

            await Task.Yield();
            var i = 0;
            EditorUtility.DisplayProgressBar("Taking screenshots", "Loading objects", 0);
            foreach (var o in objectsToCapture)
            {
                EditorUtility.DisplayProgressBar("Taking screenshots", "Capturing " + o.name,
                    (float) i / objectsToCapture.Count);
                var instance = Instantiate(o);
                instance.transform.position = _offsetPosition;
                instance.transform.rotation = Quaternion.Euler(_offsetRotation);
                instance.transform.localScale = Vector3.one * _scale;
                Destroy(instance.GetComponent<Animator>());
                ChangeLayer(instance.transform);
                Debug.Log(instance.name, instance);
                await Task.Delay(10);
                SnapshotCamera.Screenshot();
                await Task.Delay(1000);
                Destroy(instance);
                i++;
            }

            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
            Application.Quit();
        }

        public void CaptureOnce()
        {
            SnapshotCamera.Screenshot();
        }

        private void ChangeLayer(Transform transform)
        {
            transform.gameObject.layer = LayerMask.NameToLayer("Capture");
            if (transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    ChangeLayer(child);
                }
            }
        }
    }
}
#endif