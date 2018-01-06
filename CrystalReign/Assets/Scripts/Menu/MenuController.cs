using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void loadlevel(string level)
    {
        SceneManager.LoadScene(level);
    }
    public void quit()
    {
        Application.Quit();
    }
}
