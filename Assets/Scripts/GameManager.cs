using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using Vuforia;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject enemyPrefab;      // Prefab voor dit type vijand
        public Transform spawnPoint;        // Spawnpunt voor dit type vijand
        public int baseSpawnCount = 3;      // Basis aantal vijanden dat spawnt in ronde 1
        public ParticleSystem particle;
    }

    public List<EnemyType> enemyTypes; // Lijst van alle vijandtypen
    public List<Transform> spawnPoints = new List<Transform>(); // Lijst van spawnpoints (gekopieerd van targets)
    private ObserverBehaviour[] observerBehaviours; // Vuforia observer-behaviours
    private Dictionary<ObserverBehaviour, bool> targetStatuses = new Dictionary<ObserverBehaviour, bool>();

    public int currentRound = 1; // Huidige ronde
    public float spawnInterval = 1f; // Interval tussen individuele spawns
    public List<GameObject> activeEnemies = new List<GameObject>(); // Lijst van actieve vijanden in de huidige ronde
    private bool isRoundInProgress = false;
    private bool gameStarted = false;

    private int spawnersFound = 0;
    private GameObject player;

    private void Start()
    {
        observerBehaviours = FindObjectsOfType<ObserverBehaviour>();

        foreach (var observer in observerBehaviours)
        {
            if (observer.tag.Equals("Spawner"))
            {
                targetStatuses[observer] = false;
                observer.OnTargetStatusChanged += OnTargetStatusChanged;
            }
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour observer, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            targetStatuses[observer] = true;

            // Move the child spawners to the GameManager after a delay
            foreach (Transform child in observer.transform)
            {
                if (!spawnPoints.Contains(child))
                {
                    // Start the coroutine to set the position after a delay
                    StartCoroutine(SetPositionAfterDelay(child, observer, 1f));

                    // Set spawn point active and add to the list
                    child.gameObject.SetActive(true);
                    spawnPoints.Add(child);

                    spawnersFound++;

                    // Update the reference in the EnemyType
                    foreach (var enemyType in enemyTypes)
                    {
                        if (enemyType.spawnPoint == observer.transform)
                        {
                            enemyType.spawnPoint = child;
                        }
                    }
                }
            }

            CheckAllTargetsFound();

        }
        else
        {
            targetStatuses[observer] = false;
        }

        //CheckAllTargetsFound();
        
    }

    public void PlayerIsFound(GameObject newPlayer)
    {
        player = newPlayer;
        CheckAllTargetsFound();
    }

    private void CheckAllTargetsFound()
    {
        if (spawnersFound < enemyTypes.Count || player == null) return;

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint != null)
            {
                Vector3 spawnPos = spawnPoint.position;
                spawnPos.y = player.transform.position.y; 
                spawnPoint.position = spawnPos;
                UnityEngine.Debug.Log("Spawnpoint set to player height...............................................");
            }
        }

        if (!gameStarted)
        {

            gameStarted = true;
            UnityEngine.Debug.Log("All targets found! Starting first round.");
            StartCoroutine(StartNewRound());
        }
    }

    private IEnumerator StartNewRound()
    {
        yield return new WaitForSeconds(4f);

        isRoundInProgress = true;

        int roundMultiplier = currentRound;

        foreach (var enemyType in enemyTypes)
        {
            if (spawnPoints.Count == 0) yield break;

            // Pick a random spawn point
            //enemyType.spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            int spawnCount = enemyType.baseSpawnCount * roundMultiplier;

            StartCoroutine(SpawnEnemies(enemyType, spawnCount));
        }
    }

    private IEnumerator SpawnEnemies(EnemyType enemyType, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(enemyType.enemyPrefab, enemyType.spawnPoint.position, Quaternion.identity);
            activeEnemies.Add(enemy);
            enemyType.particle.Play();

            enemy.GetComponent<Health>().OnDeath += () => RemoveEnemyFromList(enemy);

            yield return new WaitForSeconds(0.3f);
            enemyType.particle.Stop();

            yield return new WaitForSeconds(spawnInterval - 0.3f);
        }
    }

    private void RemoveEnemyFromList(GameObject enemy)
    {
        activeEnemies.Remove(enemy);

        if (activeEnemies.Count == 0 && isRoundInProgress)
        {
            isRoundInProgress = false;
            currentRound++;
            StartCoroutine(StartNewRound());
        }
    }

    // Coroutine to set position after delay
    private IEnumerator SetPositionAfterDelay(Transform spawnPoint, ObserverBehaviour observer, float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Move the spawn point to the world position of the parent observer (target) after the delay
        spawnPoint.position = observer.transform.position;
        spawnPoint.SetParent(transform); // Move to GameManager

        // Set the rotation, keeping the y rotation from the behaviour and setting x and z to 0
        Vector3 rotation = observer.transform.rotation.eulerAngles;
        spawnPoint.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f); // Set x and z to 0, keep y
    }

    public void TriggerEnemiesWinAnimation()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.freezeRotation = true;
                }

                Animator animator = enemy.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetTrigger("Win");
                    enemy.GetComponent<EnemyMovement>().enabled = false;
                }
            }
        }
    }
}
