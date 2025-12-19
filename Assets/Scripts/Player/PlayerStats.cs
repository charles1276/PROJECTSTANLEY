using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class Statistic
{
    private float currentValue;
    [SerializeField] private float maxValue = 100f;
    [SerializeField] private float drainRate = 10f;
    [SerializeField] private float regenRate = 2f;
    [SerializeField] private float regenCooldown = 2f;
    private float regenCooldownTime;

    [SerializeField] private bool willDrain = true; // set infinite usage

    // constructor (initialization)
    public Statistic()
    {
        currentValue = maxValue;
        regenCooldownTime = 0f;
    }

    // drain the statistic over time
    public void Drain()
    {
        // basically infinite usage
        if (!willDrain)
        {
            return;
        }

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
    public GameObject selectedChest;

    public void Interact(InputAction.CallbackContext ctx)
    {
        // interact with selected chest
        if (ctx.performed && selectedChest != null)
        {
            selectedChest.SendMessage("Interact");
        }
    }

    public void UpdateSelectedChest(GameObject newChest)
    {
        // early termination
        if (newChest == null)
        {
            return;
        }

        if (selectedChest != null)
        {
            // compare distances
            float currentDistance = Vector2.Distance(transform.position, selectedChest.transform.position);
            float newDistance = Vector2.Distance(transform.position, newChest.transform.position);

            if (currentDistance > newDistance)
            {
                // assign new chest if it's closer
                selectedChest = newChest;
            }
        }
        else
        {
            // assign new chest if none was selected
            selectedChest = newChest;
        }
    }

    // Update is called once per frame
    void Update()
    {
        stamina.Regenerate();
        power.Regenerate();

        // skip if no chest selected
        if (selectedChest == null)
        {
            return;
        }

        // deselect chest if too far away
        if (Vector2.Distance(transform.position, selectedChest.transform.position) > 3f)
        {
            selectedChest = null;
        }
    }
}
