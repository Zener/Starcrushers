///////////////////////////////////////////////////////////////////////////////////////////////
// Starcrushers
// Programmed by Carlos Peris
// Barcelona, March-June 2007
///////////////////////////////////////////////////////////////////////////////////////////////



using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using System.IO;


namespace ShipsWar
{
    class SoundManager
    {
        static AudioEngine audioEngine;
        static WaveBank waveBank;
        static SoundBank soundBank;
        static List<Cue> playingSounds;

        public const string MUSIC_INGAME = "Ingame 2";
        public const string MUSIC_MAINMENU = "Menu Principal 2";
        public const string MUSIC_OPTION = "Option";
        public const string MUSIC_ACCEPT = "Ok";
        public const string MUSIC_VICTORY = "Victoria 2";
        public const string MUSIC_DEFEAT = "Derrota";
        public const string MUSIC_PLANETBLAST = "Planet Blast";
        public const string MUSIC_STATISTICS = "Estadistiques";
        public const string MUSIC_TAKEOFF = "Take Off";
        public const string MUSIC_STARBASEBLAST = "Escut";

        static TimeSpan timeForTakeOffSound = new TimeSpan();



        public static void StartUp()
        {
            
            if (audioEngine == null)
            {
                playingSounds = new List<Cue>();

                audioEngine = new AudioEngine(Game1.main.DataPath + @"Sound\Win\WavTest.xgs");
                waveBank = new WaveBank(audioEngine, Game1.main.DataPath + @"Sound\Wave Bank.xwb");
                soundBank = new SoundBank(audioEngine, Game1.main.DataPath + @"Sound\Win\Sound Bank.xsb");
            }
        }
        /*
        private static Cue Play(string Name)
        {5
            Cue returnValue = soundBank.GetCue(Name);
            returnValue.Play();
            return returnValue;
        }*/

        public static void Update()
        {
            
            StartUp();
            AudioCategory music = audioEngine.GetCategory("Music");
            music.SetVolume(GameVars.musicVolume);

            AudioCategory sfx = audioEngine.GetCategory("Sfx");
            sfx.SetVolume(GameVars.soundVolume);
         
        }

        public static void Update(GameTime gametime)
        {
            Update();
            
            if (timeForTakeOffSound.TotalMilliseconds > 0)
            {
                timeForTakeOffSound -= gametime.ElapsedGameTime;
            }
        }


        public static void SoundPlay(string snd)
        {
            if (snd == MUSIC_TAKEOFF || snd == MUSIC_PLANETBLAST)
            {
                if (timeForTakeOffSound.TotalMilliseconds > 0)
                {
                    return;
                }
                else
                {
                    timeForTakeOffSound = new TimeSpan(0, 0, 0, 0, 500);
                }
            }
            StartUp();
            try
            {
                //if (soundBank.IsInUse) return;
                Cue returnValue = soundBank.GetCue(snd);
                
                returnValue.Play();
                //soundBank.PlayCue(snd);
                playingSounds.Add(returnValue);
            }
            catch (Exception e) { }
            
        }

        public static void SoundStop()
        {
            
            StartUp();
            while (playingSounds.Count > 0)
            {
                playingSounds[0].Stop(AudioStopOptions.Immediate);
                playingSounds.RemoveAt(0);
            }
        }
    }
}
