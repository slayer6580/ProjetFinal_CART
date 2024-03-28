using CartControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class GameStateMachine : StateMachine<GameState>
    {
        public static GameStateMachine Instance { get; private set; }

        [SerializeField] private GameObject m_player; 
        [SerializeField] public ScoreUI m_scoreUI; 

        [HideInInspector] public bool m_isGameOver;//field
        

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
    }
}
