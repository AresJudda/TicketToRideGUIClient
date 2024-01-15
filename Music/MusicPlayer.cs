using System;
using System.IO;
using System.Windows.Media;

namespace TicketToRideGUI.Music
{
    public class MusicPlayer
    {
        public MediaPlayer mediaPlayer = new MediaPlayer();

        public MusicPlayer()
        {
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {

            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Play();
        }

        public void PlayMusic(string musicFileName)
        {
            try
            {
                string musicFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Music", musicFileName);
                mediaPlayer.Open(new Uri(musicFilePath, UriKind.Absolute));
                mediaPlayer.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al reproducir música: {ex.Message}");
            }
        }

        public void StopMusic()
        {
            mediaPlayer.Stop();
        }
    }
}

