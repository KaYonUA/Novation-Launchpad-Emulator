using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GUI_MIDI
{
    class Presset
    {
        public KeyMap kmap;
        public string[] bLabels;
        public string currentPresetName;
        public Presset()
        {
            kmap = new KeyMap();
            bLabels = new string[72];
            currentPresetName = "";
        }
        public bool loadFromFile(string filename){
            currentPresetName = filename;
            using (var br = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read)))
            {
                for (int i = 0; i < 72; i++)
                {
                    bLabels[i] = Encoding.Default.GetString(br.ReadBytes(3));
                    Console.WriteLine(bLabels[i]);
                }
                ///
                kmap.indexes.Clear();
                kmap.keys.Clear();
                ///
                int[] keys = new int[72];
                for (int i = 0; i < 72; i++)
                {
                    keys[i] = br.ReadInt32();
                }
                for (int i = 0; i < 72; i++)
                {
                    if(keys[i] != 0)
                    {
                        kmap.mapKey(keys[i], i + 1);
                    }
                }
            }
            return true;
        }
        public bool saveToFile(string filename)
        {
            BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.Create));
            for (int i = 0; i < 72; i++)
            {
                byte[] byts;
                if (bLabels[i].Length == 0)
                {
                    byts = new byte[3];
                    bw.Write(byts, 0, 3);
                }
                else if (bLabels[i].Length == 1)
                {
                    byts = new byte[3];
                    byts[0] = Encoding.Default.GetBytes(bLabels[i])[0];
                    bw.Write(byts, 0, 3);
                }
                else if (bLabels[i].Length == 2)
                {
                    byts = new byte[3];
                    byte[] temp;
                    temp = Encoding.Default.GetBytes(bLabels[i]);
                    byts[0] = temp[0];
                    byts[1] = temp[1];
                    bw.Write(byts, 0, 3);
                }
                else if (bLabels[i].Length == 3)
                {
                    byts = Encoding.Default.GetBytes(bLabels[i]);
                    bw.Write(byts, 0, 3);
                }
                
            }
            Dictionary<int, int>.KeyCollection keyValues = kmap.indexes.Keys;
            int[] keys = new int[72];
            foreach (var keyval in keyValues)
                keys[kmap.indexes[keyval]-1] = keyval;
            for (int i = 0; i < 72; i++)
                bw.Write(keys[i]);
            bw.Close();
            return true;
        }
    }
}
