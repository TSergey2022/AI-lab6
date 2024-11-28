using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            var env = transform.parent.gameObject.GetComponent<GameEnvController>();
            env.GetAgent().AddReward(-1f);
            env.GetAgent().EndEpisode();
        }
    }
}
