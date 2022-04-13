using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Key":
                other.gameObject.GetComponent<BoxCollider>().isTrigger = true;
                other.gameObject.transform.parent = gameObject.transform;
                KeyStacker.Instance.keys.Add(other.gameObject);
                EventManager.Instance.KeyStacked();
                break;
            case "Obstacle":
                KeyStacker.Instance.doJumpIndex = KeyStacker.Instance.keys.Count;
                EventManager.Instance.ObstacleTriggered();
                break;
            case "Gate":
                EventManager.Instance.GateTriggered();
                break;
        }

    }
}
