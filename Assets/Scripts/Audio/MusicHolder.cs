using UnityEngine;

public class MusicHolder : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip background;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
}
