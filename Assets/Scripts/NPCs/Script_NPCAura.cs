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
        [SerializeField] private Script_NPCSusManager _susManager;

        // If the player gets close enough to the NPC where this triggers, the game will be over right away.
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "TAG_PlayerTransformedHitbox")
            {
                Debug.Log("GG PLAYER TOO CLOSE. GG GG GAME OVER.");
                //_susManager.IsSus = true;
            }
            else
            {
                Debug.Log("The other tag was: " + other.tag);
            }
        }
    }
}
