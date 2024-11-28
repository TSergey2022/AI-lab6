using UnityEngine;
using UnityEngine.Events;
// ReSharper disable InconsistentNaming

public class DoorActivator : MonoBehaviour
{
    public UnityEvent OnActivate { get; } = new();
    public UnityEvent OnDeactivate { get; } = new();
}

public class Door : MonoBehaviour
{
    private DoorActivator[] activators;

    private int activeCounter;
    public UnityEvent onDoorOpen;

    public float GetActiveValue()
    {
        if (gameObject.activeSelf)
        {
            return (float)activeCounter / activators.Length;
        }
        return 1f;
    }
    
    private void OnActivate()
    {
        activeCounter++;
        if (gameObject.activeSelf)
        {
            var env = transform.parent.parent.GetComponent<GameEnvController>();
            env.GetAgent().AddReward(1.0f);
        }
        if (activeCounter == activators.Length) Open();
    }
    private void OnDeactivate()
    {
        activeCounter--;
        if (gameObject.activeSelf)
        {
            var env = transform.parent.parent.GetComponent<GameEnvController>();
            env.GetAgent().AddReward(-1.0f);
            env.GetAgent().EndEpisode();
        }
    }
    public void ResetActivators(DoorActivator[] newActivators)
    {
        if (activators != null)
            foreach (var activator in activators)
            {
                activator.OnActivate.RemoveListener(OnActivate);
                activator.OnDeactivate.RemoveListener(OnDeactivate);
            }
        activators = newActivators;
        gameObject.SetActive(true);
        foreach (var activator in activators)
        {
            activator.OnActivate.AddListener(OnActivate);
            activator.OnDeactivate.AddListener(OnDeactivate);
        }
    }

    void Open()
    {
        gameObject.SetActive(false);
        onDoorOpen.Invoke();
    }
}
