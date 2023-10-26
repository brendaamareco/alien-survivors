using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

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

    private void LoadCharacters()
    {
        GameObject[] charactersPrefab = Resources.LoadAll<GameObject>("Characters/");
        List<string> currentPlayers = LoadCurrentCharactersOnScene();

        foreach (GameObject characterPrefab in charactersPrefab)
        {
            DestroyAllPlayers();

            Player player = characterPrefab.GetComponent<Player>();

            if ( !currentPlayers.Contains(player.GetName()) )
            {
                GameObject character = Instantiate(characterPrefab, new Vector3(63, 0, 66), Quaternion.identity);
                character.SetActive(false);
                
                characters.Add(player.GetName(), character);
            }
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

    private List<string> LoadCurrentCharactersOnScene()
    {
        List<string> currentPlayers = new();


        foreach (GameObject currentPlayerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            try
            {
                Player currentPlayer = currentPlayerObject.GetComponent<Player>();
                currentPlayers.Add(currentPlayer.GetName());
                Debug.Log("NAME:" + currentPlayer.GetName());
                
                characters.Add(currentPlayer.GetName(), currentPlayerObject);
            }
            catch { }      
        }

        return currentPlayers;
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

