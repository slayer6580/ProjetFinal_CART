using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        [SerializeField] private Transform m_eyesFemaleTransform;
        [SerializeField] private Transform m_eyesMaleTransform;
        [SerializeField] private List<Transform> m_hairPartTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_humanFullBodyTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_humanInCostumeFullBodyTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_nonHumanFullBodyTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_maleHairTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_femaleHairTransforms = new List<Transform>();
        [SerializeField] private List<Transform> m_accessoryTransforms = new List<Transform>();

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

            if (transform.GetChild(3).name != "SM_Chr_Eyebrows_01") Debug.LogError("SM_Chr_Eyebrows_01 has been moved or renamed. Name: " + transform.GetChild(3).name);
            else m_eyebrowsTransform = transform.GetChild(3);

            if (transform.GetChild(4).name != "SM_Chr_Eyes_Female_01") Debug.LogError("SM_Chr_Eyes_Female_01 has been moved or renamed. Name: " + transform.GetChild(4).name);
            else m_eyesFemaleTransform = transform.GetChild(4);

            if (transform.GetChild(5).name != "SM_Chr_Eyes_Male_01") Debug.LogError("SM_Chr_Eyes_Male_01 has been moved or renamed. Name: " + transform.GetChild(5).name);
            else m_eyesMaleTransform = transform.GetChild(5);

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

            //if (bodyParts.GetChild(3).name != "SM_Chr_Kid_Adventure_01") Debug.LogError("SM_Chr_Hair_01 has been moved or renamed. Name: " + bodyParts.GetChild(3).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(3));

            //if (bodyParts.GetChild(5).name != "SM_Chr_Kid_CargoShorts_01") Debug.LogError("SM_Chr_Kid_CargoShorts_01 has been moved or renamed. Name: " + bodyParts.GetChild(5).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(5));

            //if (bodyParts.GetChild(6).name != "SM_Chr_Kid_Casual_04") Debug.LogError("SM_Chr_Kid_Casual_04 has been moved or renamed. Name: " + bodyParts.GetChild(6).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(6));

            //if (bodyParts.GetChild(4).name != "SM_Chr_Kid_Ballerina_01") Debug.LogError("SM_Chr_Kid_Ballerina_01 has been moved or renamed. Name: " + bodyParts.GetChild(7).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(7));

            //if (bodyParts.GetChild(8).name != "SM_Chr_Kid_Cheerleader_01") Debug.LogError("SM_Chr_Kid_Cheerleader_01 has been moved or renamed. Name: " + bodyParts.GetChild(8).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(8));

            //if (bodyParts.GetChild(9).name != "SM_Chr_Kid_Dress_01") Debug.LogError("SM_Chr_Kid_Dress_01 has been moved or renamed. Name: " + bodyParts.GetChild(9).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(9));

            //if (bodyParts.GetChild(10).name != "SM_Chr_Kid_Eastern_01") Debug.LogError("SM_Chr_Kid_Eastern_01 has been moved or renamed. Name: " + bodyParts.GetChild(10).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(10));

            //if (bodyParts.GetChild(11).name != "SM_Chr_Kid_Eastern_Skirt_01") Debug.LogError("SM_Chr_Kid_Eastern_Skirt_01 has been moved or renamed. Name: " + bodyParts.GetChild(11).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(11));

            //if (bodyParts.GetChild(12).name != "SM_Chr_Kid_Exercise_01") Debug.LogError("SM_Chr_Kid_Exercise_01 has been moved or renamed. Name: " + bodyParts.GetChild(12).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(12));

            //if (bodyParts.GetChild(13).name != "SM_Chr_Kid_Exercise_02") Debug.LogError("SM_Chr_Kid_Exercise_02 has been moved or renamed. Name: " + bodyParts.GetChild(13).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(13));

            //if (bodyParts.GetChild(14).name != "SM_Chr_Kid_Farmer_01") Debug.LogError("SM_Chr_Kid_Farmer_01 has been moved or renamed. Name: " + bodyParts.GetChild(14).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(14));

            //if (bodyParts.GetChild(15).name != "SM_Chr_Kid_Doctor_01") Debug.LogError("SM_Chr_Kid_Doctor_01 has been moved or renamed. Name: " + bodyParts.GetChild(15).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(15));

            //if (bodyParts.GetChild(16).name != "SM_Chr_Kid_Fat_02") Debug.LogError("SM_Chr_Kid_Fat_02 has been moved or renamed. Name: " + bodyParts.GetChild(16).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(16));

            //if (bodyParts.GetChild(17).name != "SM_Chr_Kid_Hoodie_01") Debug.LogError("SM_Chr_Kid_Hoodie_01 has been moved or renamed. Name: " + bodyParts.GetChild(17).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(17));

            //if (bodyParts.GetChild(18).name != "SM_Chr_Kid_Hoodie_02") Debug.LogError("SM_Chr_Kid_Hoodie_02 has been moved or renamed. Name: " + bodyParts.GetChild(18).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(18));

            //if (bodyParts.GetChild(19).name != "SM_Chr_Kid_Hoodie_03") Debug.LogError("SM_Chr_Kid_Hoodie_03 has been moved or renamed. Name: " + bodyParts.GetChild(19).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(19));

            //if (bodyParts.GetChild(20).name != "SM_Chr_Kid_Nerd_01") Debug.LogError("SM_Chr_Kid_Nerd_01 has been moved or renamed. Name: " + bodyParts.GetChild(20).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(20));

            //if (bodyParts.GetChild(21).name != "SM_Chr_Kid_Overalls_01") Debug.LogError("SM_Chr_Kid_Overalls_01 has been moved or renamed. Name: " + bodyParts.GetChild(21).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(21));

            //if (bodyParts.GetChild(22).name != "SM_Chr_Kid_Overalls_02") Debug.LogError("SM_Chr_Kid_Overalls_02 has been moved or renamed. Name: " + bodyParts.GetChild(22).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(22));

            //if (bodyParts.GetChild(23).name != "SM_Chr_Kid_Overalls_Dress_01") Debug.LogError("SM_Chr_Kid_Overalls_Dress_01 has been moved or renamed. Name: " + bodyParts.GetChild(23).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(23));

            //if (bodyParts.GetChild(24).name != "SM_Chr_Kid_PlaidShirt_01") Debug.LogError("SM_Chr_Kid_PlaidShirt_01 has been moved or renamed. Name: " + bodyParts.GetChild(24).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(24));

            //if (bodyParts.GetChild(24).name != "SM_Chr_Kid_Fat_01") Debug.LogError("SM_Chr_Kid_Fat_01 has been moved or renamed. Name: " + bodyParts.GetChild(24).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(24));

            //if (bodyParts.GetChild(25).name != "SM_Chr_Kid_PoliceOfficer_01") Debug.LogError("SM_Chr_Kid_PoliceOfficer_01 has been moved or renamed. Name: " + bodyParts.GetChild(25).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(25));

            //if (bodyParts.GetChild(26).name != "SM_Chr_Kid_PufferVest_01") Debug.LogError("SM_Chr_Kid_PufferVest_01 has been moved or renamed. Name: " + bodyParts.GetChild(26).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(26));

            //if (bodyParts.GetChild(27).name != "SM_Chr_Kid_Punk_01") Debug.LogError("SM_Chr_Kid_Punk_01 has been moved or renamed. Name: " + bodyParts.GetChild(27).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(27));

            //if (bodyParts.GetChild(28).name != "SM_Chr_Kid_Raincoat_01") Debug.LogError("SM_Chr_Kid_Raincoat_01 has been moved or renamed. Name: " + bodyParts.GetChild(28).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(28));

            //if (bodyParts.GetChild(29).name != "SM_Chr_Kid_Raincoat_02") Debug.LogError("SM_Chr_Kid_Raincoat_02 has been moved or renamed. Name: " + bodyParts.GetChild(29).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(29));

            //if (bodyParts.GetChild(30).name != "SM_Chr_Kid_Robber_01") Debug.LogError("SM_Chr_Kid_Robber_01 has been moved or renamed. Name: " + bodyParts.GetChild(30).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(30));

            //if (bodyParts.GetChild(31).name != "SM_Chr_Kid_Schoolboy_01") Debug.LogError("SM_Chr_Kid_Schoolboy_01 has been moved or renamed. Name: " + bodyParts.GetChild(31).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(31));

            //if (bodyParts.GetChild(32).name != "SM_Chr_Kid_Schoolboy_02") Debug.LogError("SM_Chr_Kid_Schoolboy_02 has been moved or renamed. Name: " + bodyParts.GetChild(32).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(32));

            //if (bodyParts.GetChild(33).name != "SM_Chr_Kid_Schoolgirl_01") Debug.LogError("SM_Chr_Kid_Schoolgirl_01 has been moved or renamed. Name: " + bodyParts.GetChild(33).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(33));

            //if (bodyParts.GetChild(34).name != "SM_Chr_Kid_Schoolgirl_02") Debug.LogError("SM_Chr_Kid_Schoolgirl_02 has been moved or renamed. Name: " + bodyParts.GetChild(34).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(34));

            //if (bodyParts.GetChild(35).name != "SM_Chr_Kid_Scout_Shorts_01") Debug.LogError("SM_Chr_Kid_Scout_Shorts_01 has been moved or renamed. Name: " + bodyParts.GetChild(35).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(35));

            //if (bodyParts.GetChild(36).name != "SM_Chr_Kid_Scout_Skirt_01") Debug.LogError("SM_Chr_Kid_Scout_Skirt_01 has been moved or renamed. Name: " + bodyParts.GetChild(36).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(36));

            //if (bodyParts.GetChild(37).name != "SM_Chr_Kid_ShirtDress_01") Debug.LogError("SM_Chr_Kid_ShirtDress_01 has been moved or renamed. Name: " + bodyParts.GetChild(37).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(37));

            //if (bodyParts.GetChild(38).name != "SM_Chr_Kid_Skater_01") Debug.LogError("SM_Chr_Kid_Skater_01 has been moved or renamed. Name: " + bodyParts.GetChild(38).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(38));

            //if (bodyParts.GetChild(39).name != "SM_Chr_Kid_SnowJacket_01") Debug.LogError("SM_Chr_Kid_SnowJacket_01 has been moved or renamed. Name: " + bodyParts.GetChild(39).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(39));

            //if (bodyParts.GetChild(40).name != "SM_Chr_Kid_Summer_01") Debug.LogError("SM_Chr_Kid_Summer_01 has been moved or renamed. Name: " + bodyParts.GetChild(40).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(40));

            //if (bodyParts.GetChild(41).name != "SM_Chr_Kid_Sweater_01") Debug.LogError("SM_Chr_Kid_Sweater_01 has been moved or renamed. Name: " + bodyParts.GetChild(41).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(41));

            //if (bodyParts.GetChild(42).name != "SM_Chr_Kid_Sweater_02") Debug.LogError("SM_Chr_Kid_Sweater_02 has been moved or renamed. Name: " + bodyParts.GetChild(42).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(42));

            //if (bodyParts.GetChild(43).name != "SM_Chr_Kid_Sweater_Dress_01") Debug.LogError("SM_Chr_Kid_Sweater_Dress_01 has been moved or renamed. Name: " + bodyParts.GetChild(43).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(43));

            //if (bodyParts.GetChild(44).name != "SM_Chr_Kid_Trucker_01") Debug.LogError("SM_Chr_Kid_Trucker_01 has been moved or renamed. Name: " + bodyParts.GetChild(44).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(44));

            //if (bodyParts.GetChild(45).name != "SM_Chr_Kid_WinterCoat_01") Debug.LogError("SM_Chr_Kid_WinterCoat_01 has been moved or renamed. Name: " + bodyParts.GetChild(45).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(45));

            //if (bodyParts.GetChild(46).name != "SM_Chr_Kid_Explorer_01") Debug.LogError("SM_Chr_Kid_Explorer_01 has been moved or renamed. Name: " + bodyParts.GetChild(46).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(46));

            //if (bodyParts.GetChild(47).name != "SM_Chr_Kid_Cowboy_01") Debug.LogError("SM_Chr_Kid_Cowboy_01 has been moved or renamed. Name: " + bodyParts.GetChild(47).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(47));

            //if (bodyParts.GetChild(48).name != "SM_Chr_Kid_Cowboy_02") Debug.LogError("SM_Chr_Kid_Cowboy_02 has been moved or renamed. Name: " + bodyParts.GetChild(48).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(48));

            //if (bodyParts.GetChild(49).name != "SM_Chr_Kid_Footballer_01") Debug.LogError("SM_Chr_Kid_Footballer_01 has been moved or renamed. Name: " + bodyParts.GetChild(49).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(49));

            //if (bodyParts.GetChild(50).name != "SM_Chr_Kid_Karate_01") Debug.LogError("SM_Chr_Kid_Karate_01 has been moved or renamed. Name: " + bodyParts.GetChild(50).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(50));

            //if (bodyParts.GetChild(51).name != "SM_Chr_Kid_Ninja_01") Debug.LogError("SM_Chr_Kid_Ninja_01 has been moved or renamed. Name: " + bodyParts.GetChild(51).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(51));

            //if (bodyParts.GetChild(52).name != "SM_Chr_Kid_Pajamas_01") Debug.LogError("SM_Chr_Kid_Pajamas_01 has been moved or renamed. Name: " + bodyParts.GetChild(52).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(52));

            //if (bodyParts.GetChild(53).name != "SM_Chr_Kid_Pilot_01") Debug.LogError("SM_Chr_Kid_Pilot_01 has been moved or renamed. Name: " + bodyParts.GetChild(53).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(53));

            //if (bodyParts.GetChild(54).name != "SM_Chr_Kid_Survivor_01") Debug.LogError("SM_Chr_Kid_Survivor_01 has been moved or renamed. Name: " + bodyParts.GetChild(54).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(54));

            //if (bodyParts.GetChild(55).name != "SM_Chr_Kid_Survivor_Vest_01") Debug.LogError("SM_Chr_Kid_Survivor_Vest_01 has been moved or renamed. Name: " + bodyParts.GetChild(55).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(55));

            //if (bodyParts.GetChild(56).name != "SM_Chr_Kid_Swimwear_01") Debug.LogError("SM_Chr_Kid_Swimwear_01 has been moved or renamed. Name: " + bodyParts.GetChild(56).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(56));

            //if (bodyParts.GetChild(57).name != "SM_Chr_Kid_Swimwear_02") Debug.LogError("SM_Chr_Kid_Swimwear_02 has been moved or renamed. Name: " + bodyParts.GetChild(57).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(57));

            //if (bodyParts.GetChild(58).name != "SM_Chr_Kid_Tracksuit_01") Debug.LogError("SM_Chr_Kid_Tracksuit_01 has been moved or renamed. Name: " + bodyParts.GetChild(58).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(58));

            //if (bodyParts.GetChild(59).name != "SM_Chr_Kid_Viking_01") Debug.LogError("SM_Chr_Kid_Viking_01 has been moved or renamed. Name: " + bodyParts.GetChild(59).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(59));

            //if (bodyParts.GetChild(60).name != "SM_Chr_Kid_Wizard_01") Debug.LogError("SM_Chr_Kid_Wizard_01 has been moved or renamed. Name: " + bodyParts.GetChild(60).name);
            //else m_humanFullBodyTransforms.Add(bodyParts.GetChild(60));
        }

        private void GetIKTransforms()
        {
            m_feetOnCartTransform = transform.GetChild(0);
            if (m_feetOnCartTransform.name != "FeetOnCart") Debug.LogError("FeetOnCart has been moved or renamed. Name is: " + m_feetOnCartTransform.name);

            m_handOnCartTransform = transform.GetChild(1);
            if (m_handOnCartTransform.name != "HandOnCart") Debug.LogError("HandOnCart has been moved or renamed. Name is: " + m_handOnCartTransform.name);

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

            //if (m_humanInCostumeFullBodyTransforms.Count != 0) return;

            ////Transform[] childrenGOs = GetComponentsInChildren<Transform>();
            //int childCount = transform.childCount;
            //Debug.Log("childCount: " + childCount);

            //for (int i = 0; i < childCount; i++)
            //{
            //    Transform childGO = transform.GetChild(i);

            //    if (childGO.name == "SM_Chr_Kid_Cardboard_Robot_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; } // Humans in costumes full bodies
            //    else if (childGO.name == "SM_Chr_Kid_Elf_Warrior_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Geisha_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Ghost_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_HolidayElf_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_JungleKid_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Knight_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Magician_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Maid_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Mummy_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Onesie_Bunny_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Onesie_Cat_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Onesie_Dino_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Onesie_Tiger_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Peasant_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Pirate_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Pirate_02") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Prince_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Princess_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Samurai_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Scifi_Casual_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Scifi_Spacesuit_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Spacesuit_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Superhero_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Superhero_02") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Survivor_Armoured_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Viking_02") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Werewolf_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Wetsuit_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //    else if (childGO.name == "SM_Chr_Kid_Witch_01") { m_humanInCostumeFullBodyTransforms.Add(childGO); return; }
            //}
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

        private void GetHairGO()
        {
            if (m_headTransform == null) GetHeadGO();
            Debug.Log("m_headTransform is: " + m_headTransform.name);
            m_hairTransform = m_headTransform.GetChild(7);
            if (m_hairTransform.name != "Hair") Debug.LogError("The game object has been moved or renamed. Name is: " + m_hairTransform.name);

            // Female hair
            if (m_hairTransform.GetChild(0).name != "SM_Chr_Attach_Android_Hair_01") Debug.LogError("SM_Chr_Attach_Android_Hair_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(0).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(0));

            if (m_hairTransform.GetChild(3).name != "SM_Chr_Hair_Afro_01") Debug.LogError("SM_Chr_Hair_Afro_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(3).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(3));

            if (m_hairTransform.GetChild(6).name != "SM_Chr_Hair_Bone_01") Debug.LogError("SM_Chr_Hair_Bone_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(6).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(6));

            if (m_hairTransform.GetChild(9).name != "SM_Chr_Hair_Bun_01") Debug.LogError("SM_Chr_Hair_Bun_01 has been moved or renamed. Name is: " + m_hairTransform.GetChild(9).name);
            else m_femaleHairTransforms.Add(m_hairTransform.GetChild(9));

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


        }

        //private void VerifyIntegrityOfHairGO()
        //{
        //    if (m_hairTransform == null) Debug.LogError("Hair not found");
        //}

        private void GetAccessoriesGO()
        {
            if (m_headTransform == null) GetHeadGO();
            Debug.Log("m_headTransform is: " + m_headTransform.name);
            m_accessoriesTransform = m_headTransform.GetChild(8);
            if (m_accessoriesTransform.name != "Accessories") Debug.LogError("Accessories has been moved or renamed. Name is: " + m_accessoriesTransform.name);

        }

        //private void VerifyIntegrityOfAccessoriesGO()
        //{
        //    throw new NotImplementedException();
        //}

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

            GetHairGO();
            //VerifyIntegrityOfHairGO();
            
            GetAccessoriesGO();
            //VerifyIntegrityOfAccessoriesGO();
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
            m_rootTransform = null;
            m_headTransform = null;
            m_hairTransform = null;
            m_accessoriesTransform = null;
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