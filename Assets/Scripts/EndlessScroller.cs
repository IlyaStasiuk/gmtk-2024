using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

struct ScrollerInstance
{
    public GameObject instance;
    public Rigidbody2D rigidbody;
    public float movementLeft;

    public ScrollerInstance(GameObject instance, float movementLeft)
    {
        this.instance = instance;
        this.rigidbody = instance.GetComponent<Rigidbody2D>();
        this.movementLeft = movementLeft;
    }
}

public class EndlessScroller : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private AnimationCurve spawnChance;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementDistance;
    [SerializeField] private float spawnAtX;


    private List<ScrollerInstance> instances = new List<ScrollerInstance>();
    private float distanceLeftToSpawnNextInstance;
    private Vector3 lastPosition = Vector3.zero;

    private void SpawnInstance()
    {
        GameObject instance = Instantiate(prefab);
        instance.transform.position = transform.position;
        Vector3 delta = new Vector3(spawnAtX, 0, 0);
        instance.transform.position += delta;

        instances.Add(new ScrollerInstance(instance, movementDistance));

        float randomValue = Random.Range(0.0f, 1.0f);
        distanceLeftToSpawnNextInstance = spawnChance.Evaluate(randomValue);
    }

    private void Update()
    {
        Vector3 positionDelta = lastPosition - transform.position;
        float deltaMovementSpeed = (movementSpeed * Time.deltaTime);
        distanceLeftToSpawnNextInstance -= Mathf.Abs(deltaMovementSpeed) + (-positionDelta.x);

        if (distanceLeftToSpawnNextInstance <= 0.0f)
            SpawnInstance();

        for (int i = instances.Count - 1; i >= 0; i--)
        {
            ScrollerInstance scrollerInstance = instances[i];
            Vector3 delta = new Vector3(deltaMovementSpeed, 0, 0);

            if (scrollerInstance.rigidbody) scrollerInstance.rigidbody.MovePosition(scrollerInstance.rigidbody.position + (Vector2)delta);
            else scrollerInstance.instance.transform.position += delta;

            scrollerInstance.movementLeft -= (Mathf.Abs(deltaMovementSpeed) + (-positionDelta.x));

            instances[i] = scrollerInstance;

            if (scrollerInstance.movementLeft <= 0.0f)
            {
                Destroy(scrollerInstance.instance);
                instances.RemoveAt(i);
            }
        }

        lastPosition = transform.position;
    }

    private void OnDrawGizmos()
    {
        Vector3 startPoint = transform.position;
        startPoint.x += spawnAtX;
        Vector3 endPoint = startPoint;
        endPoint.x -= movementDistance;
        Gizmos.DrawLine(endPoint, startPoint);
    }
}