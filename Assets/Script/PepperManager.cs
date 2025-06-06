using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepperManager : MonoBehaviour
{
    [Header("설정")]
    public GameObject pepperPrefab;
    public Transform[] spawnAreas; // 4개의 큐브 (스폰 판넬)

    [Range(0f, 1f)] public float spawnYOffset = 0.5f;

    public int maxPepperCount = 20;

    private List<GameObject> spawnedPeppers = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(3f); // 시작 후 3초 대기

        while (true)
        {
            while (spawnedPeppers.Count < maxPepperCount)
            {
                int spawnCount = Mathf.Min(4, maxPepperCount - spawnedPeppers.Count);

                for (int i = 0; i < spawnCount; i++)
                {
                    Transform area = spawnAreas[i % spawnAreas.Length];
                    Vector3 randomPos = GetRandomPointOnCube(area);

                    GameObject pepper = Instantiate(pepperPrefab, randomPos, Quaternion.identity);
                    pepper.GetComponent<DraggablePepper>().pepperManager = this;
                    spawnedPeppers.Add(pepper);
                }

                yield return new WaitForSeconds(1f); // 1초마다 4개씩
            }

            yield return null;
        }
    }

    private Vector3 GetRandomPointOnCube(Transform cube)
    {
        Vector3 center = cube.position;
        Vector3 scale = cube.localScale;

        float width = scale.x * 4.7f;
        float depth = scale.z * 6.7f;

        float halfX = width / 2f;
        float halfZ = depth / 2f;

        float randX = Random.Range(-halfX, halfX);
        float randZ = Random.Range(-halfZ, halfZ);

        return new Vector3(center.x + randX, center.y + spawnYOffset, center.z + randZ);
    }

    public void OnPepperDestroyed(GameObject pepper)
    {
        if (spawnedPeppers.Contains(pepper))
        {
            spawnedPeppers.Remove(pepper);
            StartCoroutine(RespawnAfterDelay());
        }
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        if (spawnedPeppers.Count < maxPepperCount)
        {
            Transform area = spawnAreas[Random.Range(0, spawnAreas.Length)];
            Vector3 spawnPos = GetRandomPointOnCube(area);

            GameObject newPepper = Instantiate(pepperPrefab, spawnPos, Quaternion.identity);
            newPepper.GetComponent<DraggablePepper>().pepperManager = this;
            spawnedPeppers.Add(newPepper);
        }
    }
}
