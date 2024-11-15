using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    // Lijst van alle vijandtypen
    public List<EnemyType> enemyTypes;

    // Lijst van spawnpoints (gekopieerd van targets)
    public List<Transform> spawnPoints = new List<Transform>();

    // Vuforia observer-behaviours
    private ObserverBehaviour[] observerBehaviours;
    private Dictionary<ObserverBehaviour, bool> targetStatuses = new Dictionary<ObserverBehaviour, bool>();

    // Huidige ronde
    public int currentRound = 1;

    // Interval tussen individuele spawns
    public float spawnInterval = 1f;

    // Lijst van actieve vijanden in de huidige ronde
    public List<GameObject> activeEnemies = new List<GameObject>();

    // Controleert of er een ronde bezig is
    private bool isRoundInProgress = false;
    private bool gameStarted = false;

    private void Start()
    {
        // Zoek alle observer-behaviours (targets) in de scène
        observerBehaviours = FindObjectsOfType<ObserverBehaviour>();

        // Registreer de statusverandering callbacks
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
        // Controleer of het target gedetecteerd en gevolgd wordt
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            // Update de status van dit target
            targetStatuses[observer] = true;

            // Kopieer spawnpoints van het target als deze nog niet toegevoegd zijn
            if (!spawnPoints.Contains(observer.transform))
            {
                foreach (Transform child in observer.transform)
                {
                    if (child.CompareTag("SpawnPoint"))
                    {
                        Transform newSpawnPoint = Instantiate(child, transform); // Kopieer spawnpoints naar een persistente plek
                        spawnPoints.Add(newSpawnPoint);
                    }
                }
            }

            // Controleer of alle targets gevonden zijn
            CheckAllTargetsFound();
        }
        else
        {
            // Markeer het target als niet meer gevolgd
            targetStatuses[observer] = false;
        }
    }

    private void CheckAllTargetsFound()
    {
        // Controleer of alle targets gevonden zijn
        foreach (var status in targetStatuses.Values)
        {
            if (!status) return; // Nog niet alle targets zijn gevonden
        }

        if (!gameStarted)
        {
            gameStarted = true;
            Debug.Log("Alle targets gevonden! Start de eerste ronde.");
            StartCoroutine(StartNewRound());
        }
    }

    private IEnumerator StartNewRound()
    {
        yield return new WaitForSeconds(2f);

        isRoundInProgress = true;

        // Verhoog het aantal vijanden met de ronde
        int roundMultiplier = currentRound;

        foreach (var enemyType in enemyTypes)
        {
            // Controleer of spawnpoints beschikbaar zijn
            if (spawnPoints.Count == 0) yield break;

            // Wijs een willekeurig spawnpoint toe
            enemyType.spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            // Bereken het aantal vijanden om te spawnen voor deze ronde
            int spawnCount = enemyType.baseSpawnCount * roundMultiplier;

            // Start vijand spawn voor het vijandtype
            StartCoroutine(SpawnEnemies(enemyType, spawnCount));
        }
    }

    private IEnumerator SpawnEnemies(EnemyType enemyType, int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Spawn een vijand en voeg deze toe aan de lijst van actieve vijanden
            GameObject enemy = Instantiate(enemyType.enemyPrefab, enemyType.spawnPoint.position, Quaternion.identity);
            activeEnemies.Add(enemy);
            enemyType.particle.Play();

            // Registreer callback om vijand uit de lijst te verwijderen wanneer deze sterft
            enemy.GetComponent<Health>().OnDeath += () => RemoveEnemyFromList(enemy);

            yield return new WaitForSeconds(0.3f);
            enemyType.particle.Stop();

            // Wacht voordat de volgende vijand gespawned wordt
            yield return new WaitForSeconds(spawnInterval - 0.3f);
        }
    }

    private void RemoveEnemyFromList(GameObject enemy)
    {
        // Verwijder vijand uit de lijst van actieve vijanden
        activeEnemies.Remove(enemy);

        // Controleer of alle vijanden verslagen zijn om de volgende ronde te starten
        if (activeEnemies.Count == 0 && isRoundInProgress)
        {
            isRoundInProgress = false;
            currentRound++;

            // Start de volgende ronde
            StartCoroutine(StartNewRound());
        }
    }

    // Roept deze functie aan wanneer de speler verliest
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
