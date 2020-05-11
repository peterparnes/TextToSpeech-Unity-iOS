using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SpeechToText.Scripts
{
    public class VoiceDataManager
    {
        public const string Swedish = "sv-SE";
        public const string English = "en-US";
    
        public readonly struct Voice
        {
            public Voice(string language, string name, string identifier, int quality)
            {
                Language = language;
                Identifier = identifier;
                Name = name;
                Quality = quality;
            }

            public string Language { get; }
            public string Identifier { get; }
            public string Name { get; }
            public int Quality { get; }
        
            public override string ToString() => $"({Language}, {Identifier}, {Name}, {Quality})";
        }

        private Voice[] _voices;

        public VoiceDataManager()
        {
            ParseVoiceData();
        }

        public List<Voice> GetVoicesForLanguage(string language)
        {
            var result = new List<Voice>();
            foreach (var voice in _voices)
            {
                if (voice.Language.Equals(language))
                {
                    result.Add(voice);
                }
            }
        
            Debug.Log(result);

            return result;
        }

        private void ParseVoiceData()
        {
            string[] data = Regex.Split(TextToSpeech.Instance.GetAllVoices(),"XYX");
            _voices = new Voice[data.Length/4];
            int count = 0;
            for (int i = 0; i < data.Length-3; i += 4)
            {
                Voice voice = new Voice(data[i], data[i + 1], data[i + 2], short.Parse(data[i+3]));
                _voices[count++] = voice;
            }
        }
    }
}
