using System;
using UnityEngine;

[Serializable]
public class Statistic
{
    private float currentValue;
    [SerializeField] private float maxValue = 100f;
    [SerializeField] private float drainRate = 10f;
    [SerializeField] private float regenRate = 2f;
    [SerializeField] private float regenCooldown = 2f;
    private float regenCooldownTime;

    // constructor (initialization)
    public Statistic()
    {
        currentValue = maxValue;
        regenCooldownTime = 0f;
    }

    // drain the statistic over time
    public void Drain()
    {
        currentValue -= drainRate * Time.deltaTime;
        if (currentValue < 0f)
        {
            currentValue = 0f;
        }

        regenCooldownTime = Time.time + regenCooldown;
    }

    // regenerate the statistic over time
    public void Regenerate()
    {
        if (Time.time < regenCooldownTime)
        {
            return;
        }

        currentValue += regenRate * Time.deltaTime;
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
    }

    // check if statistic can be used
    public bool CanUse()
    {
        return currentValue > 0f;
    }

    // get percentage of statistic (0 to 1)
    public float GetPercentage()
    {
        return currentValue / maxValue;
    }
}

public class PlayerStats : MonoBehaviour
{
    public Statistic stamina;
    public Statistic power;

    // Update is called once per frame
    void Update()
    {
        stamina.Regenerate();
        power.Regenerate();
    }
}
