﻿using System;

namespace Platform
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = "Particular Service Platform Launcher";
            Particular.PlatformLauncher.Launch();
        }
    }
}
