using UnityEngine;

public class PlayerFactory : MonoBehaviour
{
    public GameObject Create(PlayerId playerId)
    {
        DestroyAllPlayers();

        GameObject player = FindPlayer(playerId);
        return Instantiate(player, new Vector3(63, 0, 66), Quaternion.identity);
    }

    private GameObject FindPlayer(PlayerId playerId)
    {
        string prefabName = "";

        switch (playerId)
        {
            case PlayerId.MICHI:
                prefabName = "Michi";
                break;
            case PlayerId.DETECTIVE:
                prefabName = "MichiDetective";
                break;
            case PlayerId.FACHERO:
                prefabName = "MichiFachero";
                break;
            case PlayerId.EASTWOOD:
                prefabName = "MichiEastwood";
                break;
        }

        return Resources.Load<GameObject>("Characters/" + prefabName);
    }

    private void DestroyAllPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject p in players)
        { Destroy(p); }
    }
}

public enum PlayerId
{
    MICHI,
    EASTWOOD,
    DETECTIVE,
    FACHERO
}
