using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestarter : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float deathAltitude;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private float restartAfterTimeInDeathZone = 1.0f;

    private bool isInDeathZone = false;
    private float durationInDeathZone = 0.0f;

    public static SceneRestarter instance;

    public bool IsInDeathZone => isInDeathZone;
    public float DurationInDeathZone => durationInDeathZone;

    public void SetPlayerDied()
    {
        isInDeathZone = true;
    }

    private void Awake()
    {
        instance = this;
    }

    private void LateUpdate()
    {
        bool higherThanDeathAltitude = target.position.y > deathAltitude;

        if (higherThanDeathAltitude && !isInDeathZone)
            return;

        if (!higherThanDeathAltitude && !isInDeathZone)
        {
            Instantiate(deathParticles, target.position, Quaternion.identity);
        }

        isInDeathZone = true;

        durationInDeathZone += Time.deltaTime;

        if (durationInDeathZone > restartAfterTimeInDeathZone)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }
}