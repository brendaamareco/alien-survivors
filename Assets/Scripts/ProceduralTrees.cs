using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralTrees : MonoBehaviour
{
    public GameObject[] objetosPosibles;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(
           objetosPosibles[Random.Range(0, objetosPosibles.Length)],
           transform.position,
           Quaternion.identity
           );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
