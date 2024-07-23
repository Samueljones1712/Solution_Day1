using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
public class Program
{
    public static void Main(string[] args)
    {
        Part1("input.txt");
        Part2("input.txt");
    }
    public static void Part1(string fileName)
    {
        int result = 0;
        int resultTotal = 0;
        int position = 1;
        string resultString = "";
        foreach (string word in GetListWords(fileName))
        {
            int firstNumber = 0;
            int lastNumber = 0;

            Dictionary<int, int> NumeralDigits = GetNumbersInDigits(word);
            firstNumber = NumeralDigits.Values.First();
            lastNumber = NumeralDigits.Values.Last();

            result = int.Parse(firstNumber.ToString() + lastNumber.ToString());
            resultString += result.ToString() + ", ";
            resultTotal += result;
            position++;
        }

        Console.WriteLine($"Part 1 Result: {resultTotal}");
    }

    public static void Part2(string fileName)
    {
        int result = 0;
        int resultTotal = 0;
        int position = 1;
        string resultString = "";
        foreach (string word in GetListWords(fileName))
        {
            int firstNumber = 0;
            int lastNumber = 0;

            Dictionary<int, int> NumeralDigits = GetNumbersInDigits(word);
            NumeralDigits = GetNumbersOfWords(NumeralDigits, word);
            firstNumber = NumeralDigits.Values.First();
            lastNumber = NumeralDigits.Values.Last();

            result = int.Parse(firstNumber.ToString() + lastNumber.ToString());
            resultString += result.ToString() + ", ";
            resultTotal += result;
            position++;
        }

        Console.WriteLine($"Part 2 Result: {resultTotal}");
    }
    public static Dictionary<int, int> GetNumbersOfWords(Dictionary<int, int> numeralDigits, string word)
    {
        int positionWord = 0;
        int concurrences = 0;
        int startPosition = 0;
        Dictionary<int, string> numbers = new Dictionary<int, string>()
        {
            {1,"one"},
            {2,"two"},
            {3,"three"},
            {4,"four"},
            {5,"five"},
            {6,"six"},
            {7,"seven"},
            {8,"eight"},
            {9,"nine"}
        };
        foreach (var item in numbers)
        {
            if (word.Contains(item.Value))
            {
                string wordTemp = word;
                while(wordTemp.Contains(item.Value))
                {
                    positionWord = wordTemp.IndexOf(item.Value);
                    numeralDigits.Add(positionWord, item.Key);
                    StringBuilder sb = new StringBuilder(wordTemp);
                    for (int i = 0; i < item.Value.Length; i++)
                    {
                        sb[positionWord + i] = '-';
                    }
                    wordTemp = sb.ToString();
                }
            }
        }
        return numeralDigits.OrderBy(m => m.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
    }
    
    public static Dictionary<int, int> GetNumbersInDigits(string word)
    {
        char[] arrayChar = word.ToCharArray();
        bool lastNumberValid = false;
        bool firstNumberValid = false;
        int positionToLast = arrayChar.Length - 1;
        int positionToFirst = 0;
        var numbersInDigits = new Dictionary<int, int>();
        while (firstNumberValid == false && positionToFirst < arrayChar.Length - 1)
        {
            if (char.IsDigit(arrayChar[positionToFirst]) && !numbersInDigits.ContainsKey(positionToFirst))
            {
                numbersInDigits.Add(positionToFirst, int.Parse(arrayChar[positionToFirst].ToString()));
            }
            positionToFirst++;
        }

        while (lastNumberValid == false && positionToLast > 0)
        {
            if (char.IsDigit(arrayChar[positionToLast]) && !numbersInDigits.ContainsKey(positionToLast))
            {
                numbersInDigits.Add(positionToLast, int.Parse(arrayChar[positionToLast].ToString()));
            }
            positionToLast--;
        }
        return numbersInDigits;
    }

    public static LinkedList<string> GetListWords(string fileName)
    {

        string executableDir = AppDomain.CurrentDomain.BaseDirectory;
        string projectDir = Path.GetFullPath(Path.Combine(executableDir, @"..\..\.."));
        string relativePath = Path.Combine(projectDir, "Resources", fileName);
        LinkedList<string> listWords = new LinkedList<string>();
        if (File.Exists(relativePath))
        {
            StreamReader Textfile = new StreamReader(relativePath);
            string line;

            while ((line = Textfile.ReadLine()) != null)
            {
                listWords.AddLast(line);
            }

            Textfile.Close();

        }
        return listWords;
    }
}
