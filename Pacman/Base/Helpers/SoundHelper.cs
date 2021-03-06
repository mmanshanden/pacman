﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Base
{
    public class SoundHelper
    {
        private Dictionary<string, SoundEffect> soundEffects;
        private Dictionary<string, DateTime> startTimes;
        private Dictionary<string, TimeSpan> durations;
        private Dictionary<string, Song> songs;

        private string songPlaying; // current song

        private bool enabled;

        public bool Enabled
        {
            get { return this.enabled; }
            set
            {
                this.enabled = value;

                if (this.enabled == false)
                    MediaPlayer.Stop();
            }
        }

        public SoundHelper()
        {
            this.soundEffects = new Dictionary<string, SoundEffect>();
            this.songs = new Dictionary<string, Song>();

            this.startTimes = new Dictionary<string, DateTime>();
            this.durations = new Dictionary<string, TimeSpan>();
        }

        public void LoadAudio(ContentManager content)
        {
            this.soundEffects["live_lost"]   = content.Load<SoundEffect>("sounds/lifelost");
            this.soundEffects["ghost_dead"]  = content.Load<SoundEffect>("sounds/ghostdead");
            this.soundEffects["bubble"]      = content.Load<SoundEffect>("sounds/bubble");
            this.soundEffects["powerup"]     = content.Load<SoundEffect>("sounds/powerup");
            this.soundEffects["level_start"] = content.Load<SoundEffect>("sounds/levelstart");

            this.durations["bubble"]  = new TimeSpan(0, 0, 0, 0, 565);
            this.startTimes["bubble"] = DateTime.Now;

            this.durations["powerup"]  = new TimeSpan(0, 0, 0, 1, 800);
            this.startTimes["powerup"] = DateTime.Now;

            this.durations["ghost_dead"] = new TimeSpan(0, 0, 0, 0, 300);
            this.startTimes["ghost_dead"] = DateTime.Now;
        }

        /// <summary>
        /// Plays song unless song already playing.
        /// Stops music when no name is given.
        /// </summary>
        /// <param name="song">Song name</param>
        public void PlaySong(string song)
        {
            if (!this.Enabled)
                return;

            if (song == "")
            {
                MediaPlayer.Stop();
                return;
            }

            if (MediaPlayer.State == MediaState.Playing)
            {
                if (this.songPlaying == song)
                    return;
            }

            MediaPlayer.Volume = 0.6f;
            MediaPlayer.Play(this.songs[song]);
            this.songPlaying = song;
        }

        /// <summary>
        /// Plays soundeffect. 
        /// If a duration for given soundeffect is available and has
        /// not elapsed, this request will be ignored.
        /// </summary>
        /// <param name="soundEffect"></param>
        public void PlaySoundEffect(string soundEffect)
        {
            if (!this.Enabled)
                return;

            if (this.durations.ContainsKey(soundEffect))
            {
                DateTime now = DateTime.Now;

                // if elapsed time is bigger than duration
                if (now - this.startTimes[soundEffect] > this.durations[soundEffect])
                {
                    this.soundEffects[soundEffect].Play();
                    this.startTimes[soundEffect] = now;
                }

            }
            else
                this.soundEffects[soundEffect].Play();
        }
    }
}
