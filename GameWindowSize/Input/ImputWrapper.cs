using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace InputWrapper
{
    internal struct AllInputButtons
    {
        private const Keys kA_ButtonKey = Keys.K;
        private const Keys kB_ButtonKey = Keys.L;
        private const Keys kX_ButtonKey = Keys.J;
        private const Keys kY_ButtonKey = Keys.I;
        private const Keys kBack_ButtonKey = Keys.F1;
        private const Keys kStart_ButtonKey = Keys.F2;

        private ButtonState GetState(ButtonState gamePadButtonState, Keys key)
        {
            if (Keyboard.GetState().IsKeyDown(key))
                return ButtonState.Pressed;
            if ((GamePad.GetState(PlayerIndex.One).IsConnected))
                return gamePadButtonState;
            return ButtonState.Released;
        }

        public ButtonState A {
            get { return GetState(GamePad.GetState(PlayerIndex.One).Buttons.A, kA_ButtonKey); } }
        public ButtonState B {
            get { return GetState(GamePad.GetState(PlayerIndex.One).Buttons.B, kB_ButtonKey); } }
        public ButtonState Back {
            get { return GetState(GamePad.GetState(PlayerIndex.One).Buttons.Back, kBack_ButtonKey); } }
        public ButtonState Start {
            get { return GetState(GamePad.GetState(PlayerIndex.One).Buttons.Start, kStart_ButtonKey); } }
         public ButtonState X {
            get { return GetState(GamePad.GetState(PlayerIndex.One).Buttons.X, kX_ButtonKey); } }
        public ButtonState Y {
            get { return GetState(GamePad.GetState(PlayerIndex.One).Buttons.Y, kY_ButtonKey); } }
    }


    internal struct AllInputTriggers
    {
        private const Keys kLeftTrigger = Keys.N;
        private const Keys kRightTrigger = Keys.M;
        const float kKeyTriggerValue = 0.75f;

        private float GetTriggerState(float gamePadTrigger, Keys key)
        {
            if (Keyboard.GetState().IsKeyDown(key))
                return kKeyTriggerValue;
            if ((GamePad.GetState(PlayerIndex.One).IsConnected))
                return gamePadTrigger;
            return 0f; // nenhuma tecla pressionada
        }

        public float Left{
            get { return GetTriggerState( GamePad.GetState(PlayerIndex.One).Triggers.Left, kLeftTrigger); } }
        public float Right{
            get { return GetTriggerState( GamePad.GetState(PlayerIndex.One).Triggers.Right, kRightTrigger); } }
    }


    internal struct AllThumbSticks
    {
        const Keys kLeftThumbStickUp = Keys.W;
        const Keys kLeftThumbStickDown = Keys.S;
        const Keys kLeftThumbStickLeft = Keys.A;
        const Keys kLeftThumbStickRight = Keys.D;
        const Keys kRightThumbStickUp = Keys.Up;
        const Keys kRightThumbStickDown = Keys.Down;
        const Keys kRightThumbStickLeft = Keys.Left;
        const Keys kRightThumbStickRight = Keys.Right;
        const float kKeyDownValue = 0.75f;
        private Vector2 ThumbStickState(Vector2 thumbStickValue,
         Keys up, Keys down, Keys left, Keys right)
        {
            Vector2 r = new Vector2(0f, 0f);
            if ((GamePad.GetState(PlayerIndex.One).IsConnected))
            {
                r = thumbStickValue;
            }
            if (Keyboard.GetState().IsKeyDown(up))
                r.Y += kKeyDownValue;
            if (Keyboard.GetState().IsKeyDown(down))
                r.Y -= kKeyDownValue;
            if (Keyboard.GetState().IsKeyDown(left))
                r.X -= kKeyDownValue;
            if (Keyboard.GetState().IsKeyDown(right))
                r.X += kKeyDownValue;
            return r;
        }
        public Vector2 Left
        {
            get
            {
                return ThumbStickState(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left, kLeftThumbStickUp, kLeftThumbStickDown, kLeftThumbStickLeft, kLeftThumbStickRight);
            }
        }
        public Vector2 Right
        {
            get
            {
                return ThumbStickState(GamePad.GetState(PlayerIndex.One).ThumbSticks.Right, kRightThumbStickUp, kRightThumbStickDown, kRightThumbStickLeft, kRightThumbStickRight);
            }
        }
    }

    static class InputWrapper
    {
        static public AllInputButtons Buttons = new AllInputButtons();
        static public AllThumbSticks ThumbSticks = new AllThumbSticks();
        static public AllInputTriggers Triggers = new AllInputTriggers();
    }
}







