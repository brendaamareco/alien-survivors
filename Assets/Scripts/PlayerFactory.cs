using UnityEngine;

public class PlayerFactory : MonoBehaviour
{
    public void Create(PlayerId playerId)
    {
        GameObject player = FindPlayer(playerId);
        Instantiate(player, new Vector3(63, 0, 66), Quaternion.identity);
    }

    public Player GetPlayer(PlayerId playerId)
    { return FindPlayer(playerId).GetComponent<Player>(); }

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
}

public enum PlayerId
{
    MICHI,
    EASTWOOD,
    DETECTIVE,
    FACHERO
}
