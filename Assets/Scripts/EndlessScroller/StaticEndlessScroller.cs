using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEndlessScroller : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] float _interval = 10f;
    [SerializeField] float _altitude;

    GameObject[] _instances = new GameObject[3];
    int _currentIndex;

    void Awake()
    {
        for (int i = 0; i < _instances.Length; i++)
        {
            _instances[i] = Instantiate(_prefab, new Vector3((i - 1) * _interval, _altitude, 0), Quaternion.identity);
        }
    }

    void Update()
    {
        float normalizedCoordinate = transform.position.x / _interval - 0.5f;
        int currentMiddleCoord = (int)Mathf.Floor(normalizedCoordinate);
        // if (normalizedCoordinate < 0f) currentMiddleIndex++;

        Debug.Log(currentMiddleCoord);

        for (int i = 0; i < _instances.Length; i++)
        {
            int currentCoord = currentMiddleCoord + i;
            float currentX = currentCoord * _interval;

            int currentIndex = (currentCoord % 3 + 3) % 3;
            // if (currentCoord < 0) currentIndex += 3;

            if (!Mathf.Approximately(currentX, _instances[currentIndex].transform.position.x))
            {
                Vector3 position = _instances[currentIndex].transform.position;
                position.x = currentX;
                _instances[currentIndex].transform.position = position;
            }
        }
    }
}
