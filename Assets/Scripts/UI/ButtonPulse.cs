using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPulse : MonoBehaviour
{
    private void Start()
    {
        pulse();
    }

    public void pulse()
    {
        transform.DOScale(1.2f, 2.0f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
