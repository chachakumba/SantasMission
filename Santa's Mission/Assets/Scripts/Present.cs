using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour
{
    [SerializeField] GameObject destroyParticles;
    public float timeToDestroyParticles = 3;
    private void OnDestroy()
    {

        Destroy(Instantiate(destroyParticles, transform.position, Quaternion.identity), timeToDestroyParticles);
    }
}
