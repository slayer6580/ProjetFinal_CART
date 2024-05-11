using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class ClientPathList : MonoBehaviour
    {
        public List<GameObject> m_pathList = new List<GameObject>();

        public List<List<GameObject>> ListOfPath = new List<List<GameObject>>();

        public static ClientPathList _ClientPathList { get; private set; }

        private void Awake()
        {
            Debug.Log("AudioManager Awake");
            if (_ClientPathList != null)
            {
                Debug.LogWarning("AudioManager already exists.");
                Destroy(gameObject);
                return;
            }

            _ClientPathList = this;
        }

        private void Start()
        {
            Debug.Log("TESTffff" + this.gameObject.name);
            ListOfPath = new List<List<GameObject>>();
            int subListCount = -1;
            for (int i = 0; i < m_pathList.Count; i++)
            {
                if (m_pathList[i].name == "Separation")
                {
                    ListOfPath.Add(new List<GameObject>());
                    subListCount++;
                    continue;
                }

                if (subListCount >= 0)
                {
                    ListOfPath[subListCount].Add(m_pathList[i]);
                }

            }
        }
    }
}
