using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    public int playerGold = 0;
    public int enemyGold = 0;
    public float gainRate;
    public Text playerGoldText;
    private void Start()
    {
        // In the tutorial, the gain rate can be set to 0 to stop gold gain
        if (gainRate != 0) {
            // Gold increases by 1 for both players at a set rate
            InvokeRepeating("IncreaseGold", gainRate, gainRate);
            // Enemy gold increases by 1 extra at a reduced rate
            InvokeRepeating("IncreaseEnemyGold", gainRate / 0.5f, gainRate / 0.5f);
        }
    }
    private void IncreaseGold() {
        playerGold += 1;
        playerGoldText.text = playerGold.ToString();
        enemyGold += 1;
    }
    private void IncreaseEnemyGold() {
        enemyGold += 1;
    }
    public bool ReducePlayerGold(int amount) {
        // Checks if the player has enough gold before reducing it
        if (playerGold - amount < 0)
        {
            return false;
        }
        else {
            playerGold -= amount;
            playerGoldText.text = playerGold.ToString();
            return true;
        }
    }
    public bool ReduceEnemyGold(int amount) {
        // Checks if the enemy has enough gold before reducing it
        if (enemyGold - amount < 0)
        {
            return false;
        }
        else
        {
            enemyGold -= amount;
            return true;
        }
    }
}