using System;
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
            Debug.Log("Hej");
            _messages = new List<string>();
            _textToSpeech = TextToSpeech.Instance;
            _textToSpeech.onStartCallBack += OnStartCallBack;
            _textToSpeech.onDoneCallback += OnDoneCallback;
            
            _messages.Add("1");
            _messages.Add("2");
            Debug.Log(_messages.Count);
            Debug.Log(_messages.First());
            Debug.Log(_messages.Count);

        }

        private void OnDoneCallback()
        {
            _isSpeaking = false;
            if (_messages.Count > 0)
            {
                _textToSpeech.StartSpeak(_messages.First());
            }
        }

        private void OnStartCallBack()
        {
            _isSpeaking = true;
        }

        private void AddMessage(string message)
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
    }
}
