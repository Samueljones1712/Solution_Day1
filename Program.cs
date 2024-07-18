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
    {//Logica principal
        int result = 0;
        int resultTotal = 0;
        int position = 1;
        string resultString = "";
        //Diccionario para los numeros escritos en ingles y su respectivo valor
        
        foreach (string word in GetListWords())
        {//Recorre la lista de palabras
            int firstNumber = 0;
            int lastNumber = 0;

            Dictionary<int, int> NumeralDigits = GetNumbersInDigits(word);
            NumeralDigits = GetNumbersOfWords(NumeralDigits, word);
            firstNumber = NumeralDigits.Values.First();
            lastNumber = NumeralDigits.Values.Last();

            result = int.Parse(firstNumber.ToString() + lastNumber.ToString());

            Console.WriteLine($"Position:{position} = {result.ToString() } \n");
            resultString += result.ToString() + ", ";
            resultTotal += result;
            position++;
        }
        
        Console.WriteLine($"Resultado: {resultString}\n");
        Console.WriteLine($"Resultado total: {resultTotal}");
        return;
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
            //Si el numero esta contenido en la palabra, entonces se agrega al diccionario de respuesta.
            if (word.Contains(item.Value))
            {
                //debemos contar el numero de veces que el numero esta en la palabra
                string wordTemp = word;
                while(wordTemp.Contains(item.Value))
                {
                    positionWord = wordTemp.IndexOf(item.Value);
                    numeralDigits.Add(positionWord, item.Key);
                    //Quiero cambiar la palabra por -
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
        bool lastNumberValid = false;//Indica cuando detenerse
        bool firstNumberValid = false;
        int positionToLast = arrayChar.Length - 1;//La ubicacion en el vector
        int positionToFirst = 0;
        var numbersInDigits = new Dictionary<int, int>();//Se almacenan los valores y posicion
        //Key = posicion, Value = numero
        while (firstNumberValid == false && positionToFirst < arrayChar.Length - 1)
        {
            if (char.IsDigit(arrayChar[positionToFirst]) && !numbersInDigits.ContainsKey(positionToFirst))
            {//Evitamos que se repitan los numeros
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

    public static LinkedList<string> GetListWords()
    {//Busca las palabras en el archivo de texto
        string file = @"C:\Users\samue\Downloads\Profesion\Advent of Code\Day1\Solution_Day1\Resources\input.txt";
        LinkedList<string> listWords = new LinkedList<string>();
        Console.WriteLine("Reading File using File.ReadAllText()");

        if (File.Exists(file))
        {
            StreamReader Textfile = new StreamReader(file);
            string line;

            while ((line = Textfile.ReadLine()) != null)
            {
                listWords.AddLast(line);
            }

            Textfile.Close();

        }
        return listWords;
    }
    public static List<Match> GetMatches(LinkedList<string> strings, Regex regex)
    {
        List<Match> matches = new List<Match>();
        foreach (var item in strings)
        {
            //Obtengo las collecciones con las palabras de las listas y las voy agregando a una lista de matches
            MatchCollection matchCollection = regex.Matches(item);
            if (regex.IsMatch(item))
            {
                if (regex.Matches(item).Count > 1)
                {
                    matches.AddRange(matchCollection);
                }
                matches.AddRange(regex.Matches(item));
            }
        }
        matches = matches.OrderBy(m => m.Index).ToList();
        return matches;
    }

    public static string GetWordWithExtra(string word, char letter)
    {
        string wordWithExtra = "";
        //Excepcion para la palabra "three" y "eight"
        for (int i = 0; i < word.Length; i++)
        {
            if (i - 2 > 0 && word[i - 2] != letter && word[i - 1] == letter && word[i] != letter && i < word.Length - 1)
            {
                if (word[i - 1] == letter && word[i - 2] != letter)
                {//Si la letra anterior es la que buscamos y la anterior a la anterior no es la que buscamos, entonces
                    //agregamos la letra que buscamos antes de la letra actual e intentamos con la expresion regular si hay un match
                    //Si no hay match, entonces eliminamos la letra que agregamos y seguimos con la siguiente
                    wordWithExtra += letter;
                    wordWithExtra += word[i];
                }
                else
                {
                    wordWithExtra += word[i];
                }
            }
            else
            {
                wordWithExtra += word[i];
            }
        }
        return wordWithExtra;
    }
}
