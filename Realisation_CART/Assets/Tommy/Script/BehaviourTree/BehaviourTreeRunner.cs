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
            //To allow multiple instance of the same BehaviourTree to execute at the same time
            m_tree = m_tree.Clone();

            m_tree.m_blackboard.m_name = gameObject.name;
            //Check to integrate some AiAgent here
            m_tree.Bind();

            Scene scene = gameObject.scene;
            GameObject[] gameObjects = scene.GetRootGameObjects();
            GameObject clientPathlListGO = null;
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.GetComponent<ClientPathList>() == null) continue;

                clientPathlListGO = gameObject;
            }
            if (clientPathlListGO == null) Debug.LogError("ClientPathList not found");
            else Debug.Log("ClientPathList found");

            m_tree.m_blackboard.m_possiblePathScript = clientPathlListGO.GetComponent<ClientPathList>();
            //m_tree.m_blackboard.m_possiblePathScript = _ClientPathList;

            if (m_tree.m_blackboard.m_possiblePathScript == null) Debug.LogError("m_possiblePathScript not found");
            else Debug.Log("m_possiblePathScript found");
        }

        void Update()
        {
            m_tree.Update();
        }

	}
}
