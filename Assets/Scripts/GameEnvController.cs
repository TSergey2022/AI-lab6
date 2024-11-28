using UnityEngine;
// ReSharper disable InconsistentNaming

public class GameEnvController : MonoBehaviour
{
    public int buttonsOnEpisode = 4;
    public int boxesOnEpisode = 3;
    public int solidBoxesOnEpisode = 1;

    private AgentPusher agent;
    public GridedDistributor buttonsDistributor;
    public GridedDistributor boxDistributor;
    public GridedDistributor agentsDistributor;
    public Door door;
    public MeshCollider goal;
    private float timerStart;
    
    public AgentPusher GetAgent() => agent;
    void Start()
    {
        // door.onDoorOpen.AddListener(() => goal.gameObject.SetActive(true));
        // door.onDoorOpen.AddListener(() => print(timerStart - Time.time));
        ResetScene();
    }
    public void ResetScene()
    {
        // timerStart = Time.time;
        goal.gameObject.SetActive(false);
        var buttons = buttonsDistributor.Respawn(buttonsOnEpisode);
        var boxes = boxDistributor.Respawn(boxesOnEpisode + solidBoxesOnEpisode);
        for (var i = 0; i < boxesOnEpisode + solidBoxesOnEpisode; i++)
        {
            var box = boxes[i];
            if (i < boxesOnEpisode)
            {
                box.tag = "block";
                box.GetComponent<Rigidbody>().isKinematic = false;
                box.transform.rotation = Quaternion.Euler(0, 45, 0);
            }
            else
            {
                box.tag = "wall";
                box.GetComponent<Rigidbody>().isKinematic = true;
            }
            
        }
        var activators = new DoorActivator[buttons.Length];
        for (var i = 0; i < buttons.Length; i++)
            activators[i] = buttons[i].GetComponent<Button>();
        door.ResetActivators(activators);

        agent = agentsDistributor.Respawn(1)[0].GetComponent<AgentPusher>();
        agent.envController = this;
    }

    public void OnGoalTriggered()
    {
        agent.AddReward(10.0f);
        agent.EndEpisode();
        ResetScene();
    }
}
