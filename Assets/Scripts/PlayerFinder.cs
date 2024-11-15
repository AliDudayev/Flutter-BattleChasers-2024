using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class PlayerFinder : MonoBehaviour
{
    public GameObject Character; 
    public ObserverBehaviour[] observerBehaviours;

    void Start()
    {
        // Find all observer objects (like ImageTarget or ModelTarget) in the scene
        observerBehaviours = FindObjectsOfType<ObserverBehaviour>();

        // Register a callback for each observer to listen to status changes
        foreach (var observer in observerBehaviours)
        {
            if(observer.name != "VideoBackground" && observer.tag.Equals("Player"))
            {
                observer.OnTargetStatusChanged += OnTargetStatusChanged;
            }
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
                Character = behaviour.transform.GetChild(0).gameObject;

                GameObject instantiatedCharacter = Instantiate(Character, transform);
                //instantiatedCharacter.SetActive(true);

                // Enable all children and components recursively
                EnableAllComponents(instantiatedCharacter);

                StartCoroutine(SetCharacterPositionAfterDelay(instantiatedCharacter, behaviour, 1f));

                GameManager gameManager = FindObjectOfType<GameManager>();
                gameManager.PlayerIsFound();
            }
        }
    }

    private void EnableAllComponents(GameObject obj)
    {
        // Enable the GameObject itself
        obj.SetActive(true);

        // Loop through all components on this GameObject and enable them if possible
        foreach (var component in obj.GetComponents<Component>())
        {
            if (component is Renderer renderer)
                renderer.enabled = true;
            else if (component is Collider collider)
                collider.enabled = true;
            else if (component is Canvas canvas)
                canvas.enabled = true;
        }

        // Recursively enable all child objects and their components
        foreach (Transform child in obj.transform)
        {
            EnableAllComponents(child.gameObject);
        }
    }

    private IEnumerator SetCharacterPositionAfterDelay(GameObject instantiatedCharacter, ObserverBehaviour behaviour, float delay)
    {
        yield return new WaitForSeconds(delay);

        transform.position = behaviour.transform.position;
        transform.rotation = behaviour.transform.rotation;
    }
}
