using System;
using System.IO;
using System.Media;

namespace CyberBot
{
    public class Voice
    {
        public Voice()
        {
            greet();
        }
        public void greet()
        {
            try
            {
                string runningDir = AppDomain.CurrentDomain.BaseDirectory;
                string fullpath = Path.Combine(runningDir, "greeting.wav");
                if (File.Exists(fullpath))
                {
                    SoundPlayer play_greeting = new SoundPlayer(fullpath);
                    play_greeting.Load();
                    play_greeting.Play();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Audio not found at: {fullpath}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Audio Error: {ex.Message}");
            }
        }
    }
}