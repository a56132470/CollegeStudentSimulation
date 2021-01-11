using System;
using System.Collections.Generic;
using Base.Struct;
using Framework.UI.Tools;
using UnityEngine.Serialization;

[Serializable]
// 人类，供室友与玩家继承
public class BasePerson
{
    public string Name { get; set; }
    
    /// <summary>
    /// 属性
    /// </summary>
    [FormerlySerializedAs("property")] public PropertyStruct propertyStruct;
    /// <summary>
    /// 加成
    /// </summary>
    public BonusStruct bonus;


    public int Money { get; set; }

    public Dictionary<string, State> stateDic;

    // 用于数据持久化
    public List<string> stateKeys;

    public List<State> stateValues;

    public string[,] records = new string[24, 5];


    // 当前回合
    protected int curRound;
    /// <summary>
    /// 当前回合
    /// </summary>
    public int CurRound
    {
        get => curRound;
        set
        {
            curRound = value;
            if (curRound > 24)
            {
                curRound = 24;
            }
            if (curRound <= 0)
            {
                curRound = 1;
            }
        }
    }
    /// <summary>
    /// 添加从XML文件中获取的状态到当前字典
    /// </summary>
    /// <param name="key"></param>
    public void AddState(string key)
    {
        if (XMLManager.Instance.stateDic.ContainsKey(key))
        {
            State state = XMLManager.Instance.stateDic.GetValue(key);
            state.RemainTime = state.Duration;
            stateDic.Add(key, XMLManager.Instance.stateDic.GetValue(key));
            bonus += state.Bonus;
        }
    }
    
    public virtual void AddRecordAction(string content) { }
    
}