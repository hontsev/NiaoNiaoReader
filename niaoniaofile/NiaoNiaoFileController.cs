﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using JiebaNet.Segmenter;

namespace niaoniaofile
{
    class NiaoNiaoFileController
    {
        
        private PinYinConverter pyconv;
        public int soundheight;

        public NiaoNiaoFileController()
        {
            pyconv = new PinYinConverter();
            soundheight = 19;
        }

        public static string getHeader(int soundnum,int maxlen)
        {
            int blocknum = (maxlen / 32) + 2;
            string header = string.Format("130.0 4 4 {0} 19 0 0 0 0 0\n{1}\n", blocknum, soundnum);
            return header;
        }



        

        private static string getNiaoNiaoSoundTurnInfo(int toneIndex,int nextIndex = 0)
        {
            string res = "100,28,28,27,27,26,26,26,26,26,25,24,24,24,24,24,24,24,23,23,22,22,22,22,22,22,20,20,20,20,20,19,19,19,19,18,18,18,18,18,18,18,18,18,18,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,18,18,18,18,18,19,19,19,19,19,19,19,20,20,20 2";
            switch (toneIndex)
            {
                case 1:
                    res = "100,14,16,19,22,23,26,27,29,30,32,34,35,36,36,38,38,39,40,41,41,42,44,44,45,45,46,46,46,46,46,46,47,48,49,50,50,51,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,52,53,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54 1";

                    //res = "100,6,8,10,10,12,12,12,12,42,42,46,46,46,48,48,50,50,50,50,50,52,52,52,52,52,54,56,56,56,56,56,56,56,56,56,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,60,62,62,62,62,62,62,62,62,62,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64,64 2";
                    break;
                case 2:
                    res = "100,14,8,6,6,4,4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,4,4,4,4,6,6,6,6,8,8,8,8,10,10,12,12,14,14,16,18,20,20,24,24,26,28,30,32,36,38,40,42,42,44,46,48,50,54,56,55,56,56,57,57,58,58,59,60,60,60,62,63,64,64,64,65,65,65,65,66,66,66,66,66,66,66,68,68,68,68,68,68,68,68,68,68,68,68,68,68 2";
                    //res = "100,15,15,15,15,15,15,15,15,15,16,16,16,16,16,17,17,17,18,18,19,19,20,20,21,22,23,23,24,24,24,25,25,25,26,26,27,27,28,28,28,28,28,28,28,30,30,30,30,30,31,31,31,32,33,33,33,33,34,35,36,36,37,37,38,38,38,39,40,40,42,43,44,46,47,48,48,49,50,50,52,52,52,52,52,53,54,54,54,55,56,56,57,57,57,58,58,58,58,58,58 2";
                    //res = "100,15,15,15,15,15,15,15,15,15,16,16,16,17,17,17,18,18,18,18,23,23,23,23,23,23,23,23,23,23,23,23,23,25,25,25,25,25,25,25,25,26,26,27,27,27,29,29,29,29,29,30,30,30,30,31,31,31,34,34,35,36,36,37,37,38,39,39,40,40,43,45,45,51,51,53,54,54,55,55,59,60,60,61,61,63,64,64,64,64,66,66,66,67,67,68,68,68,68,68,82 2";
                    if (nextIndex == 1)
                    {
                        //2->1
                        res = "100,14,14,8,8,6,4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,4,4,4,5,5,6,6,7,7,7,7,7,7,8,8,8,8,9,10,10,11,11,11,11,11,13,13,13,14,14,15,15,15,16,16,17,17,18,18,18,18,18,19,20,20,20,20,20,21,21,21,21,22,23,23,24,24,24,24,24,24,24,24,24,24,25,25,25,25,25,25,25,26,26,26,26,26,26 2";
                    }
                    else if (nextIndex == 2)
                    {
                        //2->2
                        res = "100,16,16,10,10,8,6,6,6,6,6,6,6,4,4,4,4,4,4,4,4,4,4,4,6,6,8,8,8,10,10,12,12,14,16,16,18,18,20,20,22,24,24,24,26,26,26,26,28,28,30,30,32,32,32,34,34,34,34,36,36,36,38,38,40,40,42,46,46,46,48,48,50,51,52,53,53,53,53,53,53,53,53,53,53,52,52,50,50,50,48,48,47,46,46,45,44,44,43,42,40 2";
                    }
                    else if (nextIndex == 4)
                    {
                        //2->4
                        res = "100,4,4,4,4,6,4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,4,4,4,6,6,6,6,8,8,8,8,8,9,12,12,14,14,14,18,18,20,20,24,26,26,30,30,32,34,34,34,34,40,41,41,43,43,44,45,45,45,45,45,45,45,46,46,47,47,47,47,47,49,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50 2";
                    }
                    break;
                case 3:
                    res = "100,36,36,36,36,31,31,31,29,29,27,25,25,24,24,22,21,21,20,20,18,17,17,16,16,15,14,14,13,13,12,11,11,9,9,9,9,9,8,8,8,7,7,7,7,6,6,6,6,6,6,4,4,4,4,4,6,6,6,6,7,9,9,9,9,11,13,13,14,14,14,15,15,15,15,18,15,15,17,17,19,20,20,20,20,22,23,23,23,23,25,25,25,28,28,28,28,28,29,29,29 5";
                    break;
                case 4:
                    res = "100,53,53,53,53,48,48,48,48,48,48,48,48,48,48,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,46,45,45,45,44,44,43,43,43,43,43,41,41,41,38,38,35,35,33,31,31,30,30,28,25,25,24,24,23,23,23,20,20,16,15,15,12,12,12,11,11,9,9,9,7,7,6,6,5,4,4,3,3,2 3";
                    if (nextIndex == 1)
                    {
                        //4->1
                        res = "100,56,56,56,56,56,46,46,46,46,46,46,46,46,46,46,46,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,40,40,40,38,38,36,36,30,30,28,28,27,27,27,27,27,26,26,26,26,26,26,26,22,22,22,22,21,21,20,20,19,19,19,19,19,18,18,17,17,17,17,17,17,17,17,17,16,16,16,16 1";
                    }
                    else if (nextIndex == 3)
                    {
                        //4->2
                        res = "100,54,54,54,54,54,47,47,47,47,47,47,47,47,47,47,47,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,45,43,43,43,42,42,40,40,40,40,38,38,38,38,38,34,34,30,30,17,17,16,16,16,15,15,15,15,14,14,11,11,10,10,10,9,9,9,9,8,8,7,7,7,7,7,7,7,5,5,5,5 2";
                    }
                    else if (nextIndex == 4)
                    {
                        //4->4
                        res = "100,57,57,54,54,54,54,54,54,54,54,52,52,52,51,50,50,48,48,47,47,47,47,47,47,46,46,46,46,46,46,44,44,44,44,44,43,42,41,41,40,40,39,38,38,38,37,37,36,36,35,35,34,32,32,32,32,31,30,30,30,30,29,29,28,27,26,25,24,24,23,22,21,19,18,17,16,16,14,14,13,13,12,11,11,10,10,9,9,8,8,8,6,6,6,6,6,6,6,6,6 1";
                    }
                    break;
                case 5:
                    res = "100,37,37,37,36,36,24,24,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,23,22,22,21,21,21,21,21,20,20,20,20,19,19,18,18,18,18,18,17,17,17,17,16,16,15,15,15,15,15,15,15,13,13,13,13,13,12,12,10,10,10,10,9,9,8,8,8,8,8,7,7,6,6,6,6,6,6,6,4,4,4,4,4,4 4";
                    //res = "100,28,28,27,27,26,26,26,26,26,25,24,24,24,24,24,24,24,23,23,22,22,22,22,22,22,20,20,20,20,20,19,19,19,19,18,18,18,18,18,18,18,18,18,18,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,18,18,18,18,18,19,19,19,19,19,19,19,20,20,20 2";
                    break;
                default:
                    break;
            }
            return res;
        }

