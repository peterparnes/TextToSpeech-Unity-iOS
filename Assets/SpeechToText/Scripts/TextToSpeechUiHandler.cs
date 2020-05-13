using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SpeechToText.Scripts
{
    public class TextToSpeechUiHandler : MonoBehaviour
    {
        public Text textField;
        public Slider rateSlider;
        public Slider pitchSlider;
        public Text rate;
        public Text pitch;
        public Dropdown voicesSelector;
        public Text status; 
    
        private TextToSpeech _textToSpeech;
        private const string Language = VoiceDataManager.Swedish;
        private List<VoiceDataManager.Voice> _voices;
    
        public void Start()
        {
            _textToSpeech = TextToSpeech.Instance;

            VoiceDataManager vdm = new VoiceDataManager();
            _voices = vdm.GetVoicesForLanguage(Language);
            List<Dropdown.OptionData> voicesOptions = 
                _voices.Select(voice => new Dropdown.OptionData(voice.Name)).ToList();

            voicesSelector.options = voicesOptions;
        
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
            Debug.Log(message);
            // SetStatusMessage(status.text += ".");
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
            _textToSpeech.StartSpeak(textField.text);
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

        private void UpdateTextValues()
        {
            rate.text = rateSlider.value.ToString("#.00");
            pitch.text = pitchSlider.value.ToString("#.00");
        }
    }
}
