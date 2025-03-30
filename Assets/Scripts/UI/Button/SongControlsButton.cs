using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SongControlsButton : MonoBehaviour
{
    Button button;
    private Vector3 originalScale;
    private void Awake()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;
    }
    public void ClickEffect()
    {
        Sequence bounceSequence = DOTween.Sequence();

        bounceSequence.Append(transform.DOScale(originalScale * 1.2f, 0.1f)
                                     .SetEase(Ease.OutBack));
        bounceSequence.Append(transform.DOScale(originalScale, 0.1f)
                                     .SetEase(Ease.InBack));
    }
}
