using UnityEngine;

public class StopsProjectionBlue : MonoBehaviour
{
    
    [SerializeField] Animator BlueAnim;
    [SerializeField] Animator RedAnim;
    


    void Update()
    {
        if(RedAnim.GetBool("IsAttracting") == true)
        {
            BlueAnim.SetBool("IsRepeling", false);
        }
        if(BlueAnim.GetBool("IsRepeling") == true)
        {
            RedAnim.SetBool("IsAttracting", false);
        }
       
    }
    public void End()
    {
        gameObject.SetActive(false);
    }
}
