using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Base_Health_Script : Health_Script
{
    public bool friendly;
    // Base Monobehaviour functions must be called in derived classes to function and can't just be inherited
    new void Start() {
        base.Start();
    }
    // The health of the base is checked whenever the health changes
    override public void Check_Health() {
        if (health <= 0)
        {
            // Game ends (friendly = lose, not friendly = win)
            if (friendly) {
                SceneManager.LoadScene("EndLose");
            }
            else {
                SceneManager.LoadScene("EndWin");
            }
        }
    }
}
