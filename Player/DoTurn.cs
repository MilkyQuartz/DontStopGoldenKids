using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoTurn : MonoBehaviour
{
    PlayerController controller;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerController>(out controller))
        {
            controller.isTurn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out controller))
        {
            controller.isTurn = false;
        }
    }
}
