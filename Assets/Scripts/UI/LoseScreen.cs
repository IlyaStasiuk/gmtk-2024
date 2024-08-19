using System;
using DG.Tweening;
using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TMP_Text      statisticsText;
    [SerializeField] private CanvasGroup   canvasGroup;
    [SerializeField] private CanvasGroup   menuButtonCanvasGroup;

    private GameObject ui_panel_runes_go;


    public void show(int killed_count)
    {
        SoundManager.Instance.stopAllMusic();
        rectTransform.anchoredPosition = Vector2.zero;
        statisticsText.SetText($"... {(killed_count > 0 ? "but" : "and")} took {killed_count} monster{(killed_count > 1 ? "s" : "")} with you");
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1.0f, 2.5f).SetUpdate(true));
        sequence.AppendInterval(0.5f);
        sequence.Append(statisticsText.DOFade(1.0f, 1.0f).SetEase(Ease.OutSine).SetUpdate(true));
        var button_tween = menuButtonCanvasGroup.DOFade(1.0f, 1.0f).SetEase(Ease.InQuad).SetUpdate(true);
        button_tween.onComplete += () =>
        {
            menuButtonCanvasGroup.blocksRaycasts = true;
        };
        sequence.Append(button_tween);
        sequence.Play();
    }

    public void showMenu()
    {
        SoundManager.Instance.fadeToMenuMusic();
        rectTransform.DOAnchorPosX(-2560, 2.0f).SetEase(Ease.InOutQuad).SetUpdate(true);
        FindObjectOfType<MenuScreen>().showAfterLost();
        menuButtonCanvasGroup.blocksRaycasts = false;
    }

    private void Awake()
    {
        canvasGroup.alpha = 0.0f;
    }
}
