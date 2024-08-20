using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    public static PlayerContext instance;

    // public bool IsInDeathZone => SceneRestarter.instance.IsInDeathZone;
    // public float DurationInDeathZone => SceneRestarter.instance.DurationInDeathZone;
    // public bool IsTitan => PlayerTitanTransformation.instance.IsTitan;
    // public bool TransformInProgressIsTitan => PlayerTitanTransformation.instance.TransformInProgressIsTitan;
    public float Speed => _playerRigidbody.velocity.magnitude;
    public static float SpeedToKillTitan => 40f;

    Rigidbody2D _playerRigidbody;

    private void Awake()
    {
        instance = this;
        _playerRigidbody = GetComponent<Rigidbody2D>();
    }
}