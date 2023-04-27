using System;
using Zenject;

namespace Core.NotifySystem
{
    public class NotifyService : INotifyService
    {
        [Inject] private readonly NotifyView notifyView;
        
        /// <summary>
        /// 純文字彈窗
        /// </summary>
        public void DoNotify(string content)
        {
            notifyView.SetAppear(true);
            notifyView.SetContent(content);
        }

        /// <summary>
        /// 通知型彈窗，可以將確認的功能設定為空 function
        /// </summary>
        public void DoNotify(string content, Action confirmAction)
        {
            notifyView.SetAppear(true);
            notifyView.SetContent(content, confirmAction);
        }

        /// <summary>
        /// 選擇型彈窗
        /// </summary>
        public void DoNotify(string content, Action confirmAction, Action cancelAction)
        {
            notifyView.SetAppear(true);
            notifyView.SetContent(content, confirmAction, cancelAction);
        }

        public void DoClose()
        {
            notifyView.SetAppear(false);
        }
    }
}