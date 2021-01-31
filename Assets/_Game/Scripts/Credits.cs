using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] private RectTransform obj;
    [SerializeField] private Image _background;

    private void OnEnable()
    {
        obj.position = new Vector2(0, -Screen.height/2);
        StartCoroutine(Fade());
        StartCoroutine(RollTheCredits());
    }
    private IEnumerator Fade()
    {
        float tmp = 0;
        Color _color = _background.color;
        while (tmp <= 1)
        {
            tmp += Time.fixedDeltaTime;
            print("XXX");
            _background.color = new Vector4(_color.r, _color.g, _color.b, tmp);
            yield return new WaitForFixedUpdate();
        }

    }

        private IEnumerator RollTheCredits() {
        float _counter = Screen.height*2;

        float tmp = -Screen.height/1.5f;
        print(_counter);

        while (tmp <= _counter)
        {
            tmp += 2;
            //print("XXX");
            obj.position = new Vector2(0, tmp );
                    yield return new WaitForFixedUpdate();
        }

        Application.Quit();

        }
}
