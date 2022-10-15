using System.Collections;

/* 
    LANGUAGE PARSER 
*/

Word_Breaker breaker = new Word_Breaker();
ArrayList words_list = breaker.GetWords();
Lexical_Analyzer la=new Lexical_Analyzer();

await ExampleAsync(la.GetTokens(words_list));


static async Task ExampleAsync(ArrayList tokens)
{

    using StreamWriter file = new(@"E:\GITHUB\Language_Compiler\res\words.txt");

    foreach (Tokens item in tokens)
    {
        Console.WriteLine(item.ToString());
        await file.WriteLineAsync(item.ToString());
    }
}








/* 

    .UPPER(non-terminals) 

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


// // printing non terminals
// // foreach (string item in nt)
// // {
// //     await file2.WriteLineAsync(item);
// // }

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