        public string getNiaoNiaoSoundStr(string pinyin, int beginat,int length,string nextpinyin)
        {
            int toneIndex = 1;
            int nextIndex = 0;
            int.TryParse(pinyin[pinyin.Length - 1].ToString(), out toneIndex);
            if(nextpinyin.Length>0)int.TryParse(nextpinyin[nextpinyin.Length - 1].ToString(), out nextIndex);

            string realpinyin = Regex.Replace(pinyin, @"\d", "");

            string soundscale = "100,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50";

            string soundturn = getNiaoNiaoSoundTurnInfo(toneIndex,nextIndex);

            string res = string.Format(" {0} {0} {1} {2} {3} 50 50 0 0 0 0 {4} {5}\n", realpinyin, beginat, length, soundheight, soundscale, soundturn);

            return res;
        }


        public string getNiaoNiaoFile(string str)
        {
            string file = "";
            string filecontent = "";
            string[] sentences = pyconv.cutSentencesAll(str);
            int num = 0;
            int beginat = 0;
            foreach (var sentence in sentences)
            {
                beginat += 5;
                List<List<string>> pinyin = pyconv.getPinYinList(sentence);
                foreach (var p in pinyin)
                {
                    int d;
                    int length = 4;
                    for (int i = 0; i < p.Count; i++)
                    {
                        if (i < p.Count - 1)
                            //是一个词的中间字，因此间隔小
                            d = 0;
                        else
                            d = 1;

                        if (p[i].EndsWith("5"))
                        {
                            //是轻声字，间隔小
                            length = 3;
                            if (p.Count == 1 && beginat > 0) beginat -= 1;
                        }
                        else if (p[i].EndsWith("4"))
                        {
                            //去声念得短
                            length = 3;
                        }
                        else
                        {
                            length = 4;
                        }
                        string nextp = "";
                        if (i < p.Count - 1)
                        {
                            nextp = p[i + 1];
                        }
                        filecontent += getNiaoNiaoSoundStr(p[i], beginat, length, nextp);
                        beginat += d + length;
                        num++;
                    }
                }

            }
            file += getHeader(num, beginat);
            file += filecontent;
            return file;
        }
    }
}
