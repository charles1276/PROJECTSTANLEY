using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Collectibles
{
    None,
    WeightL,
    WeightM,
    WeightH,
    Negative
}

public class CollectibleObject
{
    
}

public class InventoryManager : MonoBehaviour
{
    public int selectedSlotIndex = 0;
    [SerializeField] Collectibles[] inventorySlots = new Collectibles[3];

    private GameObject HUD;

    // add a collectible to the inventory
    public void AddCollectible(Collectibles collectible)
    {
        int emptySlotIndex = FirstEmptySlot();

        // check if the inventory is full
        if (emptySlotIndex == -1)
        {
            Debug.Log("Inventory is full. Cannot add collectible.");
            return;
        }

        // find the first empty slot and add the collectible
        inventorySlots[emptySlotIndex] = collectible;
    }

    // remove a collectible from the inventory
    public void RemoveCollectible(int slotIndex)
    {
        // check if the slot index is valid
        if (slotIndex < 0 || slotIndex >= inventorySlots.Length)
        {
            Debug.Log("Invalid slot index. Cannot remove collectible.");
            return;
        }

        // remove the collectible from the specified slot
        inventorySlots[slotIndex] = Collectibles.None;
    }

    // test method to add a collectible to the inventory
    private int FirstEmptySlot()
    {
        // find the first empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] == Collectibles.None)
            {
                return i;
            }
        }

        return -1; // no empty slots
    }

    // test method to check if a collectible is in the inventory
    private bool HasItem(Collectibles collectible)
    {
        // test if the inventory contains the specified collectible
        foreach (var slot in inventorySlots)
        {
            if (slot == collectible)
            {
                return true;
            }
        }

        // collectible not found
        return false;
    }

    // --------------------------------------------------------------
    // INPUT EVENTS

    public void NextSlot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            // wrap around to the first slot if at the end
            selectedSlotIndex = (selectedSlotIndex + 1) % inventorySlots.Length;
        }
    }

    public void PreviousSlot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            // wrap around to the last slot if at the beginning
            selectedSlotIndex = (selectedSlotIndex - 1 + inventorySlots.Length) % inventorySlots.Length;
        }
    }

    public void FirstSlot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            selectedSlotIndex = 0;
        }
    }

    public void SecondSlot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            selectedSlotIndex = 1;
        }
    }

    public void ThirdSlot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            selectedSlotIndex = 2;
        }
    }

    // --------------------------------------------------------------
    // UNITY METHODS

    private void Start()
    {
        HUD = GameObject.FindGameObjectWithTag("HUD");
    }

    private void Update()
    {
        
    }
}
