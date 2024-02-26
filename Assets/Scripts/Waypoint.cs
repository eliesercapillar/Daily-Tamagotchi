using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private bool _shouldInteract = false;
    [SerializeField] private NPCState _npcBehaviour;

    public bool ShouldInteract    { get { return _shouldInteract; } }
    public NPCState NPCBehaviour  { get { return _npcBehaviour; } }
}
