using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDistanceDinos : MonoBehaviour
{
    public GameObject[] dinos;  // Array of game objects to check
    public float closeThreshold = 5f;  // Distance threshold to consider them "close"
    public int count = 0;

    private Dictionary<string, bool> battledPairs = new Dictionary<string, bool>();

    void Update()
    {
        // Loop through each pair of dino game objects
        for (int i = 0; i < dinos.Length; i++)
        {

            for (int j = i + 1; j < dinos.Length; j++)
            {

                // Create a unique key for this pair
                string pairKey = dinos[i].name + "-" + dinos[j].name;

                // Check if the distance is below the threshold
                if (!battledPairs.ContainsKey(pairKey) && AreDinosClose(dinos[i], dinos[j]))
                {
                    count++;
                    // Debug.LogError(count + ": " + dinos[i].name + " and " + dinos[j].name + " are close together!");
                    Fight(dinos[i], dinos[j]);

                    // Mark this pair as had a fight
                    battledPairs[pairKey] = true;
                }
            }
        }
    }

    private void Fight(GameObject dino1, GameObject dino2)
    {
        GameObject dinoWinner;
        GameObject dinoLoser;

        if(dino1.GetComponent<DinoProperties>().strength > dino2.GetComponent<DinoProperties>().strength)
        {
            dinoWinner = dino1;
            dinoLoser = dino2;
        }
        else
        {
            dinoWinner = dino2;
            dinoLoser = dino1;
        }

        var animationManager = FindObjectOfType<AnimationManager>();

        var dinoWinneranimator = dinoWinner.GetComponentInChildren<Animation>();
        var dinoLoseranimator = dinoLoser.GetComponentInChildren<Animation>();

        var audio = GetComponent<AudioSource>();

        var animationName = animationManager.GetAttackAnimation(dinoWinner.gameObject.name);
        dinoWinneranimator[animationName].wrapMode = WrapMode.Once;
        dinoWinneranimator.Play(animationName);

        audio.Play();


        // Add the other animations
        animationName = animationManager.GetFallAnimation(dinoLoser.gameObject.name);
        dinoLoseranimator[animationName].wrapMode = WrapMode.Once;
        dinoLoseranimator.Play(animationName);

        animationName = animationManager.GetIdleAnimation(dinoWinner.gameObject.name);
        dinoWinneranimator[animationName].wrapMode = WrapMode.Loop;
        dinoWinneranimator.PlayQueued(animationName);
    }


    // Method to check if the distance between two dinos is within the threshold

    public bool AreDinosClose(GameObject dino1, GameObject dino2)
    {
        return Vector3.Distance(dino1.transform.position, dino2.transform.position) < closeThreshold;
    }

}
