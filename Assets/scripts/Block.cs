using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
    //properties to mark blocks, value is a occupied block, active are the falling blocks. And last property is color of the block.
    public int value;
    public int active;
    public int color;
    public string owner;
    
    public Block()
    {

    }

    //Constructor without owner
    public Block(int value, int active, int color)
    {
        this.value = value;
        this.active = active;
        this.color = color;
        owner = "";
    }

    //Constructor with all properties
    public Block(int value, int active, int color, string owner)
    {
        this.value = value;
        this.active = active;
        this.color = color;
        this.owner = owner;
    }
}
