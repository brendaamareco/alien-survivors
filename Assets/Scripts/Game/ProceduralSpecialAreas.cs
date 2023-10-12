using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralSpecialAreas : MonoBehaviour
{
    public GameObject[] objetosPosibles;

    // Start is called before the first frame update
    void Start()
    {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
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
