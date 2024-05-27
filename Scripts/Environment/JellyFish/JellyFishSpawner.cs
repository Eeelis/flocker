using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JellyFishSpawner : MonoBehaviour
{
    [SerializeField] private List<JellyFish> jellyFishPrefabs;
    [SerializeField] private float frequency;
    [SerializeField] private int maxCount;

    private List<JellyFish> activeJellyFish = new List<JellyFish>();
    private List<JellyFish> jellyFishPool = new List<JellyFish>();
    private Vector3 screenBottomLeft;
    private Vector3 screenTopRight;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private void Start()
    {
        InitializeMedusaPool();
        StartCoroutine(SpawnMedusas());

        Vector3 screenBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 screenTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

        float minX = screenBottomLeft.x;
        float maxX = screenTopRight.x;
        float minY = screenBottomLeft.y;
        float maxY = screenTopRight.y;
    }

    private void InitializeMedusaPool()
    {
        for (int i = 0; i < jellyFishPrefabs.Count; i++)
        {
            JellyFish jellyFish = Instantiate(jellyFishPrefabs[i], transform);
            jellyFish.gameObject.SetActive(false);
            jellyFishPool.Add(jellyFish);
        }
    }

    private IEnumerator SpawnMedusas()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(frequency, frequency * 3));

            SpawnJellyFish();

            // return active, off-screen jelly fish to pool
            for (int i = activeJellyFish.Count - 1; i >= 0; i--)
            {
                if (Mathf.Abs(activeJellyFish[i].transform.position.y) > 40 || Mathf.Abs(activeJellyFish[i].transform.position.x) > 40)
                {
                    ReturnJellyFishToPool(activeJellyFish[i]);
                }
            } 
        }
    }

    private void SpawnJellyFish()
    {
        if (activeJellyFish.Count >= maxCount) { return; }

        JellyFish jellyFish = GetJellyFishFromPool();

        if (jellyFish != null)
        {
            GenerateInitialPositionAndRotation(jellyFish);
            jellyFish.gameObject.SetActive(true);
            activeJellyFish.Add(jellyFish);
        }
    }

    private JellyFish GetJellyFishFromPool()
    {
        if (jellyFishPool.Count > 0)
        {
            return jellyFishPool[Random.Range(0, jellyFishPool.Count)];
        }
        
        return null;
    }

    private void ReturnJellyFishToPool(JellyFish jellyFish)
    {
        jellyFish.gameObject.SetActive(false);
        activeJellyFish.Remove(jellyFish);
        jellyFishPool.Add(jellyFish);
    }

    private void GenerateInitialPositionAndRotation(JellyFish jellyFish)
    {
        // Get a random screen edge and move the jellyfish just outside of that edge
        Vector3 spawnPosition = Vector3.zero;
        float offset = 35f;
        float randomEdge = Random.Range(0, 4);
        
        switch (randomEdge)
        {
            case 0:
                spawnPosition = new Vector3(minX - offset, Random.Range(minY, maxY), 0);
                break;
            case 1:
                spawnPosition = new Vector3(maxX + offset, Random.Range(minY, maxY), 0);
                break;
            case 2:
                spawnPosition = new Vector3(Random.Range(minX, maxX), minY - offset, 0);
                break;
            case 3:
                spawnPosition = new Vector3(Random.Range(minX, maxX), maxY + offset, 0);
                break;
        }

        jellyFish.transform.position = spawnPosition;

        // Rotate the jellyfish towards the centre of the screen, offset by a random angle
        Vector3 directionToCenter = (new Vector3(0, 5, 0) - spawnPosition).normalized;
        float angleOffset = Random.Range(-45f, 45f);
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, directionToCenter) * Quaternion.Euler(0, 0, angleOffset);
        jellyFish.transform.rotation = rotation;
    }
}
