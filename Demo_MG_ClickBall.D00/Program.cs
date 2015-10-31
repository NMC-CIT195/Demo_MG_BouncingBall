﻿using System;

namespace Demo_MG_ClickBall.D00
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new ClickBall())
                game.Run();
        }
    }
#endif
}
