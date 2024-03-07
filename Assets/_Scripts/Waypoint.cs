using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC;
using Unity.VisualScripting;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private bool _shouldInteract = false;
    [SerializeField] private NPCState _npcBehaviour;
    [SerializeField] private string _description;

    public bool ShouldInteract    { get { return _shouldInteract; } }
    public NPCState NPCBehaviour  { get { return _npcBehaviour; } }
    public string Description  { get { return _description; } }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "TAG_Player")
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log($"Player: {other.gameObject.name} interacted with waypoint {gameObject.name}");
            }
        }
    }
}
