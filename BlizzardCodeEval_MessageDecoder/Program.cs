using System;
using System.IO;
using System.Collections.Generic;

namespace BlizzardCodeEval_MessageDecoder
{
    class Program
    {
        static void Main(string[] args)
        {
            List<String> keyList = new List<String>();
            InitKeyList(keyList);

            using (StreamReader reader = File.OpenText(args[0]))
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (null == line) continue;

                    //split line into header and message portions
                    int messageStart = line.IndexOf('0');
                    if (messageStart == 0) messageStart = line.IndexOf('1');

                    string header = line.Substring(0, messageStart);
                    string message = line.Substring(messageStart);

                    List<string> keys = new List<string>();
                    int keyLength = 0;

                    do
                    {
                        //get length indicator
                        keyLength = Convert.ToInt32(message.Substring(0, 3), 2);

                        //message is done, move on to the next
                        if (keyLength == 0) continue;

                        //get segment end string
                        string segmentEndIndicator = String.Empty.PadLeft(keyLength, '1');
                        int segmentEndIndex = 0;

                        //i = 3 so that we skip the length indicator portion of the segment
                        for (int i = 3; i < message.Length; i += keyLength)
                        {
                            string key = message.Substring(i, keyLength);
                            if (key == segmentEndIndicator) 
                            {
                                segmentEndIndex = i;
                                break;
                            }
                            else
                            {
                                keys.Add(key);
                            }
                        }

                        //remove the segment just processed from the message
                        message = message.Substring(segmentEndIndex + segmentEndIndicator.Length);

                    } while (keyLength > 0);


                    string decodedMessage = string.Empty;
                    foreach (string key in keys)
                    {
                        decodedMessage += header[keyList.IndexOf(key)];
                    }

                    Console.WriteLine(decodedMessage);
                }

            Console.ReadKey();
        }

        private static void InitKeyList(List<string> keyList)
        {
            //setup map for the number of keys and the length of each key up to the maximum of 7
            Dictionary<int, int> keyCreationMap = new Dictionary<int, int>();

            for (int i = 0; i < 7; i++)
            {
                int length = i * 2 + 1;
                keyCreationMap.Add(length, i + 1);
            }

            foreach (KeyValuePair<int, int> keyValuePair in keyCreationMap)
            {
                for (int i = 0; i < keyValuePair.Key; i++)
                {
                    string binResult = Convert.ToString(i, 2);
                    //pad to appropriate length
                    keyList.Add(binResult.PadLeft(keyValuePair.Value, '0'));
                }
            }
        }
    }
}
