using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColumnManager : MonoBehaviour
{
    [SerializeField] List<GameObject> slots;
    int freeSlotIndex = 0;
    int slotMax;
    public EventHandler OnFilled;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform a in transform)
            slots.Add(a.gameObject);
        slotMax = slots.Count;
    }

    public int PlaceCircle(GameObject circle, SlotManager.Player occupant)
    {
        if (!CanPlaceCircle())
            return -1;
        Instantiate(circle, slots[freeSlotIndex].transform.position, Quaternion.identity, slots[freeSlotIndex].transform);
        slots[freeSlotIndex].GetComponent<SlotManager>().SetOccupator(occupant);
        freeSlotIndex++;
        if (freeSlotIndex == slotMax)
            OnFilled?.Invoke(this, EventArgs.Empty);
        return freeSlotIndex - 1;
    }
    public bool CanPlaceCircle()
    {
        if (freeSlotIndex >= slotMax)
            return false;
        return true;
    }
    public GameObject GetSlot(int index)
    {
        return slots[index];
    }
    public void EmptySlots()
    {
        freeSlotIndex = 0;
        foreach(GameObject slot in slots)
        {
            slot.GetComponent<SlotManager>().Reset();
            if (slot.transform.childCount != 0)
            {
                GameObject slotObject = slot.transform.GetChild(0).gameObject;
                Destroy(slotObject);
            }

        }
    }
}