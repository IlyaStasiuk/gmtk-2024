using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class ComicsScreen : MonoBehaviour
{
    [SerializeField] private List<Image> _comicsImages;
    [SerializeField] private List<SoundType> _comicsSounds;
    [SerializeField] private float _timeBetweenComics = 3f;

    public Tween ShowComicsScreen()
    {
        gameObject.SetActive(true);
        var sequence = DOTween.Sequence();
        //show images with fade in and out one by one using DOTween
        for (int i = 0; i < _comicsImages.Count; i++)
        {
            var iCached = i;
            _comicsImages[i].color = new Color(1, 1, 1, 0);
            sequence.Append(_comicsImages[iCached].DOFade(1, 1));
            sequence.AppendCallback(() => SoundManager.Instance.playSound(_comicsSounds[iCached]));
            sequence.AppendInterval(_timeBetweenComics);
            sequence.Append(_comicsImages[iCached].DOFade(0, 1));
        }

        sequence.AppendCallback(() => gameObject.SetActive(false));

        return sequence;
    }
    
}
