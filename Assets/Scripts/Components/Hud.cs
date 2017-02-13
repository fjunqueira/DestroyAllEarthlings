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
        private Image image;

        [SerializeField]
        private Text victory;

        [SerializeField]
        private Text gameOver;

        [SerializeField]
        private Text pressEnter;

        public int RemainingHumans { get; set; }

        private float interpolation;

        private bool blinking = false;

        private void Start()
        {
            Destroyable.destroyed += (earthlingCount) => RemainingHumans -= earthlingCount;

            RemainingHumans = GameObject.FindObjectsOfType<Destroyable>()
                .Select(destroyable => destroyable.EarthlingCount)
                .Sum();
        }

        private void Update()
        {
            bool gameEnded = RemainingHumans <= 0 || gameDuration <= 0;

            if (gameEnded)
            {
                interpolation = Mathf.Clamp(interpolation + Time.deltaTime, 0, 1);
                this.image.material.SetFloat("_Size", Mathf.Lerp(0, 2f, interpolation));

                if (!blinking) StartCoroutine(BlinkText());

                if (Input.GetKeyUp(KeyCode.Return))
                {
                    ClearScreen();
                    SceneManager.LoadScene(0);
                }
            }

            if (RemainingHumans > 0)
            {
                hudTimer.text = string.Format("Time: {0}", Mathf.Round(Mathf.Clamp(gameDuration -= Time.deltaTime, 0, gameDuration)));
                hudScore.text = string.Format("Remaining Humans: {0}", RemainingHumans);
            }
        }

        private IEnumerator BlinkText()
        {
            blinking = true;

            while (true)
            {
                var endText = gameDuration <= 0 ? gameOver : victory;

                pressEnter.gameObject.SetActive(endText.gameObject.activeSelf);
                endText.gameObject.SetActive(!endText.gameObject.activeSelf);

                yield return new WaitForSeconds(2);
            }
        }

        private void ClearScreen()
        {
            this.image.material.SetFloat("_Size", 0);
            victory.gameObject.SetActive(false);
            gameOver.gameObject.SetActive(false);
            pressEnter.gameObject.SetActive(false);
        }

        private void OnApplicationQuit()
        {
            this.image.material.SetFloat("_Size", 0);
        }
    }
}