using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickUpHelp : MonoBehaviour
{
    [SerializeField] GameObject[] characterPrefabs;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player[] characters = GameObject.FindObjectsOfType<Player>();
            List<string> characterNames = new();

            foreach (Player character in characters)
            { characterNames.Add(character.GetName()); }

            foreach (GameObject characterPrefab in characterPrefabs) 
            { 
                if (!characterNames.Contains(characterPrefab.name)) 
                {
                    GameObject playerHelper = Instantiate(characterPrefab, other.transform.localPosition, Quaternion.identity);

                    playerHelper.tag = "PlayerAI";
                    playerHelper.GetComponent<PlayerController>().enabled = false;
                    playerHelper.GetComponent<PlayerSurroundDodgeAgent>().enabled = true;

                    break;
                }
            }

            Destroy(gameObject);
        }  
    }
}
