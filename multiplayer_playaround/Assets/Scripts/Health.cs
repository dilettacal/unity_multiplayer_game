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
            // move back to zero location
            transform.position = Vector3.zero;
        }
    }
}