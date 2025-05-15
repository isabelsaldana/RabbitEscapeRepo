using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(Movement))]
public class Dog : MonoBehaviour
{
    public Movement movement { get; private set; }
    public DogHome home { get; private set; }
    public DogScatter scatter { get; private set; }
    public DogChase chase { get; private set; }
    public DogFrightened frightened { get; private set; }
    public DogBehaviour initialBehavior;
    public Transform target;
    public int points = 200;

    
    private void Awake()
    {
        movement = GetComponent<Movement>();
        home = GetComponent<DogHome>();
        scatter = GetComponent<DogScatter>();
        chase = GetComponent<DogChase>();
        frightened = GetComponent<DogFrightened>();
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        if (home != initialBehavior) {
            home.Disable();
        }

        if (initialBehavior != null) {
            initialBehavior.Enable();
        }
    }

    public void SetPosition(Vector3 position)
    {
        // Keep the z-position the same since it determines draw depth
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Rabbit"))
        {
            if (frightened.enabled) {
                FindObjectOfType<GameManager>().DogEaten(this);
            } else {
                FindObjectOfType<GameManager>().RabbitEaten();
            }
        }
    }
    
}