using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public int selectedSlotIndex = 0;
    public GameObject[] inventorySlots = new GameObject[3];

    // hud reference
    private GameObject HUD;

    // find the collective weight of the inventory
    private Weight FindWeight()
    {
        int weight = 0;

        // sum collective weights of everything in the inventory
        foreach (var slot in inventorySlots) {
            // prevent errors
            if (slot != null)
            {
                weight += (int)slot.GetComponent<ItemInstance>().itemData.itemWeight;
            }
        }

        // make sure weight doesn't exceed the heavy weight listing
        weight = Mathf.Min(weight, (int)Weight.Heavy);

        return (Weight)weight;
    }

    // swaps out the old object with the new object
    public GameObject SwapObject(GameObject newObject)
    {
        GameObject oldObject = inventorySlots[selectedSlotIndex];
        inventorySlots[selectedSlotIndex] = newObject;
        return oldObject;
    }

    // add a collectible to the inventory
    public void AddCollectible(GameObject collectible)
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
        inventorySlots[slotIndex] = null;
    }

    // test method to add a collectible to the inventory
    public int FirstEmptySlot()
    {
        // find the first empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] == null)
            {
                return i;
            }
        }

        return -1; // no empty slots
    }

    // test method to check if a collectible is in the inventory
    private bool HasItem(GameObject collectible)
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
        // slots
        GameObject[] slots = new GameObject[3];
        slots[0] = HUD.transform.Find("Slot1").gameObject;
        slots[1] = HUD.transform.Find("Slot2").gameObject;
        slots[2] = HUD.transform.Find("Slot3").gameObject;

        for (int i = 0; i < slots.Length; i++)
        {
            // unselected -> default
            slots[i].GetComponent<Image>().sprite = slots[i].GetComponent<StateStorage>().spriteList[0];

            // selected
            if (i == selectedSlotIndex)
            {
                slots[i].GetComponent<Image>().sprite = slots[i].GetComponent<StateStorage>().spriteList[1];
            }

            // object display
            GameObject objDisp = slots[i].transform.Find("objDisp").gameObject;
            objDisp.transform.localPosition = new Vector3(1, -1 + 0.5f * Mathf.Sin(Mathf.PI * Time.time), 0);

            // non-empty slot
            if (inventorySlots[i] != null)
            {
                objDisp.SetActive(true);
                objDisp.GetComponent<Image>().sprite = inventorySlots[i].GetComponent<ItemInstance>().itemData.itemIcon;
            }
            else
            {
                // hide empty
                objDisp.SetActive(false);
            }
        }
    }
}
