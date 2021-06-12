﻿using System;
using System.Drawing;
using KeyboardLedDriver.Common;
using Spectral;

namespace KeyboardLedDriver.Generic
{
    public class GenericLedController : ILedController
    {
        private readonly LedName[] ALERT_KEYS =
        {
            LedName.Escape,
            LedName.F1,
            LedName.F2,
            LedName.F3,
            LedName.F4,
            LedName.F5,
            LedName.F6,
            LedName.F7,
            LedName.F8,
            LedName.F9,
            LedName.F10,
            LedName.F11,
            LedName.F12,
            LedName.PrintScreen,
            LedName.ScrollLock,
            LedName.Pause
        };
        public bool CurrentAlertState { get; }
        public ColorScheme CurrentColorScheme { get; }
        
        public bool IsInitialized { get; private set; }

        public bool Initialize()
        {
            IsInitialized = Led.Initialize();
            return IsInitialized && (Led.CorsairIsEnabled() || Led.LogitechIsEnabled());
        }

        public void SetColorScheme(ColorScheme scheme, bool showAlert)
        {
            if (!IsInitialized) return;

            LedName[] ledsToSet;
            Color color;

            if (!showAlert)
            {
                ledsToSet = new LedName[1] { LedName.Escape };
                color = Color.Chartreuse;
            }
            else
            {
                ledsToSet = ALERT_KEYS;
                color = Color.OrangeRed;
            }

            Led.SetColorForLeds(ledsToSet, color);
        }

        public void ShutDown()
        {
            Led.Shutdown();
        }

        public void ToggleAlert(bool alertState)
        {
            throw new NotImplementedException();
        }
    }
}
