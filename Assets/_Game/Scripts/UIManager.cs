using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] [Range(1, 4)] float animationSpeed;
    [SerializeField] [Range(0, 1)] float animationTextSpeed;
    [SerializeField] private AnimationCurve _animX, _animY;

    private string[] sayingsArr = {
"Where did my body go..."
, "I miss my heart..."
,"bla bla..."
,"<3 <3 <3"};
    [SerializeField] private Text sayBox;
    [SerializeField] private Image sayBoxBackground;

    [SerializeField] private enum performAction {Load,Quit }
    [SerializeField] private performAction action;
    private void Start()
    {
        for (int i = 0; i < sayingsArr.Length; i++)
        {
            print(sayingsArr[i]);
        }
        print(sayingsArr.Length);
        sayBox.text = "";
        sayBoxBackground.color = new Color(sayBoxBackground.color.r, sayBoxBackground.color.g, sayBoxBackground.color.b, 0);

        counterStart();
    }
    public void CloseApplication() {
        action = performAction.Quit;

    }

    public void ScaleCoroutine(RectTransform obj)
    {
        StartCoroutine(ScalePerFixed(obj));

    }
    public void LoadScene()
    {
        action = performAction.Load;


    }
    private    IEnumerator ScalePerFixed( RectTransform obj) {

        float scaleX = obj.localScale.x;
        float scaleY = obj.localScale.y;
        float tmp = 0;
        while (tmp<=1)
        {
            tmp += Time.fixedDeltaTime* animationSpeed;
            obj.localScale = new Vector2(scaleX * _animX.Evaluate(tmp), scaleY * _animY.Evaluate(tmp));

                        yield return new WaitForFixedUpdate();
        }

        switch (action)
        {
            case performAction.Load:
                print("Loaded");
                SceneManager.LoadScene("");

                break;
            case performAction.Quit:
                print("Quit");

                Application.Quit();

                break;
            
        }


    }
    public void SaySomething( ) {
        StartCoroutine(PrintString(sayBox));

    }
    public void counterStart() {

        StartCoroutine(WaitforX(Random.Range(0,6)));

    }
    private IEnumerator WaitforX(int interval) {

    yield return new WaitForSecondsRealtime(interval);
        SaySomething();
    }
    private IEnumerator Vanish_Appear(bool isVanishing) {
        float tmp ;
        Color color_tmp = sayBoxBackground.color;
        if (isVanishing)
        {

            tmp = 0;
            while (tmp <= 1)
            {
                tmp += Time.fixedDeltaTime;
                sayBoxBackground.color = new Color(color_tmp.r, color_tmp.g, color_tmp.b, tmp);
                yield return new WaitForFixedUpdate();

            }
        }
        else
        {

            tmp = 1;

            while (tmp >= 0)
            {
                tmp -= Time.fixedDeltaTime;

                sayBoxBackground.color = new Color(color_tmp.r, color_tmp.g, color_tmp.b, tmp);
                yield return new WaitForFixedUpdate();

            }
        }
       
    }
    private IEnumerator PrintString(Text _text)
    {
     StartCoroutine(   Vanish_Appear(true));
        _text.text = "";
        string saying = sayingsArr[Random.Range(0, sayingsArr.Length)];
        
            for (int i = 0; i < saying.Length; i++)
            {
            _text.text += saying[i];

            yield return new WaitForSecondsRealtime(animationTextSpeed);


                }
        yield return new WaitForSecondsRealtime(3f);

        StartCoroutine( Vanish_Appear(false));
        _text.text = "";


        counterStart();



    }
}
