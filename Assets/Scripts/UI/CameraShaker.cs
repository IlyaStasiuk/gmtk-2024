using DG.Tweening;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance { get; private set; }

    [SerializeField] private Camera cum;

    private void Awake()
    {
        Instance = this;
    }

    public void Shake(float power = 1)
    {
        if (DOTween.IsTweening(transform))
            return;

        transform.DOShakeRotation(1.0f, new Vector3(0, 0, power)).SetUpdate(true);
        transform.DOShakePosition(1.0f, power / 5.0f).SetUpdate(true);
    }
}
