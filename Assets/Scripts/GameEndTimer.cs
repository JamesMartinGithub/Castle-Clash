using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameEndTimer : MonoBehaviour
{
    private int timeM = 5;
    private int timeS = 00;
    public TMP_Text text;
    void Start() {
        // The SubtractTime method runs every second
        InvokeRepeating("SubtractTime", 1, 1);   
    }
    private void SubtractTime() {
        // Subtracts seconds, or minutes when the seconds counter is 00
        if (!(timeS <= 0 && timeM == 0)) {
            if (timeS > 0) {
                timeS -= 1;
            }
            else {
                timeS = 59;
                timeM -= 1;
            }
            // Displays both minutes and seconds in a clock format
            if (timeS <= 9) {
                // Ensures 0 seconds displays as 00
                text.text = timeM.ToString() + ":" + "0" + timeS.ToString();
            }
            else {
                text.text = timeM.ToString() + ":" + timeS.ToString();
            }
            
        }
        else {
            // Game ends in draw
            SceneManager.LoadScene("EndDraw");
        }
    }
}