using UnityEngine;

public class PowerCarrot : Carrot
{
    public float duration = 8.0f;

    protected override void Eat()
    {
        FindObjectOfType<GameManager>().PowerCarrotEaten(this);

    }

    
}
