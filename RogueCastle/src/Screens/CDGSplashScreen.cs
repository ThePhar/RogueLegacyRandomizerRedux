using System;
using System.Threading;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;
using RogueCastle.Managers;
using RogueCastle.Screens.BaseScreens;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Screens;

public class CDGSplashScreen : Screen
{
    private bool _fadingOut;
    private bool _levelDataLoaded;
    private TextObj _loadingText;
    private SpriteObj _logo;
    private float _totalElapsedTime;

    private RCScreenManager RCScreenManager => ScreenManager as RCScreenManager;
    private Game Game => ScreenManager.Game as Game;

    public override void LoadContent()
    {
        _logo = new SpriteObj("CDGLogo_Sprite")
        {
            Position = new Vector2(1320 / 2, 720 / 2),
            Rotation = 90,
            ForceDraw = true,
        };

        _loadingText = new TextObj(Game.JunicodeFont)
        {
            FontSize = 18,
            Align = Types.TextAlign.Right,
            TextureColor = new Color(100, 100, 100),
            Position = new Vector2(1320 - 40, 720 - 90),
            ForceDraw = true,
            Opacity = 0,
        };
        _loadingText.Text = "LOC_ID_SPLASH_SCREEN_1".GetString(_loadingText);

        base.LoadContent();
    }

    public override void OnEnter()
    {
        // Level data loading is multithreaded.
        _levelDataLoaded = false;
        _fadingOut = false;
        var loadingThread = new Thread(LoadLevelData);
        loadingThread.Start();

        _logo.Opacity = 0;
        Tween.To(_logo, 1, Linear.EaseNone, "delay", "0.5", "Opacity", "1");
        Tween.RunFunction(0.75f, typeof(SoundManager), "PlaySound", "CDGSplashCreak");
        base.OnEnter();
    }

    private void LoadLevelData()
    {
        lock (this)
        {
            LevelBuilder2.Initialize();
            LevelParser.ParseRooms("Map_1x1", ScreenManager.Game.Content);
            LevelParser.ParseRooms("Map_1x2", ScreenManager.Game.Content);
            LevelParser.ParseRooms("Map_1x3", ScreenManager.Game.Content);
            LevelParser.ParseRooms("Map_2x1", ScreenManager.Game.Content);
            LevelParser.ParseRooms("Map_2x2", ScreenManager.Game.Content);
            LevelParser.ParseRooms("Map_2x3", ScreenManager.Game.Content);
            LevelParser.ParseRooms("Map_3x1", ScreenManager.Game.Content);
            LevelParser.ParseRooms("Map_3x2", ScreenManager.Game.Content);
            LevelParser.ParseRooms("Map_Special", ScreenManager.Game.Content);
            LevelParser.ParseRooms("Map_DLC1", ScreenManager.Game.Content, true);
            LevelBuilder2.IndexRoomList();
            _levelDataLoaded = true;
        }
    }

    public void LoadNextScreen()
    {
        if (Game.SaveManager.FileExists(SaveType.PlayerData))
        {
            Game.SaveManager.LoadFiles(null, SaveType.PlayerData);
        }

        RCScreenManager.DisplayScreen(ScreenType.TITLE, true);
    }

    public override void Update(GameTime gameTime)
    {
        if (_levelDataLoaded == false && _logo.Opacity == 1)
        {
            var opacity = (float)Math.Abs(Math.Sin(_totalElapsedTime));
            _totalElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _loadingText.Opacity = opacity;
        }

        if (_levelDataLoaded && _fadingOut == false)
        {
            _fadingOut = true;
            var logoOpacity = _logo.Opacity;
            _logo.Opacity = 1;
            Tween.To(_logo, 1, Linear.EaseNone, "delay", "1.5", "Opacity", "0");
            Tween.AddEndHandlerToLastTween(this, "LoadNextScreen");
            Tween.To(_loadingText, 0.5f, Tween.EaseNone, "Opacity", "0");
            _logo.Opacity = logoOpacity;
        }

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Camera.GraphicsDevice.Clear(Color.Black);
        Camera.Begin();
        _logo.Draw(Camera);
        _loadingText.Draw(Camera);
        Camera.End();
        base.Draw(gameTime);
    }

    public override void Dispose()
    {
        if (IsDisposed == false)
        {
            Console.WriteLine("Disposing CDG Splash Screen");

            _logo.Dispose();
            _logo = null;
            _loadingText.Dispose();
            _loadingText = null;
            base.Dispose();
        }
    }
}
