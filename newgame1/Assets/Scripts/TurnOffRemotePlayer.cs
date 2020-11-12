using UnityEngine;
using Mirror;
using UnityEngine.Networking;

//note: add scripts game object
//		when multiplayer game start
//		turn off other players that arre not you

public class TurnOffRemotePlayer : NetworkBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Player scr = this.GetComponent<Player>();

        if(this.isLocalPlayer== true)
        {
        	scr.enabled= true;
        }
        else
        {
        	scr.enabled= false;
        }
    }
}
