using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColumnManager : MonoBehaviour
{
    [SerializeField] List<SlotManager> slots;
    int freeSlotIndex = 0;
    int slotMax;
    public EventHandler OnFilled;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform a in transform)
            slots.Add(a.gameObject.GetComponent<SlotManager>());
        slotMax = slots.Count;
    }

    public int PlaceCircle(SlotManager.Player occupant, Color color)
    {
        if (!CanPlaceCircle())
            return -1;
        slots[freeSlotIndex].FillSlot(occupant, color);
        //Instantiate(circle, slots[freeSlotIndex].transform.position, Quaternion.identity, slots[freeSlotIndex].transform);
        //slots[freeSlotIndex].SetOccupator(occupant);
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
    public SlotManager GetSlot(int index)
    {
        return slots[index];
    }
    public void EmptySlots()
    {
        freeSlotIndex = 0;
        int slotLenght = slots.Count;
        for(int i = 0; i<slotLenght;i++)
        {
            slots[i].Reset();
        }
    }
}