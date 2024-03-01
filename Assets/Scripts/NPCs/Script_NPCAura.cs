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

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.tag == "TAG_PlayerHitbox")
            {
                _susManager.IsSus = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "TAG_PlayerHitbox")
            {
                _susManager.IsSus = false;
            }
        }
    }
}
