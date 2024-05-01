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


        [SerializeField] private int m_nbOfPlayerInLeaderboard;
        [SerializeField] private string m_nameToAdd;
        [SerializeField] private int m_scoreToAdd;

        [SerializeField] private TextMeshProUGUI m_namesText;
        [SerializeField] private TextMeshProUGUI m_scoreText;


        private void Awake()
        {
            m_leaderboard = LoadLeaderboard();
            UpdateUILeaderboard();
        }

        private void Update()
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

        public bool CanPlayerBeInLeaderboard(int score)
        {

            if (m_leaderboard.m_allPlayerStats.Count < m_nbOfPlayerInLeaderboard)
                return true;

            int minScore = m_leaderboard.m_allPlayerStats[m_nbOfPlayerInLeaderboard - 1].score;
            return score > minScore;
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

            if (m_leaderboard.m_allPlayerStats.Count < m_nbOfPlayerInLeaderboard)
            {
                m_leaderboard.m_allPlayerStats.Add(new PlayerStats(_name, _score));
                Debug.Log("Added Player: " + _name + ", score: " + _score);
            }
            else
            {
                PlayerStats oldPlayer = m_leaderboard.m_allPlayerStats[m_nbOfPlayerInLeaderboard - 1];
                Debug.Log("Removed leaderboard: " + oldPlayer.name + ", score: " + oldPlayer.score);

                m_leaderboard.m_allPlayerStats[m_nbOfPlayerInLeaderboard - 1] = new PlayerStats(_name, _score);
                Debug.Log("New Player: " + _name + ", score: " + _score);

            }
            //TODO - Need reorganize based on score
            m_leaderboard.m_allPlayerStats = m_leaderboard.m_allPlayerStats.OrderByDescending(unit => unit.score).ToList();
        }

        public void SaveLeaderboard(Leaderboard leaderboard)
        {
            string jsonString = JsonUtility.ToJson(leaderboard);
            File.WriteAllText(Application.dataPath + LEADERBOARD_LOCATION, jsonString);
            Debug.Log("Saved json: " + jsonString);
        }

        public Leaderboard LoadLeaderboard()
        {
            string jsonString;

            if (!File.Exists(Application.dataPath + LEADERBOARD_LOCATION))
            {
                Leaderboard leaderboard = new Leaderboard();
                SaveLeaderboard(leaderboard);
            }

            jsonString = File.ReadAllText(Application.dataPath + LEADERBOARD_LOCATION);
            Debug.Log("Loaded json: " + jsonString);
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


