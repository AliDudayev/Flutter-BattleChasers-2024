using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Dictionary<string, string> idleAnimations = new Dictionary<string, string>();
    private Dictionary<string, string> attackAnimations = new Dictionary<string, string>();
    private Dictionary<string, string> fallAnimations = new Dictionary<string, string>();

    void Update()
    {
        // Write me a print
        //Debug.Log("Marnick stop being a noob");
    }

    // Start is called before the first frame update
    void Start()
    {
        idleAnimations.Add("TriceratopsModel", "NoseDirt");
        idleAnimations.Add("StegosaurusModel", "Shake");
        idleAnimations.Add("TyrannosaurusModel", "Idle2");

        attackAnimations.Add("TriceratopsModel", "Attack");
        attackAnimations.Add("StegosaurusModel", "TailWhip");
        attackAnimations.Add("TyrannosaurusModel", "Attack");

        fallAnimations.Add("TriceratopsModel", "Fall");
        fallAnimations.Add("StegosaurusModel", "Fall");
        fallAnimations.Add("TyrannosaurusModel", "Fall");
    }

    // Add the getters
    public string GetIdleAnimation(string modelName)
    {
        return idleAnimations[modelName];
    }

    public string GetAttackAnimation(string modelName)
    {
        return attackAnimations[modelName];
    }

    public string GetFallAnimation(string modelName)
    {
        return fallAnimations[modelName];
    }
}
