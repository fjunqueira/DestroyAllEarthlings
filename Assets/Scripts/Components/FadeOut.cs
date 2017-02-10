using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Linq;

namespace DestroyAllEarthlings
{
    public class FadeOut : MonoBehaviour
    {
        [SerializeField]
        private float speed = 1;

        [SerializeField]
        private int fadeAfter = 5;

        private float interpolation = 0.0f;

        private void Start()
        {
            var materials = this.GetComponent<Renderer>().materials;

            var startingColors = materials.Select(x => x.color);

            var transparentColor = startingColors.Select(startingColor => new Color(startingColor.r, startingColor.g, startingColor.b, 0.0f));

            var zippedMaterials = materials.Zip3(startingColors, transparentColor,
                (material, startingColor, endingColor) => new { material, startingColor, endingColor });

            var update = Observable.FromCoroutine(FadeAfter).ContinueWith(Observable.EveryUpdate());

            update.Subscribe(_ =>
            {
                interpolation += Time.deltaTime * speed;

                foreach (var zip in zippedMaterials)
                    zip.material.color = Color.Lerp(zip.startingColor, zip.endingColor, interpolation);

            }).AddTo(this);

            update.Where(_ => interpolation >= 1).Subscribe(_ => Destroy(this.gameObject)).AddTo(this);
        }

        private IEnumerator FadeAfter()
        {
            yield return new WaitForSeconds(fadeAfter);
        }
    }
}