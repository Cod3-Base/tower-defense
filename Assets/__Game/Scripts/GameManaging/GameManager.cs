using System;
using System.IO;
using System.Text;
using GMTowerDefense.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMTowerDefense.GameManaging
{
    /// <summary>
    /// This class is responsible for remembering the data, and doing things appropriately.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private GameObject liveLabel;
        [SerializeField] private GameObject scoreLabel;
        [SerializeField] private GameObject cashLabel;
        [SerializeField] private GameObject waveLabel;
        
        [SerializeField] private int defaultLiveAmount;
        [SerializeField] private int initialCash;

        [SerializeField] private string deathScene;

        private bool _dying;
        
        public int CurrentWave { get; private set; }
        public int Lives { get; private set; }
        public int Score { get; private set; }
        public int Cash { get; private set; }

        public void Awake()
        {
            Lives = defaultLiveAmount;
            Cash = initialCash;
            
            SetWaveLabelText();
            SetLiveLabelText();
            SetScoreLabelText();
            SetCashLabelText();
        }

        public void IncreaseWaves()
        {
            CurrentWave++;
            SetWaveLabelText();
        }

        public void IncreaseLives(int amount)
        {
            Lives += amount;
            SetLiveLabelText();
        }

        public void DecreaseLives(int amount)
        {
            Lives -= amount;
            SetLiveLabelText();
        }

        public void IncreaseScore(int amount)
        {
            Score += amount;
            SetScoreLabelText();
        }

        public void IncreaseCash(int amount)
        {
            Cash += amount;
            SetCashLabelText();
        }

        public void DecreaseCash(int amount)
        {
            Cash -= amount;
            SetCashLabelText();
        }

        private void SetLiveLabelText()
        {
            liveLabel.GetComponent<Label>().SetText(Lives >= 0 ? $"{Lives}" : "0");
        }
        
        private void SetScoreLabelText()
        {
            scoreLabel.GetComponent<Label>().SetText($"{Score}");
        }
        
        private void SetCashLabelText()
        {
            cashLabel.GetComponent<Label>().SetText($"${Cash}");
        }

        private void SetWaveLabelText()
        {
            waveLabel.GetComponent<Label>().SetText($"{CurrentWave}");
        }
        
        private void Update()
        {
            if (Lives <= 0 && !_dying)
                OnDeath();
        }

        private void OnDeath()
        {
            Debug.Log("Death!");

            SceneManager.LoadSceneAsync(deathScene);
        }

        [Serializable]
        private class Data
        {
            public int wave;
            public int score;
        }
        
    }
}
