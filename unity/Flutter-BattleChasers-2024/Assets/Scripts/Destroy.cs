using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    private ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            StartCoroutine(DestroyAfterParticle());
        }
    }

    private IEnumerator DestroyAfterParticle()
    {
        yield return new WaitUntil(() => !particleSystem.IsAlive());
        Destroy(gameObject);
    }
}
