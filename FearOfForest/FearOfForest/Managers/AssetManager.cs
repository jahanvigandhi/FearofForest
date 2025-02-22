using FearOfForest.Entities;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace FearOfForest.Managers
{
    public static class AssetManager
    {
        public static Texture2D PlayerRunTexture { get; private set; }
        public static Texture2D PlayerJumpTexture { get; private set; }
        public static Texture2D PlayerAttackTexture { get; private set; }
        public static Texture2D PlayerDeadTexture { get; private set; }

        public static Texture2D BoarTexture { get; private set; }
        public static Texture2D FlyTexture { get; private set; }
        public static Texture2D SnailTexture { get; private set; }
        public static Texture2D CoinTexture { get; private set; }
        public static Texture2D PlatformTexture { get; private set; }
        public static Texture2D MainMenuBackground { get; private set; }
        public static Texture2D InfoBackground { get; private set; }
        public static Texture2D CreditBackground { get; private set; }

        public static Texture2D LeaderboardBackground { get; private set; }

        public static Texture2D BackgroundLayer1 { get; private set; }
        public static Texture2D BackgroundLayer2 { get; private set; }
        public static Texture2D BackgroundLayer3 { get; private set; }

        public static Texture2D Button { get; private set; }

        // Tileset
        public static Texture2D Tileset1 { get; private set; }
        public static Texture2D Tileset2 { get; private set; }
        public static Texture2D Tileset3 { get; private set; }


        // Fonts
        public static SpriteFont MenuFont { get; private set; }
        public static SpriteFont GameFont { get; private set; }

        // Sounds
        public static SoundEffect CoinSound { get; private set; }
        public static SoundEffect LevelUpSound { get; private set; }
        public static SoundEffect SmashSound { get; private set; }
        public static SoundEffect KillSound { get; private set; }

        private static Song BackgroundMusic { get; set; }

        // Instance of the background music
        public static Song BackgroundMusicInstance { get; private set; }

        public static void PlayBackgroundMusic()
        {
            if (BackgroundMusicInstance == null)
            {
                BackgroundMusicInstance = BackgroundMusic;
                MediaPlayer.Play(BackgroundMusicInstance);
                MediaPlayer.IsRepeating = true;
            }
            else
            {
                MediaPlayer.Resume();
            }
        }

        public static void PauseBackgroundMusic()
        {
            MediaPlayer.Pause();
        }

        public static void StopBackgroundMusic()
        {
            MediaPlayer.Stop();
        }

        public static void LoadAssets(ContentManager content)
        {
            // Load Textures for Player States
            PlayerRunTexture = content.Load<Texture2D>("Player/Run");       // 8 frames
            PlayerJumpTexture = content.Load<Texture2D>("Player/Jump");     // 15 frames
            PlayerAttackTexture = content.Load<Texture2D>("Player/Attack"); // 8 frames
            PlayerDeadTexture = content.Load<Texture2D>("Player/Dead");     // 8 frames


            // Load Textures
            BoarTexture = content.Load<Texture2D>("Textures/Boar");
            FlyTexture = content.Load<Texture2D>("Textures/Fly");
            SnailTexture = content.Load<Texture2D>("Textures/Snail");
            CoinTexture = content.Load<Texture2D>("Textures/Coin");
            PlatformTexture = content.Load<Texture2D>("Textures/Platform");
            MainMenuBackground = content.Load<Texture2D>("Textures/MainMenuBackground");
            InfoBackground = content.Load<Texture2D>("Textures/InfoBackground");
            CreditBackground = content.Load<Texture2D>("Textures/CreditBackground");
            Button = content.Load<Texture2D>("Textures/Button");
            LeaderboardBackground = content.Load<Texture2D>("Textures/Leaderboard");

            // Load Background Layers
            BackgroundLayer1 = content.Load<Texture2D>("Background/BackgroundLayer1"); // Front layer
            BackgroundLayer2 = content.Load<Texture2D>("Background/BackgroundLayer2"); // Middle layer
            BackgroundLayer3 = content.Load<Texture2D>("Background/BackgroundLayer3"); // Back layer

            // Load Tilesets
            Tileset1 = content.Load<Texture2D>("Tileset/Tileset2");
            Tileset2 = content.Load<Texture2D>("Tileset/Tileset1");
            Tileset3 = content.Load<Texture2D>("Tileset/Tileset3");


            // Load Fonts
            MenuFont = content.Load<SpriteFont>("Fonts/MenuFont");
            GameFont = content.Load<SpriteFont>("Fonts/GameFont");

            // Load Sounds
            CoinSound = content.Load<SoundEffect>("Sounds/coin");
            LevelUpSound = content.Load<SoundEffect>("Sounds/level_up");
            SmashSound = content.Load<SoundEffect>("Sounds/smash");
            KillSound = content.Load<SoundEffect>("Sounds/kill");
            BackgroundMusic = content.Load<Song>("Sounds/background");
        }
    }
}
