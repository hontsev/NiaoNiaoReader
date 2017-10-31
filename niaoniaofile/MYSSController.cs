﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mySpeechSynthesizer;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;

namespace SpeechSynthesizer
{
    class MYSSController
    {
        
        private PinYinConverter pyconv;

        public int soundheight;
        public int soundSpeed;

        NNTone[] sounds;
        ToneList tl;
        public string filepath = "";
        string output =  @"output\tmp.wav";
        string outputOri = @"output\tmp_origin.wav";
        string outputTone = @"output\tmp_tone.wav";

        public MYSSController(string sourcePath)
        {
            pyconv = new PinYinConverter();
            soundheight = 120;
            soundSpeed = 600;

            //init
            filepath = sourcePath;
            sounds = NNAnalysis.getParamsFromNN(filepath);
            tl = SoundAnalysis.analysisAll(filepath);


        }

        public int[] getZipDatas(int toneNum,int beforeToneNum=-1,int nextToneNum=-1)
        {
            int[] datas;
            switch (toneNum)
            {
                case 1:
                    if (nextToneNum == 5)
                    {
                        datas = new int[5] { 110, 110, 110, 110, 90 };
                    }
                    else
                    {
                        datas = new int[5] { 110, 110, 110, 110, 110 };
                    }
                    
                    break;
                case 2:
                    datas = new int[5] { 80, 80, 90, 105, 110 };
                    break;
                case 3:
                    datas = new int[5] { 70, 60, 60, 75, 75 };
                    break;
                case 4:
                    datas = new int[5] { 110, 98, 85, 72, 60 };
                    break;
                case 5:
                default:
                    datas = new int[5] { 85, 80, 75, 72, 70 };
                    break;
            }

            //datas = new int[2] { 70, 130 };

            return datas;
        }



        public int[] getSoundData(string name,int[] pitdata, int len=500)
        {
            int[] res = SoundAnalysis.synthesis(tl, name, pitdata, len, 1.0);
            return res;
        }

        public void writeWAV(int[] wavdata,string filename=null)
        {
            if (filename == null) filename = outputTone;
            WAVControl.writeWAV(wavdata, filename);
        }

        public void playSound(string filename)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(filename);
            player.PlaySync();
        }

        public int[] showSound(string str, MyDelegates.sendStringDelegate printEvent)
        {
            printEvent(string.Format("开始生成字符串（开头为：{0}...）", (str.Length >= 7 ? str.Substring(0, 7) : str)));
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string[] sentences = pyconv.cutSentencesAll(str);
            sw.Stop();
            printEvent(string.Format("拆分句子完毕：{0}ms，共{1}个短句", sw.ElapsedMilliseconds, sentences.Length));
            int num = 0;
            int index = 0;
            int nowindex = 0;

            int scale = 2;

            List<int> allres = new List<int>();

            foreach (var sentence in sentences)
            {
                for (int s = 0; s < 10000 * ((double)100 / soundSpeed); s++)
                    {
                        allres.Add(0);
                    }
                List<List<string>> pinyin = pyconv.getPinYinList(sentence);
                foreach (var p in pinyin)
                {
                    int d;
                    int duration = 500;
                    for (int i = 0; i < p.Count; i++)
                    {
                        if (i < p.Count - 1)
                            //是一个词的中间字，因此间隔小
                            d = 0;
                        else
                            d = 1;

                        if (p[i].EndsWith("5"))
                        {
                            //轻声
                            duration = 425;
                            //if (p.Count == 1 && index > 0) index -= 1;
                        }
                        else if (p[i].EndsWith("3"))
                        {
                            //上声，念得长
                            duration = 575;
                        }
                        else if (p[i].EndsWith("4") && i == p.Count - 1)
                        {
                            //去声且在结尾，念得短
                            duration = 500;
                        }
                        else if (p[i].EndsWith("2") && i < p.Count - 1)
                        {
                            //阳平且在句中，念得短
                            duration = 475;
                        }
                        else
                        {
                            duration = 500;
                        }
                        string beforep = "";
                        int beforetonenum = -1;
                        if (i >= 1)
                        {
                            beforep = p[i - 1];
                            beforetonenum = int.Parse(beforep.Substring(beforep.Length - 1));
                        }
                        string nextp = "";
                        int nexttonenum = -1;
                        if (i < p.Count - 1)
                        {
                            nextp = p[i + 1];
                            nexttonenum = int.Parse(nextp.Substring(nextp.Length - 1));
                        }
                        int tonenum=int.Parse(p[i].Substring(p[i].Length-1));
                        string ch=p[i].Substring(0,p[i].Length-1);

                        double sneeze = 0.22;
                        double part = 1.0 * (sneeze / 2);
                        int[] thisSoundFrame = getSoundData(ch, getZipDatas(tonenum, beforetonenum, nexttonenum), duration);
                        for (int k = 0; k < thisSoundFrame.Length;k++ )
                        {
                            thisSoundFrame[k] = (int)((double)thisSoundFrame[k] * soundheight / 100);
                        }
                        
                        int partlen = (int)(thisSoundFrame.Length * part);

                            for (int k = 0; k < partlen; k++)
                            {
                                allres[allres.Count - partlen + k] = (int)(allres[allres.Count - partlen + k] * 1 + thisSoundFrame[k] * 1);

                            }
                            for (int k = partlen; k < thisSoundFrame.Length; k++)
                            {
                                allres.Add(thisSoundFrame[k]);
                            }

                        
                        if (d > 0)
                        {
                            for (int s = 0; s < 4200* ((double)100 / soundSpeed); s++)
                            {
                                allres.Add(0);
                            }
                        }
                        num++;
                    }
                }
                printEvent(string.Format("生成短句{0}/{1}", ++nowindex, sentences.Length));
            }

            
            return allres.ToArray();
        }
    }
}