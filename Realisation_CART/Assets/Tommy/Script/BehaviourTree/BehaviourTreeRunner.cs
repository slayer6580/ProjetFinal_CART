using DiscountDelirium;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BehaviourTree
{
    //Class to put on a GameObject to run the BehaviourTree
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTreeObject m_tree;

		void Start()
        {
            m_tree.m_blackboard.m_thisClient = gameObject;
            //To allow multiple instance of the same BehaviourTree to execute at the same time
            m_tree = m_tree.Clone();

            m_tree.m_blackboard.m_name = gameObject.name;
            //Check to integrate some AiAgent here
            m_tree.Bind();

            Scene scene = gameObject.scene;
            GameObject[] gameObjects = scene.GetRootGameObjects();
            GameObject clientPathlListGO = gameObjects[1].gameObject;
            if (clientPathlListGO.name != "Level_V2") Debug.LogError("Level_V2 has been moved or renamed. Name is: " + clientPathlListGO.name);
            if (clientPathlListGO.transform.GetChild(12).name != "ListOfPath") Debug.LogError("ListOfPath has been moved or renamed. Name is: " + clientPathlListGO.transform.GetChild(12).name);
            clientPathlListGO = clientPathlListGO.transform.GetChild(12).gameObject;
            if (clientPathlListGO == null) Debug.LogError("ClientPathList not found");

            m_tree.m_blackboard.m_possiblePathScript = clientPathlListGO.GetComponent<ClientPathList>();

            if (m_tree.m_blackboard.m_possiblePathScript == null) Debug.LogError("m_possiblePathScript not found");
        }

        void Update()
        {
            m_tree.Update();
        }

	}
}
