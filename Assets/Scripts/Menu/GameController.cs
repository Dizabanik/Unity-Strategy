using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject[] DeactivateGameObjectOnPause;
    public MonoBehaviour[] DeactivateScriptsOnPause;

    private void Start()
    {
        PausePanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            PauseOn();
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseOn()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);

        foreach(GameObject gameObject in DeactivateGameObjectOnPause)
        {
            gameObject.SetActive(false);
        }

        foreach (MonoBehaviour monoBehaviour in DeactivateScriptsOnPause)
        {
            monoBehaviour.enabled = false;
        }
    }
    public void PauseOff()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);

        foreach (GameObject gameObject in DeactivateGameObjectOnPause)
        {
            gameObject.SetActive(true);
        }

        foreach (MonoBehaviour monoBehaviour in DeactivateScriptsOnPause)
        {
            monoBehaviour.enabled = true;
        }
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
