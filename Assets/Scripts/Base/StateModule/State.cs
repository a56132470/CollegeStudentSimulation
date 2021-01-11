using Base.Struct;

[System.Serializable]
public class State
{
    private StateType _stateType;     // 类型
    private string _name;        // 名称
    private int _logic;          // 逻辑
    private int _talk;           // 言语
    private int _athletics;      // 体能
    private int _creativity;     // 灵感
    private BonusStruct _bonus;  // 属性加成

    public BonusStruct Bonus
    {
        get => _bonus;
        set => _bonus = value;
    }

    private int _duration;       // 持续时间
    private int _remainTime;     // 剩余时间
    private string _otherEffect; // 其他效果
    private bool _isTemp;        // 是否为临时状态
    private bool _isHide;        // 是否隐藏

    public State()
    {
        IsHide = false;
    }

    public State(StateType type, string name, int logic, int talk, int athletics, int creativity, int duration, string otherEffect, bool isTemp)
    {
        _stateType = type;
        _name = name;
        _logic = logic;
        _talk = talk;
        _athletics = athletics;
        _creativity = creativity;
        _duration = duration;
        _otherEffect = otherEffect;
        _isTemp = isTemp;
        IsHide = false;
    }

    public StateType StType { get => _stateType; set => _stateType = value; }
    public string Name { get => _name; set => _name = value; }
    public int Logic { get => _logic; set => _logic = value; }
    public int Talk { get => _talk; set => _talk = value; }
    public int Athletics { get => _athletics; set => _athletics = value; }
    public int Creativity { get => _creativity; set => _creativity = value; }
    public int Duration { get => _duration; set => _duration = value; }
    public string OtherEffect { get => _otherEffect; set => _otherEffect = value; }
    public bool IsTemp { get => _isTemp; set => _isTemp = value; }

    public int RemainTime
    {
        get => _remainTime;
        set
        {
            _remainTime = value;
            if (_remainTime < 1)
                _remainTime = 0;
        }
    }

    public bool IsHide { get => _isHide; set => _isHide = value; }
}