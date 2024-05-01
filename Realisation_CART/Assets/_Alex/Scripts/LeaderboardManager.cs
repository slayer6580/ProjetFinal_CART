using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SavingSystem
{

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

        [Header("Panels")]
        [SerializeField] private GameObject m_rankPanel;
        [SerializeField] private GameObject m_namePanel;

        [Header("Name Letters Panel")]
        [SerializeField] private List<Transform> m_lettersWheels = new List<Transform>();
        [SerializeField] private Color32 m_normalBackgroundColor;
        [SerializeField] private Color32 m_selectedBackgroundColor;
        private int[] m_letterIndexes = { 65, 65, 65 }; // 65 == A
        private int m_currentLetterPanel = 0;



        private void Awake()
        {
            m_leaderboard = LoadLeaderboard();
            UpdateUILeaderboard();
            UpdateLettersInsideWheels();
        }

        private void Update() // TESTING
        {
            SaveAndLoadTest();

            if (!m_namePanel.activeSelf)
                return;

            // change selected background color
            for (int i = 0; i < m_lettersWheels.Count; i++)
            {
                if (i == m_currentLetterPanel)
                    m_lettersWheels[i].GetChild(0).gameObject.GetComponent<Image>().color = m_selectedBackgroundColor;
                else
                    m_lettersWheels[i].GetChild(0).gameObject.GetComponent<Image>().color = m_normalBackgroundColor;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                int lastIndex = m_letterIndexes[m_currentLetterPanel] - 1;
                m_letterIndexes[m_currentLetterPanel] = GetIndex(lastIndex);
                UpdateLettersInsideWheels();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                int lastIndex = m_letterIndexes[m_currentLetterPanel] + 1;
                m_letterIndexes[m_currentLetterPanel] = GetIndex(lastIndex);
                UpdateLettersInsideWheels();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                m_currentLetterPanel--;
                if (m_currentLetterPanel < 0)
                    m_currentLetterPanel = 0;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                m_currentLetterPanel++;

                if (m_currentLetterPanel > 2)
                {
                    // Name Accepted
                    m_currentLetterPanel = 2;
                }

            }

        }

        private void UpdateLettersInsideWheels()
        {
            for (int i = 0; i < m_lettersWheels.Count; i++)
            {
                TextMeshProUGUI topLetter = m_lettersWheels[i].GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI midLetter = m_lettersWheels[i].GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI bottomLetter = m_lettersWheels[i].GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();


                topLetter.text = GetLetter(m_letterIndexes[i] - 1).ToString();
                midLetter.text = GetLetter(m_letterIndexes[i]).ToString();
                bottomLetter.text = GetLetter(m_letterIndexes[i] + 1).ToString();

            }
        }

        private int GetIndex(int index)
        {
            if (index > 90)
                index -= 26;

            if (index < 65)
                index += 26;

            return index;
        }

        private char GetLetter(int asciiNumber)
        {
            if (asciiNumber > 90)
                asciiNumber -= 26;

            if (asciiNumber < 65)
                asciiNumber += 26;

            return (char)asciiNumber;
        }


        private void SaveAndLoadTest()
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




}


