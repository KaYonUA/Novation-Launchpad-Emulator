using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace GUI_MIDI
{
    [StructLayout(LayoutKind.Explicit, Size = 20)]
    struct PresetHeader
    {
        [FieldOffset(0)]
        public int[] signature;
        [FieldOffset(12)]
        public int partsCount;
        [FieldOffset(16)]
        public int openedPart;
    }
    class PresetPart
    {
        public KeyMap keyMap;
        public string[] bLabels;

        static public int labelDataSize = 72 * 3;
        static public int keyDataSize = 72 * 4;
        public byte[] partData
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
    class Preset
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
        public Preset(int pCount,int bCount)
        {
            partsCount = pCount;
            buttonsCount = bCount;
            part = new PresetPart[pCount];
            for (int i = 0; i < pCount;i++ )
                part[i] = new PresetPart(bCount);
        }
        private static byte[] RawSerialize(object anything)
        {
            int rawsize = Marshal.SizeOf(anything);
            byte[] rawdata = new byte[rawsize];
            GCHandle handle = GCHandle.Alloc(rawdata, GCHandleType.Pinned);
            Marshal.StructureToPtr(anything, handle.AddrOfPinnedObject(), false);
            handle.Free();
            return rawdata;
        }
        private static T ReadStruct<T>(byte[] data)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            T temp = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return temp;
        }
        public bool loadFromFile(string filename)
        {
            using (var br = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read)))
            {
                int partSize = PresetPart.labelDataSize + PresetPart.keyDataSize;

                byte[] headerData = new byte[20];
                br.Read(headerData, 0, 20);
                PresetHeader header = Preset.ReadStruct<PresetHeader>(headerData);
                this.partsCount = header.partsCount;

                Console.WriteLine("Signature: " + (char)header.signature[0] + (char)header.signature[1] + (char)header.signature[2]);
                Console.WriteLine("partsCount: " + header.partsCount);
                Console.WriteLine("openedPart: " + header.openedPart);

                for (int i = 0; i < this.partsCount; i++)
                {
                    byte[] data = new byte[partSize];
                    br.Read(data, 0, partSize);
                    part[i].partData = data;
                } 
            }
            return true;
        }
        public bool saveToFile(string filename)
        {
            BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.Create));

            PresetHeader header;
            header.signature = new int[3] {'l','e','p'};
            header.openedPart = 0;
            header.partsCount = this.partsCount;

            bw.Write(Preset.RawSerialize(header));

            Console.WriteLine("Header Length: " + Marshal.SizeOf(header));

            for (int i = 0; i < this.partsCount; i++)
                bw.Write(part[i].partData);

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
