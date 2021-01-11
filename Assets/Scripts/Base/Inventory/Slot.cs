using Framework.Event;
using Framework.UI.Manager;
using Framework.UI.Tools;
using Framework.UI.UIPanel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using EventType = Framework.Event.EventType;

namespace Base.Inventory
{
    public class Slot : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
    {
        // 物品持有的item Asset
        public Item slotItem;
        // 物品图片
        public Image slotImage;
        // 告罄图标
        [FormerlySerializedAs("RunOutSprite")] public Sprite runOutSprite;
        // 持有数量
        public Text heldNum;
        // 详细信息框
        public GameObject detailedInfoGam;
        // 价格文本框
        [FormerlySerializedAs("Price")] public Text price;
        // 详细信息标题文本框
        private Text _detailedInfoTitle;
        // 详细信息描述文本框
        private Text _detailedInfoCaption;
        // 购买/使用 按钮
        private Button _buyOrUseBtn;
        // 携带状态文本数组
        private string[] _carryingStateStr;
        // 是否可售
        public bool isSell;

        private void Start()
        {
            _detailedInfoTitle = detailedInfoGam.transform.Find("Title").GetComponent<Text>();
            _detailedInfoCaption = detailedInfoGam.transform.Find("Caption").GetComponent<Text>();
            _buyOrUseBtn = detailedInfoGam.GetComponentInChildren<Button>();

            _detailedInfoTitle.text = slotItem.itemName;
            _detailedInfoCaption.text = slotItem.itemEffect;
            _carryingStateStr = new string[2];
            heldNum.text = slotItem.itemHeld.ToString();
            if (price != null)
            {
                price.text = slotItem.itemPrice.ToString();
            }
            detailedInfoGam.SetActive(false);

            _buyOrUseBtn.onClick.AddListener(OnBuyOrUseButtonClick);
            TranslateFromStrToState(slotItem.itemEffect);
        }

        /// <summary>
        /// 点击时触发的事件，商店与背包共用，点击则让物品的详细框出现
        /// </summary>
        /// <param name="eventData"></param>
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (isSell && _buyOrUseBtn.gameObject.name.Equals("Buy"))
                detailedInfoGam.SetActive(true);

            if (_buyOrUseBtn.gameObject.name.Equals("Use"))
            {
                detailedInfoGam.SetActive(true);
                _buyOrUseBtn.gameObject.SetActive(!slotItem.itemName.Equals(SlotItemType.MechanicalKeyboard));
            }
        }

        /// <summary>
        /// 指针移出时触发的事件，商店与背包共用，移出时将详细信息清除
        /// </summary>
        /// <param name="eventData"></param>
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (isSell && _buyOrUseBtn.gameObject.name.Equals("Buy"))
                detailedInfoGam.SetActive(false);

            if (_buyOrUseBtn.gameObject.name.Equals("Use"))
            {
                detailedInfoGam.SetActive(false);
                _buyOrUseBtn.gameObject.SetActive(slotItem.itemName.Equals(SlotItemType.MechanicalKeyboard));
            }
        }

        /// <summary>
        /// 点击使用或购买按钮时触发事件，是一个分支判断，购买或使用
        ///<para>购买：若当前存在此物品，则让物品的持有数量+1，若不存在此物品则让人物持有该物品，并扣除相应零花钱</para>
        ///<para>使用：为用户增加当前状态，若当前状态存在则增加当前状态持续时间，若状态不存在则增加当前状态到面板上</para>
        /// </summary>
        private void OnBuyOrUseButtonClick()
        {
            if (_buyOrUseBtn.gameObject.name.Equals("Buy"))
            {
                // 买得起就扣除相应的钱款，并在bagInventory里增加item，买不起就弹出“你买不起”
                if (GlobalManager.Instance.player.Money >= slotItem.itemPrice)
                {
                    if (GlobalManager.Instance.myBag.itemList.Contains(slotItem))
                        slotItem.itemHeld += 1;
                    else
                        GlobalManager.Instance.myBag.itemList.Add(slotItem);
                    isSell = false;
                    if (runOutSprite != null)
                        slotImage.sprite = runOutSprite;
                    detailedInfoGam.SetActive(false);
                    GlobalManager.Instance.player.Money -= slotItem.itemPrice;
                    if (slotItem.itemName.Equals(SlotItemType.MechanicalKeyboard))
                    {
                        GlobalManager.Instance.player.AddState(_carryingStateStr[0]);
                        StoreInventoryManager.Instance.RemoveItem(slotItem);
                    
                    }
                }
                else
                {
                    UIPanelManager.Instance.PushPanel(UIPanelType.Tip);
                    EventCenter.Broadcast(EventType.UpdateTip, TipCaptionName.Buy_Fail);
                }
            }
            else if (_buyOrUseBtn.gameObject.name.Equals("Use"))
            {
                Use();
            }
        }

        /// <summary>
        /// 使用道具
        /// <para>若当前持有该状态，则让当前状态的剩余时间加长</para>
        /// <para>若当前未持有该状态，则增加当前持有的状态</para>
        /// <para></para>
        /// </summary>
        private void Use()
        {
            for (int i = 0; i < _carryingStateStr.Length && _carryingStateStr[i] != null; i++)
            {
                if (GlobalManager.Instance.player.stateDic.ContainsKey(_carryingStateStr[i]))
                {
                    GlobalManager.Instance.player.stateDic.GetValue(_carryingStateStr[i]).RemainTime +=
                        GlobalManager.Instance.player.stateDic.GetValue(_carryingStateStr[i]).Duration;
                }
                else
                {
                    GlobalManager.Instance.player.AddState(_carryingStateStr[i]);
                }
            }
            if (slotItem.itemHeld > 1)
            {
                slotItem.itemHeld -= 1;
            }
            else
            {
                GlobalManager.Instance.myBag.itemList.Remove(slotItem);
                Destroy(this);
            }
            BagInventoryManager.instance.Refresh();
        }

        /// <summary>
        /// 把效果里的状态转成State类,并存进当前物品携带的类里
        /// </summary>
        /// <param name="effect"></param>
        private void TranslateFromStrToState(string effect)
        {
            // 将字符串按【】进行分割
            string[] splitEffect = effect.Split(new char[2] { '【', '】' });
            splitEffect = ToNoSpaceStr(splitEffect);

            for (int j = 0; j < 2; j++)
            {
                if (splitEffect[j + 1] != null)
                {
                    if (XMLManager.Instance.stateDic.ContainsKey(splitEffect[j + 1]))
                    {
                        _carryingStateStr[j] = splitEffect[j + 1];
                    }
                }
            }
        }

        private string[] ToNoSpaceStr(string[] strs)
        {
            string[] newStrs = new string[3];
            int index = 0;
            for (int i = 0; i < strs.Length; i++)
            {
                if (!strs[i].Equals(""))
                {
                    newStrs[index++] = strs[i];
                }
            }
            return newStrs;
        }
    }
}