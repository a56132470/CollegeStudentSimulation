using UnityEngine;
using UnityEngine.UI;

public class StatePanel : MonoBehaviour
{
    public State state;
    private Text _title;
    private Text _effect;
    private Text _remainRound;

    private void Awake()
    {
        _title = transform.Find("Title").GetComponent<Text>();
        _effect = transform.Find("Effect").GetComponent<Text>();
        _remainRound = transform.Find("RemainRound").GetComponent<Text>();
    }

    public void SetState(State st, string ef)
    {
        state = st;
        _title.text = st.Name;
        if(st.StType==StateType.Temporarily)
            _remainRound.text = "剩余" + st.RemainTime.ToString() + "回合";
        _effect.text = ef;
    }
}