using System.Collections;
using UnityEngine;

public class CrystalFallSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject crystalPrefab;

    [Header("Kristall-Sprites")]
    [SerializeField] private Sprite[] crystalSprites;

    [Header("Spawn-Bereich")]
    [SerializeField] private float spawnWidth = 5f;

    [Header("Spawn-Zeit")]
    [SerializeField] private float minSpawnDelay = 1.5f;
    [SerializeField] private float maxSpawnDelay = 3.5f;

    [Header("Größe")]
    [SerializeField] private float minScale = 0.12f;
    [SerializeField] private float maxScale = 0.25f;

    private void Start()
    {
        StartCoroutine(SpawnCrystalRoutine());
    }

    private IEnumerator SpawnCrystalRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);

            yield return new WaitForSeconds(delay);

            SpawnCrystal();
        }
    }

    private void SpawnCrystal()
    {
        if (crystalPrefab == null)
            return;

        if (crystalSprites == null || crystalSprites.Length == 0)
            return;

        float randomX = Random.Range(
            -spawnWidth / 2f,
            spawnWidth / 2f
        );

        Vector3 spawnPosition = transform.position + new Vector3(
            randomX,
            0f,
            0f
        );

        GameObject crystal = Instantiate(
            crystalPrefab,
            spawnPosition,
            Quaternion.identity
        );

        SpriteRenderer spriteRenderer =
            crystal.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            int randomSpriteIndex =
                Random.Range(0, crystalSprites.Length);

            spriteRenderer.sprite =
                crystalSprites[randomSpriteIndex];
        }

        float randomScale =
            Random.Range(minScale, maxScale);

        crystal.transform.localScale =
            new Vector3(
                randomScale,
                randomScale,
                1f
            );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(
            transform.position +
            Vector3.left * spawnWidth / 2f,

            transform.position +
            Vector3.right * spawnWidth / 2f
        );
    }
}