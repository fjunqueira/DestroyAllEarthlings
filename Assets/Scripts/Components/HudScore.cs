using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DestroyAllEarthlings
{
    public class HudScore : MonoBehaviour
    {
        [SerializeField]
        private float gameDuration = 100;

        [SerializeField]
        private Text hudScore;

        [SerializeField]
        private Text hudTimer;

        public int RemainingHumans { get; set; }

        private void Start()
        {
            RemainingHumans = GameObject.FindObjectsOfType<Destroyable>()
                .Where(x => x.HumanCount > 0)
                .Select(x => x.HumanCount)
                .Sum();
        }

        private void Update()
        {
            if (RemainingHumans != 0)
                hudTimer.text = string.Format("Time: {0}", Mathf.Round(Mathf.Clamp(gameDuration -= Time.deltaTime, 0, gameDuration)));
            
            hudScore.text = string.Format("Remaining Humans: {0}", RemainingHumans);

            if (RemainingHumans <= 0)
            {
                // Victory screen
                //SceneManager.LoadScene(1);
                return;
            }

            if (gameDuration <= 0)
            {
                if (Input.GetKeyUp(KeyCode.Return)) SceneManager.LoadScene(0);
                return;
            }
        }
    }
}