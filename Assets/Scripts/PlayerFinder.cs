using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class PlayerFinder : MonoBehaviour
{
    // Find object with dinoProperties script
    //public GameObject Character;

    //void Start()
    //{
    //    // Get the GameObject associated with the MonsterProperties component
    //    MonsterProperties monster = FindObjectOfType<MonsterProperties>();
    //    if (monster != null)
    //    {
    //        Character = monster.gameObject;
    //    }
    //}

    private GameObject Character; // To hold the first found active target
    private ObserverBehaviour[] observerBehaviours;

    void Start()
    {
        // Find all observer objects (like ImageTarget or ModelTarget) in the scene
        observerBehaviours = FindObjectsOfType<ObserverBehaviour>();

        // Register a callback for each observer to listen to status changes
        foreach (var observer in observerBehaviours)
        {
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        // Check if the target is detected and tracked
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            // Set Character to the GameObject of the first detected target if not already set
            if (Character == null)
            {
                //transform.position = behaviour.transform.position;
                //transform.rotation = behaviour.transform.rotation;

                Character = behaviour.transform.GetChild(0).gameObject;

                GameObject instantiatedCharacter = Instantiate(Character, transform);
                instantiatedCharacter.SetActive(true);

                StartCoroutine(SetCharacterPositionAfterDelay(instantiatedCharacter, behaviour, 1f));

                // De skinnedMeshRenderer en canvastonden uit dus ik zet ze aan
                //SkinnedMeshRenderer skinnedMeshRenderer =  instantiatedCharacter.GetComponentInChildren<SkinnedMeshRenderer>();
                //skinnedMeshRenderer.enabled = true;
                //Canvas canvas = instantiatedCharacter.GetComponentInChildren<Canvas>();
                //canvas.enabled = true;
            }

            if (Character == null)
            {
                //// Instantiate the character at the target's initial position
                //GameObject instantiatedCharacter = Instantiate(behaviour.transform.GetChild(0).gameObject);
                //instantiatedCharacter.SetActive(true);

                //// Start a coroutine to wait for 1 second before setting the position and rotation
                //StartCoroutine(SetCharacterPositionAfterDelay(instantiatedCharacter, behaviour, 1f));

                //// Store the instantiated character in the Character variable (if needed later)
                //Character = instantiatedCharacter;
            }
        }
    }

    private IEnumerator SetCharacterPositionAfterDelay(GameObject instantiatedCharacter, ObserverBehaviour behaviour, float delay)
    {
        // Wait for the specified amount of time (1 second in this case)
        yield return new WaitForSeconds(delay);

        // Set the position and rotation after the delay to ensure they are updated
        transform.position = behaviour.transform.position;
        transform.rotation = behaviour.transform.rotation;

        SkinnedMeshRenderer skinnedMeshRenderer = instantiatedCharacter.GetComponentInChildren<SkinnedMeshRenderer>();
        skinnedMeshRenderer.enabled = true;
        Canvas canvas = instantiatedCharacter.GetComponentInChildren<Canvas>();
        canvas.enabled = true;
    }

    //private void OnDestroy()
    //{
    //    // Unregister the event when the script is destroyed
    //    foreach (var observer in observerBehaviours)
    //    {
    //        observer.OnTargetStatusChanged -= OnTargetStatusChanged;
    //    }
    //}
}
