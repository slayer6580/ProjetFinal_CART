using CartControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class GameStateMachine2 : StateMachine2<GameState2>
    {
        public static GameStateMachine2 Instance { get; private set; }

        public CartStateMachine2 m_playerSM; 
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
            foreach (GameState2 state in m_possibleStates)
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
            m_possibleStates.Add(new GameplayState2());
            m_possibleStates.Add(new EndGameState2());
        }

        public void GetScoreFromCart(int[] data) 
        {
            m_playerScore += data[0];
            m_nbItems += data[1];
            m_nbOfCartokens += data[2];
        }

        public void SetGameOver()
        {
            m_isGameOver = true;
        }
    }
}
