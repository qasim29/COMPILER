using System.Collections;


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