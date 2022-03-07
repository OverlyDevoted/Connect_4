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
    public bool isOccuppied = false;
    public Player occupator = Player.None;
    private SpriteRenderer sprite;
  
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void FillSlot(Player occupator, Color color)
    {
        isOccuppied = true;
        this.occupator = occupator;
        sprite.color = color;
        Color opaque = sprite.color;
        opaque.a = 1f;
        sprite.color = opaque;
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
        Color a = sprite.color;
        a.a = 0;
        sprite.color = a;
    }
}
