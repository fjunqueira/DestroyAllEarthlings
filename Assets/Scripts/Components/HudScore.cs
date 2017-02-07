using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DestroyAllEarthlings
{
    public class HudScore : MonoBehaviour
    {
        [SerializeField]
        private Text hudScore;

        public int Score { get; set; }

        private void Update()
        {
            hudScore.text = Score.ToString();
        }
    }
}