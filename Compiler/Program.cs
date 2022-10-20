using System.Collections;


/* 
    LANGUAGE PARSER 
*/

// language parser program
static async Task ExampleAsync(List<Token> tokens)
{

    using StreamWriter file = new(@"E:\GITHUB\Language_Compiler\res\words.txt");

    foreach (Token item in tokens)
    {
        // Console.WriteLine(item.ToString());
        await file.WriteLineAsync(item.ToString());
    }
}
 

Word_Breaker breaker = new Word_Breaker();
Lexical_Analyzer la = new Lexical_Analyzer();

// ArrayList words_list = breaker.GetWords();
List<Token> tokens = la.GetTokens(breaker.GetWords());

await ExampleAsync(tokens);

Syntax_Analyzer sa = new Syntax_Analyzer(tokens);

System.Console.WriteLine(sa.checkSyntax());

/* 
    // importing rules into hashtable 
*/
// Hashtable rules = new Hashtable();
// foreach (string line in System.IO.File.ReadLines(@"E:\GITHUB\Language_Compiler\res\CFGs.txt"))
// {
//     if (line == "") { continue; }
//     if (line[0] == '#') { continue; }
//     string[] arr = line.Split("->");
//     if (rules.ContainsKey(arr[0]))
//     {
//          ArrayList? pro = (ArrayList?)rules[arr[0]];
//         pro?.Add(arr[1].Trim().Split(" "));
//         rules[arr[0]] = pro;
//     }
//     else
//     {
//         ArrayList val = new ArrayList();
//         val.Add(arr[1].Trim().Split(" "));
//         rules.Add(arr[0], val);
//     }
// }

// // printing rules from hash table to terminal
// string[] keys= new string[rules.Keys.Count];
// rules.Keys.CopyTo(keys,0);
// int index=0;
// foreach (ArrayList items in rules.Values)
// {
    
//     System.Console.Write($"{keys[index]} -> ");
//     index+=1;
//     System.Console.Write("[");
//     foreach (string[] item in items)
//     {
//         System.Console.Write("[");
//         foreach (string s in item)
//         {
//             System.Console.Write(s);
//             System.Console.Write(",");
//         }
//         System.Console.Write("]");

//     }
//     System.Console.Write("]\n");
// }



/* 

    .UPPER(non-terminals) in CFGs.txt  

*/

// using StreamWriter file2 = new(@"E:\GITHUB\Language_Compiler\res\CFGs.txt");
// foreach (string line in System.IO.File.ReadLines(@"E:\GITHUB\Language_Compiler\res\temp.txt"))
// {  
//     // if(line[0]=='#') continue;
//     string mline="";
//     bool flag=false;
//     foreach (char ch in line)
//     {
//         if(ch=='<') {flag=true; mline+=ch; continue;}
//         if(ch=='>') {flag=false; mline+=ch; continue;}
//         if(flag) {mline+= Char.ToUpper(ch);continue;}
//         mline+=ch;
//     }
//     await file2.WriteLineAsync(mline);
//     System.Console.WriteLine(mline);  
// }  





/* 

    CHECKING ALL NON TERMINAL ARE VALID?

*/
// // storing all non terminal in hash set from left hands side of cfgs
// HashSet<string> nt = new HashSet<string>();

// foreach (string line in System.IO.File.ReadLines(@"E:\GITHUB\Language_Compiler\res\CFGs.txt"))
// {  
//     // if(line[0]=='#') continue;
//     string mline="";
//     bool flag=false;
//     foreach (char ch in line)
//     {
//         if(ch=='#') break;
//         if(ch=='<') {flag=true; mline+=ch; continue;}
//         if(ch=='>') {flag=false; mline+=ch; break;}
//         if(flag) {mline+= ch;}
//         // mline+=ch;
//     }
//     if(mline!="") nt.Add(mline);
// }
// // printing non terminals in hashset
// // foreach (string item in nt)
// // {
// //     await file2.WriteLineAsync(item);
// // }
//  logic for validating each non terminal in CFGs.txt is in hashset?
// foreach (string line in System.IO.File.ReadLines(@"E:\GITHUB\Language_Compiler\res\CFGs.txt"))
// {  
//     string mline="";
//     bool flag=false;
//     foreach (char ch in line)
//     {   
//         if(ch=='#') break;
//         if(ch=='<') {flag=true; mline+=ch; continue;}
//         if(ch=='>' && flag==true) {
//             flag=false;
//             mline+=ch; 
//             if(nt.Contains(mline)) {System.Console.WriteLine("TRUE");}
//             else {System.Console.WriteLine(mline);}
//             mline="";
//             continue;
//         }
//         if(flag) mline+=ch ;
//     }
// }  
