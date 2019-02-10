using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseResource : MonoBehaviour
{
    // TODO:
    //  - Implement player needing certain tool to hit resources to collect them
    //  - Implement "health" for resources (i.e., takes 3 hits to destroy and collect it)
    //  - Implement spawn timer for resources

    private void OnTriggerEnter(Collider other)
    {
        // TODO: This logic will change once the resource tool is implemented. The player
        //       will need to equip the tool and hit resources to collect them.
        BaseCharacter player = other.GetComponent<BaseCharacter>();
        if (player == null) return;

        // The player collided with this resource. Log an event for now
        // and destroy the resource.
        Debug.Log("Player hit and collected " + name);
        Destroy(gameObject);
    }
}
