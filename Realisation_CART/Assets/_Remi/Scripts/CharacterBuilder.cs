using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Experimental.AI;
using System;

namespace Spawner
{
    [ExecuteInEditMode]
    public class CharacterBuilder : MonoBehaviour
    {
        [SerializeField] private Material m_polygonKids_Material_01_A;
        [SerializeField] private Material m_polygonKids_Material_02_A;
        [SerializeField] private Material m_polygonKids_Material_03_A;
        [SerializeField] private Material m_polygonKids_Material_04_A;
        [SerializeField] private Material m_shoes_Mat;
        [SerializeField] private Material m_skin_Mat;

        [SerializeField] private Transform m_handOnCartTransform = null;
        [SerializeField] private Transform m_feetOnCartTransform = null;
        [SerializeField] private Transform m_rootTransform = null;
        [SerializeField] private Transform m_headTransform = null;

        // There is a few material that can be applied to the face
        // Face_01 by default and Face_Crying, 
        [SerializeField] private Transform m_faceMatParentsTransform = null;
        [SerializeField] private Transform m_frecklesTransform = null;
        [SerializeField] private Transform m_robotFaceTransform = null;
        [SerializeField] private Transform m_hairTransform = null;
        [SerializeField] private Transform m_accessoriesTransform = null;

        [SerializeField] private Transform m_eyebrowsTransform = null;
        [SerializeField] private Transform m_eyesFemaleTransform = null;
        [SerializeField] private Transform m_eyesMaleTransform = null;

        [SerializeField] private List<Transform> m_humanMaleFullBodyTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_humanFemaleFullBodyTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_humanFemaleFullBodyHeadCoveredTransforms = new List<Transform>();

        [SerializeField] private List<Transform> m_humanMaleInCostumeFullBodyTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_humanFemaleInCostumeFullBodyTransforms = new List<Transform>();

        [SerializeField] private List<Transform> m_nonHumanMaleFullBodyTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_nonHumanFemaleFullBodyTransforms = new List<Transform>();

        [SerializeField] private List<Transform> m_maleHairTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_femaleHairTransforms = new List<Transform>();

        [SerializeField] private List<Transform> m_accessoryTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_accessoryCostumeTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_accessoryHairSlotTransforms = new List<Transform>();

        private bool m_isMale = true;

        enum BodyPartType
        {
            Human,
            HumanInCostume,
            NonHuman,
            AllBodyParts
        }

        [SerializeField] private BodyPartType m_bodyPartType = BodyPartType.Human;

        private int m_currentMaleHumanFullBodyIndex = -1;
        private int m_currentFemaleHumanFullBodyIndex = -1;

        private int m_currentMaleHumanInCostumeFullBodyIndex = -1;
        private int m_currentFemaleHumanInCostumeFullBodyIndex = -1;

        private int m_currentNonHumanMaleFullBodyIndex = -1;
        private int m_currentNonHumanFemaleFullBodyIndex = -1;

        private int m_currentBodyPartTypeIndex = -1;

        private int m_currentMaleHairIndex = -1;
        private int m_currentFemaleHairIndex = -1;


        private void Start()
        {
            m_isMale = UnityEngine.Random.Range(0, 2) == 0;
            VerifyIntegrityOfVariables();
            RandomizeFace();
            RandomizeBody();
            RandomizeHair();
        }

        private void OnEnable() // Keep the OnEnable to have the checkbox in the inspector
        { }

        private void GetFullBodyPartsGO()
        {
            Debug.Log("GetBodyPartsGO");

            if (transform.GetChild(3).name != "SM_Chr_Eyebrows_01") Debug.LogError("SM_Chr_Eyebrows_01 has been moved or renamed. Name: " + transform.GetChild(3).name);
            else m_eyebrowsTransform = transform.GetChild(3);

            if (transform.GetChild(4).name != "SM_Chr_Eyes_Female_01") Debug.LogError("SM_Chr_Eyes_Female_01 has been moved or renamed. Name: " + transform.GetChild(4).name);
            else m_eyesFemaleTransform = transform.GetChild(4);

            if (transform.GetChild(5).name != "SM_Chr_Eyes_Male_01") Debug.LogError("SM_Chr_Eyes_Male_01 has been moved or renamed. Name: " + transform.GetChild(5).name);
            else m_eyesMaleTransform = transform.GetChild(5);

            // Male full bodies
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Adventure_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_CargoShorts_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Casual_04"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Doctor_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Eastern_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Exercise_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Exercise_02"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Farmer_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Fat_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Fat_02"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Hoodie_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Hoodie_02"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Hoodie_03"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Nerd_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Overalls_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_PlaidShirt_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_PoliceOfficer_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_PufferVest_01"));

