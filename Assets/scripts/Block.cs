using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
    public int value;
    public int active;
    public int coorX;
    public int coorY;
    public int color;
    
    public Block()
    {

    }

    public Block(int value, int active)
    {
        this.value = value;
        this.active = active;
    }
}
