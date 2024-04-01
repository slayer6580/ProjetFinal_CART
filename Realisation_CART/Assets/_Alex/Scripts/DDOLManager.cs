using System.Collections.Generic;
using UnityEngine;

namespace DDOLSystem
{
    public class DDOLManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> m_dontDestroyOnLoadList = new List<GameObject>();
        // Start is called before the first frame update
        void Start()
        {
            foreach (GameObject GO in m_dontDestroyOnLoadList)
            {
                DontDestroyOnLoad(GO);
            }

        }
    }
}
