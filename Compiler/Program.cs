// 1:30  started earphnes
using System;
using System.Collections;

// Tokens t = new Tokens("b", "a", 1);

// // Example #1
// // Read the file as one string.
// string text = System.IO.File.ReadAllText(@"C:\Users\Public\TestFolder\WriteText.txt");

// // Display the file contents to the console. Variable text is a string.
// System.Console.WriteLine("Contents of WriteText.txt = {0}", text);

string[] lines = System.IO.File.ReadAllLines(@"E:\GITHUB\Language_Compiler\res\SOURCE_CODE.txt");

// // Display the file contents by using a foreach loop.
// System.Console.WriteLine("Contents of WriteLines2.txt = ");
ArrayList arlist = new ArrayList();
char[] breakers = { '(',')','[',']','{','}',                      // punctuators
                    ' ',';',':','"','~','\'',',',                // punctuators
                    '+','-','*','/','%','<','>','=','!'};       // operators

string word = "";

foreach (string line in lines)
{
    string l = line + " ";
    for (int i = 0; i < l.Length - 1; i++)
    {
        //this condition is for the comment 
        if (l[i] == '$') break;

        if (breakers.Contains(l[i]))
        {
            if (line[i] == '"')
            {
                int istart = i;
                if (word != "")
                {
                    arlist.Add(word);
                    word = "" + line[i];
                    i++;
                }
                else{
                    word = "" + line[i];
                    i++;
                }
                char ch = ' ';
                while (ch != '"' && i < l.Length - 1)
                {
                    if (line[i] == '\\')
                    {
                        ch = line[i];
                        word += ch;
                        i++;
                    }
                    ch = line[i];
                    word += ch;
                    i++;
                }
                if (i == l.Length)
                {
                    word = word.Substring(istart, i - 1);
                    arlist.Add(word);
                    word = "";
                    break;
                }
                arlist.Add(word);
                word = "";
                continue;
            }
            if (word != "")
            {
                arlist.Add(word);
                word = "";
            }
            if (l[i] == ' ')
            {
                continue;
            }

            arlist.Add(l[i].ToString());
            continue;
        }
        word = word + l[i];
    }
}

foreach (string item in arlist)
{
    System.Console.WriteLine(item);
}


// // Keep the console window open in debug mode.
// Console.WriteLine("Press any key to exit.");
// System.Console.ReadKey();






// BREAKER DIFFERENT APPROACH

// //this condition is for the comment 
// if (l[i] == '$') break;

// if (breakers.Contains(l[i]))
// {
//     if (l[i] == ' ')
//     {
//         if (word == "") continue;
//         arlist.Add(word);
//         word = "";

//     }
//     else
//     {
//         if (word != "") arlist.Add(word);
//         word ="";
//         arlist.Add(l[i].ToString());

//     }
//     continue;
