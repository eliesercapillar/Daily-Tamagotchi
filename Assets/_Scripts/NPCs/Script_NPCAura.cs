using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NPC
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Script_NPCAura : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private Script_NPCMoodManager _moodManager;

        // If the player gets close enough to the NPC where this triggers, the game will be over right away.
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "TAG_Player&NPC")
            {
                _moodManager.SusAmount = 100.0f;
                _moodManager.UpdateMood();
            }
        }
    }
}
