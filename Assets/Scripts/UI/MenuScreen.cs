using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Menu
{
    public class MenuScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Screens")]
        [SerializeField] private RectTransform screenMenu;
        [SerializeField] private RectTransform screenControls;
        [SerializeField] private RectTransform screenCredits;
        [SerializeField] private RectTransform screenSettings;

        [Header("Screen Settings")]
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;

        [Header("Screen Menu")]
        [SerializeField] private TMP_Text txtGameTitle;
        [SerializeField] private RectTransform rtGameTitle;
        [SerializeField] private RectTransform rtMenuButtons;
        [SerializeField] private CanvasGroup cgMenuButtons;

        [Header("Tween Settings")]
        [SerializeField] private float tweenDuration = 2.0f;
        [SerializeField] private Ease tweenEase = Ease.InOutQuad;

        private void Awake()
        {
            canvasGroup.alpha = 1;
            tweenMenuStart();
        }

        private void Start()
        {
            SoundManager.Instance.playMenuMusic();
            musicVolumeSlider.onValueChanged.AddListener(SoundManager.Instance.setMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SoundManager.Instance.setSFXVolume);
        }


        [Button]
        public void startGame()
        {
            SoundManager.Instance.fadeToGameMusic();
            canvasGroup.blocksRaycasts = false;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(canvasGroup.DOFade(0.0f, tweenDuration).SetEase(Ease.InQuart));
        }

        [Button]
        public void showAfterLost()
        {
            SoundManager.Instance.fadeToMenuMusic();
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1.0f;
            screenMenu.anchoredPosition = new Vector2(2560, screenMenu.anchoredPosition.y);
            screenMenu.DOAnchorPosX(0, tweenDuration).SetEase(tweenEase);
        }

        [Button]
        public void showCredits()
        {
            screenMenu.DOAnchorPosX(-2560.0f, tweenDuration).SetEase(tweenEase);
            screenCredits.DOAnchorPosX(0, tweenDuration).SetEase(tweenEase);
        }

        [Button]
        public void showControls()
        {
            screenMenu.DOAnchorPosX(-2560.0f, tweenDuration).SetEase(tweenEase);
            screenControls.DOAnchorPosX(0, tweenDuration).SetEase(tweenEase);
        }

        [Button]
        public void showSettings()
        {
            screenMenu.DOAnchorPosX(-2560.0f, tweenDuration).SetEase(tweenEase);
            screenSettings.DOAnchorPosX(0, tweenDuration).SetEase(tweenEase);
        }

        [Button]
        public void backToMenuFromCredits()
        {
            screenMenu.DOAnchorPosX(0.0f, tweenDuration).SetEase(tweenEase);
            screenCredits.DOAnchorPosX(2560.0f, tweenDuration).SetEase(tweenEase);
        }

        [Button]
        public void backToMenuFromControls()
        {
            screenMenu.DOAnchorPosX(0.0f, tweenDuration).SetEase(tweenEase);
            screenControls.DOAnchorPosX(2560.0f, tweenDuration).SetEase(tweenEase);
        }

        [Button]
        public void backToMenuFromSettings()
        {
            screenMenu.DOAnchorPosX(0.0f, tweenDuration).SetEase(tweenEase);
            screenSettings.DOAnchorPosX(2560.0f, tweenDuration).SetEase(tweenEase);
        }

        private void tweenMenuStart()
        {
            Vector2 game_name_anchored_pos = rtGameTitle.anchoredPosition;
            Vector2 buttons_anchored_pos   = rtMenuButtons.anchoredPosition;
            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(
                () => {
                    rtGameTitle.anchoredPosition += Vector2.left * 100;
                    rtMenuButtons.anchoredPosition += Vector2.down * 100;

                    txtGameTitle.alpha = 0.0f;
                    cgMenuButtons.alpha = 0.0f;
                    //buttonsCanvasGroup.interactable = false;
                })
                .Append(txtGameTitle.DOFade(1.0f, 1.0f).SetEase(Ease.OutQuad))
                .Join(rtGameTitle.DOAnchorPos(game_name_anchored_pos, tweenDuration/2.0f).SetEase(Ease.OutQuad))
                .Append(rtMenuButtons.DOAnchorPos(buttons_anchored_pos, tweenDuration/2.0f).SetEase(Ease.OutQuad))
                .Join(cgMenuButtons.DOFade(1.0f, 1.0f).SetEase(Ease.OutQuad))
                .AppendCallback(
                    () => {
                        //buttonsCanvasGroup.interactable = true;
                    })
                .Play();
        }
    }
}
