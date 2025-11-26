using UnityEngine;

public enum ObjectPolarity
{
    Positive = 1,
    Neutral = 0,
    Negative = -1,
}

public enum Weight
{
    Light = 1,
    Medium = 2,
    Heavy = 3,
}

public class ObjectProperties : MonoBehaviour
{
    [Tooltip("Polarity of the object affecting magnet interactions.")]
    public ObjectPolarity polarity = ObjectPolarity.Positive;
    public Weight weight = Weight.Medium;

    // compare weight values (static so you dont need an object to refer to the method)
    public static string CompareWeights(Weight weight1, Weight weight2)
    {
        if (weight1 == weight2)
        {
            return "Equal";
        }
        else if ((int)weight1 > (int)weight2)
        {
            return "Greater";
        }
        else
        {
            return "Less";
        }
    }

    private void Start()
    {
        //Debug.Log("Object Polarity: " + polarity);
        //Debug.Log("Object Weight: " + weight);

        ResetMass();
    }

    public void ResetMass()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.mass = (int)weight; // reset mass based on weight enum
    }
}
