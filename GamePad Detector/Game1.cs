using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GamePad_Detector
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        GamePadState gamepad1State,gamepad2State;
        JoystickState joystick1State, joystick2State;

        string connectedControllers, buttonsPressed;

        Vector2 leftStick, rightStick;
        int[] axes; // For joystick values of Joystick range from

        SpriteFont infoFont;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            infoFont = Content.Load<SpriteFont>("InfoFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            DetectControllers();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(infoFont, connectedControllers + buttonsPressed, new Vector2(10, 10), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DetectControllers()
        {
            connectedControllers = "";
            buttonsPressed = "";

            // Detects a Gamepad in Player 1
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                UpdateGamePad1();
                connectedControllers += $"Gamepad 1 Detected\nButtons: ";
                leftStick = gamepad1State.ThumbSticks.Left;
                rightStick = gamepad1State.ThumbSticks.Right;
                foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
                {
                    if (gamepad1State.IsButtonDown(button))
                    {
                        buttonsPressed += $" {button.ToString()} ";
                    }
                }
                buttonsPressed += $"\nLeft Joystick {leftStick.ToString()}";
                buttonsPressed += $"\nRight Joystick {rightStick.ToString()}";
                connectedControllers += buttonsPressed + "\n";
            }
            buttonsPressed = "";

            // Jorstick Detection in Player 1
            if (Joystick.GetState(0).IsConnected)
            {
                UpdateJoystick1();
                connectedControllers += $"\nJoystick 1 Detected\nButtons: ";
                for (int i = 0; i < joystick1State.Buttons.Length; i++)
                {
                    if (joystick1State.Buttons[i] == ButtonState.Pressed)
                        buttonsPressed += $" {i} ";
                }
                buttonsPressed += "\n";

                // Detect Joystick direction on Joystick
                axes = joystick1State.Axes;
                for (int i = 0; i < axes.Length; i++)
                {
                    buttonsPressed += $"Axis {i}: {NormalizeAxisValue(axes[i])}\n"; // Note y-axis (1 and 3) are inverted
                }

                connectedControllers += buttonsPressed + "\n";
            }
            buttonsPressed = "";

            if (GamePad.GetState(PlayerIndex.Two).IsConnected)
            {
                UpdateGamePad2();
                connectedControllers += $"Gamepad 2 Detected\nButtons: ";
                leftStick = gamepad2State.ThumbSticks.Left;
                rightStick = gamepad2State.ThumbSticks.Right;
                foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
                {
                    if (gamepad2State.IsButtonDown(button))
                    {
                        buttonsPressed += $" {button.ToString()} ";
                    }
                }
                buttonsPressed += $"\nLeft Joystick {leftStick.ToString()}";
                buttonsPressed += $"\nRight Joystick {rightStick.ToString()}";
                connectedControllers += buttonsPressed + "\n";
            }
            buttonsPressed = "";

            if (Joystick.GetState(1).IsConnected)
            {
                UpdateJoystick2();
                connectedControllers += $"\nJoystick 2 Detected\nButtons: ";
                for (int i = 0; i < joystick2State.Buttons.Length; i++)
                {
                    if (joystick2State.Buttons[i] == ButtonState.Pressed)
                        buttonsPressed += $" {i} ";
                }
                buttonsPressed += "\n";

                // Detect Joystick direction on Joystick
                axes = joystick2State.Axes;
                for (int i = 0; i < axes.Length; i++)
                {
                    buttonsPressed += $"Axis {i}: {NormalizeAxisValue(axes[i])}\n";
                }

                connectedControllers += buttonsPressed + "\n";
                buttonsPressed = "";
            }

        }
        public void UpdateGamePad1()
        {
            gamepad1State = GamePad.GetState(PlayerIndex.One);
        }

        public void UpdateGamePad2()
        {

            gamepad2State = GamePad.GetState(PlayerIndex.Two);
        }

        public void UpdateJoystick1()
        {
            joystick1State = Joystick.GetState(0);

        }
        public void UpdateJoystick2()
        {
            joystick2State = Joystick.GetState(1);
        }

        // Make Axis value be from -1 to 1 instead of -32767 to 32767
        float NormalizeAxisValue(int value)
        {
            return value / 32767f;
        }

    }
}
