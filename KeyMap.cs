using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_MIDI
{
    class KeyMap
    {
        public Dictionary<int, int> indexes;
        public Dictionary<int, int> keys;
        public KeyMap()
        {
            indexes = new Dictionary<int, int>();
            keys = new Dictionary<int, int>();
        }
        public bool mapKey(int keyValue, int buttonNo)
        {
            if (indexes.ContainsKey(keyValue) != true)
            {
                if(keys.ContainsKey(buttonNo))
                    resetKey(keys[buttonNo]);
                indexes.Add(keyValue, buttonNo);
                keys.Add(buttonNo, keyValue);
                return true;
            }
            else
                return false;
        }
        public void resetKey(int key)
        {
            keys.Remove(indexes[key]);
            indexes.Remove(key);
        }
        public void resetButton(int buttonNo)
        {
            indexes.Remove(keys[buttonNo]);
            keys.Remove(buttonNo);
        }
        public int getNote(int keyValue)
        {
            if (indexes.ContainsKey(keyValue))
                return indexes[keyValue] - 1;
            else
                return -1;
        }
        public int getKeyValue(int buttonNo)
        {
            if (keys.ContainsKey(buttonNo))
                return keys[buttonNo];
            else
                return -1;
        }

    }
}
