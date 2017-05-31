﻿using System;

namespace WebGames
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
        //[MTAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
            //System.Threading.Thread t1 = new System.Threading.Thread(t =>
            //{
            //    Game1 game = new Game1();
            //    game.Run();
            //});
            //t1.Start();

            //System.Threading.Thread t2 = new System.Threading.Thread(t =>
            //{
            //    Game1 game1 = new Game1();
            //    game1.Run();
            //});
            //t2.Start();
        }
    }
#endif
}
