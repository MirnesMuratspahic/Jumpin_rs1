import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class VoiceRecognitionService {
  recognition: any;
  isListening = false;

  constructor() {
    const { webkitSpeechRecognition }: any = window as any;
    if (webkitSpeechRecognition) {
      this.recognition = new webkitSpeechRecognition();
      this.recognition.lang = 'hr-HR';
      this.recognition.interimResults = false;
      this.recognition.maxAlternatives = 1;
    } else {
      console.error('Web Speech API nije podržan u ovom pretraživaču.');
    }
  }

  startListening(callback: (transcript: string) => void) {
    if (this.recognition) {
      this.isListening = true;
      this.recognition.start();
  
      this.recognition.onresult = (event: any) => {
        const transcript = event.results[0][0].transcript; // Dobijanje transkripta
        callback(transcript); // Slanje nazad u komponentu
      };
  
      this.recognition.onerror = (event: any) => {
        console.error('Error in voice recognition:', event.error);
      };
  
      this.recognition.onend = () => {
        this.isListening = false;
      };
    }
  }

  stopListening() {
    if (this.recognition && this.isListening) {
      this.recognition.stop();
      this.isListening = false;
    }
  }
}
