using System;
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
        public event Action OnStartGame;
        public event Action OnPause;
        public event Action OnResume;

        private static MenuScreen _instance;
        public static MenuScreen Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<MenuScreen>();
                }

                return _instance;
            }
        }

        private bool _isPaused;

        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                _isPaused = value;
                if (_isPaused)
                {
                    Time.timeScale = 0;
                    OnPause?.Invoke();
                }
                else
                {
                    Time.timeScale = 1;
                    OnResume?.Invoke();
                }
            }
        }

        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Screens")]
        [SerializeField] private RectTransform screenMenu;
        [SerializeField] private RectTransform screenControls;
        [SerializeField] private RectTransform screenCredits;
        [SerializeField] private RectTransform screenSettings;
        [SerializeField] private RectTransform screenGameGUI;
        [SerializeField] private ComicsScreen comicsScreen;
        [SerializeField] public GameUI GameUI;

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

        private static int _comicsShownTimes = 0;
        private static int _comicsMaxShownTimes = 1;

        private void Awake()
        {
            _instance = this;
            canvasGroup.alpha = 1;
            tweenMenuStart();
        }

        private void Start()
        {
            IsPaused = true;
            SoundManager.Instance.playMenuMusic();
            musicVolumeSlider.onValueChanged.AddListener(SoundManager.Instance.setMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SoundManager.Instance.setSFXVolume);

            comicsScreen.gameObject.SetActive(false);
            musicVolumeSlider.value = (musicVolumeSlider.maxValue - musicVolumeSlider.minValue) * 0.65f;
            sfxVolumeSlider.value = (sfxVolumeSlider.maxValue - sfxVolumeSlider.minValue) * 0.5f;
        }


        [Button]
        public void startGame()
        {
            GameUI.SetScoreInstant(0);

            SoundManager.Instance.fadeToGameMusic();
            canvasGroup.blocksRaycasts = false;
            Sequence sequence = DOTween.Sequence().SetUpdate(true);
            sequence.Append(canvasGroup.DOFade(0.0f, tweenDuration).SetEase(Ease.InQuart).SetUpdate(true));
            screenGameGUI.anchoredPosition = new Vector2(-2560, screenGameGUI.anchoredPosition.y);
            sequence.Join(screenGameGUI.DOAnchorPosX(0, tweenDuration).SetEase(tweenEase).SetUpdate(true));
            
            if (_comicsShownTimes < _comicsMaxShownTimes)
            {
                sequence.Append(comicsScreen.ShowComicsScreen().OnComplete(() => _comicsShownTimes++));
            }

            sequence.OnComplete(() =>
            {
                IsPaused = false;
                OnStartGame?.Invoke();
            });
        }

        [Button]
        public void showAfterLost()
        {
            IsPaused = true;
            SoundManager.Instance.fadeToMenuMusic();
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1.0f;
            screenMenu.anchoredPosition = new Vector2(2560, screenMenu.anchoredPosition.y);
            screenMenu.DOAnchorPosX(0, tweenDuration).SetEase(tweenEase).SetUpdate(true);
        }

        [Button]
        public void showCredits()
        {
            screenMenu.DOAnchorPosX(-2560.0f, tweenDuration).SetEase(tweenEase).SetUpdate(true);
            screenCredits.DOAnchorPosX(0, tweenDuration).SetEase(tweenEase).SetUpdate(true);
        }

        [Button]
        public void showControls()
        {
            screenMenu.DOAnchorPosX(-2560.0f, tweenDuration).SetEase(tweenEase).SetUpdate(true);
            screenControls.DOAnchorPosX(0, tweenDuration).SetEase(tweenEase).SetUpdate(true);
        }

        [Button]
        public void showSettings()
        {
            screenMenu.DOAnchorPosX(-2560.0f, tweenDuration).SetEase(tweenEase).SetUpdate(true);
            screenSettings.DOAnchorPosX(0, tweenDuration).SetEase(tweenEase).SetUpdate(true);
        }

        [Button]
        public void backToMenuFromCredits()
        {
            screenMenu.DOAnchorPosX(0.0f, tweenDuration).SetEase(tweenEase).SetUpdate(true);
            screenCredits.DOAnchorPosX(2560.0f, tweenDuration).SetEase(tweenEase).SetUpdate(true);
        }

        [Button]
        public void backToMenuFromControls()
        {
            screenMenu.DOAnchorPosX(0.0f, tweenDuration).SetEase(tweenEase).SetUpdate(true);
            screenControls.DOAnchorPosX(2560.0f, tweenDuration).SetEase(tweenEase).SetUpdate(true);
        }

        [Button]
        public void backToMenuFromSettings()
        {
            screenMenu.DOAnchorPosX(0.0f, tweenDuration).SetEase(tweenEase).SetUpdate(true);
            screenSettings.DOAnchorPosX(2560.0f, tweenDuration).SetEase(tweenEase).SetUpdate(true);
        }

        private void tweenMenuStart()
        {
            Vector2 game_name_anchored_pos = rtGameTitle.anchoredPosition;
            Vector2 buttons_anchored_pos   = rtMenuButtons.anchoredPosition;
            Sequence sequence = DOTween.Sequence().SetUpdate(true);
            sequence.AppendCallback(
                () => {
                    rtGameTitle.anchoredPosition += Vector2.left * 100;
                    rtMenuButtons.anchoredPosition += Vector2.down * 100;

                    txtGameTitle.alpha = 0.0f;
                    cgMenuButtons.alpha = 0.0f;
                    //buttonsCanvasGroup.interactable = false;
                })
                .Append(txtGameTitle.DOFade(1.0f, 1.0f).SetEase(tweenEase).SetUpdate(true))
                .Join(rtGameTitle.DOAnchorPos(game_name_anchored_pos, tweenDuration/2.0f).SetEase(tweenEase).SetUpdate(true))
                .Append(rtMenuButtons.DOAnchorPos(buttons_anchored_pos, tweenDuration/2.0f).SetEase(tweenEase).SetUpdate(true))
                .Join(cgMenuButtons.DOFade(1.0f, 1.0f).SetEase(tweenEase).SetUpdate(true))
                .AppendCallback(
                    () => {
                        //buttonsCanvasGroup.interactable = true;
                    })
                .SetUpdate(true)
                .Play();
        }
    }
}
