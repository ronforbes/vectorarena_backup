using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Omega;

namespace VectorArena
{
    public partial class GameOverPage : PhoneApplicationPage
    {
        GraphicsDevice graphicsDevice;
        ContentManager contentManager;
        GameTimer timer;
        SubMenuScene scene;
        UIElementRenderer elementRenderer;
        SpriteBatch spriteBatch;

        public GameOverPage()
        {
            InitializeComponent();

            App app = Application.Current as App;
            Score.Text = "Score: " + app.Score.ToString("#,#");
            app.Leaderboard.AddEntry(app.Score);
            
            //for(int e = 0; e < app.Leaderboard.Entries.Count; e++)
            //{
            //    Leaderboard.Text += app.Leaderboard.Entries[e].Rank + ". " + app.Leaderboard.Entries[e].Date + " " + app.Leaderboard.Entries[e].Score + "\n";
            //}

            graphicsDevice = SharedGraphicsDeviceManager.Current.GraphicsDevice;

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;

            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

            LayoutUpdated += new EventHandler(GameOverPage_LayoutUpdated);
        }

        void GameOverPage_LayoutUpdated(object sender, EventArgs e)
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

            Leaderboard.ItemsSource = (Application.Current as App).Leaderboard.Entries;

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

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            NavigationService.GoBack();

            base.OnBackKeyPress(e);
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

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
        }
    }
}