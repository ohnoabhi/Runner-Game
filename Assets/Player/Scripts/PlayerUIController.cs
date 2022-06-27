using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    private new Transform camera;
    private int currentHealth;
    [SerializeField] private TextMeshProUGUI updateText;

    private void OnEnable()
    {
        // GameManager.OnPlayerHealthChange += SetPlayerHealth;
        updateText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        // GameManager.OnPlayerHealthChange -= SetPlayerHealth;
    }

    public void SetCamera(Transform camera)
    {
        this.camera = camera;
    }

    private bool isPlaying = false;
    private float timer = 0;

    private void Update()
    {
        if (camera) transform.forward = camera.forward;

        if (isPlaying && timer <= 0)
        {
            animationCoroutine = StartCoroutine(AnimateTextHide());
        }

        if (isPlaying && timer > 0) timer -= Time.deltaTime;
    }

    private int damage;

    [Button]
    public void SetPlayerHealth(int health)
    {
        var diff = health - currentHealth;
        if (diff == 0) return;

        if (damage > 0 && diff < 0 || damage < 0 && diff > 0) damage = diff;
        else damage += diff;

        updateText.text = (damage > 0 ? "+" : "") + damage;
        updateText.color = (damage > 0 ? Color.green : Color.red);
        if (currentHealth > 0)
        {
            currentHealth = health;
            healthText.text = currentHealth.ToString();
            healthText.transform.DOScale(0.8f, 0.1f).OnComplete(() =>
            {
                healthText.transform.DOScale(1.2f, 0.3f).OnComplete(() =>
                {
                    healthText.transform.DOScale(1f, 0.2f);
                });
            });
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);
            if (gameObject.activeInHierarchy)
                animationCoroutine = StartCoroutine(AnimateTextShow());
        }
        else
        {
            currentHealth = health;
            healthText.text = currentHealth.ToString();
        }

        // healthText.text = health.ToString();
    }

    private Coroutine animationCoroutine;


    private IEnumerator AnimateTextShow()
    {
        isPlaying = true;
        timer = 1;
        updateText.gameObject.SetActive(true);
        updateText.rectTransform.anchoredPosition = new Vector2(updateText.rectTransform.anchoredPosition.x, -60);
        updateText.color = new Color(updateText.color.r, updateText.color.g, updateText.color.b, 0);
        updateText.transform.localScale = Vector3.zero;
        var t = 0f;

        while (t < 1)
        {
            updateText.transform.localScale =
                Vector3.Lerp(updateText.transform.localScale, Vector3.one, t);
            updateText.color = new Color(updateText.color.r, updateText.color.g, updateText.color.b, t);
            t += 5 * Time.deltaTime;
            yield return null;
        }

        damage = 0;
    }

    private IEnumerator AnimateTextHide()
    {
        isPlaying = false;
        timer = 1;

        var t = 0f;
        var position = new Vector2(updateText.rectTransform.anchoredPosition.x, -125);
        while (updateText.rectTransform.anchoredPosition != position)
        {
            updateText.rectTransform.anchoredPosition =
                Vector2.Lerp(updateText.rectTransform.anchoredPosition, position, t);
            t += 5 * Time.deltaTime;
            updateText.color = new Color(updateText.color.r, updateText.color.g, updateText.color.b, 1 - t);
            yield return null;
        }

        updateText.gameObject.SetActive(false);
        animationCoroutine = null;
        damage = 0;
        isPlaying = false;
    }

    private IEnumerator AnimateText()
    {
        healthText.text = currentHealth.ToString();
        healthText.transform.DOScale(0.8f, 0.1f).OnComplete(() =>
        {
            healthText.transform.DOScale(1.2f, 0.3f).OnComplete(() => { healthText.transform.DOScale(1f, 0.2f); });
        });
        updateText.rectTransform.DOAnchorPosY(-60, 0);
        updateText.DOFade(0, 0.2f);
        yield return updateText.transform.DOScale(Vector3.zero, .2f).WaitForCompletion();
        updateText.gameObject.SetActive(true);
        updateText.transform.DOScale(Vector3.one, 0.5f);
        yield return updateText.DOFade(1, 0.5f).WaitForCompletion();
        updateText.rectTransform.DOAnchorPosY(-125, 0.5f);
        yield return updateText.DOFade(0, 0.5f).WaitForCompletion();
        // yield return new WaitForSeconds(0.4f);
        updateText.gameObject.SetActive(false);
        animationCoroutine = null;
        damage = 0;
    }

    private IEnumerator ContinuesAnimateText()
    {
        healthText.text = currentHealth.ToString();
        updateText.gameObject.SetActive(true);
        updateText.rectTransform.DOAnchorPosY(-125, 0.5f);
        yield return updateText.DOFade(0, 0.5f).WaitForCompletion();
        // yield return new WaitForSeconds(0.4f);
        updateText.gameObject.SetActive(false);
        animationCoroutine = null;
        damage = 0;
    }

    public void SetTextColor(Color color)
    {
        healthText.color = color;
    }
}