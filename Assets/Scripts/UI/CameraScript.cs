using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Camera camera;


    public void tweenDamageTaken()
    {
        if (DOTween.IsTweening(camera))
            return;
        camera.DOShakeRotation(1.0f, new Vector3(0, 0, 1));
        camera.DOShakePosition(1.0f, 0.2f);
    }
}
