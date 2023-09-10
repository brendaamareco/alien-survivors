using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralTrees : MonoBehaviour
{
    public GameObject[] objetosPosibles;

    void Start()
    {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i ++)
        {
           GameObject tree = Instantiate(
           objetosPosibles[Random.Range(0, objetosPosibles.Length)],
           transform.GetChild(i).transform.position,
           Quaternion.identity
           );

           tree.transform.parent = transform.GetChild(i).transform;      
        }
    }

}
