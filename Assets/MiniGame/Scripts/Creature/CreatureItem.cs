using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class CreatureItem : MonoBehaviour
{
    [SerializeField] int creatureId;

    [SerializeField] int keysRequired = 3;

    [SerializeField] Button unlockBtn;

    [SerializeField] TMP_Text buttonTxt;

    [SerializeField] GameObject cage;

    [SerializeField] GameObject creature;

    [SerializeField] Transform finalPos;

    [SerializeField] float speed;

    [SerializeField] private Transform soliderFinalPos;

    [SerializeField] private CageGuard[] guards;

    [HideInInspector] public string CreatureKey;

    public bool IsUnlocked
    {
        get => PlayerPrefs.GetInt(
            CreatureKey, 0) == 1;

        private set => PlayerPrefs.SetInt(
            CreatureKey, value ? 1 : 0);
    }

    private void Start()
    {
        if (CreatureData.instance.ReturnCreature(creatureId) != null)
        {
            GameObject creatureReturned = CreatureData.instance.ReturnCreature(creatureId);

            var newCreature = Instantiate(creatureReturned, creature.transform.position, creature.transform.rotation,
                gameObject.transform);

            Destroy(creature.gameObject);

            creature = newCreature;

            //creature.transform.SetParent(this.transform);
        }

        if (IsUnlocked)
        {
            CageUnlocked();
        }
        else
        {
            CageLocked();
        }
    }

    private void CageUnlocked()
    {
        Debug.Log("Creature outside cage");

        buttonTxt.text = "Unlocked";

        cage.SetActive(false);

        creature.SetActive(false);

        foreach (var cageGuard in guards)
        {
            cageGuard.gameObject.SetActive(false);
        }
    }

    private void CageLocked()
    {
        Debug.Log("Creature in cage");

        unlockBtn.onClick.RemoveAllListeners();
        unlockBtn.onClick.AddListener(() => OnClickUnlock());

        buttonTxt.text = "Unlock";
    }

    public void OnClickUnlock()
    {
        int keys = CollectablesManager.Get(CollectableType.Key);

        if (keys >= keysRequired)
        {
            IsUnlocked = true;
            CollectablesManager.Remove(CollectableType.Key, keysRequired);
            OpenCage();
        }

        else
        {
            Debug.Log("more keys required");
        }
    }

    private async void OpenCage()
    {
        Debug.Log("cage open");

        buttonTxt.text = "Unlocked";

        cage.SetActive(false);
        creature.GetComponent<Animator>().SetBool("Running", true);

        InitGuards();
        while (creature.transform.position != finalPos.position)
        {
            var targetPosition =
                Vector3.MoveTowards(creature.transform.position, finalPos.position, speed * Time.deltaTime);
            creature.transform.rotation = Quaternion.LookRotation(targetPosition - creature.transform.position);
            creature.transform.position = targetPosition;

            await Task.Yield();
        }

        MapManager.instance.OnUnlock();
        Destroy(creature.gameObject);
    }

    private async void InitGuards()
    {
        await Task.Delay(300);
        foreach (var guard in guards)
        {
            guard.Run(soliderFinalPos.position);
        }
    }
}