
namespace Framework.Event
{
   public enum EventType 
   {
      GameInit,           // 游戏初始化
      RefreshRoommate,    // 更新室友信息
      UpdateTip,          // 更新提示
      UpdateToast,        // 更新带双按钮的提示
      UpdateActionCaption,
      NextRound,          // 下一回合
      ChangeSkin,         // 更换首页皮肤
      UpdateActionPanelEvent, // 更新事件栏
      UpdateRoundTxt,
      ControllUIOnOff
   }
}
