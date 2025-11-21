using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections; 

public class StartMenuMethods : MonoBehaviour
{

    public void startbutton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void quitbutton()
    {
        Debug.Log("Quit the game");
        Application.Quit();
    }
    //this brings you to the credits scene
    public void credtsbutton() 
    {        
        SceneManager.LoadScene("Credits");
    }
    //this is going to be used to bring you back to the main menu from other scenes
    public void backbutton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public IEnumerator RestartCoroutine(string sceneName)
    {
        SceneManager.LoadScene("main menu");
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}
