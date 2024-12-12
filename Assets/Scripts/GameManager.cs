using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;
using Vuforia;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        // Prefab voor dit type vijand
        public GameObject enemyPrefab;
        // Spawnpunt voor dit type vijand
        public Transform spawnPoint;
        // Basis aantal vijanden dat spawnt in ronde 1
        public int baseSpawnCount = 3;      
        public ParticleSystem particle;
    }

    // voor de boss draken
    public List<GameObject> BossesPrefabs = new List<GameObject>();
    public GameObject portal;

    // Lijst van alle vijandtypen
    public List<EnemyType> enemyTypes;
    // Lijst van spawnpoints (gekopieerd van targets)
    private List<Transform> spawnPoints = new List<Transform>();
    // Vuforia observer-behaviours
    private ObserverBehaviour[] observerBehaviours; 
    private Dictionary<ObserverBehaviour, bool> targetStatuses = new Dictionary<ObserverBehaviour, bool>();

    // Huidige ronde
    public int currentRound = 1;
    // Interval tussen individuele spawns
    public float spawnInterval = 1f;
    // Lijst van actieve vijanden in de huidige ronde
    public List<GameObject> activeEnemies = new List<GameObject>(); 
    private bool isRoundInProgress = false;
    private bool gameStarted = false;

    private int spawnersFound = 0;
    private GameObject player;

    [SerializeField] List<GameObject> startTimer = new List<GameObject>();
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject endScreen;

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

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnDragon();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            GameObject dragon = GameObject.Find("Red_Usurper");
            dragon.GetComponent<Health>().ApplyDamage(1000, transform.position);

        }
    }

    private void SpawnDragon()
    {
        // activeEnemies.Add(boss);
        // boss.GetComponent<Health>().OnDeath += () => RemoveEnemyFromList(boss);
        Vector3 location = player.transform.position;
        location.y += 15;
        GameObject portalInstance = Instantiate(portal, location, Quaternion.identity);
        StartCoroutine(RemovePortal(portalInstance, location, portalInstance));
    }

    private IEnumerator RemovePortal(GameObject portal, Vector3 location, GameObject portalInstance)
    {
        yield return new WaitForSeconds(1f);

        int randomInt = Random.Range(0, BossesPrefabs.Count);
        GameObject boss = Instantiate(BossesPrefabs[randomInt], location, Quaternion.identity);
        
    
        ParticleSystem portalParticle = portalInstance.transform.GetChild(1).GetComponent<ParticleSystem>();
        portalParticle.Play();
        yield return new WaitForSeconds(1f);
        portalParticle.Stop();
        yield return new WaitForSeconds(2f);
    
        Destroy(portal);
    }

    private void OnTargetStatusChanged(ObserverBehaviour observer, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            targetStatuses[observer] = true;

            foreach (Transform child in observer.transform)
            {
                if (!spawnPoints.Contains(child))
                {
                    StartCoroutine(SetPositionAfterDelay(child, observer, 1f));

                    child.gameObject.SetActive(true);
                    spawnPoints.Add(child);

                    spawnersFound++;

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
        startScreen.SetActive(true);
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
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        GameObject lastItem = null;
        foreach (var item in startTimer)
        {
            item.SetActive(true);
            if(lastItem != null)
            {
                lastItem.SetActive(false);
            }
            lastItem = item;
            yield return new WaitForSeconds(1.5f);
        }
        lastItem.SetActive(false);

        StartCoroutine(StartNewRound());
    }

    private IEnumerator StartNewRound()
    {
        yield return new WaitForSeconds(4f);

        isRoundInProgress = true;

        int roundMultiplier = currentRound;

        foreach (var enemyType in enemyTypes)
        {
            if (spawnPoints.Count == 0) yield break;

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

    private IEnumerator SetPositionAfterDelay(Transform spawnPoint, ObserverBehaviour observer, float delay)
    {
        yield return new WaitForSeconds(delay);

        spawnPoint.position = observer.transform.position;
        spawnPoint.SetParent(transform);

        Vector3 rotation = observer.transform.rotation.eulerAngles;
        spawnPoint.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    public void TriggerEnemiesWinAnimation()
    {
        endScreen.SetActive(true);

        GameObject scoreObject = GameObject.Find("Points");
        int score = int.Parse(scoreObject.GetComponent<Text>().text);

        GetComponent<GameEnd>().SetEndScore(score);

        GameObject endScoreObject = GameObject.Find("End Score");
        endScoreObject.GetComponent<Text>().text = score.ToString();

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
