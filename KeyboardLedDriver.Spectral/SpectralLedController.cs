using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using AutoMapper;
using KeyboardLedDriver.Common;
using Spectral;

namespace KeyboardLedDriver.Spectral
{
    public class GenericLedController : ILedController
    {
        private readonly LedName[] _defaultAlertKeys =
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

        private Mapper _keyMapper;

        public bool CurrentAlertState { get; }

        public ColorScheme CurrentColorScheme { get; }
        
        public bool IsInitialized { get; private set; }

        public GenericLedController()
        {
            CreateKeyMappings();
        }

        public bool Initialize()
        {
            IsInitialized = Led.Initialize();
            return IsInitialized && (Led.CorsairIsEnabled() || Led.LogitechIsEnabled());
        }

        public void SetColorScheme(ColorScheme scheme, bool showAlert)
        {
            if (!IsInitialized) return;

            Color color;

            if (!showAlert)
            {
                color = Color.Green;
            }
            else
            {
                color = Color.Red;
            }

            Led.SetColorForLeds(_defaultAlertKeys, color);
        }

        public bool SetColorForKeys(ColorScheme color, List<string> keys)
        {
            if (keys == null) return false;

            var ledsToSet = new List<LedName>();

            foreach (var key in keys)
            {
                if (Enum.TryParse<LedName>(key, out var led))
                {
                    ledsToSet.Add(led);
                }
            }

            if (ledsToSet.Count == 0) return false;
            
            return Led.SetColorForLeds(ledsToSet, color.R, color.G, color.B);
        }

        public void ShutDown()
        {
            Led.Shutdown();
        }

        public void ToggleAlert(bool alertState)
        {
            throw new NotImplementedException();
        }

        public LedName MapTest(string key)
        {
            return _keyMapper.Map<LedName>(key);
        }

        private void CreateKeyMappings()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<string, LedName>()
                .ConvertUsing((s => Enum.Parse<LedName>(s))));
            _keyMapper = new Mapper(config);
            
                //.ForMember(dest => dest,
                //o =>
                //    o.MapFrom(src => src.Equals(o.ToString()))));
        }
    }
}
