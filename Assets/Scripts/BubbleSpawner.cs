using System.Collections;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [Header("Blasen")]
    [SerializeField]
    private Bubble bubblePrefab;

    [SerializeField]
    private Sprite[] bubbleSprites;

    [SerializeField]
    private Transform bubbleParent;

    [Header("Zeit zwischen Gruppen")]
    [SerializeField]
    private float minimumSpawnDelay = 1.5f;

    [SerializeField]
    private float maximumSpawnDelay = 4f;

    [Header("Blasen pro Gruppe")]
    [SerializeField]
    private int minimumBubblesPerGroup = 1;

    [SerializeField]
    private int maximumBubblesPerGroup = 3;

    [SerializeField]
    private float minimumDelayInsideGroup = 0.08f;

    [SerializeField]
    private float maximumDelayInsideGroup = 0.3f;

    [Header("Spawn-Bereich")]
    [SerializeField]
    private Vector2 spawnArea =
        new Vector2(0.25f, 0.1f);

    [Header("Geschwindigkeit")]
    [SerializeField]
    private float minimumRiseSpeed = 0.35f;

    [SerializeField]
    private float maximumRiseSpeed = 0.75f;

    [Header("Lebensdauer")]
    [SerializeField]
    private float minimumLifetime = 3f;

    [SerializeField]
    private float maximumLifetime = 6f;

    [Header("Maximale Steighöhe")]
    [SerializeField]
    private float minimumRiseHeight = 1.5f;

    [SerializeField]
    private float maximumRiseHeight = 3.5f;

    [Header("Seitliche Bewegung")]
    [SerializeField]
    private float minimumHorizontalDrift = 0.01f;

    [SerializeField]
    private float maximumHorizontalDrift = 0.06f;

    [SerializeField]
    private float minimumSwayAmount = 0.03f;

    [SerializeField]
    private float maximumSwayAmount = 0.12f;

    [SerializeField]
    private float minimumSwaySpeed = 1.2f;

    [SerializeField]
    private float maximumSwaySpeed = 2.8f;

    [Header("Größe")]
    [SerializeField]
    private float minimumScale = 0.5f;

    [SerializeField]
    private float maximumScale = 1.1f;

    private Coroutine spawnCoroutine;

    private void OnEnable()
    {
        spawnCoroutine =
            StartCoroutine(SpawnRoutine());
    }

    private void OnDisable()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        // Verhindert, dass alle Spawner gleichzeitig beginnen
        yield return new WaitForSeconds(
            Random.Range(
                0.1f,
                maximumSpawnDelay
            )
        );

        while (true)
        {
            int bubbleCount =
                Random.Range(
                    minimumBubblesPerGroup,
                    maximumBubblesPerGroup + 1
                );

            for (int i = 0; i < bubbleCount; i++)
            {
                SpawnBubble();

                if (i < bubbleCount - 1)
                {
                    float delayInsideGroup =
                        Random.Range(
                            minimumDelayInsideGroup,
                            maximumDelayInsideGroup
                        );

                    yield return new WaitForSeconds(
                        delayInsideGroup
                    );
                }
            }

            float nextGroupDelay =
                Random.Range(
                    minimumSpawnDelay,
                    maximumSpawnDelay
                );

            yield return new WaitForSeconds(
                nextGroupDelay
            );
        }
    }

    private void SpawnBubble()
    {
        if (bubblePrefab == null)
        {
            Debug.LogWarning(
                "Im BubbleSpawner wurde kein Bubble Prefab zugewiesen.",
                this
            );

            return;
        }

        if (
            bubbleSprites == null ||
            bubbleSprites.Length == 0
        )
        {
            Debug.LogWarning(
                "Im BubbleSpawner wurden keine Bubble Sprites zugewiesen.",
                this
            );

            return;
        }

        Vector3 randomSpawnOffset =
            new Vector3(
                Random.Range(
                    -spawnArea.x * 0.5f,
                    spawnArea.x * 0.5f
                ),
                Random.Range(
                    -spawnArea.y * 0.5f,
                    spawnArea.y * 0.5f
                ),
                0f
            );

        Vector3 spawnPosition =
            transform.position +
            randomSpawnOffset;

        Bubble newBubble = Instantiate(
            bubblePrefab,
            spawnPosition,
            Quaternion.identity,
            bubbleParent
        );

        Sprite randomSprite =
            bubbleSprites[
                Random.Range(
                    0,
                    bubbleSprites.Length
                )
            ];

        newBubble.Initialize(
            randomSprite,

            Random.Range(
                minimumRiseSpeed,
                maximumRiseSpeed
            ),

            Random.Range(
                minimumLifetime,
                maximumLifetime
            ),

            Random.Range(
                minimumRiseHeight,
                maximumRiseHeight
            ),

            Random.Range(
                minimumHorizontalDrift,
                maximumHorizontalDrift
            ),

            Random.Range(
                minimumSwayAmount,
                maximumSwayAmount
            ),

            Random.Range(
                minimumSwaySpeed,
                maximumSwaySpeed
            ),

            Random.Range(
                minimumScale,
                maximumScale
            )
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(
            transform.position,
            new Vector3(
                spawnArea.x,
                spawnArea.y,
                0f
            )
        );
    }

    private void OnValidate()
    {
        maximumSpawnDelay =
            Mathf.Max(
                minimumSpawnDelay,
                maximumSpawnDelay
            );

        maximumBubblesPerGroup =
            Mathf.Max(
                minimumBubblesPerGroup,
                maximumBubblesPerGroup
            );

        maximumDelayInsideGroup =
            Mathf.Max(
                minimumDelayInsideGroup,
                maximumDelayInsideGroup
            );

        maximumRiseSpeed =
            Mathf.Max(
                minimumRiseSpeed,
                maximumRiseSpeed
            );

        maximumLifetime =
            Mathf.Max(
                minimumLifetime,
                maximumLifetime
            );

        maximumRiseHeight =
            Mathf.Max(
                minimumRiseHeight,
                maximumRiseHeight
            );

        maximumHorizontalDrift =
            Mathf.Max(
                minimumHorizontalDrift,
                maximumHorizontalDrift
            );

        maximumSwayAmount =
            Mathf.Max(
                minimumSwayAmount,
                maximumSwayAmount
            );

        maximumSwaySpeed =
            Mathf.Max(
                minimumSwaySpeed,
                maximumSwaySpeed
            );

        maximumScale =
            Mathf.Max(
                minimumScale,
                maximumScale
            );
    }
}