using UnityEngine;

public class PlayerScream : MonoBehaviour
{
    [SerializeField] Rigidbody2D _playerRigidbody;
    [SerializeField] float _height = 80f;
    [SerializeField] float _speed = 40f;
    [SerializeField] float _delay = 5f;

    float _last;

    private void Update()
    {
        if (Time.time < _last + _delay) return;

        float speed = _playerRigidbody.velocity.magnitude;
        float height = _playerRigidbody.position.y;
        if (height >= _height && speed >= _speed)
        {
            SoundManager.Instance.playSound(SoundType.TITAN_SCREAM_2);
            _last = Time.time;
        }
    }
}