namespace BehaviourTree
{
    public class SubTree : LeafNode
    {
		public BehaviourTreeObject m_tree;
		private bool m_isTreeCopied = false;
		protected override void OnStart()
		{
			if(m_isTreeCopied == false)
			{
				m_isTreeCopied = true;

				//To allow multiple instance of the same BehaviourTree to execute at the same time
				m_tree = m_tree.Clone();

				//Check to integrate some AiAgent here
				m_tree.Bind();			
			}

			m_tree.m_blackboard = m_blackboard;
		}

		protected override void OnStop()
		{
			m_tree.m_rootNode.m_state = Node.State.Running;
		}

		protected override State OnUpdate()
		{
			return m_tree.Update();
		}
	}
}
