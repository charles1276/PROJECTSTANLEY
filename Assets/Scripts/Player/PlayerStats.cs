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

    private float drainStat(float stat)
    {
        stat -= 10f * Time.fixedDeltaTime;
        // clamp stat to minimum of 0
        if (stat < 0f)
        {
            stat = 0f;
        }

        return stat;
    }

    private float regenStat(float stat, float maxstat)
    {
        stat += 5f * Time.fixedDeltaTime;
        // clamp stat to maximum
        if (stat > maxstat)
        {
            stat = maxstat;
        }

        return stat;
    }

    public void drainStamina()
    {
        stamina = drainStat(stamina);
    }

    public void regenStamina()
    {
        stamina = regenStat(stamina, maxStamina);
    }

    public void drainPower()
    {
        power = drainStat(power);
    }

    public void regenPower()
    {
        power = regenStat(power, maxPower);
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
