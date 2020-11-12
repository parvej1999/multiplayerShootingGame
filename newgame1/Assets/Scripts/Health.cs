using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.Networking;
public class Health : NetworkBehaviour
{
    public const int maxHealth = 100 ;
    [SyncVar] public int currentHealth = maxHealth ;
    public RectTransform healthBar;
    public bool destroyOnDeath;
    private NetworkStartPosition[] spawnPoints;

    void Start()
    {
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
    }
    
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            /*if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                currentHealth = maxHealth;
                RpcRespawn();
            }*/
            Destroy(gameObject);
        }
        healthBar.sizeDelta = new Vector2(currentHealth * 2, healthBar.sizeDelta.y);
    }
    [ClientRpc]
    void RpcRespawn()
    {
    	if (isLocalPlayer)
    	{
    	    Vector3 spawnPoint = Vector3.zero;
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            transform.position = spawnPoint;

    	}
    }
}
