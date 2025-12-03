using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainRate = 10f;
    [SerializeField] private float staminaRegenRate = 5f;
    private float stamina; // drains when sprinting

    [Header("Power")]
    [SerializeField] private float maxPower = 100f;
    [SerializeField] private float powerDrainRate = 1f;
    public float powerRegenRate = 1f;
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
        // debug key to drain stamina
        if (Input.GetKey(KeyCode.Semicolon))
        {
            drainStamina();
        }
        // debug key to drain power
        if (Input.GetKey(KeyCode.Quote))
        {
            drainPower();
        }
    }

    public bool canSprint()
    {
        return stamina > 0f;
    }

    private float drainStat(float stat, float drainRate)
    {
        stat -= drainRate * Time.fixedDeltaTime;
        // clamp stat to minimum of 0
        if (stat < 0f)
        {
            stat = 0f;
        }

        return stat;
    }

    private float regenStat(float stat, float maxstat, float regenRate)
    {
        stat += regenRate * Time.fixedDeltaTime;
        // clamp stat to maximum
        if (stat > maxstat)
        {
            stat = maxstat;
        }

        return stat;
    }

    public void drainStamina()
    {
        stamina = drainStat(stamina, staminaDrainRate);
    }

    public void regenStamina()
    {
        stamina = regenStat(stamina, maxStamina, staminaRegenRate);
    }

    public void drainPower()
    {
        power = drainStat(power, powerDrainRate);
    }

    public void regenPower()
    {
        power = regenStat(power, maxPower, powerDrainRate);
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
