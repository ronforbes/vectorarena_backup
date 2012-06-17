using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;

namespace VectorArena
{
    public class Settings
    {
        IsolatedStorageSettings settings;

        const string SoundEnabledKey = "SoundEnabled";
        const string MusicEnabledKey = "MusicEnabled";
        const string VolumeKey = "Volume";

        const bool SoundEnabledDefault = true;
        const bool MusicEnabledDefault = true;
        const int VolumeDefault = 10;

        public const float MaxVolume = 10.0f;

        public Settings()
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
        }

        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(Key))
            {
                // If the value has changed
                if (settings[Key] != value)
                {
                    // Store the new value
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (settings.Contains(Key))
            {
                value = (T)settings[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        public void Save()
        {
            settings.Save();
        }

        public bool SoundEnabled
        {
            get { return GetValueOrDefault<bool>(SoundEnabledKey, SoundEnabledDefault); }
            set { if (AddOrUpdateValue(SoundEnabledKey, value)) Save(); }
        }

        public string SoundEnabledContent
        {
            get { return SoundEnabled ? "On" : "Off"; }
        }

        public bool MusicEnabled
        {
            get { return GetValueOrDefault<bool>(MusicEnabledKey, MusicEnabledDefault); }
            set { if (AddOrUpdateValue(MusicEnabledKey, value)) Save(); }
        }

        public string MusicEnabledContent
        {
            get { return MusicEnabled ? "On" : "Off"; }
        }

        public int Volume
        {
            get
            {
                return GetValueOrDefault<int>(VolumeKey, VolumeDefault);
            }
            set
            {
                if (AddOrUpdateValue(VolumeKey, value))
                {
                    Save();
                }
            }
        }
    }
}
