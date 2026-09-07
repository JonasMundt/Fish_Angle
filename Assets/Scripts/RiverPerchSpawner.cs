using System.Collections;
using UnityEngine;

public class RiverPerchSpawner : MonoBehaviour
{
    [Header("Fische")]
    [SerializeField] private FishSwimmer riverPerchPrefab;
    [SerializeField] private FishSwimmer catsharkPrefab;

    [Header("Inventar")]
    [SerializeField] private FishInventory fishInventory;

    [Min(1)]
    [SerializeField] private int catsharkUnlockSlots = 5;

    [Header("Katzenhai Spawnchance")]
    [Range(0f, 1f)]
    [SerializeField] private float catsharkSpawnChance = 0.3f;

    [Header("Sprite-Ausrichtung")]

    [Tooltip(
        "Aktivieren, wenn das Barsch-Sprite standardmäßig nach rechts schaut."
    )]
    [SerializeField] private bool perchSpriteFacesRight = false;

    [Tooltip(
        "Aktivieren, wenn das Katzenhai-Sprite standardmäßig nach rechts schaut."
    )]
    [SerializeField] private bool catsharkSpriteFacesRight = false;

    [Header("Spawnpunkte")]
    [SerializeField] private Transform leftSpawnPoint;
    [SerializeField] private Transform rightSpawnPoint;

    [Header("Zufällige Höhe")]
    [SerializeField] private float verticalSpawnRange = 4f;

    [Header("Zeit zwischen Fischen")]
    [SerializeField] private float minimumSpawnDelay = 3f;
    [SerializeField] private float maximumSpawnDelay = 8f;

    [Header("Barsch Geschwindigkeit")]
    [Min(0.1f)]
    [SerializeField] private float perchMinimumSwimSpeed = 0.8f;

    [Min(0.1f)]
    [SerializeField] private float perchMaximumSwimSpeed = 1.5f;

    [Header("Katzenhai Geschwindigkeit")]
    [Min(0.1f)]
    [SerializeField] private float catsharkMinimumSwimSpeed = 1.5f;

    [Min(0.1f)]
    [SerializeField] private float catsharkMaximumSwimSpeed = 2.4f;

    [Header("Barsch Größe")]
    [SerializeField] private float perchMinimumScale = 0.8f;
    [SerializeField] private float perchMaximumScale = 1.1f;

    [Header("Katzenhai Größe")]
    [SerializeField] private float catsharkMinimumScale = 0.9f;
    [SerializeField] private float catsharkMaximumScale = 1.15f;

    [Header("Anzahl")]
    [SerializeField] private int maximumActiveFish = 2;

    [Header("Organisation")]
    [SerializeField] private Transform fishParent;

    private int activeFishCount;
    private Coroutine spawnCoroutine;

    private void OnEnable()
    {
        spawnCoroutine =
            StartCoroutine(
                SpawnRoutine()
            );
    }

    private void OnDisable()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(
                spawnCoroutine
            );

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
            if (
                activeFishCount <
                maximumActiveFish
            )
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
        FishSwimmer selectedPrefab =
            ChooseFishPrefab();

        if (!selectedPrefab)
        {
            return;
        }

        if (
            !leftSpawnPoint ||
            !rightSpawnPoint
        )
        {
            Debug.LogWarning(
                "Linker oder rechter Spawnpunkt fehlt.",
                this
            );

            return;
        }

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

        bool isCatshark =
            selectedPrefab ==
            catsharkPrefab;

        bool selectedSpriteFacesRight =
            isCatshark
                ? catsharkSpriteFacesRight
                : perchSpriteFacesRight;

        float randomScale =
            isCatshark
                ? Random.Range(
                    catsharkMinimumScale,
                    catsharkMaximumScale
                )
                : Random.Range(
                    perchMinimumScale,
                    perchMaximumScale
                );

        newFish.transform.localScale =
            Vector3.one *
            randomScale;

        float randomSpeed =
            isCatshark
                ? Random.Range(
                    catsharkMinimumSwimSpeed,
                    catsharkMaximumSwimSpeed
                )
                : Random.Range(
                    perchMinimumSwimSpeed,
                    perchMaximumSwimSpeed
                );

        activeFishCount++;

        newFish.Initialize(
            targetPosition,
            randomSpeed,
            selectedSpriteFacesRight,
            HandleFishDespawned
        );
    }

    private FishSwimmer ChooseFishPrefab()
    {
        if (!riverPerchPrefab)
        {
            Debug.LogWarning(
                "Im RiverPerchSpawner fehlt das Flussbarsch-Prefab.",
                this
            );

            return null;
        }

        bool catsharkIsUnlocked =
            fishInventory &&
            fishInventory.MaximumFishCount >=
            catsharkUnlockSlots;

        if (
            catsharkIsUnlocked &&
            catsharkPrefab &&
            Random.value <
            catsharkSpawnChance
        )
        {
            return catsharkPrefab;
        }

        return riverPerchPrefab;
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
        if (
            !leftSpawnPoint ||
            !rightSpawnPoint
        )
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

        perchMaximumSwimSpeed =
            Mathf.Max(
                perchMinimumSwimSpeed,
                perchMaximumSwimSpeed
            );

        catsharkMaximumSwimSpeed =
            Mathf.Max(
                catsharkMinimumSwimSpeed,
                catsharkMaximumSwimSpeed
            );

        perchMaximumScale =
            Mathf.Max(
                perchMinimumScale,
                perchMaximumScale
            );

        catsharkMaximumScale =
            Mathf.Max(
                catsharkMinimumScale,
                catsharkMaximumScale
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

        catsharkUnlockSlots =
            Mathf.Max(
                1,
                catsharkUnlockSlots
            );
    }
}