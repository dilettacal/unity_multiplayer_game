using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public const int maxHealth = 100;
    public int currentHealth = maxHealth;

    //Reference to healthbar
    public RectTransform healthBar;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
        }
        //Update size of healthbar
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }
}