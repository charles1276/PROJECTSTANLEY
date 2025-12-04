using UnityEngine;
using UnityEngine.SceneManagement;

public class Hazard : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
