using System;
using UnityEngine;

public class OmniEnemy : MonoBehaviour // Omni betekend allesomvattend :P
{
    public GameObject player;
    public float movementSpeed = 1f;

    private void Update()
    {
        if (!player)
        {
            transform.LookAt(player.transform);
            transform.Translate(transform.forward * movementSpeed * Time.deltaTime);
        }
    }
}