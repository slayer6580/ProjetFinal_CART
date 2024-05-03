using UnityEngine;

namespace Spawner
{
    public class CharacterBodyParts : MonoBehaviour
    {
        //private GameObject[]  

        private void Start()
        {
            GameObject bodypartsGO = transform.GetChild(0).gameObject;

            if (bodypartsGO.name != "Bodyparts") Debug.LogWarning("BodyParts is not the first child.");

            GameObject[] bodypartsGOs = bodypartsGO.GetComponentsInChildren<GameObject>();
            
            foreach (GameObject bodypart in bodypartsGOs)
            {

            }
        }

        private void OnEnable() // Keep the OnEnable to have the checkbox in the inspector
        {}
    }
}