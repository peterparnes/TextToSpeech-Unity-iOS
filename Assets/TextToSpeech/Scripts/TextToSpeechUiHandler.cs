using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TextToSpeech
{
    public class TextToSpeechUiHandler : MonoBehaviour
    {
        public Text textField;
        public Slider rateSlider;
        public Slider pitchSlider;
        public Text rate;
        public Text pitch;
        public Dropdown languagesSelector;
        public Dropdown voicesSelector;
        public Text status; 
    
        private TextToSpeech _textToSpeech;
        private const string DefaultLanguage = VoiceDataManager.Swedish;
        private List<VoiceDataManager.Voice> _voices;
        private TextToSpeechBuffer _buffer;
        private IEnumerable<string> _languages;
        private VoiceDataManager _vdm;
    
        public void Start()
        {
            _textToSpeech = TextToSpeech.Instance;
            _buffer = new TextToSpeechBuffer(); 

            _vdm = new VoiceDataManager();

            _languages = _vdm.GetAllLanguages();
            var languagesDropdown = new List<Dropdown.OptionData>();
            int initialIndex = 0;
            int index = 0;
            foreach (var language in _languages)
            {
                languagesDropdown.Add(new Dropdown.OptionData(language));
                if (language == DefaultLanguage)
                {
                    initialIndex = index;
                }
                index++;
            }
            languagesSelector.options = languagesDropdown;
            languagesSelector.SetValueWithoutNotify(initialIndex);

            PopulateVoices(DefaultLanguage);
        
            rateSlider.value = _textToSpeech.rate;
            pitchSlider.value = _textToSpeech.pitch;
        
            OnPitchAndRateChanged();

            SetStatusMessage("");
        
            _textToSpeech.onStartCallBack += OnStartCallBack;
            _textToSpeech.onSpeakRangeCallback += OnSpeakRangeCallback;
            _textToSpeech.onDoneCallback += OnDoneCallback;
        }
        
        private void OnDoneCallback()
        {
            SetStatusMessage("");
        }

        private void OnSpeakRangeCallback(string message)
        {
            SetStatusMessage("Speaking: " + message);
        }

        private void OnStartCallBack()
        {
            SetStatusMessage("Speaking");
        }

        public void SetStatusMessage(string message)
        {
            status.text = message;
        }
 
        public void OnSpeak()
        {
            _buffer.AddMessage(textField.text);
        }

        public void OnStopSpeak()
        {
            _textToSpeech.StopSpeak();
        }

        public void OnPitchAndRateChanged()
        {
            _textToSpeech.Setting(_voices[voicesSelector.value].Identifier, pitchSlider.value, rateSlider.value);
            UpdateTextValues();
        }

        public void OnLanguageChanged()
        {
            PopulateVoices(GetCurrentLanguage());
            OnPitchAndRateChanged();
        }
        
        private string GetCurrentLanguage()
        {
            return _languages.ElementAt(languagesSelector.value);
        }

        private void PopulateVoices(string language)
        {
            _voices = _vdm.GetVoicesForLanguage(language);
            List<Dropdown.OptionData> voicesOptions = 
                _voices.Select(voice => new Dropdown.OptionData(voice.Name)).ToList();
            voicesSelector.options = voicesOptions;
        }

        private void UpdateTextValues()
        {
            rate.text = rateSlider.value.ToString("#.00");
            pitch.text = pitchSlider.value.ToString("#.00");
        }
    }
}
