using UnityEngine;

public class WeightManagement : MonoBehaviour
{
    // IMPORTANT NOTE: This script should be attached to a GameObject that has a Rigidbody2D component.

    // thresholds for weight levels
    [Header("Weight Thresholds")]
    [Tooltip("Mass above this value is considered heavy.")]
    public float heavyThreshold = 10.0f;
    [Tooltip("Mass above this value is considered medium.")]
    public float mediumThreshold = 2.0f;

    // determine weight level based on Rigidbody2D mass
    public byte FindWeightLevel(Rigidbody2D rb)
    {
        // weight level of the object (1 = light, 2 = medium, 3 = heavy)
        byte weightLevel = 0;

        if (rb != null)
        {
            if (rb.mass > heavyThreshold)
            {
                weightLevel = 1; // Heavy
            }
            else if (rb.mass > mediumThreshold)
            {
                weightLevel = 2; // Medium
            }
            else
            {
                weightLevel = 3; // Light
            }
        }

        return weightLevel;
    }

    public string compareWeights(GameObject objOne, GameObject objTwo)
    {
        // get Rigidbody2D components
        Rigidbody2D rbOne = objOne.GetComponent<Rigidbody2D>();
        Rigidbody2D rbTwo = objTwo.GetComponent<Rigidbody2D>();

        // throw error if either object lacks Rigidbody2D
        if (rbOne == null || rbTwo == null)
        {
            return "One or both objects do not have a Rigidbody2D component.";
        }

        // grab weight levels
        int weightOne = FindWeightLevel(objOne.GetComponent<Rigidbody2D>());
        int weightTwo = FindWeightLevel(objOne.GetComponent<Rigidbody2D>());

        // compare weight levels
        if (weightOne == weightTwo)
        {
            return "SAME";
        }
        else if (weightOne > weightTwo)
        {
            return "FIRST_HEAVIER";
        }
        else
        {
            return "SECOND_HEAVIER";
        }
    }
}
