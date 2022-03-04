using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public enum Player
    {
        Red,
        Yellow,
        None
    }
    bool isOccuppied = false;
    public Player occupator = Player.None;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsOccupied()
    {
        return isOccuppied;
    }
    public Player GetOccupator()
    {
        return occupator;
    }
    public void SetOccupator(Player occupator)
    {
        this.occupator = occupator;
    }
    public void Reset()
    {
        isOccuppied = false;
        occupator = Player.None;
}
}
