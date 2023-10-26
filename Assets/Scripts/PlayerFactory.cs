using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{
    private Dictionary<string, GameObject> characters;
    private static PlayerFactory instance;

    private void Awake()
    {
        characters = new Dictionary<string, GameObject>();

        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            characters = new Dictionary<string, GameObject>();
            //LoadCharacters();
            DestroyAllPlayers();
        }   
    }

    private Player LoadCharacter(string characterID)
    {
        GameObject[] charactersPrefab = Resources.LoadAll<GameObject>("Characters/");

        foreach (GameObject characterPrefab in charactersPrefab)
        {
            Player player = characterPrefab.GetComponent<Player>();

            if ( characterID == player.GetName())
            {
                GameObject character = Instantiate(characterPrefab, new Vector3(63, 0, 66), Quaternion.identity);
                //characters.Add(player.GetName(), character);

                return player;
            }
        }

        return null;
    }

    public GameObject Create(string playerId)
    {
        DestroyAllPlayers();
        Player player = LoadCharacter(playerId);

        //GameObject player = characters[playerId];

        //player.SetActive(true);

        return player.gameObject;
    }

    private void DestroyAllPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject p in players)
            Destroy(p);

        //foreach (GameObject p in players)
        //{ p.SetActive(false); }
    }
}

