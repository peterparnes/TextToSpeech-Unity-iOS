using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TextToSpeech
{
    public class TextToSpeechBuffer 
    {
        private List<string> _messages;
        private TextToSpeech _textToSpeech;
        private bool _isSpeaking; 

        public TextToSpeechBuffer()
        {
            _messages = new List<string>();
            _textToSpeech = TextToSpeech.Instance;
            _textToSpeech.onStartCallBack += OnStartCallBack;
            _textToSpeech.onDoneCallback += OnDoneCallback;
        }

        private void OnDoneCallback()
        {
            _isSpeaking = false;
            if (_messages.Count > 0)
            {
                _textToSpeech.StartSpeak(_messages.First());
                _messages.RemoveAt(0);
            }
        }

        private void OnStartCallBack()
        {
            _isSpeaking = true;
        }

        public void AddMessage(string message)
        {
            if (!_isSpeaking)
            {
                _textToSpeech.StartSpeak(message);
            }
            else
            {
                _messages.Add(message);
            }
        }

        public void ClearMessages()
        {
            _messages.Clear();
        }
    }
}
