using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCutScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene("EndCutScene");
    }
}  