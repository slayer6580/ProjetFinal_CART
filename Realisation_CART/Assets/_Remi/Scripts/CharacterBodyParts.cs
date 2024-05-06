using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Spawner
{
    [ExecuteInEditMode]
    public class CharacterBodyParts : MonoBehaviour
    {
        [SerializeField] private Material m_polygonKids_Material_01_A;
        [SerializeField] private Material m_polygonKids_Material_02_A;
        [SerializeField] private Material m_polygonKids_Material_03_A;
        [SerializeField] private Material m_polygonKids_Material_04_A;
        [SerializeField] private Material m_shoes_Mat;
        [SerializeField] private Material m_skin_Mat;

        [SerializeField] private Transform m_handOnCartTransform = null;
        [SerializeField] private Transform m_feetOnCartTransform = null;
        [SerializeField] private Transform m_hipGOTransform = null;

        [SerializeField] private Transform m_rootGO = null;

        // There is a few material that can be applied to the face
        // Face_01 by default and Face_Crying, 
        [SerializeField] private Transform m_faceMatParentsTransform = null;
        [SerializeField] private Transform m_frecklesTransform = null;
        [SerializeField] private Transform m_robotFaceTransform = null;
        [SerializeField] private Transform m_hairTransform = null;
        [SerializeField] private Transform m_accessoriesTransform = null;

        [SerializeField] private Transform m_eyebrowsTransform = null;
        [SerializeField] private Transform m_eyesFemaleTransform;
        [SerializeField] private Transform m_eyesMaleTransform;
        [SerializeField] private List<Transform> m_hairPartTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_humanFullBodyTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_humanInCostumeFullBodyTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_nonHumanFullBodyTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_hairTransforms = new List<Transform>();

        enum BodyPartType
        {
            Human,
            HumanInCostume,
            NonHuman,
            AllBodyParts
        }

        [SerializeField] private BodyPartType m_bodyPartType = BodyPartType.Human;

        private int m_currentHumanFullBodyIndex = -1;
        private int m_currentHumanInCostumeFullBodyIndex = -1;
        private int m_currentNonHumanFullBodyIndex = -1;
        private int m_currentBodyPartTypeIndex = -1;
        

        private void Start()
        {
            //if (m_handOnCartGO != null) return;
            //InitializeVariables();
            VerifyIntegrityOfVariables();
            RandomizeFace();
            RandomizeBody();
        }

        private void OnEnable() // Keep the OnEnable to have the checkbox in the inspector
        { }

        private void GetFullBodyPartsGO()
        {
            Debug.Log("GetBodyPartsGO");

            m_handOnCartTransform = transform.Find("HandOnCart");
            m_feetOnCartTransform = transform.Find("FeetOnCart");
            m_hipGOTransform = transform.Find("Hip");
            m_rootGO = transform.Find("Root");
            m_eyebrowsTransform = transform.Find("SM_Chr_Eyebrows_01");
            m_eyesFemaleTransform = transform.Find("SM_Chr_Eyes_Female_01");
            m_eyesMaleTransform = transform.Find("SM_Chr_Eyes_Male_01");
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Adventure_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Ballerina_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_CargoShorts_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Casual_04"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Doctor_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Cheerleader_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Dress_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Eastern_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Eastern_Skirt_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Exercise_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Exercise_02"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Farmer_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Fat_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Fat_02"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Hoodie_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Hoodie_02"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Hoodie_03"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Nerd_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Overalls_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Overalls_02"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Overalls_Dress_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_PlaidShirt_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_PoliceOfficer_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_PufferVest_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Punk_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Raincoat_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Raincoat_02"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Robber_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Schoolboy_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Schoolboy_02"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Schoolgirl_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Schoolgirl_02"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Scout_Shorts_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Scout_Skirt_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_ShirtDress_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Skater_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_SnowJacket_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Summer_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Sweater_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Sweater_02"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Sweater_Dress_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Trucker_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_WinterCoat_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Explorer_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Cowboy_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Cowboy_02"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Footballer_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Karate_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Ninja_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Pajamas_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Pilot_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Survivor_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Survivor_Vest_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Swimwear_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Swimwear_02"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Tracksuit_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Viking_01"));
            m_humanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Wizard_01"));

            if (m_handOnCartTransform != null) return;

            //Transform[] childrenGOs = GetComponentsInChildren<Transform>();
            int childCount = transform.childCount;
            Debug.Log("childCount: " + childCount);
            for (int i = 0; i < childCount; i++)
            {
                Transform childGO = transform.GetChild(i);

                if (m_handOnCartTransform == null && childGO.name == "HandOnCart") { m_handOnCartTransform = childGO; return; }
                else if (m_feetOnCartTransform == null && childGO.name == "FeetOnCart") { m_feetOnCartTransform = childGO; return; }
                else if (m_hipGOTransform == null && childGO.name == "Hip") { m_hipGOTransform = childGO; return; }
                else if (m_rootGO == null && childGO.name == "Root") { m_rootGO = childGO; return; }
                else if (m_eyebrowsTransform == null && childGO.name == "SM_Chr_Eyebrows_01") { m_eyebrowsTransform = childGO; return; }// Face parts
                else if (m_eyesFemaleTransform == null && childGO.name == "SM_Chr_Eyes_Female_01") { m_eyesFemaleTransform = childGO; return; }
                else if (m_eyesMaleTransform == null && childGO.name == "SM_Chr_Eyes_Male_01") { m_eyesMaleTransform = childGO; return; }
                else if (childGO.name == "SM_Chr_Kid_Adventure_01") { m_humanFullBodyTransforms.Add(childGO); return; } // casual human full bodies
                else if (childGO.name == "SM_Chr_Kid_Ballerina_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_CargoShorts_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Casual_04") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Doctor_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Cheerleader_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Dress_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Eastern_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Eastern_Skirt_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Exercise_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Exercise_02") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Farmer_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Fat_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Fat_02") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Hoodie_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Hoodie_02") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Hoodie_03") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Nerd_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Overalls_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Overalls_02") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Overalls_Dress_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_PlaidShirt_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_PoliceOfficer_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_PufferVest_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Punk_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Raincoat_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Raincoat_02") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Robber_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Schoolboy_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Schoolboy_02") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Schoolgirl_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Schoolgirl_02") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Scout_Shorts_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Scout_Skirt_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_ShirtDress_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Skater_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_SnowJacket_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Summer_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Sweater_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Sweater_02") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Sweater_Dress_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Trucker_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_WinterCoat_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Explorer_01") { m_humanFullBodyTransforms.Add(childGO); return; }// Humans in costumes that can pass as cusual humans full bodies
                else if (childGO.name == "SM_Chr_Kid_Cowboy_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Cowboy_02") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Footballer_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Karate_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Ninja_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Pajamas_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Pilot_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Survivor_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Survivor_Vest_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Swimwear_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Swimwear_02") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Tracksuit_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Viking_01") { m_humanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Wizard_01") { m_humanFullBodyTransforms.Add(childGO); return; }
            }
        }

        private void VerifyIntegrityOfFullBodyPartsGO()
        {
            Debug.Log("VerifyIntegrityOfBodyPartsGO");
            if (m_handOnCartTransform == null) Debug.LogError("Hand on cart not found");
            if (m_feetOnCartTransform == null) Debug.LogError("Feet on cart not found");
            if (m_rootGO == null) Debug.LogError("Root not found");
            if (m_eyebrowsTransform == null) Debug.LogError("Eyebrows not found");
            if (m_humanFullBodyTransforms.Count == 0) Debug.LogError("Human full bodies not found");
        }

        private void GetFullCostumeBodyPartsGO()
        {
            Debug.Log("GetFullCostumeBodyPartsGO");

            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Cardboard_Robot_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Elf_Warrior_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Geisha_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Ghost_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_HolidayElf_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_JungleKid_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Knight_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Magician_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Maid_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Mummy_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Onesie_Bunny_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Onesie_Cat_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Onesie_Dino_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Onesie_Tiger_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Peasant_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Pirate_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Pirate_02"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Prince_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Princess_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Samurai_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Scifi_Casual_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Scifi_Spacesuit_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Spacesuit_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Superhero_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Superhero_02"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Survivor_Armoured_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Viking_02"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Werewolf_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Wetsuit_01"));
            m_humanInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Witch_01"));

            if (m_humanInCostumeFullBodyTransforms.Count != 0) return;

            //Transform[] childrenGOs = GetComponentsInChildren<Transform>();
            int childCount = transform.childCount;
            Debug.Log("childCount: " + childCount);

            for (int i = 0; i < childCount; i++)
            {
                Transform childGO = transform.GetChild(i);

                if (childGO.name == "SM_Chr_Kid_Cardboard_Robot_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; } // Humans in costumes full bodies
                else if (childGO.name == "SM_Chr_Kid_Elf_Warrior_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Geisha_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Ghost_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_HolidayElf_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_JungleKid_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Knight_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Magician_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Maid_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Mummy_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Onesie_Bunny_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Onesie_Cat_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Onesie_Dino_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Onesie_Tiger_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Peasant_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Pirate_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Pirate_02") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Prince_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Princess_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Samurai_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Scifi_Casual_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Scifi_Spacesuit_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Spacesuit_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Superhero_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Superhero_02") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Survivor_Armoured_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Viking_02") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Werewolf_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Wetsuit_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Witch_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            }
        }

        private void VerifyIntegrityOfCostumeBodyPartsGO()
        {
            if (m_humanInCostumeFullBodyTransforms.Count == 0) Debug.LogError("Human in costume full bodies not found");
        }

        private void GetFullNonHumanBodyPartsGO()
        {
            Debug.Log("GetFullCostumeBodyPartsGO");
            //Transform[] childrenGOs = GetComponentsInChildren<Transform>();

            m_nonHumanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Alien_01"));
            m_nonHumanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Alien_02"));
            m_nonHumanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Android_01"));
            m_nonHumanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Demon_01"));
            m_nonHumanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Goblin_01"));
            m_nonHumanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Goblin_02"));
            m_nonHumanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Pig_01"));
            m_nonHumanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Robot_01"));
            m_nonHumanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Skeleton_01"));
            m_nonHumanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Troll_01"));
            m_nonHumanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Zombie_01"));
            m_nonHumanFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Zombie_Dress_01"));

            if (m_nonHumanFullBodyTransforms.Count != 0) return;

            int childCount = transform.childCount;
            Debug.Log("childCount: " + childCount);

            for (int i = 0; i < childCount; i++)
            {
                Transform childGO = transform.GetChild(i);
                if (childGO.name == "SM_Chr_Kid_Alien_01") { m_nonHumanFullBodyTransforms.Add(childGO); return; } // Non-human full bodies
                else if (childGO.name == "SM_Chr_Kid_Alien_02") { m_nonHumanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Android_01") { m_nonHumanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Demon_01") { m_nonHumanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Goblin_01") { m_nonHumanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Goblin_02") { m_nonHumanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Pig_01") { m_nonHumanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Robot_01") { m_nonHumanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Skeleton_01") { m_nonHumanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Troll_01") { m_nonHumanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Zombie_01") { m_nonHumanFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Zombie_Dress_01") { m_nonHumanFullBodyTransforms.Add(childGO); return; }
            }
        }

        private void VerifyIntegrityOfNonHumanBodyPartsGO()
        {
            if (m_nonHumanFullBodyTransforms.Count == 0) Debug.LogError("Non-human full bodies not found");
        }

        private GameObject GetHeadGO()
        {
            // TODO: Remi To avoid getting this during the game loop
            // the NPC could be initialized in a pool before the game starts
            Debug.Log("GetHeadGO");
            if (m_rootGO == null) m_rootGO = transform.Find("Root");
            GameObject headGO = m_rootGO.transform.GetChild(0).gameObject;
            if (headGO.name != "Hips") Debug.LogError("The game object has been moved or renamed.");
            headGO = headGO.transform.GetChild(0).gameObject;
            if (headGO.name != "Spine_01") Debug.LogError("The game object has been moved or renamed.");
            headGO = headGO.transform.GetChild(0).gameObject;
            if (headGO.name != "Spine_02") Debug.LogError("The game object has been moved or renamed.");
            headGO = headGO.transform.GetChild(0).gameObject;
            if (headGO.name != "Spine_03") Debug.LogError("The game object has been moved or renamed.");
            headGO = headGO.transform.GetChild(2).gameObject;
            if (headGO.name != "Neck") Debug.LogError("The game object has been moved or renamed.");
            headGO = headGO.transform.GetChild(0).gameObject;
            if (headGO.name != "Head") Debug.LogError("The game object has been moved or renamed.");

            return headGO;
        }

        private void GetFacePartsGO()
        {
            Debug.Log("GetFacePartsGO");
            //Transform[] headChildrenGO = GetHeadGO().GetComponentsInChildren<Transform>();

            m_faceMatParentsTransform = GetHeadGO().transform.GetChild(4);
            if (m_faceMatParentsTransform.name != "SM_Chr_Kid_Face_01") Debug.LogError("The game object has been moved or renamed. Name is: " + m_faceMatParentsTransform.name);

            m_frecklesTransform = GetHeadGO().transform.GetChild(5);
            if (m_frecklesTransform.name != "SM_Chr_Kid_Face_Freckles_01") Debug.LogError("The game object has been moved or renamed. Name is: " + m_frecklesTransform.name);

            m_robotFaceTransform = GetHeadGO().transform.GetChild(6);
            if (m_robotFaceTransform.name != "SM_Chr_Kid_Robot_Face_01") Debug.LogError("The game object has been moved or renamed. Name is: " + m_robotFaceTransform.name);

            if (m_faceMatParentsTransform != null) return;

            int childCount = GetHeadGO().transform.childCount;
            //foreach (Transform headChildGO in headChildrenGO)
            for (int i = 0; i < childCount; i++)
            {
                Transform headChildGO = GetHeadGO().transform.GetChild(i);
                if (m_faceMatParentsTransform == null && headChildGO.name == "SM_Chr_Kid_Face_01") { m_faceMatParentsTransform = headChildGO; Debug.Log("Face mat parent found"); return; }
                else if (m_frecklesTransform == null && headChildGO.name == "SM_Chr_Kid_Face_Freckles_01") { m_frecklesTransform = headChildGO; return; }
                else if (m_robotFaceTransform == null && headChildGO.name == "SM_Chr_Kid_Robot_Face_01") { m_robotFaceTransform = headChildGO; return; }
            }
        }

        private void VerifyIntegrityOfFacePartsGO()
        {
            Debug.Log("VerifyIntegrityOfFacePartsGO");
            if (m_faceMatParentsTransform == null) Debug.LogError("Face mat parent not found");
            if (m_frecklesTransform == null) Debug.LogError("Freckles not found");
            if (m_robotFaceTransform == null) Debug.LogError("Robot face not found");
        }

        private void GetHairGO()
        {
            m_hairTransform = GetHeadGO().transform.GetChild(7);
            if (m_hairTransform.name != "Hair") Debug.LogError("The game object has been moved or renamed. Name is: " + m_hairTransform.name);
            m_hairTransform = m_hairTransform.GetChild(0);
        }

        private void VerifyIntegrityOfHairGO()
        {
            throw new NotImplementedException();
        }

        internal void InitializeVariables()
        {
            Debug.Log("InitializeVariables");
            if (gameObject.name != "CharacterBuilder") Debug.LogWarning("CharacterBuilder is not the name of the current GameObject.");

            GetMaterials();
            VerifyMaterialIntegrity();

            GetFullBodyPartsGO();
            VerifyIntegrityOfFullBodyPartsGO();

            GetFullCostumeBodyPartsGO();
            VerifyIntegrityOfCostumeBodyPartsGO();

            GetFullNonHumanBodyPartsGO();
            VerifyIntegrityOfNonHumanBodyPartsGO();

            GetFacePartsGO();
            VerifyIntegrityOfFacePartsGO();

            GetHairGO();
            VerifyIntegrityOfHairGO();
        }

        private void GetMaterials()
        {
            Debug.Log("GetMaterials");

            //EmptyAllVariables();
            //VerifyIntegrityOfVariables(); // Create recursing loop when called from
            //                                 The initialize var button in editor

            string materialPath01 = "Assets/AllPolyPack/PolygonKids/Materials/PolygonKids_Material_01_A.mat";
            string materialPath02 = "Assets/AllPolyPack/PolygonKids/Materials/PolygonKids_Material_02_A.mat";
            string materialPath03 = "Assets/AllPolyPack/PolygonKids/Materials/PolygonKids_Material_03_A.mat";
            string materialPath04 = "Assets/AllPolyPack/PolygonKids/Materials/PolygonKids_Material_04_A.mat";
            string materialPath05 = "Assets/_Remi/Materials/Skin.mat";
            string materialPath06 = "Assets/_Remi/Materials/Shoes.mat";

            m_polygonKids_Material_01_A = (Material)AssetDatabase.LoadAssetAtPath(materialPath01, typeof(Material));
            m_polygonKids_Material_02_A = (Material)AssetDatabase.LoadAssetAtPath(materialPath02, typeof(Material));
            m_polygonKids_Material_03_A = (Material)AssetDatabase.LoadAssetAtPath(materialPath03, typeof(Material));
            m_polygonKids_Material_04_A = (Material)AssetDatabase.LoadAssetAtPath(materialPath04, typeof(Material));
            m_skin_Mat = (Material)AssetDatabase.LoadAssetAtPath(materialPath05, typeof(Material));
            m_shoes_Mat = (Material)AssetDatabase.LoadAssetAtPath(materialPath06, typeof(Material));


            //PolygonKids_Material_01_A = Resources.Load<Material>("Assets/PolygonKids/Materials/PolygonKids_Material_01_A");
            //PolygonKids_Material_03_A = Resources.Load<Material>("Assets/PolygonKids/Materials/PolygonKids_Material_03_A");
            //PolygonKids_Material_04_A = Resources.Load<Material>("Assets/PolygonKids/Materials/PolygonKids_Material_04_A");
            //PolygonKids_Material_02_A = Resources.Load<Material>("Assets/PolygonKids/Materials/PolygonKids_Material_02_A");
        }

        internal void VerifyIntegrityOfVariables()
        {
            //Debug.Log("VerifyIntegrityOfVariables");

            if (m_polygonKids_Material_01_A == null) // One missing variable is too many
            {
                Debug.Log("One or more materials are missing. Initializing variables.");
                EmptyAllVariables();
                InitializeVariables();
            }
        }

        private void VerifyMaterialIntegrity()
        {
            Debug.Log("VerifyMaterialIntegrity");
            if (m_polygonKids_Material_01_A == null) Debug.LogError("PolygonKids_Mat_01 not found");
            if (m_polygonKids_Material_02_A == null) Debug.LogError("PolygonKids_Mat_02 not found");
            if (m_polygonKids_Material_03_A == null) Debug.LogError("PolygonKids_Mat_03 not found");
            if (m_polygonKids_Material_04_A == null) Debug.LogError("PolygonKids_Mat_04 not found");
            if (m_skin_Mat == null) Debug.LogError("Skin_Mat not found");
            if (m_shoes_Mat == null) Debug.LogError("Shoes_Mat not found");
        }

        internal void EmptyAllVariables()
        {
            Debug.Log("Empty All Variables");
            m_polygonKids_Material_01_A = null;
            m_polygonKids_Material_02_A = null;
            m_polygonKids_Material_03_A = null;
            m_polygonKids_Material_04_A = null;
            m_skin_Mat = null;
            m_shoes_Mat = null;
            m_eyesFemaleTransform = null;
            m_eyesMaleTransform = null;
            m_handOnCartTransform = null;
            m_feetOnCartTransform = null;
            m_hipGOTransform = null;
            m_rootGO = null;
            m_faceMatParentsTransform = null;
            m_frecklesTransform = null;
            m_robotFaceTransform = null;
            m_eyebrowsTransform = null;
            m_hairPartTransforms.Clear();
            m_humanFullBodyTransforms.Clear();
            m_humanInCostumeFullBodyTransforms.Clear();
            m_nonHumanFullBodyTransforms.Clear();
            ResetCurrentBodyPartsIndexes();
        }

        private void ResetCurrentBodyPartsIndexes()
        {
            m_currentHumanFullBodyIndex = -1;
            m_currentHumanInCostumeFullBodyIndex = -1;
            m_currentNonHumanFullBodyIndex = -1;
            m_currentBodyPartTypeIndex = -1;
        }

        internal void DisableAllbodyParts()
        {
            Debug.Log("Disable All body parts");
            DisableFullBodyParts();
            DisableFaceParts();
            ResetCurrentBodyPartsIndexes();
        }

        private void DisableFullBodyParts()
        {
            //Debug.Log("Disable Full Body Parts");
            //VerifyIntegrityOfVariables();

            foreach (Transform humanFullBody in m_humanFullBodyTransforms)
            {
                if (humanFullBody.gameObject.activeSelf) humanFullBody.gameObject.SetActive(false); //Debug.Log("Human full body deactivated");
            }

            foreach (Transform humanInCostumeFullBody in m_humanInCostumeFullBodyTransforms)
            {
                if (humanInCostumeFullBody.gameObject.activeSelf) humanInCostumeFullBody.gameObject.SetActive(false); //Debug.Log("Human in costume full body deactivated");
            }

            foreach (Transform nonHumanFullBody in m_nonHumanFullBodyTransforms)
            {
                if (nonHumanFullBody.gameObject.activeSelf) nonHumanFullBody.gameObject.SetActive(false); //Debug.Log("Non-human full body deactivated");
            }
        }

        private void DisableFaceParts()
        {
            //InitializeVariables();
            if (m_frecklesTransform.gameObject.activeSelf) m_frecklesTransform.gameObject.SetActive(false);
            if (m_faceMatParentsTransform.gameObject.activeSelf) m_faceMatParentsTransform.gameObject.SetActive(false);
            if (m_eyebrowsTransform.gameObject.activeSelf) m_eyebrowsTransform.gameObject.SetActive(false);
            if (m_eyesFemaleTransform.gameObject) m_eyesFemaleTransform.gameObject.SetActive(false);
            if (m_eyesMaleTransform.gameObject) m_eyesMaleTransform.gameObject.SetActive(false);
            if (m_robotFaceTransform.gameObject.activeSelf) m_robotFaceTransform.gameObject.SetActive(false);
        }

        internal void RandomizeFace()
        {
            //Debug.Log("Randomize Face");

            DisableFaceParts();

            int randomFace = UnityEngine.Random.Range(0, 2);
            int randomFreckles = UnityEngine.Random.Range(0, 2);
            int randomEyeBrows = UnityEngine.Random.Range(0, 2);
            int maleOrFemale = UnityEngine.Random.Range(0, 2);
            //int hasEyes = UnityEngine.Random.Range(0, 2);

            if (randomFace == 0) m_faceMatParentsTransform.gameObject.SetActive(true);
            else m_faceMatParentsTransform.gameObject.SetActive(false);

            if (randomFreckles == 0) m_frecklesTransform.gameObject.SetActive(true);
            else m_frecklesTransform.gameObject.SetActive(false);

            if (randomEyeBrows == 0) m_eyebrowsTransform.gameObject.SetActive(true);
            else m_eyebrowsTransform.gameObject.SetActive(false);

            if (maleOrFemale == 0)
            {
                //if (hasEyes == 0) m_eyesFemale.gameObject.SetActive(true);
                //else m_eyesFemale.gameObject.SetActive(false);

                m_eyesFemaleTransform.gameObject.SetActive(true);
            }
            else
            {
                //if (hasEyes == 0) m_eyesMale.gameObject.SetActive(true);
                //else m_eyesMale.gameObject.SetActive(false);

                m_eyesMaleTransform.gameObject.SetActive(true);
            }
        }

        internal void RandomizeBody()
        {
            //Debug.Log("Randomize Full Body");

            DisableFullBodyParts();
            
            int randomNonHumanFullBody = UnityEngine.Random.Range(0, m_nonHumanFullBodyTransforms.Count);
            int randomHumanFullBody = UnityEngine.Random.Range(0, m_humanFullBodyTransforms.Count);
            int randomHumanInCostumeFullBody = UnityEngine.Random.Range(0, m_humanInCostumeFullBodyTransforms.Count);

            if (m_bodyPartType == BodyPartType.Human)
            {
                m_humanFullBodyTransforms[randomHumanFullBody].gameObject.SetActive(true);
                m_currentHumanFullBodyIndex = randomHumanFullBody;
            }
            else if (m_bodyPartType == BodyPartType.HumanInCostume)
            {
                m_humanInCostumeFullBodyTransforms[randomHumanInCostumeFullBody].gameObject.SetActive(true);
                m_currentHumanInCostumeFullBodyIndex = randomHumanInCostumeFullBody;
            }
            else if (m_bodyPartType == BodyPartType.NonHuman)
            {
                m_nonHumanFullBodyTransforms[randomNonHumanFullBody].gameObject.SetActive(true);
                m_currentNonHumanFullBodyIndex = randomNonHumanFullBody;
            }
            else if (m_bodyPartType == BodyPartType.AllBodyParts)
            {
                int randomBodyPartType = UnityEngine.Random.Range(0, 3);
                if (randomBodyPartType == 0) m_humanFullBodyTransforms[randomHumanFullBody].gameObject.SetActive(true);
                else if (randomBodyPartType == 1) m_humanInCostumeFullBodyTransforms[randomHumanInCostumeFullBody].gameObject.SetActive(true);
                else m_nonHumanFullBodyTransforms[randomNonHumanFullBody].gameObject.SetActive(true);
                m_currentBodyPartTypeIndex = randomBodyPartType;
            }

            RandomizeBodyMaterial();
        }

        internal void RandomizeBodyMaterial()
        {
            int randomMaterial = UnityEngine.Random.Range(0, 4);
            Material material = null;

            if (randomMaterial == 0)
            {
                material = new Material(m_polygonKids_Material_01_A);
            }
            else if (randomMaterial == 1)
            {
                material = new Material(m_polygonKids_Material_02_A);
            }
            else if (randomMaterial == 2)
            {
                material = new Material(m_polygonKids_Material_03_A);
            }
            else
            {
                material = new Material(m_polygonKids_Material_04_A);
            }

            m_humanFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials = new Material[] { material };
        }

        private int GetCurrentBodyIndex()
        {
            if (m_bodyPartType == BodyPartType.Human && m_currentHumanFullBodyIndex != -1) 
                return m_currentHumanFullBodyIndex;
            else if (m_bodyPartType == BodyPartType.HumanInCostume && m_currentHumanInCostumeFullBodyIndex != -1) 
                return m_currentHumanInCostumeFullBodyIndex;
            else if (m_bodyPartType == BodyPartType.NonHuman && m_currentNonHumanFullBodyIndex != -1) 
                return m_currentNonHumanFullBodyIndex;
            else if (m_bodyPartType == BodyPartType.AllBodyParts)
                return m_currentBodyPartTypeIndex;
            else 
            {
                Debug.LogError("No body part type selected or no body part index found.");
                return -1; 
            }
        }

        internal void GiveMeBob(bool hasCustomBoots = false)
        {
            //Debug.Log("Give Me Bob");

            DisableAllbodyParts();

            GameObject adventurer = m_humanFullBodyTransforms[0].gameObject;
            if (adventurer.name != "SM_Chr_Kid_Explorer_01") Debug.LogError("The game object has been moved or renamed.");

            adventurer.SetActive(true);

            SkinnedMeshRenderer skinnedMeshRenderer = adventurer.GetComponent<SkinnedMeshRenderer>();
            if (adventurer != null && skinnedMeshRenderer.sharedMaterials.Length > 0)
            {
                GetMaterials();
                VerifyMaterialIntegrity();

                // TODO: Remi: The use of new in game loop should be replaced by for instance:
                // With a pool of NPCs that init this before the game starts
                Material newSkinMat = new Material(m_skin_Mat);
                Material newClothesMat = new Material(m_polygonKids_Material_01_A);
                Material newShoesMat = new Material(m_polygonKids_Material_01_A);

                if (hasCustomBoots)
                    newShoesMat = new Material(m_shoes_Mat);

                skinnedMeshRenderer.materials = new Material[] { newSkinMat, newClothesMat, newShoesMat };


                skinnedMeshRenderer.sharedMaterials[0].color = new Color32(184, 145, 107, 255);
            }

            m_faceMatParentsTransform.gameObject.SetActive(true);
            m_eyesFemaleTransform.gameObject.SetActive(true);
            m_eyebrowsTransform.gameObject.SetActive(true);
        }
    }
}