            // Female full bodies with head covered
            m_humanFemaleFullBodyHeadCoveredTransforms.Add(transform.Find("SM_Chr_Kid_Raincoat_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Robber_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Schoolboy_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Schoolboy_02"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Scout_Shorts_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Skater_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_SnowJacket_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Summer_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Sweater_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Sweater_02"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Trucker_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_WinterCoat_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Explorer_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Cowboy_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Cowboy_02"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Footballer_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Karate_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Ninja_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Pajamas_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Pilot_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Survivor_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Survivor_Vest_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Swimwear_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Tracksuit_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Viking_01"));
            m_humanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Wizard_01"));

            // Female full bodies
            m_humanFemaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Ballerina_01"));
            m_humanFemaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Cheerleader_01"));
            m_humanFemaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Dress_01"));
            m_humanFemaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Eastern_Skirt_01"));
            m_humanFemaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Overalls_02"));
            m_humanFemaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Overalls_Dress_01"));
            m_humanFemaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Punk_01"));
            m_humanFemaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Raincoat_02"));
            m_humanFemaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Schoolgirl_01"));
            m_humanFemaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Schoolgirl_02"));
            m_humanFemaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Scout_Skirt_01"));
            m_humanFemaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_ShirtDress_01"));
            m_humanFemaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Sweater_Dress_01"));
            m_humanFemaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Swimwear_02"));
        }

        private void GetIKTransforms()
        {
            m_handOnCartTransform = transform.GetChild(0);
            if (m_handOnCartTransform.name != "HandOnCart") Debug.LogError("HandOnCart has been moved or renamed. Name is: " + transform.GetChild(0).name);

            m_feetOnCartTransform = transform.GetChild(1);
            if (m_feetOnCartTransform.name != "FeetOnCart") Debug.LogError("FeetOnCart has been moved or renamed. Name is: " + transform.GetChild(1).name);

            m_rootTransform = transform.GetChild(2);
            if (m_rootTransform.name != "Root") Debug.LogError("Root has been moved or renamed. Name is: " + m_rootTransform.name);
        }

        private void VerifyIntegrityOfFullBodyPartsGO()
        {
            Debug.Log("VerifyIntegrityOfBodyPartsGO");
            if (m_handOnCartTransform == null) Debug.LogError("Hand on cart not found");
            if (m_feetOnCartTransform == null) Debug.LogError("Feet on cart not found");
            if (m_rootTransform == null) Debug.LogError("Root not found");
            if (m_eyebrowsTransform == null) Debug.LogError("Eyebrows not found");
            if (m_humanMaleFullBodyTransforms.Count == 0) Debug.LogError("Human full bodies not found");
        }

        private void GetFullCostumeBodyPartsGO()
        {
            Debug.Log("GetFullCostumeBodyPartsGO");

            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Cardboard_Robot_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Elf_Warrior_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Ghost_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_HolidayElf_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_JungleKid_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Knight_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Magician_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Mummy_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Onesie_Bunny_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Onesie_Cat_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Onesie_Dino_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Onesie_Tiger_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Peasant_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Pirate_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Pirate_02"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Prince_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Samurai_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Scifi_Casual_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Scifi_Spacesuit_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Spacesuit_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Superhero_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Survivor_Armoured_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Viking_02"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Werewolf_01"));
            m_humanMaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Wetsuit_01"));

            // Female costumes
            m_humanFemaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Geisha_01"));
            m_humanFemaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Princess_01"));
            m_humanFemaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Maid_01"));
            m_humanFemaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Superhero_02"));
            m_humanFemaleInCostumeFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Witch_01"));
        }

        private void VerifyIntegrityOfCostumeBodyPartsGO()
        {
            if (m_humanMaleInCostumeFullBodyTransforms.Count == 0) Debug.LogError("Human in costume full bodies not found");
        }

        private void GetFullNonHumanBodyPartsGO()
        {
            Debug.Log("GetFullCostumeBodyPartsGO");

            // Male non-human full bodies
            m_nonHumanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Alien_01"));
            m_nonHumanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Alien_02"));
            m_nonHumanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Android_01"));
            m_nonHumanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Demon_01"));
            m_nonHumanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Goblin_01"));
            m_nonHumanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Pig_01"));
            m_nonHumanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Robot_01"));
            m_nonHumanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Skeleton_01"));
            m_nonHumanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Troll_01"));
            m_nonHumanMaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Zombie_01"));

            // Female non-human full bodies
            m_nonHumanFemaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Zombie_Dress_01"));
            m_nonHumanFemaleFullBodyTransforms.Add(transform.Find("SM_Chr_Kid_Goblin_02"));

            if (m_nonHumanMaleFullBodyTransforms.Count != 0) return;

            int childCount = transform.childCount;
            Debug.Log("childCount: " + childCount);

            for (int i = 0; i < childCount; i++)
            {
                Transform childGO = transform.GetChild(i);
                if (childGO.name == "SM_Chr_Kid_Alien_01") { m_nonHumanMaleFullBodyTransforms.Add(childGO); return; } // Non-human full bodies
                else if (childGO.name == "SM_Chr_Kid_Alien_02") { m_nonHumanMaleFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Android_01") { m_nonHumanMaleFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Demon_01") { m_nonHumanMaleFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Goblin_01") { m_nonHumanMaleFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Goblin_02") { m_nonHumanMaleFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Pig_01") { m_nonHumanMaleFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Robot_01") { m_nonHumanMaleFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Skeleton_01") { m_nonHumanMaleFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Troll_01") { m_nonHumanMaleFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Zombie_01") { m_nonHumanMaleFullBodyTransforms.Add(childGO); return; }
                else if (childGO.name == "SM_Chr_Kid_Zombie_Dress_01") { m_nonHumanMaleFullBodyTransforms.Add(childGO); return; }
            }
        }

        private void VerifyIntegrityOfNonHumanBodyPartsGO()
        {
            if (m_nonHumanMaleFullBodyTransforms.Count == 0) Debug.LogError("Non-human full bodies not found");
        }

        private void GetHeadGO()
        {
            // TODO: Remi To avoid getting this during the game loop
            // the NPC could be initialized in a pool before the game starts
            Debug.Log("GetHeadGO");
            Debug.Log("Current transform is: " + transform.name);
            if (m_rootTransform == null) GetIKTransforms();

            if (m_rootTransform.name != "Root") Debug.LogError("Root has been moved or renamed. Name is: " + transform.GetChild(2).name);

            Transform hips = m_rootTransform.transform.GetChild(0);
            if (hips.name != "Hips") Debug.LogError("Hips has been moved or renamed. Name is: " + hips.name);
            Transform spine_01 = hips.transform.GetChild(0);
            if (spine_01.name != "Spine_01") Debug.LogError("Spine_01 has been moved or renamed. Name is: " + spine_01.name);
            Transform spine_02 = spine_01.transform.GetChild(0);
            if (spine_02.name != "Spine_02") Debug.LogError("Spine_02 has been moved or renamed. Name is: " + spine_02.name);
            Transform spine_03 = spine_02.transform.GetChild(0);
            if (spine_03.name != "Spine_03") Debug.LogError("Spine_03 has been moved or renamed. Name is: " + spine_03.name);
            Transform neck = spine_03.transform.GetChild(2);
            if (neck.name != "Neck") Debug.LogError("Neck has been moved or renamed. Name is: " + neck.name);
            Transform head = neck.transform.GetChild(0);
            if (head.name != "Head") Debug.LogError("Head has been moved or renamed. Name is: " + head.name);
            Debug.Log("Head is: " + head.name);
            m_headTransform = head;
        }

        private void GetHairGO()
        {
            m_hairTransform = m_headTransform.GetChild(7);
            if (m_hairTransform.name != "Hair") Debug.LogError("The game object has been moved or renamed. Name is: " + m_hairTransform.name);
        }

        private void GetFacePartsGO()
        {
            Debug.Log("GetFacePartsGO");
            //Transform[] headChildrenGO = GetHeadGO().GetComponentsInChildren<Transform>();

            if (m_headTransform == null) GetHeadGO();
            Debug.Log("m_headGO is: " + m_headTransform.name);

            m_faceMatParentsTransform = m_headTransform.GetChild(4);
            if (m_faceMatParentsTransform.name != "SM_Chr_Kid_Face_01") Debug.LogError("SM_Chr_Kid_Face_01 has been moved or renamed. Name is: " + m_faceMatParentsTransform.name + " Parent is: " + m_headTransform.name);

            m_frecklesTransform = m_headTransform.GetChild(5);
            if (m_frecklesTransform.name != "SM_Chr_Kid_Face_Freckles_01") Debug.LogError("SM_Chr_Kid_Face_Freckles_01 has been moved or renamed. Name is: " + m_frecklesTransform.name + " Parent is: " + m_headTransform.name);

            m_robotFaceTransform = m_headTransform.GetChild(6);
            if (m_robotFaceTransform.name != "SM_Chr_Kid_Robot_Face_01") Debug.LogError("SM_Chr_Kid_Robot_Face_01 has been moved or renamed. Name is: " + m_robotFaceTransform.name + " Parent is: " + m_headTransform.name);

            if (m_faceMatParentsTransform != null) return;

            int childCount = m_headTransform.childCount;
            //foreach (Transform headChildGO in headChildrenGO)
            for (int i = 0; i < childCount; i++)
            {
                Transform headChildGO = m_headTransform.GetChild(i);
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

        private void GetHairGOs()
        {
            if (m_headTransform == null) GetHeadGO();
            //Debug.Log("m_headTransform is: " + m_headTransform.name);

            if (m_hairTransform == null) GetHairGO();

            // Female hair
            if (m_hairTransform.GetChild(0).name != "SM_Chr_Attach_Android_Hair_01") Debug.LogError("SM_Chr_Attach_Android_Hair_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(0).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(0));

            if (m_hairTransform.GetChild(3).name != "SM_Chr_Hair_Afro_01") Debug.LogError("SM_Chr_Hair_Afro_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(3).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(3));

            if (m_hairTransform.GetChild(6).name != "SM_Chr_Hair_Bone_01") Debug.LogError("SM_Chr_Hair_Bone_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(6).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(6));

            if (m_hairTransform.GetChild(9).name != "SM_Chr_Hair_Bun_01") Debug.LogError("SM_Chr_Hair_Bun_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(9).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(9));

            if (m_hairTransform.GetChild(26).name != "SM_Chr_Hair_Headband_01") Debug.LogError("SM_Chr_Hair_Headband_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(26).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(26));

            if (m_hairTransform.GetChild(29).name != "SM_Chr_Hair_Long_03") Debug.LogError("SM_Chr_Hair_Long_03 has been moved or renamed. Name is: " + m_hairTransform.GetChild(30).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(29));

            if (m_hairTransform.GetChild(30).name != "SM_Chr_Hair_Long_04") Debug.LogError("SM_Chr_Hair_Long_04 has been moved or renamed. Name is: " + m_hairTransform.GetChild(30).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(30));

            if (m_hairTransform.GetChild(31).name != "SM_Chr_Hair_Long_05") Debug.LogError("SM_Chr_Hair_Long_05 has been moved or renamed. Name is: " + m_hairTransform.GetChild(31).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(31));

            if (m_hairTransform.GetChild(32).name != "SM_Chr_Hair_Long_06") Debug.LogError("SM_Chr_Hair_Long_06 has been moved or renamed. Name is: " + m_hairTransform.GetChild(32).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(32));

            if (m_hairTransform.GetChild(46).name != "SM_Chr_Hair_Pigtails_01") Debug.LogError("SM_Chr_Hair_Pigtails_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(46).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(46));

            if (m_hairTransform.GetChild(47).name != "SM_Chr_Hair_Pigtails_02") Debug.LogError("SM_Chr_Hair_Pigtails_02 has been moved or renamed. Name is: " + m_hairTransform.GetChild(47).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(47));

            if (m_hairTransform.GetChild(48).name != "SM_Chr_Hair_Pigtails_03") Debug.LogError("SM_Chr_Hair_Pigtails_03 has been moved or renamed. Name is: " + m_hairTransform.GetChild(48).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(48));

            if (m_hairTransform.GetChild(49).name != "SM_Chr_Hair_Ribbon_01") Debug.LogError("SM_Chr_Hair_Ribbon_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(49).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(49));

            if (m_hairTransform.GetChild(50).name != "SM_Chr_Hair_Ribbon_02") Debug.LogError("SM_Chr_Hair_Ribbon_02 has been moved or renamed. Name is: " + m_hairTransform.GetChild(50).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(50));

            if (m_hairTransform.GetChild(55).name != "SM_Chr_Hair_SpikeyLong_01") Debug.LogError("SM_Chr_Hair_SpikeyLong_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(55).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(55));

            // Male hair
            if (m_hairTransform.GetChild(1).name != "SM_Chr_Attach_Android_Hair_02") Debug.LogError("SM_Chr_Attach_Android_Hair_02 has been moved or renamed. Name is: " + m_hairTransform.GetChild(1).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(1));

            if (m_hairTransform.GetChild(2).name != "SM_Chr_Attach_Android_Hair_03") Debug.LogError("SM_Chr_Attach_Android_Hair_03 has been moved or renamed. Name is: " + m_hairTransform.GetChild(2).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(2));

            if (m_hairTransform.GetChild(4).name != "SM_Chr_Hair_Afro_02") Debug.LogError("SM_Chr_Hair_Afro_02 has been moved or renamed. Name is: " + m_hairTransform.GetChild(4).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(4));

            if (m_hairTransform.GetChild(5).name != "SM_Chr_Hair_Bandana_01") Debug.LogError("SM_Chr_Hair_Bandana_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(5).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(5));

            if (m_hairTransform.GetChild(7).name != "SM_Chr_Hair_Bowl_01") Debug.LogError("SM_Chr_Hair_Bowl_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(7).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(7));

            if (m_hairTransform.GetChild(8).name != "SM_Chr_Hair_Bowl_02") Debug.LogError("SM_Chr_Hair_Bowl_02 has been moved or renamed. Name is: " + m_hairTransform.GetChild(8).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(8));

            if (m_hairTransform.GetChild(13).name != "SM_Chr_Hair_Clean_01") Debug.LogError("SM_Chr_Hair_Clean_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(13).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(13));

            if (m_hairTransform.GetChild(14).name != "SM_Chr_Hair_Clean_02") Debug.LogError("SM_Chr_Hair_Clean_02 has been moved or renamed. Name is: " + m_hairTransform.GetChild(14).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(14));

            if (m_hairTransform.GetChild(15).name != "SM_Chr_Hair_Clean_03") Debug.LogError("SM_Chr_Hair_Clean_03 has been moved or renamed. Name is: " + m_hairTransform.GetChild(15).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(15));

            if (m_hairTransform.GetChild(16).name != "SM_Chr_Hair_Clean_04") Debug.LogError("SM_Chr_Hair_Clean_04 has been moved or renamed. Name is: " + m_hairTransform.GetChild(16).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(16));

            if (m_hairTransform.GetChild(17).name != "SM_Chr_Hair_Clean_05") Debug.LogError("SM_Chr_Hair_Clean_05 has been moved or renamed. Name is: " + m_hairTransform.GetChild(17).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(17));

            if (m_hairTransform.GetChild(19).name != "SM_Chr_Hair_Fade_01") Debug.LogError("SM_Chr_Hair_Fade_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(19).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(19));

            if (m_hairTransform.GetChild(20).name != "SM_Chr_Hair_Flattop_01") Debug.LogError("SM_Chr_Hair_Flattop_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(20).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(20));

            if (m_hairTransform.GetChild(21).name != "SM_Chr_Hair_Fringe_01") Debug.LogError("SM_Chr_Hair_Fringe_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(21).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(21));

            if (m_hairTransform.GetChild(25).name != "SM_Chr_Hair_HatHair_Short_01") Debug.LogError("SM_Chr_Hair_HatHair_Short_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(25).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(25));

            if (m_hairTransform.GetChild(35).name != "SM_Chr_Hair_Messy_02") Debug.LogError("SM_Chr_Hair_Messy_02 has been moved or renamed. Name is: " + m_hairTransform.GetChild(35).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(35));

            if (m_hairTransform.GetChild(36).name != "SM_Chr_Hair_Messy_03") Debug.LogError("SM_Chr_Hair_Messy_03 has been moved or renamed. Name is: " + m_hairTransform.GetChild(36).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(36));

            if (m_hairTransform.GetChild(38).name != "SM_Chr_Hair_Messy_05") Debug.LogError("SM_Chr_Hair_Messy_05 has been moved or renamed. Name is: " + m_hairTransform.GetChild(38).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(38));

            if (m_hairTransform.GetChild(39).name != "SM_Chr_Hair_Messy_06") Debug.LogError("SM_Chr_Hair_Messy_06 has been moved or renamed. Name is: " + m_hairTransform.GetChild(39).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(39));

            if (m_hairTransform.GetChild(45).name != "SM_Chr_Hair_Mohawk_05") Debug.LogError("SM_Chr_Hair_Mohawk_05 has been moved or renamed. Name is: " + m_hairTransform.GetChild(45).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(45));

            if (m_hairTransform.GetChild(51).name != "SM_Chr_Hair_Spikey_01") Debug.LogError("SM_Chr_Hair_Spikey_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(51).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(51));

            if (m_hairTransform.GetChild(52).name != "SM_Chr_Hair_Spikey_02") Debug.LogError("SM_Chr_Hair_Spikey_02 has been moved or renamed. Name is: " + m_hairTransform.GetChild(52).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(52));

            if (m_hairTransform.GetChild(53).name != "SM_Chr_Hair_Spikey_03") Debug.LogError("SM_Chr_Hair_Spikey_03 has been moved or renamed. Name is: " + m_hairTransform.GetChild(53).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(53));

            if (m_hairTransform.GetChild(54).name != "SM_Chr_Hair_Spikey_04") Debug.LogError("SM_Chr_Hair_Spikey_04 has been moved or renamed. Name is: " + m_hairTransform.GetChild(54).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(54));

            if (m_hairTransform.GetChild(56).name != "SM_Chr_Hair_Sweptback_01") Debug.LogError("SM_Chr_Hair_Sweptback_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(56).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(56));

            if (m_hairTransform.GetChild(57).name != "SM_Chr_Hair_Sweptback_02") Debug.LogError("SM_Chr_Hair_Sweptback_02 has been moved or renamed. Name is: " + m_hairTransform.GetChild(57).name);
            else m_maleHairTransforms.Add(m_hairTransform.GetChild(57));


            // Mixed gender hair
            if (m_hairTransform.GetChild(9).name != "SM_Chr_Hair_Bun_01") Debug.LogError("SM_Chr_Hair_Bun_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(9).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(9));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(9));
            }

            if (m_hairTransform.GetChild(10).name != "SM_Chr_Hair_Bun_02") Debug.LogError("SM_Chr_Hair_Bun_02 has been moved or renamed. Name is: " + m_hairTransform.GetChild(10).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(10));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(10));
            }

            if (m_hairTransform.GetChild(11).name != "SM_Chr_Hair_Bun_03") Debug.LogError("SM_Chr_Hair_Bun_03 has been moved or renamed. Name is: " + m_hairTransform.GetChild(11).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(11));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(11));
            }

            if (m_hairTransform.GetChild(12).name != "SM_Chr_Hair_Bun_04") Debug.LogError("SM_Chr_Hair_Bun_04 has been moved or renamed. Name is: " + m_hairTransform.GetChild(12).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(12));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(12));
            }

            if (m_hairTransform.GetChild(18).name != "SM_Chr_Hair_Cornrows_01") Debug.LogError("SM_Chr_Hair_Cornrows_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(18).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(18));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(18));
            }

            if (m_hairTransform.GetChild(22).name != "SM_Chr_Hair_Fringe_02") Debug.LogError("SM_Chr_Hair_Fringe_02 has been moved or renamed. Name is: " + m_hairTransform.GetChild(22).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(22));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(22));
            }

            if (m_hairTransform.GetChild(23).name != "SM_Chr_Hair_HatHair_Long_01") Debug.LogError("SM_Chr_Hair_HatHair_Long_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(23).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(23));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(23));
            }

            if (m_hairTransform.GetChild(25).name != "SM_Chr_Hair_HatHair_Short_01") Debug.LogError("SM_Chr_Hair_HatHair_Short_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(25).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(25));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(25));
            }

            if (m_hairTransform.GetChild(27).name != "SM_Chr_Hair_Long_01") Debug.LogError("SM_Chr_Hair_Long_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(27).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(27));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(27));
            }

            if (m_hairTransform.GetChild(33).name != "SM_Chr_Hair_Long_07") Debug.LogError("SM_Chr_Hair_Long_07 has been moved or renamed. Name is: " + m_hairTransform.GetChild(33).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(33));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(33));
            }

            if (m_hairTransform.GetChild(34).name != "SM_Chr_Hair_Messy_01") Debug.LogError("SM_Chr_Hair_Messy_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(34).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(34));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(34));
            }

            if (m_hairTransform.GetChild(37).name != "SM_Chr_Hair_Messy_04") Debug.LogError("SM_Chr_Hair_Messy_04 has been moved or renamed. Name is: " + m_hairTransform.GetChild(37).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(37));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(37));
            }

            if (m_hairTransform.GetChild(40).name != "SM_Chr_Hair_Messy_07") Debug.LogError("SM_Chr_Hair_Messy_07 has been moved or renamed. Name is: " + m_hairTransform.GetChild(40).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(40));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(40));
            }

            if (m_hairTransform.GetChild(41).name != "SM_Chr_Hair_Mohawk_01") Debug.LogError("SM_Chr_Hair_Mohawk_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(41).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(41));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(41));
            }

            if (m_hairTransform.GetChild(42).name != "SM_Chr_Hair_Mohawk_02") Debug.LogError("SM_Chr_Hair_Mohawk_02 has been moved or renamed. Name is: " + m_hairTransform.GetChild(42).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(42));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(42));
            }

            if (m_hairTransform.GetChild(43).name != "SM_Chr_Hair_Mohawk_03") Debug.LogError("SM_Chr_Hair_Mohawk_03 has been moved or renamed. Name is: " + m_hairTransform.GetChild(43).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(43));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(43));
            }

            if (m_hairTransform.GetChild(44).name != "SM_Chr_Hair_Mohawk_04") Debug.LogError("SM_Chr_Hair_Mohawk_04 has been moved or renamed. Name is: " + m_hairTransform.GetChild(44).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(44));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(44));
            }

            if (m_hairTransform.GetChild(58).name != "SM_Chr_Hair_Sweptback_03") Debug.LogError("SM_Chr_Hair_Sweptback_03 has been moved or renamed. Name is: " + m_hairTransform.GetChild(58).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(58));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(58));
            }

