using UnityEngine;
using UnityEngine.SceneManagement;  

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
    public void credtsbutton() 
    {        
        SceneManager.LoadScene("Credits");
    }
    public void backbutton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
