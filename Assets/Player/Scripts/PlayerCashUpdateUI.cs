using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerCashUpdateUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro TextTransform;
    private List<TextMeshPro> availableTexts = new List<TextMeshPro>();
    private List<TextMeshPro> usingTexts = new List<TextMeshPro>();

    public async void Show(Transform parent, float yStartPos)
    {
        var text = GetText();
        text.transform.SetParent(parent);
        text.gameObject.SetActive(false);
        text.transform.DOLocalMove(new Vector3(0, yStartPos, 0), 0);
        text.gameObject.SetActive(true);
        text.DOFade(0, 0);
        await Task.Yield();
        text.DOFade(1, 0.2f);
        await Task.Delay(200);
        text.transform.DOLocalMoveY(yStartPos + 1.2f, 1);
        text.DOFade(0, 1f);
        await Task.Delay(1000);
        text.gameObject.SetActive(false);
        text.transform.SetParent(null);
        availableTexts.Add(text);
        usingTexts.Remove(text);
    }

    private TextMeshPro GetText()
    {
        if (availableTexts.Count > 0)
        {
            var text = availableTexts[0];
            usingTexts.Add(text);
            availableTexts.RemoveAt(0);
            return text;
        }
        else
        {
            var text = Instantiate(TextTransform);
            usingTexts.Add(text);
            return text;
        }
    }
}