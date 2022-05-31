using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;

public class CreatureItem : MonoBehaviour
{
    [BoxGroup("Lock")] [SerializeField] int keysRequired = 3;
    [BoxGroup("Lock")] [SerializeField] private GameObject lockedObj;
    [BoxGroup("Lock")] [SerializeField] private GameObject unlockedObj;
    [BoxGroup("Lock")] [SerializeField] private TextMeshProUGUI requiredText;

    [FoldoutGroup("Creature")] [SerializeField]
    Transform creatureParent;

    [FoldoutGroup("Creature")] [SerializeField]
    float speed;

    [FoldoutGroup("Creature")] [SerializeField]
    Transform finalPos;

    [FoldoutGroup("Creature")] [SerializeField]
    GameObject cage;

    [FoldoutGroup("Soldier")] [SerializeField]
    private CageGuard[] guards;

    [FoldoutGroup("Soldier")] [SerializeField]
    private Transform soliderFinalPos;


    [HideInInspector] public string CreatureKey;

    public bool IsUnlocked
    {
        get => PlayerPrefs.GetInt(
            CreatureKey, 0) == 1;

        private set => PlayerPrefs.SetInt(
            CreatureKey, value ? 1 : 0);
    }

    private int CreatureIndex
    {
        get => PlayerPrefs.GetInt(CreatureKey + "Index", -1);
        set => PlayerPrefs.SetInt(CreatureKey + "Index", value);
    }

    private void Start()
    {
        if (CreatureIndex < 0)
        {
            CreatureIndex = CreatureData.Instance.RandomIndex;
        }

        if (CreatureIndex >= 0)
        {
            foreach (Transform child in creatureParent)
            {
                Destroy(child.gameObject);
            }

            var creatureInstance = Instantiate(CreatureData.Instance.ReturnCreature(CreatureIndex), creatureParent);
            creatureInstance.transform.localPosition = Vector3.zero;
            creatureInstance.transform.localRotation = Quaternion.identity;
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
        lockedObj.SetActive(false);
        unlockedObj.SetActive(true);

        cage.SetActive(true);
        cage.transform.DOMoveY(9, 0);
        creatureParent.gameObject.SetActive(false);

        foreach (var cageGuard in guards)
        {
            cageGuard.gameObject.SetActive(false);
        }
    }

    private void CageLocked()
    {
        requiredText.text = "Requires " + keysRequired;
        lockedObj.SetActive(true);
        unlockedObj.SetActive(false);
    }

    public void OnClickUnlock()
    {
        var keys = CollectablesManager.Get(CollectableType.Key);

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
        lockedObj.SetActive(false);

        cage.transform.DOMoveY(9, 1f);

        creatureParent.GetComponentInChildren<Animator>().SetBool("Running", true);

        InitGuards();
        while (creatureParent.transform.position != finalPos.position)
        {
            var targetPosition =
                Vector3.MoveTowards(creatureParent.transform.position, finalPos.position, speed * Time.deltaTime);
            creatureParent.transform.rotation =
                Quaternion.LookRotation(targetPosition - creatureParent.transform.position);
            creatureParent.transform.position = targetPosition;

            await Task.Yield();
        }

        unlockedObj.SetActive(true);

        MapManager.instance.OnUnlock();
        Destroy(creatureParent.gameObject);
    }

    private async void InitGuards()
    {
        await Task.Delay(1200);
        foreach (var guard in guards)
        {
            guard.Run(soliderFinalPos.position);
        }
    }
}