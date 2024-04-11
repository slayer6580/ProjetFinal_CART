using CartControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class GameStateMachine : StateMachine<GameState>
    {
        public static GameStateMachine Instance { get; private set; }

        public CartStateMachine m_playerSM; 
        [SerializeField] public ScoreUI m_scoreUI; 

        [HideInInspector] public bool m_isGameOver;//field
        [HideInInspector] public int m_playerScore = 0;
        [HideInInspector] public int m_nbItems = 0;
        [HideInInspector] public int m_nbOfCartokens = 0;


        private void Awake()
        {
            if (Instance != null && Instance != this) 
            {
                Destroy(this);
                return;
            }       
            Instance = this;
        }

        protected override void Start()
        {
            base.Start();
            foreach (GameState state in m_possibleStates)
            {
                state.OnStart(this);
            }
            m_currentState.OnEnter();
        }

        protected override void Update()
        {
            m_currentState.OnUpdate();
            TryToChangeState();
        }

        protected override void FixedUpdate()
        {
            m_currentState.OnFixedUpdate();
        }

        protected override void CreatePossibleStateList()
        {
            m_possibleStates.Add(new GameplayState());
            m_possibleStates.Add(new EndGameState());
        }

        public void GetScoreFromCart(Vector3 data) 
        {         
            m_nbItems += (int)data.x;
			m_playerScore += (int)data.y;
			m_nbOfCartokens += (int)data.z;
        }
    }
}
