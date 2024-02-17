using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("NPCs / Enemies")]
    [Tooltip("A list of NPC's the player must avoid.")]
    [SerializeField] List<GameObject> _npcs;
    [SerializeField] List<Path> _paths;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
