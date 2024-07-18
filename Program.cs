using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
public class Program
{
    public static void Main(string[] args)
    {//Logica principal
        int result = 0;
        int resultTotal = 0;
        int position = 1;
        int count = 1;//Para obtener el valor del numero en la lista de palabras
        string pattern = @"(one|two|three|four|five|six|seven|eight|nine|ten)";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        string[] strings = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };
        string resultString = "";
        string resultString2 = "";
        //string pattern = @"(one|two|three|four|five|six|seven|eight|nine|ten)";

        foreach (string word in GetListWords())
        {//Recorre la lista de palabras
            int firstNumber = 0;
            int lastNumber = 0;
            int numberMatches = 0;
            LinkedList<string> wordsWithExtras = new LinkedList<string>();
            //Recorrer la palabra y busca las e que estan solas y le agrega una extra al lado
            //para que la expresion regular pueda encontrarlas
            /*
            string intento = GetWordWithExtra(word, 'e');
            intento+= GetWordWithExtra(word, 'o');
            intento+= GetWordWithExtra(word, 't');
            intento+= GetWordWithExtra(word, 'n');
            */
            numberMatches = regex.Count(word);

            if (regex.Count(GetWordWithExtra(word, 'e')) > numberMatches)
            {//continuar con la siguiente palabra
                wordsWithExtras.AddLast(GetWordWithExtra(word, 'e'));
            }
            //Excepcion para la palabra "three" y "eight"
            else if(regex.Count(GetWordWithExtra(word, 'o')) > numberMatches)
            {
                wordsWithExtras.AddLast(GetWordWithExtra(word, 'o'));
            }
            //Excepcion para la palabra "two" y "one"
            else if (regex.Count(GetWordWithExtra(word, 't')) > numberMatches)
            {
                wordsWithExtras.AddLast(GetWordWithExtra(word, 't'));
            }
            //Excepcion para la palabra "eight" y "three"
            else if (regex.Count(GetWordWithExtra(word, 'n')) > numberMatches)
            {
                wordsWithExtras.AddLast(GetWordWithExtra(word, 'n'));
            }
            else
            {
                wordsWithExtras.AddLast(word);
            }

            //Tenemos que escoger entre la palabra original y las demas palabras con extra, por la palabra mas larga(la mas completa)
            string wordWithExtra = wordsWithExtras.First();
            foreach (var item in wordsWithExtras)
            {
                if (item.Length > wordWithExtra.Length)
                {
                    wordWithExtra = item;
                }
            }
            MatchCollection matches = regex.Matches(wordWithExtra);
            Dictionary<int, int> numbersInDigits = GetNumbersInDigits(wordWithExtra);
            //numbersInDigits.Append(GetNumbersInDigits(wordWithExtraS));


            //Ya tenemos los numeros enteros y su posicion en la cadena dentro de un diccionario
            //Tambien, ya tenemos los numeros escritos en palabras
            //Ahora, solo debemos escoger cuales estan en los extremos
            //Comparando la primer posicion del match y la ultima.
            
            if (numbersInDigits.Count == 0 && matches.Count > 0)
            {
                foreach (var item in strings)
                {
                    if (item.Equals(matches.First().Value))
                    {
                        firstNumber = count;
                    }
                    if (item.Equals(matches.Last().Value) && matches.Count > 1)
                    {
                        lastNumber = count;
                    }
                    count++;
                }
                count = 1;
            }
            if (numbersInDigits.Count > 0 && matches.Count == 0)
            {
                firstNumber = numbersInDigits.First().Value;
                if (numbersInDigits.Count > 1)
                {
                    lastNumber = numbersInDigits.Last().Value;
                }

            }

            if (numbersInDigits.Count > 0 && matches.Count > 0)
            {//hay numeros

                if (matches.First().Index > numbersInDigits.First().Key)
                {
                    firstNumber = numbersInDigits.First().Value;
                }
                else
                {
                    foreach (var item in strings)
                    {
                        if (item.Equals(matches.First().Value))
                        {
                            firstNumber = count;
                            count = 1;
                            break;
                        }
                        count++;
                    }
                }
                if (matches.Last().Index < numbersInDigits.Last().Key)
                {
                    lastNumber = numbersInDigits.Last().Value;
                }
                else
                {
                    foreach (var item in strings)
                    {
                        if (item.Equals(matches.Last().Value))
                        {
                            lastNumber = count;
                            count = 1;
                            break;
                        }
                        count++;
                    }
                }

            }
            if (lastNumber > 0)
            {
                result = int.Parse(firstNumber.ToString() + lastNumber.ToString());
            }
            else
            {
                result = int.Parse(firstNumber.ToString() + firstNumber.ToString());
            }
            resultString += "I:"+position +" - "+ result.ToString() + "\n";
            resultString2 += result.ToString() + ", ";
            resultTotal += result;
            
        }
        Console.WriteLine($"Resultado: {resultString} \n");
        Console.WriteLine($"Resultado: {resultString2}\n");
        Console.WriteLine($"Resultado total: {resultTotal}");
        return;
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
                if(regex.Matches(item).Count > 1)
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
        string file = @"C:\Users\samue\Downloads\Profesion\Advent of Code\Day1\Solution_Day1\Resources\data.txt";
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
}
