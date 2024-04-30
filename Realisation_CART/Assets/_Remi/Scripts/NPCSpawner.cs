using BoxSystem;
using CartControl;
using Cinemachine;
using DiscountDelirium;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Spawner
{
    public class NPCSpawner : MonoBehaviour
    {
        [field: SerializeField] private GameObject NPCPrefab { get;  set; } = null;

        [field: SerializeField] private GameObject SpawningZones { get;  set; } = null;

        private List<GameObject> EntranceSpawningSpots { get; set; } = new List<GameObject>();


        private void Awake()
        {
            foreach (Transform child in SpawningZones.transform)
            {
                foreach (Transform grandChild in child)
                {
                    EntranceSpawningSpots.Add(grandChild.gameObject);
                }
            }

            foreach (var spot in EntranceSpawningSpots)
            {
                NPCPrefab = Instantiate(NPCPrefab, spot.transform.position, Quaternion.identity);
                //NPCPrefab.GetComponent<CartStateMachine>().Cart = NPCPrefab.gameObject;
                //NPCPrefab.GetComponent<CartStateMachine>().CartRB = NPCPrefab.GetComponent<Rigidbody>();
                //NPCPrefab.GetComponent<CartStateMachine>().CamBrain = CamBrain;
                //NPCPrefab.GetComponent<CartStateMachine>().VirtualCamera = VirtualCamera;

                //Animator animator = NPCPrefab.GetComponentInChildren<Animator>();
                //if (animator == null || animator.gameObject.name != "SM_Chr_Kid_Adventure_01") Debug.LogError("Animator not found");

                //NPCPrefab.GetComponent<CartStateMachine>().HumanAnimCtrlr = animator;

                //GameObject rigGO = animator.transform.GetChild(1).gameObject;
                //if (rigGO == null || rigGO.name != "FeetOnCart") Debug.LogError("FeetOnCart not found");
                //Rig rig = rigGO.GetComponent<Rig>();

                //NPCPrefab.GetComponent<CartStateMachine>().FeetOnCartRig = rig;
                //NPCPrefab.GetComponent<CartStateMachine>().CartMovement = NPCPrefab.GetComponent<CartMovement>();
                //NPCPrefab.GetComponent<CartStateMachine>().GrindVfx = NPCPrefab.GetComponentInChildren<ManageGrindVfx>();

            }
        }
    }
}
