using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{

   public  GameObject particle;

    private void OnDisable()
    {
        //Done so we avoid spawning particles when changing scenes
        if (!gameObject.scene.isLoaded) return;
            Instantiate(particle, transform.position, Quaternion.identity);
    }
}
