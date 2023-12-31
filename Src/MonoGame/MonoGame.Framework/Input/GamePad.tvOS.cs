﻿// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using GameController;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using System;

namespace Microsoft.Xna.Framework.Input
{
    static partial class GamePad
    {
        internal static bool MenuPressed = false;

        private static int PlatformGetMaxIndex()
        {
            return 4;
        }

        private static GamePadCapabilities PlatformGetCapabilities(int index)
        {
            var ind = (GCControllerPlayerIndex)index;
            foreach (var controller in GCController.Controllers)
            {
                if (controller == null)
                    continue;
                if (controller.PlayerIndex == ind)
                    return GetCapabilities(controller);
            }
            return new GamePadCapabilities { IsConnected = false };
        }
               
        private static GamePadState PlatformGetState(int index, GamePadDeadZone deadZoneMode)
        {
            var ind = (GCControllerPlayerIndex)index;


            var buttons = new List<Buttons>();
            bool connected = false;
            ButtonState Up = ButtonState.Released;
            ButtonState Down = ButtonState.Released;
            ButtonState Left = ButtonState.Released;
            ButtonState Right = ButtonState.Released;

            foreach  (var controller in GCController.Controllers) {

                if (controller == null)
                    continue;

                if (controller.PlayerIndex != ind)
                    continue;

                connected = true;
                if (MenuPressed)
                {
                    buttons.Add(Buttons.Back);
                    MenuPressed = false;
                }

                if (controller.ExtendedGamepad != null)
                {
                    if (controller.ExtendedGamepad.ButtonA.IsPressed == true && !buttons.Contains (Buttons.A))
                        buttons.Add(Buttons.A);
                    if (controller.ExtendedGamepad.ButtonB.IsPressed == true && !buttons.Contains (Buttons.B))
                        buttons.Add(Buttons.B);
                    if (controller.ExtendedGamepad.ButtonX.IsPressed == true && !buttons.Contains (Buttons.X))
                        buttons.Add(Buttons.X);
                    if (controller.ExtendedGamepad.ButtonY.IsPressed == true && !buttons.Contains (Buttons.Y))
                        buttons.Add(Buttons.Y);
                    
                    Up = controller.ExtendedGamepad.DPad.Up.IsPressed ? ButtonState.Pressed : ButtonState.Released;
                    Down = controller.ExtendedGamepad.DPad.Down.IsPressed ? ButtonState.Pressed : ButtonState.Released;
                    Left = controller.ExtendedGamepad.DPad.Left.IsPressed ? ButtonState.Pressed : ButtonState.Released;
                    Right = controller.ExtendedGamepad.DPad.Right.IsPressed ? ButtonState.Pressed : ButtonState.Released;
                   
                }
                else if (controller.Gamepad != null)
                {
                    if (controller.Gamepad.ButtonA.IsPressed == true && !buttons.Contains (Buttons.A))
                        buttons.Add(Buttons.A);
                    if (controller.Gamepad.ButtonB.IsPressed == true && !buttons.Contains (Buttons.B))
                        buttons.Add(Buttons.B);
                    if (controller.Gamepad.ButtonX.IsPressed == true && !buttons.Contains (Buttons.X))
                        buttons.Add(Buttons.X);
                    if (controller.Gamepad.ButtonY.IsPressed == true && !buttons.Contains (Buttons.Y))
                        buttons.Add(Buttons.Y);
                    Up = controller.Gamepad.DPad.Up.IsPressed ? ButtonState.Pressed : ButtonState.Released;
                    Down = controller.Gamepad.DPad.Down.IsPressed ? ButtonState.Pressed : ButtonState.Released;
                    Left = controller.Gamepad.DPad.Left.IsPressed ? ButtonState.Pressed : ButtonState.Released;
                    Right = controller.Gamepad.DPad.Right.IsPressed ? ButtonState.Pressed : ButtonState.Released;

                }
                else if (controller.MicroGamepad != null)
                {
                    if (controller.MicroGamepad.ButtonA.IsPressed == true && !buttons.Contains (Buttons.A))
                        buttons.Add(Buttons.A);
                    if (controller.MicroGamepad.ButtonX.IsPressed == true && !buttons.Contains (Buttons.X))
                        buttons.Add(Buttons.X);
                    Up = controller.MicroGamepad.Dpad.Up.IsPressed ? ButtonState.Pressed : ButtonState.Released;
                    Down = controller.MicroGamepad.Dpad.Down.IsPressed ? ButtonState.Pressed : ButtonState.Released;
                    Left = controller.MicroGamepad.Dpad.Left.IsPressed ? ButtonState.Pressed : ButtonState.Released;
                    Right = controller.MicroGamepad.Dpad.Right.IsPressed ? ButtonState.Pressed : ButtonState.Released;
                }
            }
            var state = new GamePadState(
                new GamePadThumbSticks(),
                new GamePadTriggers(),
                new GamePadButtons(buttons.ToArray()),
                new GamePadDPad (Up, Down, Left, Right));
            state.IsConnected = connected;
            return state;
        }

        private static bool PlatformSetVibration(int index, float leftMotor, float rightMotor)
        {
            return false;
        }

        private static GamePadCapabilities GetCapabilities(GCController controller)
        {
            //All iOS controllers have these basics
            var capabilities = new GamePadCapabilities()
                {
                    IsConnected = false,
                    GamePadType = GamePadType.GamePad,
                };
            if (controller.ExtendedGamepad != null)
            {
                capabilities.HasAButton = true;
                capabilities.HasBButton = true;
                capabilities.HasXButton = true;
                capabilities.HasYButton = true;
                capabilities.HasBackButton = true;
                capabilities.HasDPadUpButton = true;
                capabilities.HasDPadDownButton = true;
                capabilities.HasDPadLeftButton = true;
                capabilities.HasDPadRightButton = true;
                capabilities.HasLeftShoulderButton = true;
                capabilities.HasRightShoulderButton = true;
                capabilities.HasLeftTrigger = true;
                capabilities.HasRightTrigger = true;
                capabilities.HasLeftXThumbStick = true;
                capabilities.HasLeftYThumbStick = true;
                capabilities.HasRightXThumbStick = true;
                capabilities.HasRightYThumbStick = true;
            }
            else if (controller.Gamepad != null)
            {
                capabilities.HasAButton = true;
                capabilities.HasBButton = true;
                capabilities.HasXButton = true;
                capabilities.HasYButton = true;
                capabilities.HasBackButton = true;
                capabilities.HasDPadUpButton = true;
                capabilities.HasDPadDownButton = true;
                capabilities.HasDPadLeftButton = true;
                capabilities.HasDPadRightButton = true;
                capabilities.HasLeftShoulderButton = true;
                capabilities.HasRightShoulderButton = true;
            }
            else if (controller.MicroGamepad != null)
            {
                capabilities.HasAButton = true;
                capabilities.HasXButton = true;
                capabilities.HasBackButton = true;
                capabilities.HasDPadUpButton = true;
                capabilities.HasDPadDownButton = true;
                capabilities.HasDPadLeftButton = true;
                capabilities.HasDPadRightButton = true;
            }
            return capabilities;
        }


    }
}
