using UnityEngine;

namespace UI.Screens
{
    public class GameFinisherScreen : BaseScreen
    {
        [SerializeField] private Transform parent;

        public GameObject SetUI(GameObject ui)
        {
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }

            if (!ui) return null;
            var instance = Instantiate(ui, parent);
            return instance;
        }
    }
}