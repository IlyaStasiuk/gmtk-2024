using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Menu;
using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
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
        statisticsText.SetText($"And slayed {killed_count} monster{(killed_count > 1 ? "s" : "")} on the way");
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1.0f, 2.5f));
        sequence.AppendInterval(0.5f);
        sequence.Append(statisticsText.DOFade(1.0f, 1.0f).SetEase(Ease.OutSine));
        var button_tween = menuButtonCanvasGroup.DOFade(1.0f, 1.0f).SetEase(Ease.InQuad);
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
        rectTransform.DOAnchorPosX(-2560, 2.0f).SetEase(Ease.InOutQuad);
        FindObjectOfType<MenuScreen>().showAfterLost();
        menuButtonCanvasGroup.blocksRaycasts = false;
    }

    private void Awake()
    {
        canvasGroup.alpha = 0.0f;
    }
}
