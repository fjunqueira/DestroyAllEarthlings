using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class FadeOut : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private int fadeAfter = 5;

    private void Start()
    {
        var material = this.GetComponent<Renderer>().material;
        
        var startingColor = material.color;

        var transparentColor = new Color(startingColor.r, startingColor.g, startingColor.b, 0.0f);

        var interpolation = Observable.EveryUpdate()
            .Select(_ => Time.deltaTime)
            .Scan(0.0f, (acc, next) => (acc + (next / 10) * speed));

        var update = Observable.FromCoroutine(FadeAfter).ContinueWith(interpolation);

        update.Subscribe(i => material.color = Color.Lerp(startingColor, transparentColor, i)).AddTo(this);

        update.Where(x => x >= 1).Subscribe(_ => Destroy(this.gameObject)).AddTo(this);
    }

    private IEnumerator FadeAfter()
    {
        yield return new WaitForSeconds(fadeAfter);
    }
}
