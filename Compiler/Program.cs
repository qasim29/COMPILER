using System;
using System.Collections;

// Tokens t = new Tokens("b", "a", 1);

// // Example #1
// // Read the file as one string.
// string text = System.IO.File.ReadAllText(@"C:\Users\Public\TestFolder\WriteText.txt");

// // Display the file contents to the console. Variable text is a string.
// System.Console.WriteLine("Contents of WriteText.txt = {0}", text);

// Example #2
// Read each line of the file into a string array. Each element
// of the array is one line of the file.
string[] lines = System.IO.File.ReadAllLines(@"E:\GITHUB\Language_Compiler\res\SOURCE_CODE.txt");

// // Display the file contents by using a foreach loop.
// System.Console.WriteLine("Contents of WriteLines2.txt = ");
ArrayList arlist = new ArrayList();
char[] breakers = { '(',')','[',']','{','}',                      // punctuators
                    ' ',';',':','"','~','\'',                    // punctuators
                    '+','-','*','/','%','<','>','=','!'};       // operators

string word = "";
foreach (string line in lines)
{
    foreach (char ch in line+" ")
    {
        if (breakers.Contains(ch))
        {
            if (word!="") 
            {
               arlist.Add(word);
               word="";
            }
            if (ch==' ')
            {
                continue;
            }
            arlist.Add(ch);            
            continue;
        }
        word = word + ch;
    }
}

foreach (string item in arlist)
{
    System.Console.WriteLine(item);
}


// // Keep the console window open in debug mode.
// Console.WriteLine("Press any key to exit.");
// System.Console.ReadKey();



