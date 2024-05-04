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

        [SerializeField] private Transform m_handOnCartGO = null;
        [SerializeField] private Transform m_feetOnCartGO = null;
        [SerializeField] private Transform m_hiptGO = null;

        [SerializeField] private Transform m_rootGO = null;

        // There is a few material that can be applied to the face
        // Face_01 by default and Face_Crying, 
        [SerializeField] private Transform m_faceMatParentsGO = null;
        [SerializeField] private Transform m_frecklesGO = null;
        [SerializeField] private Transform m_robotFaceGO = null;
        [SerializeField] private Transform m_eyebrows = null;
        [SerializeField] private Transform m_eyesFemale;
        [SerializeField] private Transform m_eyesMale;
        [SerializeField] private List<Transform> m_hairParts = new List<Transform>();
        [SerializeField] private List<Transform> m_humanFullBodies = new List<Transform>();
        [SerializeField] private List<Transform> m_humanInCostumeFullBodies = new List<Transform>();
        [SerializeField] private List<Transform> m_nonHumanFullBodies = new List<Transform>();

        enum BodyPartType
        {
            Human,
            HumanInCostume,
            NonHuman,
            AllBodyParts
        }

        [SerializeField] private BodyPartType m_bodyPartType = BodyPartType.Human;

        private void Start()
        {
            if (m_handOnCartGO != null) return;
            InitializeVariables();
        }

        private void OnEnable() // Keep the OnEnable to have the checkbox in the inspector
        { }

        private void GetFullBodyPartsGO()
        {
            Debug.Log("GetBodyPartsGO");

            m_handOnCartGO = transform.Find("HandOnCart");
            m_feetOnCartGO = transform.Find("FeetOnCart");
            m_hiptGO = transform.Find("Hip");
            m_rootGO = transform.Find("Root");
            m_eyebrows = transform.Find("SM_Chr_Eyebrows_01");
            m_eyesFemale = transform.Find("SM_Chr_Eyes_Female_01");
            m_eyesMale = transform.Find("SM_Chr_Eyes_Male_01");
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Adventure_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Ballerina_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Casual_04"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Doctor_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Cheerleader_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Dress_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Eastern_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Eastern_Skirt_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Exercise_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Exercise_02"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Farmer_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Fat_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Fat_02"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Hoodie_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Hoodie_02"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Hoodie_03"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Nerd_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Overalls_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Overalls_02"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Overalls_Dress_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_PlaidShirt_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_PoliceOfficer_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_PufferVest_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Punk_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Raincoat_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Raincoat_02"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Robber_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Schoolboy_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Schoolboy_02"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Schoolgirl_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Schoolgirl_02"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Scout_Shorts_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Scout_Skirt_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_ShirtDress_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Skater_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_SnowJacket_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Summer_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Sweater_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Sweater_02"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Sweater_Dress_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Trucker_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_WinterCoat_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Explorer_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Cowboy_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Cowboy_02"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Footballer_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Karate_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Ninja_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Pajamas_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Pilot_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Survivor_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Survivor_Vest_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Swimwear_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Swimwear_02"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Tracksuit_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Viking_01"));
            m_humanFullBodies.Add(transform.Find("SM_Chr_Kid_Wizard_01"));

            if (m_handOnCartGO != null) return;

            //Transform[] childrenGOs = GetComponentsInChildren<Transform>();
            int childCount = transform.childCount;
            Debug.Log("childCount: " + childCount);
            for (int i = 0; i < childCount; i++)
            {
                Transform childGO = transform.GetChild(i);

                if (m_handOnCartGO == null && childGO.name == "HandOnCart") { m_handOnCartGO = childGO; return; }
                else if (m_feetOnCartGO == null && childGO.name == "FeetOnCart") { m_feetOnCartGO = childGO; return; }
                else if (m_hiptGO == null && childGO.name == "Hip") { m_hiptGO = childGO; return; }
                else if (m_rootGO == null && childGO.name == "Root") { m_rootGO = childGO; return; }
                else if (m_eyebrows == null && childGO.name == "SM_Chr_Eyebrows_01") { m_eyebrows = childGO; return; }// Face parts
                else if (m_eyesFemale == null && childGO.name == "SM_Chr_Eyes_Female_01") { m_eyesFemale = childGO; return; }
                else if (m_eyesMale == null && childGO.name == "SM_Chr_Eyes_Male_01") { m_eyesMale = childGO; return; }
                else if (childGO.name == "SM_Chr_Kid_Adventure_01") { m_humanFullBodies.Add(childGO); return; } // casual human full bodies
                else if (childGO.name == "SM_Chr_Kid_Ballerina_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Casual_04") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Doctor_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Cheerleader_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Dress_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Eastern_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Eastern_Skirt_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Exercise_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Exercise_02") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Farmer_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Fat_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Fat_02") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Hoodie_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Hoodie_02") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Hoodie_03") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Nerd_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Overalls_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Overalls_02") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Overalls_Dress_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_PlaidShirt_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_PoliceOfficer_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_PufferVest_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Punk_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Raincoat_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Raincoat_02") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Robber_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Schoolboy_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Schoolboy_02") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Schoolgirl_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Schoolgirl_02") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Scout_Shorts_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Scout_Skirt_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_ShirtDress_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Skater_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_SnowJacket_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Summer_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Sweater_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Sweater_02") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Sweater_Dress_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Trucker_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_WinterCoat_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Explorer_01") { m_humanFullBodies.Add(childGO); return; }// Humans in costumes that can pass as cusual humans full bodies
                else if (childGO.name == "SM_Chr_Kid_Cowboy_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Cowboy_02") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Footballer_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Karate_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Ninja_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Pajamas_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Pilot_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Survivor_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Survivor_Vest_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Swimwear_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Swimwear_02") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Tracksuit_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Viking_01") { m_humanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Wizard_01") { m_humanFullBodies.Add(childGO); return; }
            }
        }

        private void VerifyIntegrityOfFullBodyPartsGO()
        {
            Debug.Log("VerifyIntegrityOfBodyPartsGO");
            if (m_handOnCartGO == null) Debug.LogError("Hand on cart not found");
            if (m_feetOnCartGO == null) Debug.LogError("Feet on cart not found");
            if (m_rootGO == null) Debug.LogError("Root not found");
            if (m_eyebrows == null) Debug.LogError("Eyebrows not found");
            if (m_humanFullBodies.Count == 0) Debug.LogError("Human full bodies not found");
        }

        private void GetFullCostumeBodyPartsGO()
        {
            Debug.Log("GetFullCostumeBodyPartsGO");

            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Cardboard_Robot_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Elf_Warrior_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Geisha_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Ghost_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_HolidayElf_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_JungleKid_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Knight_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Magician_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Maid_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Mummy_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Onesie_Bunny_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Onesie_Cat_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Onesie_Dino_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Onesie_Tiger_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Peasant_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Pirate_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Pirate_02"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Prince_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Princess_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Samurai_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Scifi_Casual_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Scifi_Spacesuit_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Spacesuit_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Superhero_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Superhero_02"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Survivor_Armoured_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Viking_02"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Werewolf_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Wetsuit_01"));
            m_humanInCostumeFullBodies.Add(transform.Find("SM_Chr_Kid_Witch_01"));

            if (m_humanInCostumeFullBodies.Count != 0) return;

            //Transform[] childrenGOs = GetComponentsInChildren<Transform>();
            int childCount = transform.childCount;
            Debug.Log("childCount: " + childCount);

            for (int i = 0; i < childCount; i++)
            {
                Transform childGO = transform.GetChild(i);

                if (childGO.name == "SM_Chr_Kid_Cardboard_Robot_01") { m_humanInCostumeFullBodies.Add(childGO); return; } // Humans in costumes full bodies
                else if (childGO.name == "SM_Chr_Kid_Elf_Warrior_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Geisha_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Ghost_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_HolidayElf_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_JungleKid_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Knight_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Magician_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Maid_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Mummy_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Onesie_Bunny_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Onesie_Cat_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Onesie_Dino_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Onesie_Tiger_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Peasant_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Pirate_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Pirate_02") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Prince_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Princess_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Samurai_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Scifi_Casual_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Scifi_Spacesuit_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Spacesuit_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Superhero_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Superhero_02") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Survivor_Armoured_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Viking_02") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Werewolf_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Wetsuit_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Witch_01") { m_humanInCostumeFullBodies.Add(childGO); return; }
            }
        }

        private void VerifyIntegrityOfCostumeBodyPartsGO()
        {
            if (m_humanInCostumeFullBodies.Count == 0) Debug.LogError("Human in costume full bodies not found");
        }

        private void GetFullNonHumanBodyPartsGO()
        {
            Debug.Log("GetFullCostumeBodyPartsGO");
            //Transform[] childrenGOs = GetComponentsInChildren<Transform>();

            m_nonHumanFullBodies.Add(transform.Find("SM_Chr_Kid_Alien_01"));
            m_nonHumanFullBodies.Add(transform.Find("SM_Chr_Kid_Alien_02"));
            m_nonHumanFullBodies.Add(transform.Find("SM_Chr_Kid_Android_01"));
            m_nonHumanFullBodies.Add(transform.Find("SM_Chr_Kid_Demon_01"));
            m_nonHumanFullBodies.Add(transform.Find("SM_Chr_Kid_Goblin_01"));
            m_nonHumanFullBodies.Add(transform.Find("SM_Chr_Kid_Goblin_02"));
            m_nonHumanFullBodies.Add(transform.Find("SM_Chr_Kid_Pig_01"));
            m_nonHumanFullBodies.Add(transform.Find("SM_Chr_Kid_Robot_01"));
            m_nonHumanFullBodies.Add(transform.Find("SM_Chr_Kid_Skeleton_01"));
            m_nonHumanFullBodies.Add(transform.Find("SM_Chr_Kid_Troll_01"));
            m_nonHumanFullBodies.Add(transform.Find("SM_Chr_Kid_Zombie_01"));
            m_nonHumanFullBodies.Add(transform.Find("SM_Chr_Kid_Zombie_Dress_01"));

            if (m_nonHumanFullBodies.Count != 0) return;

            int childCount = transform.childCount;
            Debug.Log("childCount: " + childCount);

            for (int i = 0; i < childCount; i++)
            {
                Transform childGO = transform.GetChild(i);
                if (childGO.name == "SM_Chr_Kid_Alien_01") { m_nonHumanFullBodies.Add(childGO); return; } // Non-human full bodies
                else if (childGO.name == "SM_Chr_Kid_Alien_02") { m_nonHumanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Android_01") { m_nonHumanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Demon_01") { m_nonHumanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Goblin_01") { m_nonHumanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Goblin_02") { m_nonHumanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Pig_01") { m_nonHumanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Robot_01") { m_nonHumanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Skeleton_01") { m_nonHumanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Troll_01") { m_nonHumanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Zombie_01") { m_nonHumanFullBodies.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Zombie_Dress_01") { m_nonHumanFullBodies.Add(childGO); return; }
            }
        }

        private void VerifyIntegrityOfNonHumanBodyPartsGO()
        {
            if (m_nonHumanFullBodies.Count == 0) Debug.LogError("Non-human full bodies not found");
        }

        private GameObject GetHeadGO()
        {
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

            m_faceMatParentsGO = GetHeadGO().transform.GetChild(4);
            if (m_faceMatParentsGO.name != "SM_Chr_Kid_Face_01") Debug.LogError("The game object has been moved or renamed. Name is: " + m_faceMatParentsGO.name);

            m_frecklesGO = GetHeadGO().transform.GetChild(5);
            if (m_frecklesGO.name != "SM_Chr_Kid_Face_Freckles_01") Debug.LogError("The game object has been moved or renamed. Name is: " + m_frecklesGO.name);

            m_robotFaceGO = GetHeadGO().transform.GetChild(6);
            if (m_robotFaceGO.name != "SM_Chr_Kid_Robot_Face_01") Debug.LogError("The game object has been moved or renamed. Name is: " + m_robotFaceGO.name);

            if (m_faceMatParentsGO != null) return;

            int childCount = GetHeadGO().transform.childCount;
            //foreach (Transform headChildGO in headChildrenGO)
            for (int i = 0; i < childCount; i++)
            {
                Transform headChildGO = GetHeadGO().transform.GetChild(i);
                if (m_faceMatParentsGO == null && headChildGO.name == "SM_Chr_Kid_Face_01") { m_faceMatParentsGO = headChildGO; Debug.Log("Face mat parent found"); return; }
                else if (m_frecklesGO == null && headChildGO.name == "SM_Chr_Kid_Face_Freckles_01") { m_frecklesGO = headChildGO; return; }
                else if (m_robotFaceGO == null && headChildGO.name == "SM_Chr_Kid_Robot_Face_01") { m_robotFaceGO = headChildGO; return; }
            }
        }

        private void VerifyIntegrityOfFacePartsGO()
        {
            Debug.Log("VerifyIntegrityOfFacePartsGO");
            if (m_faceMatParentsGO == null) Debug.LogError("Face mat parent not found");
            if (m_frecklesGO == null) Debug.LogError("Freckles not found");
            if (m_robotFaceGO == null) Debug.LogError("Robot face not found");
        }

        internal void InitializeVariables()
        {
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
        }

        private void GetMaterials()
        {
            Debug.Log("GetMaterials");
            string materialPath01 = "Assets/PolygonKids/Materials/PolygonKids_Material_01_A.mat";
            string materialPath02 = "Assets/PolygonKids/Materials/PolygonKids_Material_02_A.mat";
            string materialPath03 = "Assets/PolygonKids/Materials/PolygonKids_Material_03_A.mat";
            string materialPath04 = "Assets/PolygonKids/Materials/PolygonKids_Material_04_A.mat";
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
            m_eyesFemale = null;
            m_eyesMale = null;
            m_handOnCartGO = null;
            m_feetOnCartGO = null;
            m_hiptGO = null;
            m_rootGO = null;
            m_faceMatParentsGO = null;
            m_frecklesGO = null;
            m_robotFaceGO = null;
            m_eyebrows = null;
            m_hairParts.Clear();
            m_humanFullBodies.Clear();
            m_humanInCostumeFullBodies.Clear();
            m_nonHumanFullBodies.Clear();
        }

        internal void DisableAllbodyParts()
        {
            Debug.Log("Disable All body parts");
            DisableFullBodyParts();
            DisableFaceParts();
        }

        private void DisableFullBodyParts()
        {
            InitializeVariables();
            foreach (Transform humanFullBody in m_humanFullBodies)
            {
                if (humanFullBody.gameObject.activeSelf) humanFullBody.gameObject.SetActive(false);
            }

            foreach (Transform humanInCostumeFullBody in m_humanInCostumeFullBodies)
            {
                if (humanInCostumeFullBody.gameObject.activeSelf) humanInCostumeFullBody.gameObject.SetActive(false);
            }

            foreach (Transform nonHumanFullBody in m_nonHumanFullBodies)
            {
                if (nonHumanFullBody.gameObject.activeSelf) nonHumanFullBody.gameObject.SetActive(false);
            }
        }

        private void DisableFaceParts()
        {
            InitializeVariables();
            if (m_frecklesGO.gameObject.activeSelf) m_frecklesGO.gameObject.SetActive(false);
            if (m_faceMatParentsGO.gameObject.activeSelf) m_faceMatParentsGO.gameObject.SetActive(false);
            if (m_eyebrows.gameObject.activeSelf) m_eyebrows.gameObject.SetActive(false);
            if (m_eyesFemale.gameObject) m_eyesFemale.gameObject.SetActive(false);
            if (m_eyesMale.gameObject) m_eyesMale.gameObject.SetActive(false);
        }

        internal void RandomizeFace()
        {
            Debug.Log("Randomize Face");

            DisableFaceParts();

            int randomFace = UnityEngine.Random.Range(0, 2);
            int randomFreckles = UnityEngine.Random.Range(0, 2);
            int randomEyeBrows = UnityEngine.Random.Range(0, 2);
            int maleOrFemale = UnityEngine.Random.Range(0, 2);
            int hasEyes = UnityEngine.Random.Range(0, 2);

            if (randomFace == 0) m_faceMatParentsGO.gameObject.SetActive(true);
            else m_faceMatParentsGO.gameObject.SetActive(false);

            if (randomFreckles == 0) m_frecklesGO.gameObject.SetActive(true);
            else m_frecklesGO.gameObject.SetActive(false);

            if (randomEyeBrows == 0) m_eyebrows.gameObject.SetActive(true);
            else m_eyebrows.gameObject.SetActive(false);

            if (maleOrFemale == 0)
            {
                if (hasEyes == 0) m_eyesFemale.gameObject.SetActive(true);
                else m_eyesFemale.gameObject.SetActive(false);
            }
            else
            {
                if (hasEyes == 0) m_eyesMale.gameObject.SetActive(true);
                else m_eyesMale.gameObject.SetActive(false);
            }

        }

        internal void RandomizeFullBody()
        {
            Debug.Log("Randomize Full Body");

            DisableFullBodyParts();

            int randomHumanFullBody = UnityEngine.Random.Range(0, m_humanFullBodies.Count);
            int randomHumanInCostumeFullBody = UnityEngine.Random.Range(0, m_humanInCostumeFullBodies.Count);
            int randomNonHumanFullBody = UnityEngine.Random.Range(0, m_nonHumanFullBodies.Count);

            if (m_bodyPartType == BodyPartType.Human)
                m_humanFullBodies[randomHumanFullBody].gameObject.SetActive(true);
            else if (m_bodyPartType == BodyPartType.HumanInCostume)
                m_humanInCostumeFullBodies[randomHumanInCostumeFullBody].gameObject.SetActive(true);
            else if (m_bodyPartType == BodyPartType.NonHuman)
                m_nonHumanFullBodies[randomNonHumanFullBody].gameObject.SetActive(true);
            else
            {
                int randomBodyPartType = UnityEngine.Random.Range(0, 3);
                if (randomBodyPartType == 0) m_humanFullBodies[randomHumanFullBody].gameObject.SetActive(true);
                else if (randomBodyPartType == 1) m_humanInCostumeFullBodies[randomHumanInCostumeFullBody].gameObject.SetActive(true);
                else m_nonHumanFullBodies[randomNonHumanFullBody].gameObject.SetActive(true);
            }
        }

        internal void GiveMeBob()
        {
            Debug.Log("Give Me Bob");

            DisableAllbodyParts();

            GameObject adventurer = m_humanFullBodies[0].gameObject;
            adventurer.SetActive(true);
            //adventurer.GetComponent<SkinnedMeshRenderer>().materials[0].color = new Color32(184, 145, 107, 255);

            SkinnedMeshRenderer skinnedMeshRenderer = adventurer.GetComponent<SkinnedMeshRenderer>();
            if (adventurer != null && skinnedMeshRenderer.sharedMaterials.Length > 0)
            {
                GetMaterials();
                VerifyMaterialIntegrity();
                Material newSkinMat = new Material(m_skin_Mat);
                Material newClothesMat = new Material(m_polygonKids_Material_01_A);
                // delete skinnedMeshRenderer
                //DestroyImmediate(skinnedMeshRenderer);
                //skinnedMeshRenderer = new SkinnedMeshRenderer();
                //skinnedMeshRenderer.material = newMaterial;
                //skinnedMeshRenderer.materials[0] = newSkinMat;
                //skinnedMeshRenderer.materials[1] = newClothesMat;
                //skinnedMeshRenderer.materials[2] = newClothesMat;
                skinnedMeshRenderer.materials = new Material[] { newSkinMat, newClothesMat, newClothesMat };
                //skinnedMeshRenderer.sharedMaterials[0] = newSkinMat;
                //skinnedMeshRenderer.sharedMaterials[1] = newClothesMat;
                //skinnedMeshRenderer.sharedMaterials[2] = newClothesMat;

                skinnedMeshRenderer.sharedMaterials[0].color = new Color32(184, 145, 107, 255);
            }

            m_faceMatParentsGO.gameObject.SetActive(true);
            m_eyesFemale.gameObject.SetActive(true);
            m_eyebrows.gameObject.SetActive(true);
        }
    }
}