using System;

namespace Demo_MG_ClickBall.Sprint1
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
            using (var game = new BouncingBall())
                game.Run();
        }
    }
#endif
}
