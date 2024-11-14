using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public GameObject[] monsters;  // Array of game objects to check
    //monsters  = FindObjectsOfType<MonsterProperties>().Select(g => g.gameObject).ToArray();

    // Find me all object that has the script MonsterProperties

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

    // Huidige ronde
    public int currentRound = 1;

    // Interval tussen individuele spawns
    public float spawnInterval = 1f;

    // Lijst van actieve vijanden in de huidige ronde
    public List<GameObject> activeEnemies = new List<GameObject>();

    // Controleert of er een ronde bezig is
    private bool isRoundInProgress = false;

    public int spawnCounter;

    private void Start()
    {
        // Start de eerste ronde
        StartCoroutine(StartNewRound());
    }

    private IEnumerator StartNewRound()
    {
        // Korte vertraging voordat de ronde begint
        yield return new WaitForSeconds(2f);

        isRoundInProgress = true;

        // Verhoog het aantal vijanden met de ronde
        int roundMultiplier = currentRound;

        foreach (var enemyType in enemyTypes)
        {
            // Bereken het aantal vijanden om te spawnen voor deze ronde
            int spawnCount = enemyType.baseSpawnCount * roundMultiplier;

            spawnCounter = spawnCount;

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
                // Zoek de Animator van de vijand en speel de 'Win' animatie
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
