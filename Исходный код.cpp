#pragma comment(lib,"winmm.lib")
#include <iostream>
#include <vector>
#include <thread>
#include <cstdlib>
#include <Windows.h>
#include <conio.h>
#include "RtMidi.h"
using namespace std;

int main(){
	RtMidiOut *midiout = 0;
	std::string portName;
	std::vector<unsigned char> message;
	// RtMidiOut constructor
	try {
		midiout = new RtMidiOut(RtMidi::WINDOWS_MM);
	}
	catch (RtMidiError &error) {
		error.printMessage();
		exit(EXIT_FAILURE);
	}
	int notes[64];
	for (int i = 0; i < 32;i++)
		notes[i] = 67-i;
	for (int i = 32; i < 64; i++)
		notes[i] = 99-(i-32);
	midiout->openPort(1);

	message.push_back(144);
	message.push_back(0);
	message.push_back(90);
	int i = 0;
	while (true){
		switch (getch()){
		case 'a':
			i = 67;
			break;
		case 'b':
			i = 79;
			break;
		default:
			break;
		}
		message[0] = 144;
		message[1] = i;
		message[2] = 90;
		midiout->sendMessage(&message);

		std::cout << "Note On | ";
		Sleep(20);
		std::cout << "Note Off | ";

		message[0] = 128;
		message[1] = i;
		message[2] = 40;
		midiout->sendMessage(&message);
	}
	delete midiout;
	system("pause");
	return 0;
}