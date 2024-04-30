using System.IO;
using UnityEngine;

namespace SavingSystem
{
    public class SaveAndLoad : MonoBehaviour
    {
        private const string SAVE_LOCATION = "/_Alex/save.txt";

        private void Awake()
        {
            SaveTest saveTest = new SaveTest { test = 4 };

            SaveTest(saveTest);
            SaveTest newLoad = LoadTest();
            Debug.Log("Loaded value: " + newLoad.test);
        }

        public void SaveTest(SaveTest objectToSave)
        {
            string jsonString = JsonUtility.ToJson(objectToSave);

            File.WriteAllText(Application.dataPath + SAVE_LOCATION, jsonString);
           
        }

        public SaveTest LoadTest()
        {
            if (File.Exists(Application.dataPath + SAVE_LOCATION))
            {
                string jsonString = File.ReadAllText(Application.dataPath + SAVE_LOCATION);
                return JsonUtility.FromJson<SaveTest>(jsonString);
            }
            else
            {
                Debug.Log("No save");
                return null;
            }
        }


    }



    public class SavedLeaderboard
    {
        public string[] m_allNames;
        public float[] m_allScores;
    }

    public class SaveTest
    {
        public int test;
    }

}


