﻿using System;

namespace Core.NotifySystem
{
    public interface INotifyService
    {
        void DoNotify(string content);
        void DoNotify(string content, Action confirmAction);
        void DoNotify(string content, Action confirmAction, Action cancelAction);
        void DoClose();
    }
}