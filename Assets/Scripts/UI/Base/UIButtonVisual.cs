using UnityEngine;
using UnityEngine.UI;

public class UIButtonVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image targetImage;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightedColor = Color.yellow;
    [SerializeField] private Color pressedColor = Color.gray;

    [Header("Optional Scale Effect")]
    [SerializeField] private bool useScaleEffect = true;
    [SerializeField] private float highlightedScale = 1.1f;

    private Vector3 originalScale;

    private void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponentInChildren<Image>();

        originalScale = transform.localScale;
        SetNormal();
    }

    public void SetHighlighted(bool value)
    {
        if (value)
        {
            targetImage.color = highlightedColor;

            if (useScaleEffect)
                transform.localScale = originalScale * highlightedScale;
        }
        else
        {
            SetNormal();
        }
    }

    public void PlayPressed()
    {
        targetImage.color = pressedColor;
    }

    private void SetNormal()
    {
        targetImage.color = normalColor;

        if (useScaleEffect)
            transform.localScale = originalScale;
    }
}