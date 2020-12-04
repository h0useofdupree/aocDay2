﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Diagnostics;
using System.Threading;

namespace aocDay2
{
    class MainClass
    {
        //Static Vars
        public static String fileName = "/home/juuls/AdventOfCode/2ndDay/puzzleInput.txt"; //Path to puzzle input
        public static StreamReader sr = new StreamReader(fileName);
        public static Int32 lineCnt = 0;

        public static void Main(string[] args)
        {
            //Vars
            lineCnt = getLineCount(); // Get line count
            String[] lineArray = getLinesToArray(); //Array with lines from raw.txt

            //REGEX 
            //String numExpr = @"\b(\d{1}|\d{2})[-](\d{1}|\d{2})\b";
            String passwordExpr = @"\b[^:]*$";
            String modifierExpr = @"\w(?=[:])";

            //Main values
            //Example of valid line : "1-6 a: afonaauwiv" //Should equal VALID because 'a' exists 1-6 times in said String
            //Example of invalid line: "3-5 b: buhafunvnb" //Should equal NOT VALID because 'b' only exists 2 times instead of 3-5 times 
            Int32[,] ranges = extractRange(lineArray);
            String[] mods = extractModifier(lineArray, modifierExpr);
            String[] pwStrings = extractPassword(lineArray, passwordExpr);

            //Get results!!!
            checkPolicy(pwStrings, mods, ranges);

            /*PROBLEM:
            As in the first example, there is a String containing 'z' 10 times. Range is 6-9 and it still outputs VALID*/

        }
        public static void checkPolicy(String[] pwStrings, String[] mods, Int32[,] ranges)
        {
            int validCount = 0;
            int counter = 0;
            for (int i = 0; i < 5; i++) //Change to i < pwStrings.Length for full list
            {
                counter++;
                int count = 0;
                String pw = pwStrings[i];
                for (int j = 0; j < pw.Length; j++)
                {
                    if (pw[j].Equals(Char.Parse(mods[i])))
                    {
                        count++;
                    }
                }
                Console.WriteLine($"Password: {pw}");
                Console.WriteLine($"Modifier: {mods[i]}\n");
                Console.WriteLine($"Range: {ranges[i, 0]}-{ranges[i, 1]}");
                Console.WriteLine($"Occurences: {count}");

                if (Enumerable.Range(ranges[i, 0], ranges[i, 1]).Contains(count))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("VALID\n");
                    validCount++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("NOT VALID\n");
                }
                Thread.Sleep(1);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("_______________________________\n\n");
            }
            Console.WriteLine(counter);
            Console.WriteLine(validCount);
        }


        public static Int32[,] extractRange(String[] lineArray)
        {
            Int32[,] ranges = new Int32[lineCnt, lineCnt];
            for (int i = 0; i < lineCnt; i++)
            {
                String[] splitted = lineArray[i].Split('-');
                ranges[i,0] = Int32.Parse(splitted[0]);
                ranges[i,1] = Int32.Parse(splitted[1].Split(' ').GetValue(0).ToString());
            }

            return ranges;
        }
        public static String[] extractModifier(String[] lineArray, String modExpr)
        {
            String[] modArr = new String[lineCnt];
            for (int i = 0; i < lineCnt; i++)
            {
                modArr[i] = Regex.Match(lineArray[i], modExpr).Value;
            }
            return modArr;
        }
        public static String[] extractPassword(String[] lineArray, String pwExpr)
        {
            String[] pw = new String[lineCnt];
            for (int i = 0; i < lineCnt; i++)
            {
                pw[i] = Regex.Match(lineArray[i], pwExpr).Value;
            }
            return pw;
        }

        public static String[] getLinesToArray()
        {
            String[] lines = new String[lineCnt];
            lines = File.ReadAllLines(fileName);
            return lines;
        }

        public static Int32 getLineCount()
        {
            while (sr.ReadLine() != null)
            {
                lineCnt++;
            }
            return lineCnt;
        }
    }
}
