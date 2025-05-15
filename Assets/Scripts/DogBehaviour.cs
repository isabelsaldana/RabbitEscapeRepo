using UnityEngine;

public abstract class DogBehaviour : MonoBehaviour
{
    public Dog dog { get; private set; }
    public float duration;

    private void Awake()
    {
        dog = GetComponent<Dog>();
        enabled = false; 
    }

    public void Enable()
    {
        Enable(duration);
    }

    public virtual void Enable(float duration)
    {
        enabled = true;

        CancelInvoke();
        Invoke(nameof(Disable), duration);
    }

    public virtual void Disable()
    {
        enabled = false;

        CancelInvoke();
    }
}
