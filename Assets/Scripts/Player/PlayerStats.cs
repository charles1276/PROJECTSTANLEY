using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stamina")]
    public float maxStamina = 100f;
    public float staminaDrainRate = 10f;
    public float staminaRegenRate = 5f;
    private float stamina; // drains when sprinting

    [Header("Power")]
    public float maxPower = 100f;
    public float powerDrainRate = 1f;
    private float power;   // drains with magnet usage

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stamina = maxStamina;
        power = maxPower;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // drain power over time
        power -= powerDrainRate * Time.fixedDeltaTime;
        // clamp power to minimum of 0
        if (power < 0f)
        {
            power = 0f;
        }
    }

    public bool canSprint()
    {
        return stamina > 0f;
    }

    public void drainStamina()
    {
        stamina -= 10f * Time.fixedDeltaTime;
        // clamp stamina to minimum of 0
        if (stamina < 0f)
        {
            stamina = 0f;
        }
    }

    public void regenStamina()
    {
        stamina += 5f * Time.fixedDeltaTime;
        // clamp stamina to maximum
        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
    }

    // get stamina percentage (0 to 1)
    public float getStaminaPercentage()
    {
        return stamina / maxStamina;
    }

    // get power percentage (0 to 1)
    public float getPowerPercentage()
    {
        return power / maxPower;
    }
}
