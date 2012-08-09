using System;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Omega;

namespace VectorArena
{
    public partial class GamePage : PhoneApplicationPage
    {
        GraphicsDevice graphicsDevice;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        ContentManager contentManager;
        GameTimer timer;
        GameplayScene scene;
        AudioManager audioManager;
        Settings settings;
        bool isNewPageInstance = false;

        public GamePage()
        {
            InitializeComponent();

            graphicsDevice = SharedGraphicsDeviceManager.Current.GraphicsDevice;

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;
            
            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

            isNewPageInstance = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            spriteBatch = new SpriteBatch(graphicsDevice);
            spriteFont = contentManager.Load<SpriteFont>("SpriteFont");

            graphicsDevice.BlendState = BlendState.Additive;

            if (isNewPageInstance)
            {
                if (scene == null)
                {
                    if (State.Count > 0)
                    {
                        scene = (GameplayScene)State["Scene"];
                    }
                    else
                    {
                        audioManager = (Application.Current as App).AudioManager;
                        
                        string mode;

                        if (NavigationContext.QueryString.TryGetValue("mode", out mode))
                        {
                            if (mode == "marathon")
                                scene = new MarathonScene(graphicsDevice, audioManager);
                            else if (mode == "timeattack")
                                scene = new TimeAttackScene(graphicsDevice, audioManager);
                        }                        
                    }
                }

                scene.LoadContent(contentManager);                
            }

            settings = new Settings();

            isNewPageInstance = false;

            // Start the timer
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            scene.OnNavigatedFrom();

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            VirtualThumbsticks.Update();

            scene.Update(e);

            if (scene.EndScene) 
            {
                (Application.Current as App).Score = scene.ShipManager.PlayerShip.Score;
                NavigationService.Navigate(new Uri("/Pages/GameOverPage.xaml", UriKind.Relative));
            }
        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            scene.Draw();
        }
    }
}