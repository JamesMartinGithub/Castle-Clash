using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadNextScene : MonoBehaviour
{
    public string scene;
    public float time;
    private void Start() {
        Invoke("LoadScene", time);
    }
    private void LoadScene() {
        // The specified scene is loaded after the given time
        SceneManager.LoadScene(scene);
    }
}