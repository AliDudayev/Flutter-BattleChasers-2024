using UnityEngine;

public class SpawnOnMovement : MonoBehaviour
{
    public GameObject prefab;

    public float spawnDistance = 10f; 

    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        if (distanceMoved >= spawnDistance)
        {
            SpawnObject();

            lastPosition = transform.position;
        }
    }

    private void SpawnObject()
    {
        if (prefab != null)
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}
