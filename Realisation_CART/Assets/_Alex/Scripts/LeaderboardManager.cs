using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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
        [SerializeField] private int m_scoreToAdd;

        [Header("TMP_UI here")]
        [SerializeField] private TextMeshProUGUI m_namesText;
        [SerializeField] private TextMeshProUGUI m_scoreText;

        [Header("Panels")]
        [SerializeField] private GameObject m_rankPanel;
        [SerializeField] private GameObject m_namePanel;
        [SerializeField] private GameObject m_leaderboardPanel;

        [Header("Name Letters Panel")]
        [SerializeField] private List<Transform> m_lettersWheels = new List<Transform>();
        [SerializeField] private Color32 m_normalBackgroundColor;
        [SerializeField] private Color32 m_selectedBackgroundColor;

        [Header("Bouton")]
        [SerializeField] private Image m_confirmButton;
        [SerializeField] private Color32 m_normalButtonColor;
        [SerializeField] private Color32 m_selectedButtonColor;


        private int[] m_letterIndexes = { 65, 65, 65 }; // 65 == A
        private int m_currentWheelSelected = 0;



        private void Awake()
        {
            m_leaderboard = LoadLeaderboard();
            UpdateUILeaderboard();
            UpdateLettersInsideWheels();
        }

        private void Update() // TESTING
        {
            DebugUpdate();
            NamePanelInputs();
            RankPanelInputs();

        }

        private void RankPanelInputs()
        {
            if (!m_rankPanel.activeSelf)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
                QuitLeaderboardPanel();
        }

        private void NamePanelInputs()
        {
            if (!m_namePanel.activeSelf)
                return;

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                GoUp();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                GoDown();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                GoLeft();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                GoRight();
            }

            if (Input.GetKeyDown(KeyCode.Return) && m_currentWheelSelected == m_lettersWheels.Count)
                NameIsChosen_BT();
        }

        private void GoUp()
        {
            if (m_currentWheelSelected == m_lettersWheels.Count)
                return;

            int newIndex = m_letterIndexes[m_currentWheelSelected] - 1;
            m_letterIndexes[m_currentWheelSelected] = GetIndex(newIndex);
            UpdateLettersInsideWheels();
        }

        private void GoDown()
        {
            if (m_currentWheelSelected == m_lettersWheels.Count)
                return;

            int newIndex = m_letterIndexes[m_currentWheelSelected] + 1;
            m_letterIndexes[m_currentWheelSelected] = GetIndex(newIndex);
            UpdateLettersInsideWheels();
        }

        private void GoLeft()
        {
            m_currentWheelSelected--;
            if (m_currentWheelSelected < 0)
                m_currentWheelSelected = 0;

            m_confirmButton.GetComponent<Image>().color = m_normalButtonColor;
            ChangeBackGroundColorSelected();
        }

        private void GoRight()
        {
            m_currentWheelSelected++;

            if (m_currentWheelSelected > m_lettersWheels.Count - 1)
            {
                // On confirm button
                m_confirmButton.GetComponent<Image>().color = m_selectedButtonColor;
                m_currentWheelSelected = m_lettersWheels.Count;
            }
            else
            {
                m_confirmButton.GetComponent<Image>().color = m_normalButtonColor;
            }

            ChangeBackGroundColorSelected();
        }

        private void ChangeBackGroundColorSelected()
        {
            for (int i = 0; i < m_lettersWheels.Count; i++)
            {
                if (i == m_currentWheelSelected)
                    m_lettersWheels[i].GetChild(0).gameObject.GetComponent<Image>().color = m_selectedBackgroundColor;
                else
                    m_lettersWheels[i].GetChild(0).gameObject.GetComponent<Image>().color = m_normalBackgroundColor;
            }

            if (m_currentWheelSelected == m_lettersWheels.Count)
                m_confirmButton.GetComponent<Image>().color = m_selectedButtonColor;
            else
                m_confirmButton.GetComponent<Image>().color = m_normalButtonColor;


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


        private void DebugUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OpenLeaderboardEndGame();
            }

            if (Input.GetKeyDown(KeyCode.N)) // Keep for test
            {
                ResetLeaderboard();
                Debug.Log("Leaderboard Reset");
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

            SaveLeaderboard(m_leaderboard);
        }

        public void SaveLeaderboard(Leaderboard leaderboard)
        {
            // convert gameobject in json text
            string jsonString = JsonUtility.ToJson(leaderboard);
            // store json text in .txt
            File.WriteAllText(Application.dataPath + LEADERBOARD_LOCATION, jsonString);
            UpdateUILeaderboard();
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

        // Just to see ranks in main menu
        public void OpenRankPanel()
        {
            if (m_leaderboardPanel.activeSelf)
                return;

            m_leaderboardPanel.SetActive(true);
            m_rankPanel.SetActive(true);
        }

        public void OpenLeaderboardEndGame()
        {
            if (m_leaderboardPanel.activeSelf)
                return;

            m_leaderboardPanel.SetActive(true);

            if (CanPlayerBeInLeaderboard(m_scoreToAdd))
            {
                m_namePanel.SetActive(true);
                ChangeBackGroundColorSelected();
            }
            else
            {
                m_rankPanel.SetActive(true);

            }
        }

        public void NameIsChosen_BT()
        {
            string name = "";

            foreach (Transform _text in m_lettersWheels)
            {
                TextMeshProUGUI midLetter = _text.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
                name += midLetter.text;
            }

            AddPlayerToLeaderboard(name, m_scoreToAdd);
            m_rankPanel.SetActive(true);
            m_namePanel.SetActive(false);
        }

        public void QuitLeaderboardPanel()
        {
            m_namePanel.SetActive(false);
            m_rankPanel.SetActive(false);
            m_leaderboardPanel.SetActive(false);
        }

        public void OnNavigate(Vector2 pressValue)
        {

            if (!m_namePanel.activeSelf)
                return;

            if (pressValue.x > 0.7)
            {
                GoRight();
            }
            else if (pressValue.x < -0.7)
            {
                GoLeft();
            }
            else if (pressValue.y == 1)
            {
                GoUp();
            }
            else if (pressValue.y == -1)
            {
                GoDown();
            }

        }

        public void OnAccept()
        {
            if (!m_namePanel.activeSelf)
                return;

            if (m_currentWheelSelected == m_lettersWheels.Count)
                NameIsChosen_BT();
        }
        public void OnDecline()
        {
            if (!m_rankPanel.activeSelf)
                return;

            QuitLeaderboardPanel();
        }

    }




}


