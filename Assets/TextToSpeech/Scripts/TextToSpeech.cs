using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TextToSpeech.Scripts
{
    public class TextToSpeech : MonoBehaviour
    {
        #region Init

        private static TextToSpeech _instance;

        public static TextToSpeech Instance
        {
            get
            {
                if (_instance == null)
                {
                    Init();
                }
                return _instance;
            }
        }

        private static void Init()
        {
            if (Instance != null) return;
            GameObject obj = new GameObject {name = "TextToSpeech"};
            _instance = obj.AddComponent<TextToSpeech>();
        }

        private void Awake()
        {
            _instance = this;
        }
        #endregion

        public Action onStartCallBack;
        public Action onDoneCallback;
        public Action<string> onSpeakRangeCallback;

        [Range(0, 1)]
        public float rate = 0.5f; //[min - max] Default 0.5
        [Range(0, 2)]
        public float pitch = 1f; //[0.5 - 2] Default 1

        public void Setting(string identifier, float newPitch, float newRate)
        {
            pitch = newPitch;
            rate = newRate;
            
#if UNITY_EDITOR
#elif UNITY_IPHONE
            _TAG_SettingSpeak(identifier, pitch, rate);
#endif
        }
        
        public void StartSpeak(string message)
        {
#if UNITY_EDITOR
            Debug.Log("StartSpeak: " + message);
#elif UNITY_IPHONE
            _TAG_StartSpeak(message);
#endif
        }
        
        public void StopSpeak()
        {
#if UNITY_EDITOR
            Debug.Log("StopSpeak");
#elif UNITY_IPHONE
            _TAG_StopSpeak();
#endif
        }

        public string GetAllVoices()
        {
//#if UNITY_EDITOR
            return TextToSpeechDevData.SpeechData;
//#elif UNITY_IPHONE
//            return _TAG_GetAllVoices();
//#endif
        }

        public void OnSpeechRange(string message)
        {
            if (onSpeakRangeCallback != null && message != null)
            {
                onSpeakRangeCallback(message);
            }
        }
        
        public void OnStart(string message)
        {
            onStartCallBack?.Invoke();
        }
        
        public void OnDone(string message)
        {
            onDoneCallback?.Invoke();
        }
        
        public void OnError(string message)
        {
        }
        
        public void OnMessage(string message)
        {
        }
        
        /** Denotes the language is available for the language by the locale, but not the country and variant. */
        private const int LangAvailable = 0;
        /** Denotes the language data is missing. */
        private const int LangMissingData = -1;
        /** Denotes the language is not supported. */
        private const int LangNotSupported = -2;
        
        public void OnSettingResult(string @params)
        {
            int error = int.Parse(@params);
            string message;
            if (error == LangMissingData || error == LangNotSupported)
            {
                message = "This Language is not supported";
            }
            else
            {
                message = "This Language valid";
            }
            Debug.Log(message);
        }

#if UNITY_IPHONE
        [DllImport("__Internal")]
        private static extern void _TAG_StartSpeak(string message);

        [DllImport("__Internal")]
        private static extern void _TAG_SettingSpeak(string identifier, float pitch, float rate);

        [DllImport("__Internal")]
        private static extern void _TAG_StopSpeak();
#endif
    }
}