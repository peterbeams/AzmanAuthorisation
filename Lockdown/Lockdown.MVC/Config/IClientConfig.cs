﻿namespace Lockdown.MVC.Config
{
    public interface IClientConfig
    {
        void UseNamedPipeClient();
        void UseDebugClient();
        void UseDebugClient(string[] roleNames);
    }
}