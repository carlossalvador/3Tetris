using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
    public int value;
    public int active;
    public int color;
    
    public Block()
    {

    }

    public Block(int value, int active, int color)
    {
        this.value = value;
        this.active = active;
        this.color = color;
    }
}