            if (m_hairTransform.GetChild(59).name != "SM_Chr_Hair_Sweptback_04") Debug.LogError("SM_Chr_Hair_Sweptback_04 has been moved or renamed. Name is: " + m_hairTransform.GetChild(59).name);
            else
            {
                m_maleHairTransforms.Add(m_hairTransform.GetChild(59));
                m_femaleHairTransforms.Add(m_hairTransform.GetChild(59));
            }

            if (m_accessoriesTransform == null) GetAccessoriesGO();

            // Accessories costume
            if (m_accessoriesTransform.GetChild(0).name != "SM_Chr_Attach_Cardboard_DragonHead_01") Debug.LogError("SM_Chr_Attach_Cardboard_DragonHead_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(0).name);
            else m_accessoryCostumeTransforms.Add(m_accessoriesTransform.GetChild(0));

            if (m_accessoriesTransform.GetChild(1).name != "SM_Chr_Attach_Cardboard_KnightHelmet_01") Debug.LogError("SM_Chr_Attach_Cardboard_KnightHelmet_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(1).name);
            else m_accessoryCostumeTransforms.Add(m_accessoriesTransform.GetChild(1));

            if (m_accessoriesTransform.GetChild(2).name != "SM_Chr_Attach_Crown_01") Debug.LogError("SM_Chr_Attach_Crown_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(2).name);
            else m_accessoryCostumeTransforms.Add(m_accessoriesTransform.GetChild(2));

            if (m_accessoriesTransform.GetChild(4).name != "SM_Chr_Attach_Elf_Ears_01") Debug.LogError("SM_Chr_Attach_Elf_Ears_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(4).name);
            else m_accessoryCostumeTransforms.Add(m_accessoriesTransform.GetChild(4));

            if (m_accessoriesTransform.GetChild(5).name != "SM_Chr_Attach_Elf_Ears_02") Debug.LogError("SM_Chr_Attach_Elf_Ears_02 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(5).name);
            else m_accessoryCostumeTransforms.Add(m_accessoriesTransform.GetChild(5));

            if (m_accessoriesTransform.GetChild(18).name != "SM_Chr_Attach_Goblin_Mouth_01") Debug.LogError("SM_Chr_Attach_Goblin_Mouth_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(18).name);
            else m_accessoryCostumeTransforms.Add(m_accessoriesTransform.GetChild(18));

            if (m_accessoriesTransform.GetChild(77).name != "SM_Chr_Attach_Knight_Helmet_01") Debug.LogError("SM_Chr_Attach_Knight_Helmet_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(77).name);
            else m_accessoryCostumeTransforms.Add(m_accessoriesTransform.GetChild(77));

            // Accessories casual

            if (m_accessoriesTransform.GetChild(6).name != "SM_Chr_Attach_EyePatch_01") Debug.LogError("SM_Chr_Attach_EyePatch_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(6).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(6));

            if (m_accessoriesTransform.GetChild(7).name != "SM_Chr_Attach_Facemask_01") Debug.LogError("SM_Chr_Attach_Facemask_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(7).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(7));

            if (m_accessoriesTransform.GetChild(8).name != "SM_Chr_Attach_FakeBeard_01") Debug.LogError("SM_Chr_Attach_FakeBeard_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(8).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(8));

            if (m_accessoriesTransform.GetChild(9).name != "SM_Chr_Attach_FakeMoustache_01") Debug.LogError("SM_Chr_Attach_FakeMoustache_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(9).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(9));

            if (m_accessoriesTransform.GetChild(10).name != "SM_Chr_Attach_Glasses_3D_01") Debug.LogError("SM_Chr_Attach_Glasses_3D_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(10).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(10));

            if (m_accessoriesTransform.GetChild(11).name != "SM_Chr_Attach_Glasses_04") Debug.LogError("SM_Chr_Attach_Glasses_04 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(11).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(11));

            if (m_accessoriesTransform.GetChild(12).name != "SM_Chr_Attach_Glasses_Big_01") Debug.LogError("SM_Chr_Attach_Glasses_Big_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(12).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(12));

            if (m_accessoriesTransform.GetChild(13).name != "SM_Chr_Attach_Glasses_Nerd_01") Debug.LogError("SM_Chr_Attach_Glasses_Nerd_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(13).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(13));

            if (m_accessoriesTransform.GetChild(14).name != "SM_Chr_Attach_Glasses_Pilot_01") Debug.LogError("SM_Chr_Attach_Glasses_Pilot_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(14).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(14));

            if (m_accessoriesTransform.GetChild(15).name != "SM_Chr_Attach_Glasses_Round_01") Debug.LogError("SM_Chr_Attach_Glasses_Round_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(15).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(15));

            if (m_accessoriesTransform.GetChild(16).name != "SM_Chr_Attach_Glasses_ThickRim_01") Debug.LogError("SM_Chr_Attach_Glasses_ThickRim_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(16).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(16));

            if (m_accessoriesTransform.GetChild(17).name != "SM_Chr_Attach_Glasses_Thin_01") Debug.LogError("SM_Chr_Attach_Glasses_Thin_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(17).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(17));

            if (m_accessoriesTransform.GetChild(74).name != "SM_Chr_Attach_Helmet_Troll_01") Debug.LogError("SM_Chr_Attach_Helmet_Troll_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(74).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(74));

            if (m_accessoriesTransform.GetChild(75).name != "SM_Chr_Attach_Helmet_Viking_01") Debug.LogError("SM_Chr_Attach_Helmet_Viking_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(75).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(75));

            if (m_accessoriesTransform.GetChild(79).name != "SM_Chr_Attach_Mask_Fox_01") Debug.LogError("SM_Chr_Attach_Mask_Fox_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(77).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(77));

            if (m_accessoriesTransform.GetChild(78).name != "SM_Chr_Attach_Mask_Alien_01") Debug.LogError("SM_Chr_Attach_Mask_Alien_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(78).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(78));

            if (m_accessoriesTransform.GetChild(84).name != "SM_Chr_Attach_Mask_Plate_Lion_01") Debug.LogError("SM_Chr_Attach_Mask_Plate_Lion_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(84).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(84));

            if (m_accessoriesTransform.GetChild(85).name != "SM_Chr_Attach_Mask_Plate_Panda_01") Debug.LogError("SM_Chr_Attach_Mask_Plate_Panda_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(85).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(85));

            if (m_accessoriesTransform.GetChild(86).name != "SM_Chr_Attach_Mask_Pumpkin_01") Debug.LogError("SM_Chr_Attach_Mask_Pumpkin_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(86).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(86));

            if (m_accessoriesTransform.GetChild(87).name != "SM_Chr_Attach_Mask_Robber_01") Debug.LogError("SM_Chr_Attach_Mask_Robber_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(87).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(87));

            if (m_accessoriesTransform.GetChild(88).name != "SM_Chr_Attach_Mask_Samurai_01") Debug.LogError("SM_Chr_Attach_Mask_Samurai_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(88).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(88));

            if (m_accessoriesTransform.GetChild(91).name != "SM_Chr_Attach_Mustache_01") Debug.LogError("SM_Chr_Attach_Mustache_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(91).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(91));

            if (m_accessoriesTransform.GetChild(95).name != "SM_Chr_Attach_Pirate_Beard_01") Debug.LogError("SM_Chr_Attach_Pirate_Beard_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(95).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(95));

            if (m_accessoriesTransform.GetChild(96).name != "SM_Chr_Attach_Pirate_Eyepatch_01") Debug.LogError("SM_Chr_Attach_Pirate_Eyepatch_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(96).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(96));

            if (m_accessoriesTransform.GetChild(97).name != "SM_Chr_Attach_Robot_Face_01") Debug.LogError("SM_Chr_Attach_Robot_Face_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(97).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(97));

            if (m_accessoriesTransform.GetChild(98).name != "SM_Chr_Attach_Scifi_Eyepeice_01") Debug.LogError("SM_Chr_Attach_Scifi_Eyepeice_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(98).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(98));

            if (m_accessoriesTransform.GetChild(99).name != "SM_Chr_Attach_Scifi_Glasses_01_01") Debug.LogError("SM_Chr_Attach_Scifi_Glasses_01_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(99).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(99));

            if (m_accessoriesTransform.GetChild(100).name != "SM_Chr_Attach_Scifi_VR_01") Debug.LogError("SM_Chr_Attach_Scifi_VR_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(100).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(100));

            if (m_accessoriesTransform.GetChild(108).name != "SM_Chr_Attach_Swimming_Goggles_01") Debug.LogError("SM_Chr_Attach_Swimming_Goggles_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(108).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(108));

            if (m_accessoriesTransform.GetChild(109).name != "SM_Chr_Attach_SwimmingMask_01") Debug.LogError("SM_Chr_Attach_SwimmingMask_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(109).name);
            else m_accessoryTransforms.Add(m_accessoriesTransform.GetChild(109));


            // Accessories hair slot
            if (m_accessoriesTransform.GetChild(3).name != "SM_Chr_Attach_Earmuffs_01") Debug.LogError("SM_Chr_Attach_Earmuffs_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(3).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(3));

            if (m_accessoriesTransform.GetChild(19).name != "SM_Chr_Attach_Hat_Beanie_Earflaps_01") Debug.LogError("SM_Chr_Attach_Hat_Beanie_Earflaps_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(19).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(19));

            if (m_accessoriesTransform.GetChild(20).name != "SM_Chr_Attach_Hat_Beanie_Large_01") Debug.LogError("SM_Chr_Attach_Hat_Beanie_Large_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(20).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(20));

            if (m_accessoriesTransform.GetChild(21).name != "SM_Chr_Attach_Hat_Beanie_Small_01") Debug.LogError("SM_Chr_Attach_Hat_Beanie_Small_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(21).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(21));

            if (m_accessoriesTransform.GetChild(22).name != "SM_Chr_Attach_Hat_Cap_Back_01") Debug.LogError("SM_Chr_Attach_Hat_Cap_Back_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(22).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(22));

            if (m_accessoriesTransform.GetChild(23).name != "SM_Chr_Attach_Hat_Cap_Back_02") Debug.LogError("SM_Chr_Attach_Hat_Cap_Back_02 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(23).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(23));

            if (m_accessoriesTransform.GetChild(24).name != "SM_Chr_Attach_Hat_Cap_Forward_01") Debug.LogError("SM_Chr_Attach_Hat_Cap_Forward_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(24).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(24));

            if (m_accessoriesTransform.GetChild(25).name != "SM_Chr_Attach_Hat_Cap_Forward_02") Debug.LogError("SM_Chr_Attach_Hat_Cap_Forward_02 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(25).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(25));

            if (m_accessoriesTransform.GetChild(26).name != "SM_Chr_Attach_Hat_Captian_01") Debug.LogError("SM_Chr_Attach_Hat_Captian_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(26).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(26));

            if (m_accessoriesTransform.GetChild(27).name != "SM_Chr_Attach_Hat_Chef_01") Debug.LogError("SM_Chr_Attach_Hat_Chef_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(27).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(27));

            if (m_accessoriesTransform.GetChild(28).name != "SM_Chr_Attach_Hat_Cowboy_01") Debug.LogError("SM_Chr_Attach_Hat_Cowboy_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(28).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(28));

            if (m_accessoriesTransform.GetChild(29).name != "SM_Chr_Attach_Hat_Cowboy_Flat_01") Debug.LogError("SM_Chr_Attach_Hat_Cowboy_Flat_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(29).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(29));

            if (m_accessoriesTransform.GetChild(30).name != "SM_Chr_Attach_Hat_DoctorMirror_01") Debug.LogError("SM_Chr_Attach_Hat_DoctorMirror_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(30).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(30));

            if (m_accessoriesTransform.GetChild(31).name != "SM_Chr_Attach_Hat_Easter_01") Debug.LogError("SM_Chr_Attach_Hat_Easter_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(31).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(31));

            if (m_accessoriesTransform.GetChild(32).name != "SM_Chr_Attach_Hat_Elf_01") Debug.LogError("SM_Chr_Attach_Hat_Elf_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(32).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(32));

            if (m_accessoriesTransform.GetChild(33).name != "SM_Chr_Attach_Hat_Fancy_01") Debug.LogError("SM_Chr_Attach_Hat_Fancy_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(33).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(33));

            if (m_accessoriesTransform.GetChild(34).name != "SM_Chr_Attach_Hat_Farmer_01") Debug.LogError("SM_Chr_Attach_Hat_Farmer_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(34).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(34));

            if (m_accessoriesTransform.GetChild(35).name != "SM_Chr_Attach_Hat_Fedora_01") Debug.LogError("SM_Chr_Attach_Hat_Fedora_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(35).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(35));

            if (m_accessoriesTransform.GetChild(36).name != "SM_Chr_Attach_Hat_Fez_01") Debug.LogError("SM_Chr_Attach_Hat_Fez_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(36).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(36));

            if (m_accessoriesTransform.GetChild(37).name != "SM_Chr_Attach_Hat_Fisherman_01") Debug.LogError("SM_Chr_Attach_Hat_Fisherman_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(37).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(37));

            if (m_accessoriesTransform.GetChild(38).name != "SM_Chr_Attach_Hat_Flatcap_01") Debug.LogError("SM_Chr_Attach_Hat_Flatcap_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(38).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(38));

            if (m_accessoriesTransform.GetChild(39).name != "SM_Chr_Attach_Hat_Flowers_01") Debug.LogError("SM_Chr_Attach_Hat_Flowers_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(39).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(39));

            if (m_accessoriesTransform.GetChild(40).name != "SM_Chr_Attach_Hat_Jester_01") Debug.LogError("SM_Chr_Attach_Hat_Jester_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(40).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(40));

            if (m_accessoriesTransform.GetChild(41).name != "SM_Chr_Attach_Hat_Magician_01") Debug.LogError("SM_Chr_Attach_Hat_Magician_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(41).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(41));

            if (m_accessoriesTransform.GetChild(42).name != "SM_Chr_Attach_Hat_PaperCrown_01") Debug.LogError("SM_Chr_Attach_Hat_PaperCrown_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(42).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(42));

            if (m_accessoriesTransform.GetChild(43).name != "SM_Chr_Attach_Hat_Party_01") Debug.LogError("SM_Chr_Attach_Hat_Party_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(43).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(43));

            if (m_accessoriesTransform.GetChild(44).name != "SM_Chr_Attach_Hat_Pirate_01") Debug.LogError("SM_Chr_Attach_Hat_Pirate_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(44).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(44));

            if (m_accessoriesTransform.GetChild(45).name != "SM_Chr_Attach_Hat_Police_01") Debug.LogError("SM_Chr_Attach_Hat_Police_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(45).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(45));

            if (m_accessoriesTransform.GetChild(46).name != "SM_Chr_Attach_Hat_Propeller_01") Debug.LogError("SM_Chr_Attach_Hat_Propeller_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(46).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(46));

            if (m_accessoriesTransform.GetChild(47).name != "SM_Chr_Attach_Hat_Racoon_01") Debug.LogError("SM_Chr_Attach_Hat_Racoon_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(47).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(47));

            if (m_accessoriesTransform.GetChild(48).name != "SM_Chr_Attach_Hat_Rim_01") Debug.LogError("SM_Chr_Attach_Hat_Rim_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(48).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(48));

            if (m_accessoriesTransform.GetChild(49).name != "SM_Chr_Attach_Hat_Sombrero_01") Debug.LogError("SM_Chr_Attach_Hat_Sombrero_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(49).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(49));

            if (m_accessoriesTransform.GetChild(50).name != "SM_Chr_Attach_Hat_Sunshade_01") Debug.LogError("SM_Chr_Attach_Hat_Sunshade_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(50).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(50));

            if (m_accessoriesTransform.GetChild(51).name != "SM_Chr_Attach_Hat_Ushanka_01") Debug.LogError("SM_Chr_Attach_Hat_Ushanka_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(51).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(51));

            if (m_accessoriesTransform.GetChild(52).name != "SM_Chr_Attach_Hat_Witch_01") Debug.LogError("SM_Chr_Attach_Hat_Witch_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(52).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(52));

            if (m_accessoriesTransform.GetChild(53).name != "SM_Chr_Attach_Hat_Wizard_01") Debug.LogError("SM_Chr_Attach_Hat_Wizard_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(53).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(53));

            if (m_accessoriesTransform.GetChild(54).name != "SM_Chr_Attach_Headband_01") Debug.LogError("SM_Chr_Attach_Headband_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(54).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(54));

            if (m_accessoriesTransform.GetChild(55).name != "SM_Chr_Attach_Headphones_01") Debug.LogError("SM_Chr_Attach_Headphones_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(55).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(55));

            if (m_accessoriesTransform.GetChild(56).name != "SM_Chr_Attach_Helmet_Builder_01") Debug.LogError("SM_Chr_Attach_Helmet_Builder_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(56).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(56));

            if (m_accessoriesTransform.GetChild(57).name != "SM_Chr_Attach_Helmet_Builder_Goggles_01") Debug.LogError("SM_Chr_Attach_Helmet_Builder_Goggles_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(57).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(57));

            if (m_accessoriesTransform.GetChild(58).name != "SM_Chr_Attach_Helmet_Cardboard_01") Debug.LogError("SM_Chr_Attach_Helmet_Cardboard_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(58).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(58));

            if (m_accessoriesTransform.GetChild(59).name != "SM_Chr_Attach_Helmet_Cardboard_02") Debug.LogError("SM_Chr_Attach_Helmet_Cardboard_02 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(59).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(59));

            if (m_accessoriesTransform.GetChild(60).name != "SM_Chr_Attach_Helmet_Cardboard_Robot_01") Debug.LogError("SM_Chr_Attach_Helmet_Cardboard_Robot_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(60).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(60));

            if (m_accessoriesTransform.GetChild(61).name != "SM_Chr_Attach_Helmet_Fireman_01") Debug.LogError("SM_Chr_Attach_Helmet_Fireman_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(61).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(61));

            if (m_accessoriesTransform.GetChild(62).name != "SM_Chr_Attach_Helmet_Football_01") Debug.LogError("SM_Chr_Attach_Helmet_Football_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(62).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(62));

            if (m_accessoriesTransform.GetChild(63).name != "SM_Chr_Attach_Helmet_Pilot_01") Debug.LogError("SM_Chr_Attach_Helmet_Pilot_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(63).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(63));

            if (m_accessoriesTransform.GetChild(64).name != "SM_Chr_Attach_Helmet_Pilot_Goggles_01") Debug.LogError("SM_Chr_Attach_Helmet_Pilot_Goggles_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(64).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(64));

            if (m_accessoriesTransform.GetChild(65).name != "SM_Chr_Attach_Helmet_Pot_01") Debug.LogError("SM_Chr_Attach_Helmet_Pot_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(65).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(65));

            if (m_accessoriesTransform.GetChild(66).name != "SM_Chr_Attach_Helmet_Samurai_01") Debug.LogError("SM_Chr_Attach_Helmet_Samurai_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(66).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(66));

            if (m_accessoriesTransform.GetChild(67).name != "SM_Chr_Attach_Helmet_Scifi_Spacesuit_01") Debug.LogError("SM_Chr_Attach_Helmet_Scifi_Spacesuit_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(67).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(67));

            if (m_accessoriesTransform.GetChild(68).name != "SM_Chr_Attach_Helmet_Scooter_01") Debug.LogError("SM_Chr_Attach_Helmet_Scooter_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(68).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(68));

            if (m_accessoriesTransform.GetChild(69).name != "SM_Chr_Attach_Helmet_Skate_01") Debug.LogError("SM_Chr_Attach_Helmet_Skate_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(69).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(69));

            if (m_accessoriesTransform.GetChild(71).name != "SM_Chr_Attach_Helmet_Snow_Goggles_01") Debug.LogError("SM_Chr_Attach_Helmet_Snow_Goggles_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(71).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(71));

            if (m_accessoriesTransform.GetChild(72).name != "SM_Chr_Attach_Helmet_Soldier_01") Debug.LogError("SM_Chr_Attach_Helmet_Soldier_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(72).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(72));

            if (m_accessoriesTransform.GetChild(73).name != "SM_Chr_Attach_Helmet_Strainer_01") Debug.LogError("SM_Chr_Attach_Helmet_Strainer_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(73).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(73));

            if (m_accessoriesTransform.GetChild(76).name != "SM_Chr_Attach_Kid_Adventurer_Hat_01") Debug.LogError("SM_Chr_Attach_Kid_Adventurer_Hat_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(76).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(76));

            if (m_accessoriesTransform.GetChild(80).name != "SM_Chr_Attach_Mask_Horse_01") Debug.LogError("SM_Chr_Attach_Mask_Horse_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(80).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(80));

            if (m_accessoriesTransform.GetChild(81).name != "SM_Chr_Attach_Mask_Luchador_01") Debug.LogError("SM_Chr_Attach_Mask_Luchador_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(81).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(81));

            if (m_accessoriesTransform.GetChild(82).name != "SM_Chr_Attach_Mask_Paintball_01") Debug.LogError("SM_Chr_Attach_Mask_Paintball_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(82).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(82));

            if (m_accessoriesTransform.GetChild(83).name != "SM_Chr_Attach_Mask_Panda_01") Debug.LogError("SM_Chr_Attach_Mask_Panda_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(83).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(83));

            if (m_accessoriesTransform.GetChild(89).name != "SM_Chr_Attach_Mask_Tiger_01") Debug.LogError("SM_Chr_Attach_Mask_Tiger_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(89).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(89));

            if (m_accessoriesTransform.GetChild(90).name != "SM_Chr_Attach_Mask_Werewolf_01") Debug.LogError("SM_Chr_Attach_Mask_Werewolf_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(91).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(90));

            if (m_accessoriesTransform.GetChild(92).name != "SM_Chr_Attach_Pajamas_Hat_01") Debug.LogError("SM_Chr_Attach_Pajamas_Hat_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(92).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(92));

            if (m_accessoriesTransform.GetChild(93).name != "SM_Chr_Attach_Paper_Hat_01") Debug.LogError("SM_Chr_Attach_Paper_Hat_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(93).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(93));

            if (m_accessoriesTransform.GetChild(94).name != "SM_Chr_Attach_Pirate_Bandana_01") Debug.LogError("SM_Chr_Attach_Pirate_Bandana_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(94).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(94));

            if (m_accessoriesTransform.GetChild(101).name != "SM_Chr_Attach_ScoutHat_01") Debug.LogError("SM_Chr_Attach_ScoutHat_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(101).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(101));

            if (m_accessoriesTransform.GetChild(102).name != "SM_Chr_Attach_ScoutHat_02") Debug.LogError("SM_Chr_Attach_ScoutHat_02 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(102).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(102));

            if (m_accessoriesTransform.GetChild(103).name != "SM_Chr_Attach_Spacesuit_Helmet_01") Debug.LogError("SM_Chr_Attach_Spacesuit_Helmet_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(103).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(103));

            if (m_accessoriesTransform.GetChild(104).name != "SM_Chr_Attach_SummerHat_01") Debug.LogError("SM_Chr_Attach_SummerHat_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(104).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(104));

            if (m_accessoriesTransform.GetChild(105).name != "SM_Chr_Attach_Superhero_Mask_01") Debug.LogError("SM_Chr_Attach_Superhero_Mask_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(105).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(105));

            if (m_accessoriesTransform.GetChild(106).name != "SM_Chr_Attach_Superhero_Mask_02") Debug.LogError("SM_Chr_Attach_Superhero_Mask_02 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(106).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(106));

            if (m_accessoriesTransform.GetChild(107).name != "SM_Chr_Attach_Swimming_Cap_01") Debug.LogError("SM_Chr_Attach_Swimming_Cap_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(107).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(107));

            if (m_accessoriesTransform.GetChild(110).name != "SM_Chr_Attach_Tiara_01") Debug.LogError("SM_Chr_Attach_Tiara_01 has been moved or renamed. Name is: " + m_accessoriesTransform.GetChild(110).name);
            else m_accessoryHairSlotTransforms.Add(m_accessoriesTransform.GetChild(110));
        }

        private void GetAccessoriesGO()
        {
            if (m_headTransform == null) GetHeadGO();
            Debug.Log("m_headTransform is: " + m_headTransform.name);
            m_accessoriesTransform = m_headTransform.GetChild(8);
            if (m_accessoriesTransform.name != "Accessories") Debug.LogError("Accessories has been moved or renamed. Name is: " + m_accessoriesTransform.name);
        }

        internal void InitializeVariables()
        {
            Debug.Log("InitializeVariables");
            if (gameObject.name != "CharacterBuilder") Debug.LogWarning("CharacterBuilder is not the name of the current GameObject.");

            GetMaterials();
            VerifyMaterialIntegrity();

            GetFullBodyPartsGO();
            GetIKTransforms();
            VerifyIntegrityOfFullBodyPartsGO();

            GetFullCostumeBodyPartsGO();
            VerifyIntegrityOfCostumeBodyPartsGO();

            GetFullNonHumanBodyPartsGO();
            VerifyIntegrityOfNonHumanBodyPartsGO();

            GetFacePartsGO();
            VerifyIntegrityOfFacePartsGO();

            GetHairGOs();
            //VerifyIntegrityOfHairGO();

            GetAccessoriesGO();
            //VerifyIntegrityOfAccessoriesGO();
        }

        private void GetMaterials()
        {
            Debug.Log("GetMaterials");

            string materialPath01 = "Materials/PolygonKids_Material_01_A";
            string materialPath02 = "Materials/PolygonKids_Material_02_A";
            string materialPath03 = "Materials/PolygonKids_Material_03_A";
            string materialPath04 = "Materials/PolygonKids_Material_04_A";

            string materialPath05 = "Materials/Skin";
            string materialPath06 = "Materials/Shoes";

            m_polygonKids_Material_01_A = Resources.Load<Material>(materialPath01);
            m_polygonKids_Material_02_A = Resources.Load<Material>(materialPath02);
            m_polygonKids_Material_03_A = Resources.Load<Material>(materialPath03);
            m_polygonKids_Material_04_A = Resources.Load<Material>(materialPath04);
            m_skin_Mat = Resources.Load<Material>(materialPath05);
            m_shoes_Mat = Resources.Load<Material>(materialPath06);
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
            m_rootTransform = null;
            m_headTransform = null;
            m_hairTransform = null;
            m_accessoriesTransform = null;
            m_faceMatParentsTransform = null;
            m_frecklesTransform = null;
            m_robotFaceTransform = null;
            m_eyebrowsTransform = null;
            m_maleHairTransforms.Clear();
            m_femaleHairTransforms.Clear();
            m_humanMaleFullBodyTransforms.Clear();
            m_humanFemaleFullBodyTransforms.Clear();
            m_humanFemaleFullBodyHeadCoveredTransforms.Clear();
            m_humanMaleInCostumeFullBodyTransforms.Clear();
            m_humanFemaleInCostumeFullBodyTransforms.Clear();
            m_nonHumanMaleFullBodyTransforms.Clear();
            m_nonHumanFemaleFullBodyTransforms.Clear();
            m_accessoryTransforms.Clear();
            m_accessoryCostumeTransforms.Clear();
            m_accessoryHairSlotTransforms.Clear();
            ResetCurrentBodyPartsIndexes();
        }

        private void ResetCurrentBodyPartsIndexes()
        {
            m_currentMaleHumanFullBodyIndex = -1;
            m_currentMaleHumanInCostumeFullBodyIndex = -1;
            m_currentNonHumanMaleFullBodyIndex = -1;
            m_currentBodyPartTypeIndex = -1;
            m_currentMaleHairIndex = -1;
        }

        internal void DisableAllbodyParts()
        {
            Debug.Log("Disable All body parts");
            DisableFullBodyParts();
            DisableFaceParts();
            DisableHairParts();
            ResetCurrentBodyPartsIndexes();
        }

        private void DisableFullBodyParts()
        {
            //Debug.Log("Disable Full Body Parts");
            //VerifyIntegrityOfVariables();

            foreach (Transform humanFullBody in m_humanMaleFullBodyTransforms)
            {
                if (humanFullBody.gameObject.activeSelf) humanFullBody.gameObject.SetActive(false); //Debug.Log("Human full body deactivated");
            }

            foreach (Transform humanInCostumeFullBody in m_humanMaleInCostumeFullBodyTransforms)
            {
                if (humanInCostumeFullBody.gameObject.activeSelf) humanInCostumeFullBody.gameObject.SetActive(false); //Debug.Log("Human in costume full body deactivated");
            }

            foreach (Transform nonHumanFullBody in m_nonHumanMaleFullBodyTransforms)
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

        private void DisableHairParts()
        {
            if (m_hairTransform == null) GetHairGO();

            //Debug.Log("Disable Hair Parts : m_hairTransform.childCount is: " + m_hairTransform.childCount);

            for (int i = 0; i < m_hairTransform.childCount; i++)
            {
                Transform hairPart = m_hairTransform.GetChild(i);
                if (hairPart.gameObject.activeSelf) hairPart.gameObject.SetActive(false);
            }

            for (int i = 0; i < m_accessoryHairSlotTransforms.Count; i++)
            {
                Transform accessoryHairSlot = m_accessoryHairSlotTransforms[i];
                if (accessoryHairSlot.gameObject.activeSelf) accessoryHairSlot.gameObject.SetActive(false);
            }

            for (int i = 0; i < m_femaleHairTransforms.Count; i++)
            {
                Transform femaleHair = m_femaleHairTransforms[i];
                if (femaleHair.gameObject.activeSelf) femaleHair.gameObject.SetActive(false);
            }

            for (int i = 0; i < m_maleHairTransforms.Count; i++)
            {
                Transform maleHair = m_maleHairTransforms[i];
                if (maleHair.gameObject.activeSelf) maleHair.gameObject.SetActive(false);
            }
        }

        internal void RandomizeFace()
        {
            //Debug.Log("Randomize Face");

            DisableFaceParts();

            int randomFace = UnityEngine.Random.Range(0, 2);
            int randomFreckles = UnityEngine.Random.Range(0, 2);
            int randomEyeBrows = UnityEngine.Random.Range(0, 2);

            //int hasEyes = UnityEngine.Random.Range(0, 2);

            if (randomFace == 0) m_faceMatParentsTransform.gameObject.SetActive(true);
            else m_faceMatParentsTransform.gameObject.SetActive(false);

            if (randomFreckles == 0) m_frecklesTransform.gameObject.SetActive(true);
            else m_frecklesTransform.gameObject.SetActive(false);

            if (randomEyeBrows == 0) m_eyebrowsTransform.gameObject.SetActive(true);
            else m_eyebrowsTransform.gameObject.SetActive(false);

            if (m_isMale)
            {
                m_eyesMaleTransform.gameObject.SetActive(true);
            }
            else
            {
                m_eyesFemaleTransform.gameObject.SetActive(true);
            }
        }

        internal void RandomizeBody()
        {
            //Debug.Log("Randomize Full Body");

            DisableFullBodyParts();

            if (m_bodyPartType == BodyPartType.Human)
            {
                if (m_isMale)
                {
                    //Debug.Log("Human male");
                    int randomHumanFullBody = UnityEngine.Random.Range(0, m_humanMaleFullBodyTransforms.Count);
                    m_humanMaleFullBodyTransforms[randomHumanFullBody].gameObject.SetActive(true);
                    m_currentMaleHumanFullBodyIndex = randomHumanFullBody;
                    CheckIfIsBob(m_humanMaleFullBodyTransforms, randomHumanFullBody);
                }
                else
                {
                    int randomHumanFullBody = UnityEngine.Random.Range(0, m_humanFemaleFullBodyTransforms.Count);
                    //Debug.Log("Human female randomHumanFullBody is: " + randomHumanFullBody);
                    m_humanFemaleFullBodyTransforms[randomHumanFullBody].gameObject.SetActive(true);
                    m_currentFemaleHumanFullBodyIndex = randomHumanFullBody;
                }
            }
            else if (m_bodyPartType == BodyPartType.HumanInCostume)
            {
                if (m_isMale)
                {
                    Debug.Log("Human male in costume");
                    int randomHumanInCostumeFullBody = UnityEngine.Random.Range(0, m_humanMaleInCostumeFullBodyTransforms.Count);
                    m_humanMaleInCostumeFullBodyTransforms[randomHumanInCostumeFullBody].gameObject.SetActive(true);
                    m_currentMaleHumanInCostumeFullBodyIndex = randomHumanInCostumeFullBody;
                }
                else
                {
                    Debug.Log("Human female in costume");
                    int randomHumanInCostumeFullBody = UnityEngine.Random.Range(0, m_humanFemaleInCostumeFullBodyTransforms.Count);
                    m_humanFemaleInCostumeFullBodyTransforms[randomHumanInCostumeFullBody].gameObject.SetActive(true);
                    m_currentFemaleHumanInCostumeFullBodyIndex = randomHumanInCostumeFullBody;
                }
            }
            else if (m_bodyPartType == BodyPartType.NonHuman)
            {
                if (m_isMale)
                {
                    Debug.Log("Non-human male");
                    int randomNonHumanFullBody = UnityEngine.Random.Range(0, m_nonHumanMaleFullBodyTransforms.Count);
                    m_nonHumanMaleFullBodyTransforms[randomNonHumanFullBody].gameObject.SetActive(true);
                    m_currentNonHumanMaleFullBodyIndex = randomNonHumanFullBody;
                }
                else
                {
                    Debug.Log("Non-human female");
                    int randomNonHumanFullBody = UnityEngine.Random.Range(0, m_nonHumanFemaleFullBodyTransforms.Count);
                    m_nonHumanFemaleFullBodyTransforms[randomNonHumanFullBody].gameObject.SetActive(true);
                    m_currentNonHumanFemaleFullBodyIndex = randomNonHumanFullBody;
                }

            }
            else if (m_bodyPartType == BodyPartType.AllBodyParts)
            {
                int randomNonHumanFullBody = UnityEngine.Random.Range(0, m_nonHumanMaleFullBodyTransforms.Count);
                int randomHumanFullBody = UnityEngine.Random.Range(0, m_humanMaleFullBodyTransforms.Count);
                int randomHumanInCostumeFullBody = UnityEngine.Random.Range(0, m_humanMaleInCostumeFullBodyTransforms.Count);

                Debug.Log("All body parts female");
                int randomBodyPartType = RandomizeAllBodyParts(randomNonHumanFullBody, randomHumanFullBody, randomHumanInCostumeFullBody);
                m_currentBodyPartTypeIndex = randomBodyPartType;
            }

            RandomizeBodyMaterial();
        }

        internal void RandomizeHair()
        {
            //Debug.Log("Randomize Hair");

            DisableHairParts();

            bool randomHasHair = UnityEngine.Random.Range(0, 2) == 0 ? true : false;

            if (randomHasHair)
            {
                //Debug.Log("Random has hair");
                if (m_isMale)
                {
                    int randomHair = UnityEngine.Random.Range(0, m_maleHairTransforms.Count);
                    m_maleHairTransforms[randomHair].gameObject.SetActive(true);
                    m_currentMaleHairIndex = randomHair;
                }
                else
                {
                    int randomHair = UnityEngine.Random.Range(0, m_femaleHairTransforms.Count);
                    m_femaleHairTransforms[randomHair].gameObject.SetActive(true);
                    m_currentMaleHairIndex = randomHair;
                }

                RandomizeHairMaterial();
                return;
            }

            bool randomHasAccessory = UnityEngine.Random.Range(0, 2) == 0 ? true : false;

            if (randomHasAccessory)
            {
                //Debug.Log("Random has accessory");

                int randomAccessory = UnityEngine.Random.Range(0, m_accessoryHairSlotTransforms.Count);
                if (m_isMale) m_currentMaleHairIndex = randomAccessory;
                else m_currentFemaleHairIndex = randomAccessory;

                RandomizeAccessoryMaterial();
            }

        }

        private int RandomizeAllBodyParts(int randomNonHumanFullBody, int randomHumanFullBody, int randomHumanInCostumeFullBody)
        {
            int randomBodyPartType = UnityEngine.Random.Range(0, 3);

            if (randomBodyPartType == 0)
            {
                m_humanMaleFullBodyTransforms[randomHumanFullBody].gameObject.SetActive(true);
                CheckIfIsBob(m_humanMaleFullBodyTransforms, randomHumanFullBody);
            }
            else if (randomBodyPartType == 1)
            {
                m_humanMaleInCostumeFullBodyTransforms[randomHumanInCostumeFullBody].gameObject.SetActive(true);
                CheckIfIsBob(m_humanMaleInCostumeFullBodyTransforms, randomHumanInCostumeFullBody);
            }
            else
            {
                m_nonHumanMaleFullBodyTransforms[randomNonHumanFullBody].gameObject.SetActive(true);
                CheckIfIsBob(m_nonHumanMaleFullBodyTransforms, randomNonHumanFullBody);
            }

            return randomBodyPartType;
        }

        private void CheckIfIsBob(List<Transform> listOfBodies, int randomHumanInCostumeFullBody)
        {
            // TODO: Remi: This is a temporary solution so that the SM_Chr_Kid_Adventure_01 has all the materials on his body parts
            if (listOfBodies[randomHumanInCostumeFullBody].name != "SM_Chr_Kid_Adventure_01")
            {
                //Debug.Log("Not Bob");
                RandomizeBodyMaterial();
                return;
            }

            GiveMeBob();
        }

        internal void RandomizeBodyMaterial()
        {
            //Debug.Log("Randomize Body Material");
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

            if (material == null) Debug.LogError("Material is null");

            //Debug.Log("Material is: " + material.name + " GetCurrentBodyIndex() is: " + GetCurrentBodyIndex());
            if (m_bodyPartType == BodyPartType.Human)
            {
                if (m_isMale)
                {
                    //Debug.Log("Human male GetCurrentBodyIndex(): " + GetCurrentBodyIndex());
                    if (GetCurrentBodyIndex() < m_humanMaleFullBodyTransforms.Count)
                        m_humanMaleFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials = new Material[] { material };
                    CheckMatIntegrity(m_humanMaleFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials);
                }
                else
                {
                    //Debug.Log("Human female GetCurrentBodyIndex(): " + GetCurrentBodyIndex());
                    m_humanFemaleFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials = new Material[] { material };
                    CheckMatIntegrity(m_humanFemaleFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials);
                }
            }
            else if (m_bodyPartType == BodyPartType.HumanInCostume)
            {
                if (m_isMale)
                {
                    m_humanMaleInCostumeFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials = new Material[] { material };
                    CheckMatIntegrity(m_humanMaleInCostumeFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials);
                }
                else
                {
                    m_humanFemaleInCostumeFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials = new Material[] { material };
                    CheckMatIntegrity(m_humanFemaleInCostumeFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials = new Material[] { material });
                }
            }
            else if (m_bodyPartType == BodyPartType.NonHuman)
            {
                if (m_isMale)
                {
                    m_nonHumanMaleFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials = new Material[] { material };
                    CheckMatIntegrity(m_nonHumanMaleFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials = new Material[] { material });
                }
                else
                {
                    m_nonHumanFemaleFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials = new Material[] { material };
                    CheckMatIntegrity(m_nonHumanFemaleFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials);
                }
            }
            else if (m_bodyPartType == BodyPartType.AllBodyParts)
            {
                if (m_isMale)
                {
                    m_humanMaleFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials = new Material[] { material };
                    CheckMatIntegrity(m_humanMaleFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials);
                    m_humanMaleInCostumeFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials = new Material[] { material };
                    CheckMatIntegrity(m_humanMaleInCostumeFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials);
                    m_nonHumanMaleFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials = new Material[] { material };
                    CheckMatIntegrity(m_nonHumanMaleFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials);
                }
                else
                {
                    m_humanFemaleFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials = new Material[] { material };
                    CheckMatIntegrity(m_humanFemaleFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials);
                    m_humanFemaleInCostumeFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials = new Material[] { material };
                    CheckMatIntegrity(m_humanFemaleInCostumeFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials);
                    m_nonHumanFemaleFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials = new Material[] { material };
                    CheckMatIntegrity(m_nonHumanFemaleFullBodyTransforms[GetCurrentBodyIndex()].GetComponent<SkinnedMeshRenderer>().materials);
                }
            }
        }

        private void CheckMatIntegrity(Material[] materials)
        {
            //Debug.Log("CheckMatIntegrity");
            if (materials == null) Debug.LogError("Materials are null");
            if (materials.Length == 0) Debug.LogError("Materials length is 0");

            foreach (Material mat in materials)
            {
                if (mat == null) Debug.LogError("Material is null");
            }

        }

        internal void RandomizeHairMaterial()
        {
            //Debug.Log("Randomize Hair Material");
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

            if (material == null) Debug.LogError("Material is null");

            //Debug.Log("Material is: " + material.name);

            if (m_isMale)
                m_maleHairTransforms[GetCurrentHairIndex()].GetComponent<MeshRenderer>().materials = new Material[] { material };
            else
                m_femaleHairTransforms[GetCurrentHairIndex()].GetComponent<MeshRenderer>().materials = new Material[] { material };
        }

        private void RandomizeAccessoryMaterial()
        {
            //Debug.Log("Randomize Accessory Material");
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

            if (material == null) Debug.LogError("Material is null");

            //Debug.Log("Material is: " + material.name);
            m_accessoryHairSlotTransforms[GetCurrentAccessoryIndex()].GetComponent<MeshRenderer>().materials = new Material[] { material };
        }

        private int GetCurrentBodyIndex()
        {
            if (m_bodyPartType == BodyPartType.Human)
            {
                if (m_isMale)
                {
                    //Debug.Log("m_currentMaleHumanFullBodyIndex is: " + m_currentMaleHumanFullBodyIndex);
                    return m_currentMaleHumanFullBodyIndex;
                }
                else
                {
                    //Debug.Log("m_currentFemaleHumanFullBodyIndex is: " + m_currentFemaleHumanFullBodyIndex);
                    return m_currentFemaleHumanFullBodyIndex;
                }
            }
            else if (m_bodyPartType == BodyPartType.HumanInCostume)
            {
                if (m_isMale)
                {
                    //Debug.Log("m_currentMaleHumanInCostumeFullBodyIndex is: " + m_currentMaleHumanInCostumeFullBodyIndex);
                    return m_currentMaleHumanInCostumeFullBodyIndex;
                }
                else
                {
                    //Debug.Log("m_currentFemaleHumanInCostumeFullBodyIndex is: " + m_currentFemaleHumanInCostumeFullBodyIndex);
                    return m_currentFemaleHumanInCostumeFullBodyIndex;
                }
            }
            else if (m_bodyPartType == BodyPartType.NonHuman)
            {
                if (m_isMale)
                {
                    //Debug.Log("m_currentNonHumanMaleFullBodyIndex is: " + m_currentNonHumanMaleFullBodyIndex);
                    return m_currentNonHumanMaleFullBodyIndex;
                }
                else
                {
                    //Debug.Log("m_currentNonHumanFemaleFullBodyIndex is: " + m_currentNonHumanFemaleFullBodyIndex);
                    return m_currentNonHumanFemaleFullBodyIndex;
                }
            }
            else if (m_bodyPartType == BodyPartType.AllBodyParts)
            {
                return m_currentBodyPartTypeIndex;
            }
            else
            {
                Debug.LogError("No body part type selected or no body part index found. Current body part type is: " + m_bodyPartType);
                return -1;
            }
        }

        private int GetCurrentHairIndex()
        {
            if (m_currentMaleHairIndex != -1 && m_currentMaleHairIndex < m_hairTransform.childCount)
                return m_currentMaleHairIndex;
            else
            {
                return -1;
            }
        }

        private int GetCurrentAccessoryIndex()
        {
            if (m_isMale)
            {
                if (m_currentMaleHairIndex != -1 && m_currentMaleHairIndex < m_accessoryHairSlotTransforms.Count)
                    return m_currentMaleHairIndex;
                else
                {
                    return -1;
                }
            }
            else
            {
                if (m_currentFemaleHairIndex != -1 && m_currentFemaleHairIndex < m_accessoryHairSlotTransforms.Count)
                    return m_currentFemaleHairIndex;
                else
                {
                    return -1;
                }
            }

        }

        internal void GiveMeBob(bool hasCustomBoots = false)
        {
            Debug.Log("Give Me Bob");

            DisableAllbodyParts();

            GameObject adventurer = m_humanMaleFullBodyTransforms[0].gameObject;
            if (adventurer.name != "SM_Chr_Kid_Adventure_01") Debug.LogError("SM_Chr_Kid_Adventure_01 has been moved or renamed. Current name is: " + adventurer.name);
            adventurer.SetActive(true);

            if (m_hairTransform == null) GetHairGO();
            GameObject hair = m_hairTransform.GetChild(54).gameObject;
            if (hair.name != "SM_Chr_Hair_Spikey_04") Debug.LogError("SM_Chr_Hair_Spikey_04 has been moved or renamed. Name is: " + hair.name);
            hair.SetActive(true);

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