using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

struct ScrollerInstance
{
    public GameObject instance;
    public Rigidbody2D rigidbody;

    public ScrollerInstance(GameObject instance)
    {
        this.instance = instance;
        this.rigidbody = instance.GetComponent<Rigidbody2D>();
    }
}

public class EndlessScroller : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float spawnIntervalMin;
    [SerializeField] private float spawnIntervalMax;
    [SerializeField] private float spawnOffset;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float altitude;


    private List<ScrollerInstance> instances = new List<ScrollerInstance>();
    private float nextSpawnPointLeft;
    private float nextSpawnPointRight;
    private Vector3 lastPosition = Vector3.zero;

    private float HalfVieportWidth
    {
        get
        {
            float orthographicSize = Camera.main.orthographicSize;
            float aspectRatio = Camera.main.aspect;
            float halfWidth = orthographicSize * aspectRatio;
            // float halfHeight = orthographicSize;
            return halfWidth;
        }
    }

    private float LeftBound => transform.position.x - HalfVieportWidth - spawnOffset;
    private float RightBound => transform.position.x + HalfVieportWidth + spawnOffset;

    private void SpawnInstance(float positionX)
    {
        Vector3 position = new(positionX, altitude, 0);
        GameObject instance = Instantiate(prefab, position, Quaternion.identity);
        instances.Add(new ScrollerInstance(instance));
    }

    private void DespawnInstance(int i)
    {
        Destroy(instances[i].instance);
        instances.RemoveAt(i);
    }

    private float RandomNextDistance()
    {
        return Random.Range(spawnIntervalMin, spawnIntervalMax);
    }

    private void Awake()
    {
        nextSpawnPointLeft = -RandomNextDistance();
        nextSpawnPointRight = RandomNextDistance();
    }

    private void Update()
    {
        Vector3 positionDelta = transform.position - lastPosition;
        float movementDelta = movementSpeed * Time.deltaTime;

        nextSpawnPointLeft -= movementDelta;
        nextSpawnPointRight -= movementDelta;

        float rightBound = RightBound;
        float leftBound = LeftBound;

        if (nextSpawnPointRight <= rightBound)
        {
            SpawnInstance(nextSpawnPointRight);
            nextSpawnPointRight += RandomNextDistance();
        }

        if (nextSpawnPointLeft >= leftBound)
        {
            SpawnInstance(nextSpawnPointLeft);
            nextSpawnPointLeft -= RandomNextDistance();
        }

        for (int i = instances.Count - 1; i >= 0; i--)
        {
            ScrollerInstance scrollerInstance = instances[i];
            Vector3 delta = new Vector3(movementDelta, 0, 0);

            if (scrollerInstance.rigidbody) scrollerInstance.rigidbody.MovePosition(scrollerInstance.rigidbody.position + (Vector2)delta);
            else scrollerInstance.instance.transform.position += delta;

            float instanceX = scrollerInstance.instance.transform.position.x;

            if (instanceX > rightBound)
            {
                DespawnInstance(i);
                nextSpawnPointRight = instanceX;
            }

            if (instanceX < leftBound)
            {
                DespawnInstance(i);
                nextSpawnPointLeft = instanceX;
            }
        }

        lastPosition = transform.position;
    }

    // private void OnDrawGizmos()
    // {
    //     Vector3 startPoint = transform.position;
    //     startPoint.x += spawnAtX;
    //     Vector3 endPoint = startPoint;
    //     endPoint.x -= movementDistance;
    //     Gizmos.DrawLine(endPoint, startPoint);
    // }
}