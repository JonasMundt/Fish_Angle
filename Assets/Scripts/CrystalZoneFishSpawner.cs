using System.Collections;
using UnityEngine;

public class CrystalZoneFishSpawner : MonoBehaviour
{
    [Header("Fische")]
    [SerializeField] private FishSwimmer crystalEelPrefab;
    [SerializeField] private FishSwimmer prismTroutPrefab;

    [Header("Spawnwahrscheinlichkeit")]
    [Range(0f, 1f)]
    [SerializeField] private float prismTroutSpawnChance = 0.25f;

    [Header("Sprite-Ausrichtung")]
    [Tooltip("Aktivieren, wenn das gezeichnete Kristallaal-Sprite standardmäßig nach rechts schaut.")]
    [SerializeField] private bool crystalEelSpriteFacesRight = false;

    [Tooltip(
        "Aktivieren, wenn das gezeichnete Prismaforellen-Sprite standardmäßig nach rechts schaut."
    )]
    [SerializeField] private bool prismTroutSpriteFacesRight = true;

    [Header("Spawnpunkte")]
    [SerializeField] private Transform leftSpawnPoint;
    [SerializeField] private Transform rightSpawnPoint;

    [Header("Zufällige Höhe")]
    [SerializeField] private float verticalSpawnRange = 6f;

    [Header("Zeit zwischen Fischen")]
    [SerializeField] private float minimumSpawnDelay = 4f;
    [SerializeField] private float maximumSpawnDelay = 8f;

    [Header("Kristallaal Geschwindigkeit")]
    [Min(0.1f)]
    [SerializeField] private float minimumSwimSpeed = 1.2f;

    [Min(0.1f)]
    [SerializeField] private float maximumSwimSpeed = 2f;

    [Header("Kristallaal Größe")]
    [SerializeField] private float minimumScale = 0.9f;
    [SerializeField] private float maximumScale = 1.1f;

    [Header("Anzahl")]
    [Min(1)]
    [SerializeField] private int maximumActiveFish = 2;

    [Header("Organisation")]
    [SerializeField] private Transform fishParent;

    private int activeFishCount;
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
        yield return new WaitForSeconds(
            Random.Range(
                minimumSpawnDelay,
                maximumSpawnDelay
            )
        );

        while (true)
        {
            if (activeFishCount < maximumActiveFish)
            {
                SpawnFish();
            }

            float nextDelay =
                Random.Range(
                    minimumSpawnDelay,
                    maximumSpawnDelay
                );

            yield return new WaitForSeconds(
                nextDelay
            );
        }
    }

    private void SpawnFish()
    {
        if (!crystalEelPrefab || !prismTroutPrefab)
        {
            Debug.LogWarning(
                "Im CrystalZoneFishSpawner fehlt ein Fisch-Prefab.",
                this
            );

            return;
        }

        if (!leftSpawnPoint || !rightSpawnPoint)
        {
            Debug.LogWarning(
                "Linker oder rechter Spawnpunkt fehlt.",
                this
            );

            return;
        }

        /*
        * Fischart auswählen:
        *
        * 25 % Prismaforelle
        * 75 % Kristallaal
        */
        bool spawnPrismTrout =
            Random.value <
            prismTroutSpawnChance;

        FishSwimmer selectedPrefab =
            spawnPrismTrout
                ? prismTroutPrefab
                : crystalEelPrefab;

        bool selectedSpriteFacesRight =
            spawnPrismTrout
                ? prismTroutSpriteFacesRight
                : crystalEelSpriteFacesRight;

        bool spawnOnLeft =
            Random.value < 0.5f;

        Transform startPoint =
            spawnOnLeft
                ? leftSpawnPoint
                : rightSpawnPoint;

        Transform targetPoint =
            spawnOnLeft
                ? rightSpawnPoint
                : leftSpawnPoint;

        float randomY =
            transform.position.y +
            Random.Range(
                -verticalSpawnRange * 0.5f,
                verticalSpawnRange * 0.5f
            );

        Vector3 spawnPosition =
            new Vector3(
                startPoint.position.x,
                randomY,
                startPoint.position.z
            );

        Vector3 targetPosition =
            new Vector3(
                targetPoint.position.x,
                randomY,
                targetPoint.position.z
            );

        FishSwimmer newFish =
            Instantiate(
                selectedPrefab,
                spawnPosition,
                Quaternion.identity,
                fishParent
            );

        float randomScale =
            Random.Range(
                minimumScale,
                maximumScale
            );

        newFish.transform.localScale =
            Vector3.one *
            randomScale;

        float randomSpeed =
            Random.Range(
                minimumSwimSpeed,
                maximumSwimSpeed
            );

        activeFishCount++;

        newFish.Initialize(
            targetPosition,
            randomSpeed,
            selectedSpriteFacesRight,
            HandleFishDespawned
        );
    }

    private void HandleFishDespawned()
    {
        activeFishCount =
            Mathf.Max(
                0,
                activeFishCount - 1
            );
    }

    private void OnDrawGizmosSelected()
    {
        if (!leftSpawnPoint || !rightSpawnPoint)
        {
            return;
        }

        float centerX =
            (
                leftSpawnPoint.position.x +
                rightSpawnPoint.position.x
            ) * 0.5f;

        float width =
            Mathf.Abs(
                rightSpawnPoint.position.x -
                leftSpawnPoint.position.x
            );

        Gizmos.DrawWireCube(
            new Vector3(
                centerX,
                transform.position.y,
                transform.position.z
            ),
            new Vector3(
                width,
                verticalSpawnRange,
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

        maximumSwimSpeed =
            Mathf.Max(
                minimumSwimSpeed,
                maximumSwimSpeed
            );

        maximumScale =
            Mathf.Max(
                minimumScale,
                maximumScale
            );

        maximumActiveFish =
            Mathf.Max(
                1,
                maximumActiveFish
            );

        verticalSpawnRange =
            Mathf.Max(
                0f,
                verticalSpawnRange
            );
    }
}