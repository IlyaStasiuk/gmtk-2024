using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestarter : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float deathAltitude;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private float restartAfterTimeInDeathZone = 1.0f;

    private bool isDead = false;
    private float durationInDeathZone = 0.0f;

    public static SceneRestarter instance;

    public bool IsInDeathZone => isDead;
    public float DurationInDeathZone => durationInDeathZone;

    public void SetPlayerDied()
    {
        isDead = true;
        SoundManager.Instance.playSound(SoundType.TITAN_SCREAM_1);
    }

    void DeathByFall()
    {
        SetPlayerDied();
        Instantiate(deathParticles, target.position, Quaternion.identity);
    }

    private void Awake()
    {
        instance = this;
    }

    private void LateUpdate()
    {
        bool lowerThanDeathAltitude = target.position.y < deathAltitude;

        if (!isDead && lowerThanDeathAltitude)
        {
            DeathByFall();
        }

        if (isDead)
        {
            durationInDeathZone += Time.deltaTime;

            if (durationInDeathZone > restartAfterTimeInDeathZone)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            }
        }
    }
}