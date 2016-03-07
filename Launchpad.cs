using NAudio.Midi;

namespace GUI_MIDI
{
    class Launchpad
    {
        private MidiOut midiout;

        public readonly int mainPadButtonsCount = 64;
        public readonly int rightPadButtonsCount = 8;
        private static readonly int lColumnMax = 67;
        private static readonly int rColumnMax = 99;

        private int[] mainPad;
        private bool[] noteStatus;

        public Launchpad(int deviceID)
        {
            midiout = new MidiOut(deviceID);

            int[] leftColumn = new int[32];
            int[] rightColumn = new int[32];

            for (int i = 0; i < 32; i++)
                leftColumn[i] = i + 36;
            for (int i = 0; i < 32; i++)
                rightColumn[i] = i + 68;

            mainPad = new int[72];
            for (int i = 0, l = 0; i < 8; i++)
            {
                for (int z = 3; z >= 0; z--, l++)
                    mainPad[l] = leftColumn[31 - (z + (i * 4))];
                for (int z = 3; z >= 0; z--, l++)
                    mainPad[l] = rightColumn[31 - (z + (i * 4))];
            }

            for (int i = 0; i < rightPadButtonsCount; i++)
            {
                mainPad[64+i] = 100 + i;
            }
            noteStatus = new bool[72];
        }

        private bool checkButton(int buttonNo)
        {
            if (buttonNo >= 0 && buttonNo <= 72)
                return true;
            else
                return false;
        }

        public int getButtonNote(int buttonNo)
        {
            if (buttonNo >= 0 && buttonNo <= 71)
                return mainPad[buttonNo];
            else
                return -1;
        }

        static public int getButtonNo(int note)
        {
            //if (note == lColumnMax)
            //    return 3;
            //if (note == rColumnMax)
            //    return 63;
            if (note <= lColumnMax)
                return 8 * ((lColumnMax - note) / 4) + (3 - ((lColumnMax - note) % 4));
            else if (note <= rColumnMax)
                return 8 * ((rColumnMax - note) / 4) + (7 - ((rColumnMax - note) % 4));
            else
                if (note <= 107)
                    return note - 36;
                else 
                    return -1;
        }

        public void startPlayingNote(int buttonNo)
        {
            if (checkButton(buttonNo) && noteStatus[buttonNo] == false)
            {
                // while(true)
                midiout.Send(MidiMessage.StartNote(getButtonNote(buttonNo), 127, 5).RawData);
                //Program.port.sendCommand(BitConverter.GetBytes(MidiMessage.StartNote(getButtonNote(buttonNo), 127, 1).RawData));
                //Console.WriteLine("Note: " + getButtonNote(buttonNo) + " start play.");
                noteStatus[buttonNo] = true;
            }
        }
        public void stopPlayingNote(int buttonNo)
        {
            if (checkButton(buttonNo) && noteStatus[buttonNo] == true)
            {
                midiout.Send(MidiMessage.StopNote(getButtonNote(buttonNo), 0, 5).RawData);
                //Program.port.sendCommand(BitConverter.GetBytes(MidiMessage.StopNote(getButtonNote(buttonNo), 0, 1).RawData));
                //Console.WriteLine("Note: " + getButtonNote(buttonNo) + " stop play.");
                noteStatus[buttonNo] = false;
            }
        }
    }
}
