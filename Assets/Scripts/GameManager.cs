using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] monsters;  // Array of game objects to check

    // Find me all object that has the script MonsterProperties


    // Start is called before the first frame update
    void Start()
    {
        monsters  = FindObjectsOfType<MonsterProperties>().Select(g => g.gameObject).ToArray();

        foreach (var monster in monsters)
        {
            Debug.Log($"This is a scary monster rawr: ----------------> {monster.name}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
