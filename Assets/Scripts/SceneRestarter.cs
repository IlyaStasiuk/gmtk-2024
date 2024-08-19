using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestarter : MonoBehaviour
{
        [SerializeField] private Transform target;
        [SerializeField] private float altitude;

        private void LateUpdate()
        {
                if(target.transform.position.y > altitude)
                        return;

                SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
}