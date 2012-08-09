using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Omega;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Windows.Navigation;

namespace VectorArena.Pages
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        GraphicsDevice graphicsDevice;
        ContentManager contentManager;
        GameTimer timer;
        SubMenuScene scene;
        AudioManager audioManager;
        UIElementRenderer elementRenderer;
        SpriteBatch spriteBatch;
        Settings settings;

        public SettingsPage()
        {
            audioManager = (Application.Current as App).AudioManager;
            settings = (Application.Current as App).Settings;

            InitializeComponent();

            graphicsDevice = SharedGraphicsDeviceManager.Current.GraphicsDevice;

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;

            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

            LayoutUpdated += new EventHandler(SettingsPage_LayoutUpdated);
        }

        void SettingsPage_LayoutUpdated(object sender, EventArgs e)
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

            scene = new SubMenuScene(graphicsDevice);
            scene.LoadContent(contentManager);

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
            scene.Update(e);
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

        private void soundEnabled_Click(object sender, RoutedEventArgs e)
        {
            soundEnabled.Content = (bool)soundEnabled.IsChecked ? "On" : "Off";

            audioManager.SoundEnabled = settings.SoundEnabled;
        }

        private void musicEnabled_Click(object sender, RoutedEventArgs e)
        {
            musicEnabled.Content = (bool)musicEnabled.IsChecked ? "On" : "Off";

            audioManager.MusicEnabled = settings.MusicEnabled;

            if (audioManager.MusicEnabled && audioManager.State == Microsoft.Xna.Framework.Media.MediaState.Stopped)
            {
                audioManager.PlaySong("BasicDrumBeat", true);
            }
        }

        private void volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            audioManager.MusicVolume = (float)settings.Volume / Settings.MaxVolume;
            audioManager.SoundVolume = (float)settings.Volume / Settings.MaxVolume;
        }
    }
}