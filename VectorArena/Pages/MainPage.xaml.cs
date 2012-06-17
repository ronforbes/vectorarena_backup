using System;
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
    public partial class MainPage : PhoneApplicationPage
    {
        GraphicsDevice graphicsDevice;
        ContentManager contentManager;
        GameTimer timer;
        Scene scene;
        ParticleManager particleManager;
        AudioManager audioManager;
        Random random;
        int particleEffectDelay;
        UIElementRenderer elementRenderer;
        SpriteBatch spriteBatch;
        Settings settings;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            graphicsDevice = SharedGraphicsDeviceManager.Current.GraphicsDevice;

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;

            particleEffectDelay = 0;

            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

            LayoutUpdated += new EventHandler(MainPage_LayoutUpdated);
        }

        void MainPage_LayoutUpdated(object sender, EventArgs e)
        {
            // Create the UIElementRenderer to draw the XAML page to a texture.

            // Check for 0 because when we navigate away the LayoutUpdate event
            // is raised but ActualWidth and ActualHeight will be 0 in that case.
            if ((ActualWidth > 0) && (ActualHeight > 0))
            {
                SharedGraphicsDeviceManager.Current.PreferredBackBufferWidth = (int)ActualWidth;
                SharedGraphicsDeviceManager.Current.PreferredBackBufferHeight = (int)ActualHeight;
            }

            // make sure page size is valid
            if (ActualWidth == 0 || ActualHeight == 0)
                return;

            // see if we already have the right sized renderer
            if (elementRenderer != null &&
                elementRenderer.Texture != null &&
                elementRenderer.Texture.Width == (int)ActualWidth &&
                elementRenderer.Texture.Height == (int)ActualHeight)
            {
                return;
            }

            // dispose the current renderer
            //if (elementRenderer != null)
            //    elementRenderer.Dispose();

            if (null == elementRenderer)
            {
                if (ActualHeight > ActualWidth)
                    elementRenderer = new UIElementRenderer(this, (int)ActualHeight, (int)ActualWidth);
                else
                    elementRenderer = new UIElementRenderer(this, (int)ActualWidth, (int)ActualHeight);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            spriteBatch = new SpriteBatch(graphicsDevice);

            graphicsDevice.BlendState = BlendState.Additive;

            particleManager = new ParticleManager(10000);

            audioManager = (Application.Current as App).AudioManager;

            scene = new Scene(graphicsDevice);
            scene.AddActor(new Starfield());
            scene.AddActor(new Grid());
            scene.AddActor(particleManager);
            scene.AddActor(audioManager);
            scene.AddPostprocess(new Bloom(new SpriteBatch(graphicsDevice), graphicsDevice));
            scene.LoadContent(contentManager);

            random = new Random();

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

            base.OnNavigatedFrom(e);
        }

        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            scene.Update();

            if (particleEffectDelay++ >= 1)
            {
                particleEffectDelay = 0;
                particleManager.CreateParticleEffect(25, new Vector3(random.Next(-400, 400), random.Next(-200, 200), random.Next(0, 0)), random.Next(50), new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()));
            }

            audioManager.PlaySong("BasicDrumBeat", true);
        }

        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            // Render the Silverlight controls using the UIElementRenderer.
            elementRenderer.Render();

            scene.Draw();

            spriteBatch.Begin();

            // Using the texture from the UIElementRenderer, 
            // draw the Silverlight controls to the screen.
            spriteBatch.Draw(elementRenderer.Texture, Vector2.Zero, Color.White);

            spriteBatch.End();
        }

        // Simple button Click event handler to take us to the second page
        private void MarathonButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/GamePage.xaml?mode=marathon", UriKind.Relative));
        }

        private void TimeAttackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/GamePage.xaml?mode=timeattack", UriKind.Relative));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/SettingsPage.xaml", UriKind.Relative));
        }
    }
}