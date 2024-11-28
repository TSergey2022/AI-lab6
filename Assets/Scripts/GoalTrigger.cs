using UnityEngine;
using UnityEngine.Events;

public class GoalTrigger : MonoBehaviour
{
    public UnityEvent finished;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("agent")) finished.Invoke();
    }
}
