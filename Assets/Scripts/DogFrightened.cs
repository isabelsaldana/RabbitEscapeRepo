using UnityEngine;
using System.Collections;

public class DogFrightened : DogBehaviour
{
    public SpriteRenderer body;
    public SpriteRenderer blue;
    public SpriteRenderer white;

    private bool eaten;
    private Coroutine flashRoutine;

    public override void Enable(float duration)
    {
        base.Enable(duration);

        body.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        //if (flashRoutine != null)
        //StopCoroutine(flashRoutine);

        Invoke(nameof(Flash), duration / 2f);
    }

    public override void Disable()
    {
        base.Disable();

        body.enabled = true;
        blue.enabled = false;
        white.enabled = false;

        //if (flashRoutine != null)
        //    StopCoroutine(flashRoutine);
        //flashRoutine = null;
    }

    private void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false;
            white.enabled = true;
            white.GetComponent<AnimatedSprite>().Restart();
        }
    }

    private void Eaten()
    {
        eaten = true;
        
        Vector3 position = this.dog.home.inside.position;
        position.z = this.dog.transform.position.z;
        this.dog.transform.position = position;

        // dog.SetPosition(dog.home.inside.position);
        dog.home.Enable(duration);

        //if (flashRoutine != null)
        //    StopCoroutine(flashRoutine);
        //flashRoutine = null;

        body.enabled = false;
        blue.enabled = false;
        white.enabled = false;
    }


    private void OnEnable()
    {
        blue.GetComponent<AnimatedSprite>().Restart();
        dog.movement.speedMultiplier = 0.5f;
        eaten = false;
    }

    private void OnDisable()
    {
        dog.movement.speedMultiplier = 1f;
        eaten = false;

        // if (flashRoutine != null)
        //    StopCoroutine(flashRoutine);
        //flashRoutine = null;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (dog.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            dog.movement.SetDirection(direction);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (enabled)
            {
                Eaten();
            }
        }
    }
}
