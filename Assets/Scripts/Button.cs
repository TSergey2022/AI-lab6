using UnityEngine;
using UnityEngine.Tilemaps;

// ReSharper disable InconsistentNaming

public enum ButtonStates {
    Unpressed,
    Pressed
}

public class Button : DoorActivator
{
    MeshRenderer mesh;
    ButtonStates state = ButtonStates.Unpressed;
    [SerializeField]
    Material pressedMaterial;
    [SerializeField]
    Material unpressedMaterial;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.material = unpressedMaterial;
        tag = "button_bad";
    }

    int collisionsCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("agent")) {
            return;
            if (collisionsCount <= 0) {
                var env = transform.parent.GetComponent<GameEnvController>();
                env.GetAgent().AddReward(-1.0f);
            }
            return;
        }
        collisionsCount++;
        if (state != ButtonStates.Unpressed) return;
        state = ButtonStates.Pressed;
        mesh.material = pressedMaterial;
        tag = "button_ok";
        OnActivate.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        return;
        if (!other.gameObject.CompareTag("agent")) return;
        if (collisionsCount <= 0) {
            var env = transform.parent.GetComponent<GameEnvController>();
            env.GetAgent().AddReward(-0.1f);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("agent")) return;
        collisionsCount--;
        if (collisionsCount > 0 || state != ButtonStates.Pressed) return;
        state = ButtonStates.Unpressed;
        mesh.material = unpressedMaterial;
        tag = "button_bad";
        OnDeactivate.Invoke();
    }
    
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("agent")) return;
        collisionsCount++;
        if (state != ButtonStates.Unpressed) return;
        state = ButtonStates.Pressed;
        mesh.material = pressedMaterial;
        tag = "button_ok";
        OnActivate.Invoke();
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.CompareTag("agent")) return;
        collisionsCount--;
        if (collisionsCount > 0 || state != ButtonStates.Pressed) return;
        state = ButtonStates.Unpressed;
        mesh.material = unpressedMaterial;
        tag = "button_bad";
        OnDeactivate.Invoke();
    }
}
