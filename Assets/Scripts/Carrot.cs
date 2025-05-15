using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Carrot : MonoBehaviour
{

    public int points = 10; 

    protected virtual void Eat()
    {
        FindObjectOfType<GameManager>().CarrotEaten(this);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Rabbit")){
            Eat();
        }
    }
}
