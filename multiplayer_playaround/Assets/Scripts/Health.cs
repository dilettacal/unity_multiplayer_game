using UnityEngine;
using UnityEngine.UI;
//Synchro on server
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour
{
    public bool destroyOnDeath;
    public const int maxHealth = 100;

    //Synchro variable currenthealth
    //Synchro only happens on server side --> We need to synchronize foreground on client side!
    //[SyncVar]
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;
    public RectTransform healthBar;

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
        //check if server
        if (!isServer)
        {
            return;
        }

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                // currentHealth = 0;
                //Debug.Log("Dead!");
                //RPC SPAWN
                currentHealth = maxHealth;
                // called on the Server, but invoked on the Clients
                RpcRespawn();
            }
        }

    }

    void OnChangeHealth(int health)
    {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            //SPAWN and RESPAWN
            // Set the spawn point to origin as a default value
            Vector3 spawnPoint = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick one at random
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            // Set the player’s position to the chosen spawn point
            transform.position = spawnPoint;

        }
    }
}