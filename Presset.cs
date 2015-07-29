using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GUI_MIDI
{
    struct PresetPart
    {
        public KeyMap keyMap;
        public string[] bLabels;

        static public int labelDataSize = 72 * 3;
        static public int keyDataSize = 72 * 4;
        public byte[] packData
        {
            get
            {
                byte[] data = new byte[labelDataSize + keyDataSize];
                int dIndex = 0;

                for (int i = 0; i < 72; i++)
                {
                    byte[] temp;
                    switch(bLabels[i].Length)
                    {
                        case 0:
                            data[dIndex] = 0;
                            data[dIndex + 1] = 0;
                            data[dIndex + 2] = 0;
                            break;
                        case 1:
                            data[dIndex] = Encoding.Default.GetBytes(bLabels[i])[0];
                            data[dIndex + 1] = 0;
                            data[dIndex + 2] = 0;
                            break;
                        case 2:
                            temp = Encoding.Default.GetBytes(bLabels[i]);
                            data[dIndex] = temp[0];
                            data[dIndex + 1] = temp[1];
                            data[dIndex + 2] = 0;
                            break;
                        case 3:
                            temp = Encoding.Default.GetBytes(bLabels[i]);
                            data[dIndex] = temp[0];
                            data[dIndex + 1] = temp[1];
                            data[dIndex + 2] = temp[2];
                            break;
                        default:
                            break;
                    }
                    dIndex += 3;
                }

                Dictionary<int, int>.KeyCollection keyValues = keyMap.indexes.Keys;
                int[] keys = new int[72];
                foreach (var keyval in keyValues)
                    keys[keyMap.indexes[keyval] - 1] = keyval;

                byte[] buffer;
                foreach (var keyValue in keys)
                {
                    buffer = BitConverter.GetBytes(keyValue);

                    data[dIndex] = buffer[0];
                    data[dIndex + 1] = buffer[1];
                    data[dIndex + 2] = buffer[2];
                    data[dIndex + 3] = buffer[3]; 

                    dIndex += 4;
                }

                return data;
            }
            set
            {
                keyMap.indexes.Clear();
                keyMap.keys.Clear();

                int dIndex = 0;
                byte[] buffer = new byte[3];
                for (int i = 0; i < 72; i++)
                {
                    buffer[0] = value[dIndex];
                    buffer[1] = value[dIndex + 1];
                    buffer[2] = value[dIndex + 2];

                    bLabels[i] = Encoding.Default.GetString(buffer);
                    Console.WriteLine(bLabels[i]);

                    dIndex += 3;
                }
                

                int keyValue;
                buffer = new byte[4];
                for (int i = 0; i < 72; i++)
                {
                    buffer[0] = value[dIndex];
                    buffer[1] = value[dIndex + 1];
                    buffer[2] = value[dIndex + 2];
                    buffer[3] = value[dIndex + 3];

                    keyValue = BitConverter.ToInt32(buffer, 0);
                    if (keyValue != 0)
                        keyMap.mapKey(keyValue, i + 1);

                    dIndex += 4;
                }
            }
        }
        public PresetPart(int buttonsCount)
        {
            keyMap = new KeyMap();
            bLabels = new string[buttonsCount];
            for (int i = 0; i < buttonsCount; i++)
                bLabels[i] = " ";
        }
        
        
    }
    class Presset
    {
        PresetPart[] part;
        int pIndex;
        public int buttonsCount{ get; private set; }
        public int partsCount{ get; private set; }
        public PresetPart currentPart { get { return part[pIndex]; } }
        public PresetPart this[int index]
        {
            get { return part[index]; }
        }
        public Presset(int pCount,int bCount)
        {
            partsCount = pCount;
            buttonsCount = bCount;
            part = new PresetPart[pCount];
            for (int i = 0; i < pCount;i++ )
                part[i] = new PresetPart(bCount);
        }
        public bool loadFromFile(string filename)
        {
            using (var br = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read)))
            {
                int partSize = PresetPart.labelDataSize + PresetPart.keyDataSize;
                for (int i = 0; i < this.partsCount; i++)
                {
                    byte[] data = new byte[partSize];
                    br.Read(data, 0, partSize);
                    part[i].packData = data;
                } 
            }
            return true;
        }
        public bool saveToFile(string filename)
        {
            BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.Create));

            for (int i = 0; i < this.partsCount; i++)
                bw.Write(part[i].packData);

            bw.Close();
            return true;
        }
        public void selectPart(int partIndex)
        {
            pIndex = partIndex;
        }
        public PresetPart getPart(int index){
            if (index < partsCount)
                return part[index];
            else
                return new PresetPart(0);
        }
    }
}
