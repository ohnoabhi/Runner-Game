using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ClaimAnimationManager : MonoBehaviour
{
    private static ClaimAnimationManager instance;
    [SerializeField] private Image iconPrefab;
    private List<Image> availableItems;
    private List<Image> usingItems;

    private void Awake()
    {
        instance = this;
        availableItems = new List<Image>();
        usingItems = new List<Image>();
    }

    private Image GetItem()
    {
        if (availableItems.Count > 0)
        {
            var image = availableItems[0];
            availableItems.Remove(image);
            usingItems.Add(image);
            return image;
        }
        else
        {
            var image = Instantiate(instance.iconPrefab, instance.transform);
            usingItems.Add(image);
            return image;
        }
    }

    public static async Task SpawnIcon(CollectableType type, Vector3 startPosition, Vector2 startSize,
        Action onComplete = null)
    {
        var endPosition = TopHudController.instance.GetIconPos(type);
        var endSize = TopHudController.instance.GetIconSizeDelta(type);

        var delay = 100;
        var sprite = CollectablesManager.GetIcon(type);
        var tasks = new List<Task>();
        var audio = CollectablesManager.GetAudioName(type);
        for (var i = 0; i < 10; i++)
        {
            var image = instance.GetItem();
            image.gameObject.SetActive(true);
            image.sprite = sprite;
            image.rectTransform.sizeDelta = startSize;
            image.transform.position = startPosition;
            tasks.Add(MoveCoin(image, endPosition, endSize, audio));
            delay -= 10;
            if (delay < 0) delay = 0;
            await Task.Delay(delay);
        }

        await Task.WhenAll(tasks);
        onComplete?.Invoke();
    }

    private static async Task MoveCoin(Image item, Vector3 target, Vector2 targetSize, string audio)
    {
        item.transform.DOMove(target, 0.5f);
        await item.rectTransform.DOSizeDelta(targetSize, 0.5f).AsyncWaitForCompletion();
        AudioManager.Play(audio);
        item.gameObject.SetActive(false);
        instance.usingItems.Remove(item);
        instance.availableItems.Add(item);
    }
}