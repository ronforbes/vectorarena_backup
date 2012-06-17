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
        public enum GameType
        {
            Marathon,
            TimeAttack
        }

        enum MessageType
        {
            ClientConnecting,
            ClientConnected,
            ClientDisconnecting,
            ClientDisconnected,
            UpdateShip
        }

        GameType gameType;

        GraphicsDevice graphicsDevice;
        
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        ContentManager contentManager;
        GameTimer timer;
        
        Scene scene;
        ShipManager shipManager;
        BulletManager bulletManager;
        DroneManager droneManager;
        MultiplierManager multiplierManager;
        ParticleManager particleManager;
        AudioManager audioManager;
        HUD hud;
        GameClient gameClient;
        Random random;
        Timer sendTimer;
        TimeSpan timeAttackTimer;
        TimeSpan postGameTimer;
        Settings settings;

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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string mode;

            if (NavigationContext.QueryString.TryGetValue("mode", out mode))
            {
                if (mode == "marathon")
                    gameType = GameType.Marathon;
                else if (mode == "timeattack")
                    gameType = GameType.TimeAttack;
            }

            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            spriteBatch = new SpriteBatch(graphicsDevice);
            spriteFont = contentManager.Load<SpriteFont>("SpriteFont");

            graphicsDevice.BlendState = BlendState.Additive;

            shipManager = new ShipManager(10);
            shipManager.ActivateShip(true, gameType);

            bulletManager = new BulletManager(100);
            droneManager = new DroneManager(50);
            multiplierManager = new MultiplierManager(100);
            particleManager = new ParticleManager(10000);
            audioManager = (Application.Current as App).AudioManager;
            hud = new HUD(gameType, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            timeAttackTimer = TimeSpan.FromMinutes(2);
            postGameTimer = TimeSpan.FromSeconds(3);

            shipManager.Initialize(ref droneManager, ref bulletManager, ref multiplierManager, ref particleManager, ref audioManager);
            droneManager.Initialize(ref shipManager, ref bulletManager, ref multiplierManager, ref particleManager, ref audioManager);
            bulletManager.Initialize(ref shipManager, ref droneManager, ref multiplierManager, ref particleManager, ref audioManager);
            multiplierManager.Initialize(ref shipManager, ref droneManager, ref bulletManager, ref particleManager, ref audioManager);
            droneManager.ShipManager = shipManager;
            hud.PlayerShip = shipManager.PlayerShip;
            hud.TimeAttackTimer = timeAttackTimer;

            scene = new Scene(graphicsDevice);
            scene.AddActor(new Starfield());
            scene.AddActor(new Grid());
            scene.AddActor(shipManager);
            scene.AddActor(bulletManager);
            scene.AddActor(droneManager);
            scene.AddActor(multiplierManager);
            scene.AddActor(particleManager);
            scene.AddActor(audioManager);
            scene.AddActor(hud);
            scene.AddPostprocess(new Bloom(new SpriteBatch(graphicsDevice), graphicsDevice));
            scene.Camera.TargetActor = shipManager.PlayerShip;

            scene.LoadContent(contentManager);

            audioManager.TargetListener = shipManager.PlayerShip;

            random = new Random();

            settings = new Settings();
            audioManager.Stop();

            gameClient = new GameClient("192.168.1.3", 1337);
            gameClient.Received += new ReceiveEventHandler(OnReceive);

            sendTimer = new Timer(new TimerCallback(OnSend), null, 0, 1000);

            gameClient.Send(MessageType.ClientConnecting.ToString());

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

            audioManager.Stop();

            gameClient.Send(MessageType.ClientDisconnecting.ToString());

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            VirtualThumbsticks.Update();

            scene.Update();

            if (gameType == GameType.TimeAttack)
            {
                if (timeAttackTimer.TotalSeconds > 0)
                {
                    timeAttackTimer -= e.ElapsedTime;
                    hud.TimeAttackTimer = timeAttackTimer;
                }

                if (timeAttackTimer.TotalSeconds <= 0)
                {
                    droneManager.KillDrones();
                    droneManager.Active = false;

                    postGameTimer -= e.ElapsedTime;
                }
                if (postGameTimer.TotalSeconds < 0)
                    shipManager.PlayerShip.GameOver = true;
            }

            if (shipManager.PlayerShip.GameOver) 
            {
                (Application.Current as App).Score = shipManager.PlayerShip.Score;
                NavigationService.Navigate(new Uri("/Pages/GameOverPage.xaml", UriKind.Relative));
            }

            audioManager.PlaySong("ParticleFusion", true);
        }

        void OnSend(object sender)
        {
            //gameClient.Send(MessageType.UpdateShip.ToString() + "," + string.Join(",",
            //    ship.Position.X, ship.Position.Y,
            //    ship.Velocity.X, ship.Velocity.Y));
        }

        void OnReceive(object sender, ReceiveEventArgs e) 
        { 
        
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