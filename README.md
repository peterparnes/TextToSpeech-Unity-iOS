# Text to speech on iOS from Unity 
This is a major restructure of the original repository. This one is focused on only speech to text for iOS and includes an updated example UI to show and select voices. It is also targeted towards Swedish but language can easily can changed in the code. The UI support changing voice, rate och pitch and also callback from native side with info if we are speaking or not. 

## Native Speech and Text
* TextToSpeech iOS: https://developer.apple.com/reference/avfoundation
* Install new voices via settings->Accessibility->Spoken Content->Voices

Note. There is a bug in iOS14 (up to at least 14.0.1) where the names are empty. Fix: Open your settings as above and open your corresponding language. If there is a voice with an empty name then select it. For me, it disappeared and then the voice names works again. 

## Xcode
* Tested with XCode 12 and iOS 14.0
* No need to add any framework  

## Contact 
Peter Parnes, peter@parnes.com
