using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SavingSystem
{
    public class LeaderboardManager : MonoBehaviour
    {
        private const string LEADERBOARD_LOCATION = "/_Alex/LeaderBoardSave.txt";
        private Leaderboard m_leaderboard;

        [Header("Top what?")]
        [SerializeField] private int m_nbOfPlayerInLeaderboard; // once determined, transform in const

        [Header("Debug")]
        [SerializeField] private string m_nameToAdd;
        [SerializeField] private int m_scoreToAdd;

        [Header("TMP_UI here")]
        [SerializeField] private TextMeshProUGUI m_namesText;
        [SerializeField] private TextMeshProUGUI m_scoreText;


        private void Awake()
        {
            m_leaderboard = LoadLeaderboard();
            UpdateUILeaderboard();
        }

        private void Update() // TESTING
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (CanPlayerBeInLeaderboard(m_scoreToAdd))
                {
                    AddPlayerToLeaderboard(m_nameToAdd, m_scoreToAdd);
                }
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveLeaderboard(m_leaderboard);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                m_leaderboard = LoadLeaderboard();
                UpdateUILeaderboard();
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                ResetLeaderboard();
            }

        }

        private void ResetLeaderboard()
        {
            SaveLeaderboard(new Leaderboard());
            m_leaderboard = LoadLeaderboard();
        }

        public bool CanPlayerBeInLeaderboard(int playerScore)
        {
            bool freeSpaceInLeaderboard = m_leaderboard.m_allPlayerStats.Count < m_nbOfPlayerInLeaderboard;
            if (freeSpaceInLeaderboard)
                return true;

            int scoreOfLastPersonOnLeaderboard = m_leaderboard.m_allPlayerStats[m_nbOfPlayerInLeaderboard - 1].score;
            return playerScore > scoreOfLastPersonOnLeaderboard;
        }

        private void UpdateUILeaderboard()
        {
            string names = "";
            string scores = "";

            foreach (PlayerStats stats in m_leaderboard.m_allPlayerStats)
            {
                names += stats.name + "\n";
                scores += stats.score + "\n";
            }

            m_namesText.text = names;
            m_scoreText.text = scores;

        }

        public void AddPlayerToLeaderboard(string _name, int _score)
        {
            bool freeSpaceInLeaderboard = m_leaderboard.m_allPlayerStats.Count < m_nbOfPlayerInLeaderboard;

            if (freeSpaceInLeaderboard)
            {
                m_leaderboard.m_allPlayerStats.Add(new PlayerStats(_name, _score));
            }
            else
            {
                int lastIndex = m_nbOfPlayerInLeaderboard - 1;
                m_leaderboard.m_allPlayerStats[lastIndex] = new PlayerStats(_name, _score);
            }
          
            // put in order by descending
            m_leaderboard.m_allPlayerStats = m_leaderboard.m_allPlayerStats.OrderByDescending(unit => unit.score).ToList();
        }

        public void SaveLeaderboard(Leaderboard leaderboard)
        {
            // convert gameobject in json text
            string jsonString = JsonUtility.ToJson(leaderboard);
            // store json text in .txt
            File.WriteAllText(Application.dataPath + LEADERBOARD_LOCATION, jsonString);
        }

        public Leaderboard LoadLeaderboard()
        {
            string jsonString;

            // if save dont exist, create an empty one
            if (!File.Exists(Application.dataPath + LEADERBOARD_LOCATION))
            {
                Leaderboard leaderboard = new Leaderboard();
                SaveLeaderboard(leaderboard);
            }

            // take json text from .txt
            jsonString = File.ReadAllText(Application.dataPath + LEADERBOARD_LOCATION);
            // convert json text to gameobject
            return JsonUtility.FromJson<Leaderboard>(jsonString);

        }
    }

    [System.Serializable]
    public struct PlayerStats
    {
        public PlayerStats(string _name, int _score)
        {
            name = _name;
            score = _score;
        }

        public string name;
        public int score;
    }

    public class Leaderboard
    {
        public List<PlayerStats> m_allPlayerStats;
    }


}


