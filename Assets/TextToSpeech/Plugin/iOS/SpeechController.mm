#import "AVFoundation/AVFoundation.h"

@interface SpeechController : NSObject 
@end 

@interface SpeechController () <AVSpeechSynthesizerDelegate>
{
    AVSpeechSynthesizer *speechSynthesizer;
    NSString * speakText;
    NSString * identifier;
    float pitch;
    float rate;
}
@end

@implementation SpeechController

- (id)init
{
    self = [super init];
    speechSynthesizer = [[AVSpeechSynthesizer alloc] init];
    speechSynthesizer.delegate = self;
    return self;
}

- (void)SettingSpeak: (const char *) _identifier pitchSpeak: (float)_pitch rateSpeak:(float)_rate
{
    identifier = [NSString stringWithUTF8String:_identifier];
    pitch = _pitch;
    rate = _rate;
    UnitySendMessage("TextToSpeech", "OnMessage", "Setting Success");
}

- (void)StartSpeak: (const char *) _text
{
    if([speechSynthesizer isSpeaking] == false) {
        speakText = [NSString stringWithUTF8String:_text];
        // NSLog(@"%@", speakText);
        AVSpeechUtterance *utterance = [[AVSpeechUtterance alloc] initWithString:speakText];
        utterance.voice = [AVSpeechSynthesisVoice voiceWithIdentifier:identifier];
        utterance.pitchMultiplier = pitch;
        utterance.rate = rate;
        
        // Use defaults for now
        // utterance.preUtteranceDelay = 0.2f;
        // utterance.postUtteranceDelay = 0.2f;

        [speechSynthesizer speakUtterance:utterance];
    }
}

- (void)StopSpeak
{
    if([speechSynthesizer isSpeaking]) {
        [speechSynthesizer stopSpeakingAtBoundary:AVSpeechBoundaryImmediate];
    }
}

- (void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer
willSpeakRangeOfSpeechString:(NSRange)characterRange
                utterance:(AVSpeechUtterance *)utterance
{
    NSString *subString = [speakText substringWithRange:characterRange];
    UnitySendMessage("TextToSpeech", "OnSpeechRange", [subString UTF8String]);
}

- (void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer
didStartSpeechUtterance:(AVSpeechUtterance *)utterance
{
    UnitySendMessage("TextToSpeech", "OnStart", "onStart");
}

- (void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer
 didFinishSpeechUtterance:(AVSpeechUtterance *)utterance
{
    UnitySendMessage("TextToSpeech", "OnDone", "onDone");
}

@end

extern "C" {
    SpeechController *su = [[SpeechController alloc] init];
    
    void _TAG_StartSpeak(const char * _text){
        [su StartSpeak:_text];
    }
    
    void _TAG_StopSpeak(){
        [su StopSpeak];
    }
    
    void _TAG_SettingSpeak(const char * _identifier, float _pitch, float _rate){
        [su SettingSpeak:_identifier pitchSpeak:_pitch rateSpeak:_rate];
    }

}
