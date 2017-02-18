using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
using System;

namespace DestroyAllEarthlings
{
    public class Hud : MonoBehaviour
    {
        [SerializeField]
        private float gameDuration = 100;

        [SerializeField]
        private Text hudScore;

        [SerializeField]
        private Text hudTimer;

        [SerializeField]
        private Text hudEscapees;

        [SerializeField]
        private Image image;

        [SerializeField]
        private Text allHumans;

        [SerializeField]
        private Text mostHumans;

        [SerializeField]
        private Text timesUp;

        [SerializeField]
        private Text tooManyEscapees;

        [SerializeField]
        private Text pressEnter;

        public int RemainingHumans { get; set; }

        public int Escapees { get; set; }

        private float interpolation;

        private bool blinking = false;

        private int maximumEscapees = 0;

        [SerializeField]
        private int humanPerEscapee = 5;

        private void Start()
        {
            Destroyable.destroyed += AddPoints;

            Bunker.escaped += AddEscapee;

            RemainingHumans = GameObject.FindObjectsOfType<Destroyable>()
                .Select(destroyable => destroyable.EarthlingCount)
                .Sum();

            maximumEscapees = RemainingHumans / humanPerEscapee;
        }

        private void Update()
        {
            bool gameEnded = RemainingHumans <= 0 || gameDuration <= 0 || Escapees > maximumEscapees;

            if (gameEnded)
            {
                Destroyable.destroyed -= AddPoints;
                Bunker.escaped -= AddEscapee;

                interpolation = Mathf.Clamp(interpolation + Time.deltaTime, 0, 1);
                this.image.material.SetFloat("_Size", Mathf.Lerp(0, 2f, interpolation));

                if (!blinking) StartCoroutine(BlinkText());

                if (Input.GetKeyUp(KeyCode.Return))
                {
                    ClearScreen();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }

            if (!gameEnded)
                gameDuration -= Time.deltaTime;

            hudTimer.text = string.Format("Time: {0}", Mathf.Round(Mathf.Clamp(gameDuration, 0, gameDuration)));
            hudScore.text = string.Format("Remaining Humans: {0}", RemainingHumans);
            hudEscapees.text = string.Format("Escapees: {0}", Escapees);
        }

        private IEnumerator BlinkText()
        {
            blinking = true;

            while (true)
            {
                var endText = RemainingHumans <= 0 && Escapees == 0 ? allHumans : 
                              Escapees > maximumEscapees ? tooManyEscapees : 
                              Escapees + RemainingHumans <= maximumEscapees ? mostHumans : timesUp;

                pressEnter.gameObject.SetActive(endText.gameObject.activeSelf);
                endText.gameObject.SetActive(!endText.gameObject.activeSelf);

                yield return new WaitForSeconds(2);
            }
        }

        private void ClearScreen()
        {
            this.image.material.SetFloat("_Size", 0);
            timesUp.gameObject.SetActive(false);
            allHumans.gameObject.SetActive(false);
            tooManyEscapees.gameObject.SetActive(false);
            mostHumans.gameObject.SetActive(false);
            pressEnter.gameObject.SetActive(false);
        }

        private void OnApplicationQuit()
        {
            this.image.material.SetFloat("_Size", 0);
        }

        private void AddPoints(int earthlingCount)
        {
            RemainingHumans -= earthlingCount;
        }

        private void AddEscapee()
        {
            Escapees++;
            RemainingHumans--;
        }
    }
}