using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private float _effectAnimDuration = 0.2f;
    [SerializeField] private float _textAnimDuration = 0.2f;
    [SerializeField] private float _tweenMaxScaleSize = 1.5f;
    [SerializeField] private Ease _scoreTweenEase = Ease.InOutBack;
    
    [SerializeField] private Color _scoreTweenColor = new Color(0.56f, 0.98f, 1f);
    [SerializeField] private Color _scoreTweenColorSmallTitan = new Color(0.69f, 1f, 0.02f);
    [SerializeField] private Color _scoreTweenColorMediumTitan = new Color(1f, 0.78f, 0f);
    [SerializeField] private Color _scoreTweenColorBigTitan = new Color(1f, 0.2f, 0f);


    private int _realScore;
    private int _tweenedScore;

    public void AddScore(int score)
    {
        var color = score switch
        { 
            100 => _scoreTweenColorSmallTitan,
            500 => _scoreTweenColorMediumTitan,
            1000 => _scoreTweenColorBigTitan,
            _ => _scoreTweenColor
        };

        TweenScore(_realScore + score, color);
    }

    public void SetScoreInstant(int score)
    {
        _realScore = score;
        _tweenedScore = score;
        SetScore(score);
    }

    private void SetScore(int score)
    {
        _scoreText.text = score.ToString();
    }
    
    private void TweenScore(int score, Color color)
    {
        _realScore = score;

        var colorTween = DOTween.Sequence();
        var scaleTween = DOTween.Sequence();
        
        colorTween.Append(DOTextMeshProColor(color, _effectAnimDuration / 2.0f));
        colorTween.Append(DOTextMeshProColor(Color.white, _effectAnimDuration / 2.0f));

        scaleTween.Append(_scoreText.transform.DOScale(_tweenMaxScaleSize, _effectAnimDuration / 2.0f).SetEase(_scoreTweenEase));
        scaleTween.Append(_scoreText.transform.DOScale( 1, _effectAnimDuration / 2.0f).SetEase(_scoreTweenEase));

        //tween _scoreText.text with score value
        DOTween.Sequence()
            .Join(DOScoreText(_realScore, _textAnimDuration))
            .Join(colorTween)
            .Join(scaleTween);
    }
    
    private Tween DOTextMeshProColor(Color color, float duration)
    {
        return DOTween.To(() => _scoreText.color, x => _scoreText.color = x, color, duration).SetEase(_scoreTweenEase);
    }
    
    private Tween DOScoreText(int score, float duration)
    {
        return DOTween.To(() => _tweenedScore, x => _tweenedScore = x, score, duration).SetEase(_scoreTweenEase).OnUpdate(() => SetScore(_tweenedScore));
    }

    [Button]
    private void CheatAddScore()
    {
        AddScore(100);
    }
}
