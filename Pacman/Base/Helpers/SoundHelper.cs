using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Base
{
    public class SoundHelper
    {
        private Dictionary<string, SoundEffect> soundEffects;
        private Dictionary<string, Song> songs;

        private string songPlaying;

        public bool Enabled { get; set; }

        public SoundHelper()
        {
            this.soundEffects = new Dictionary<string, SoundEffect>();
            this.songs = new Dictionary<string, Song>();
        }

        public void LoadAudio(ContentManager content)
        {
            this.soundEffects["game_over"] = content.Load<SoundEffect>("sounds/gameover");
            this.soundEffects["bubble"] = content.Load<SoundEffect>("sounds/bubble");
            this.soundEffects["powerup"] = content.Load<SoundEffect>("sounds/powerup");
        }

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

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(this.songs[song]);
            this.songPlaying = song;
        }

        public void PlaySoundEffect(string soundEffect)
        {
            if (!this.Enabled)
                return;

            this.soundEffects[soundEffect].Play();
        }
    }
}
