using System.Collections.Generic;
using Framework.UI.Tools;
using Framework.UI.UIPanel;
using LitJson;
using UnityEngine;

namespace Framework.UI.Manager
{
    public class UIPanelManager : MonoBehaviour
    {
        public static UIPanelManager Instance;
        private Transform _canvasTransform;

        private Transform CanvasTransform
        {
            get
            {
                if (_canvasTransform == null)
                {
                    _canvasTransform = GameObject.Find("Canvas").transform;
                }
                return _canvasTransform;
            }
        }
    
        private Dictionary<string, string> _panelPathDict;
        private Dictionary<string, BasePanel> _panelDict;
        private Stack<BasePanel> _panelStack;

        private void Awake()
        {
            ParseUiPanelTypeJson();
            if (Instance == null)
            {
                Instance = this;
            }
        }

        /// <summary>
        /// 将UIPanel推入栈
        /// </summary>
        /// <param name="panelType"></param>
        /// <param name="intent"></param>
        public void PushPanel(string panelType,object intent = null)
        {
            if (_panelStack == null)
            {
                _panelStack = new Stack<BasePanel>();
            }
            // 停止上一个界面
            if (_panelStack.Count > 0)
            {
                BasePanel topPanel = _panelStack.Peek();
                topPanel.OnPause();
            }

            BasePanel panel = GetPanel(panelType);
            _panelStack.Push(panel);
            panel.OnEnter(intent);
        }
        /// <summary>
        ///  弹出栈顶的UIPanel，并恢复弹出后栈顶的面板
        /// </summary>
        public void PopPanel()
        {
            if (_panelStack == null)
            {
                _panelStack = new Stack<BasePanel>();
            }
            // 停止上一个界面
            if (_panelStack.Count <= 0)
            {
                return;
            }
            // 退出栈顶面板
            BasePanel topPanel = _panelStack.Pop();
            topPanel.OnExit();
            // 恢复上一个面板
            if (_panelStack.Count > 0)
            {
                BasePanel panel = _panelStack.Peek();
                panel.OnResume();
            }
        }

        public BasePanel GetPanel(string panelType)
        {
            if (_panelDict == null)
            {
                _panelDict = new Dictionary<string, BasePanel>();
            }
            BasePanel panel = _panelDict.GetValue(panelType);
            // 如果没有实例化面板，寻找路径进行实例化，并且存储到已经实例化好的字典面板中
            if (panel == null)
            {
                string path = _panelPathDict.GetValue(panelType);
            
                GameObject panelGo = Instantiate(Resources.Load<GameObject>(path), CanvasTransform, false);
                panel = panelGo.GetComponent<BasePanel>();
                _panelDict.Add(panelType, panel);
                panel.Init();
            }
            return panel;
        }

        // 解析Json文件
        private void ParseUiPanelTypeJson()
        {
            _panelPathDict = new Dictionary<string, string>();
            var textUiPanelType = Resources.Load<TextAsset>("UIPanelTypeJson");
            var panelInfoList = JsonMapper.ToObject<UIPanelInfoList>(textUiPanelType.text);

            foreach (UIPanelInfo panelInfo in panelInfoList.panelInfoList)
            {
                _panelPathDict.Add(panelInfo.panelType, panelInfo.path);
            }
        }
    }
}