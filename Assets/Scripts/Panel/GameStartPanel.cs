using Framework.UI.Manager;
using Framework.UI.UIPanel;
using UnityEngine;
using UnityEngine.UI;

namespace Panel
{
    public class GameStartPanel : BasePanel
    {
        private Button _startButton;
        private Button _settingButton;

        private void Update()
        {
            // 点击esc退出
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        public override void Init()
        {
            base.Init();
            _startButton = transform.Find("StartBtn").GetComponent<Button>();
            _settingButton = transform.Find("SettingBtn").GetComponent<Button>();
        }

        public override void OnEnter(object intent = null)
        {
            base.OnEnter(intent);
            _startButton.onClick.AddListener(OnStartButtonClick);
            _settingButton.onClick.AddListener(OnSettingButtonClick);
        }

        public override void OnExit()
        {
            base.OnExit();
            _startButton.onClick.RemoveListener(OnStartButtonClick);
            _settingButton.onClick.RemoveListener(OnSettingButtonClick);
        }
        public override void OnPause()
        {
            base.OnPause();
            gameObject.SetActive(false);
        }
        public override void OnResume()
        {
            base.OnResume();
            gameObject.SetActive(true);
        }
        /// <summary>
        /// 开始游戏按钮
        /// </summary>
        private void OnStartButtonClick()
        {
            UIPanelManager.Instance.PushPanel(UIPanelType.Save);
        }

        /// <summary>
        /// 设置按钮
        /// </summary>
        private void OnSettingButtonClick()
        {
            UIPanelManager.Instance.PushPanel(UIPanelType.Setting);
        }
    }
}