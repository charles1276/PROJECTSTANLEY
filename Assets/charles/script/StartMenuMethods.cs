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
    public void Quitbutton()
    {
        Debug.Log("Quit the game");
        Application.Quit();
    }
    //this brings you to the credits scene
    public void credtsbutton() 
    {        
        SceneManager.LoadScene("Credits");
    }
    
    public void restartbutton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   
}
