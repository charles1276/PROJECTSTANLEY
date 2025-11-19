using UnityEngine;
using UnityEngine.SceneManagement;  

public class StartMenuMethods : MonoBehaviour
{
    public void startbutton(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("sceneName");
    }
    public void quitbutton()
    {
        Debug.Log("Quit the game");
        Application.Quit();
    }
    public void credtsbutton() 
    {        
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }
}
