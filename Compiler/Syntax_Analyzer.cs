using System.Collections;
using System.Collections.Generic;
public class Syntax_Analyzer
{
    List<Token> tokens;
    Dictionary<string,List<string[]>> rules;
    int index =0;
    Lexical_Analyzer la = new Lexical_Analyzer();
    public Syntax_Analyzer(List<Token> tokens)
    {
        this.rules = new Dictionary<string,List<string[]>>();
        this.tokens = tokens;
        this.getRules();
        // this.printRules();
    }
    private void getRules()
    {
        foreach (string line in System.IO.File.ReadLines(@"E:\GITHUB\Language_Compiler\res\temp.txt"))
        {
            if (line == "") { continue; }
            if (line[0] == '#') { continue; }
            string[] arr = line.Split("->");
            if (rules.ContainsKey(arr[0].Trim())) rules[arr[0].Trim()].Add(arr[1].Trim().Split(" "));   
            else
            {
                List<string[]> val = new List<string[]>();
                val.Add(arr[1].Trim().Split(" "));
                rules.Add(arr[0].Trim(), val);
            }
        }
    }
   
    public void printRules()
    {
        // printing rules from hash table to terminal
        string[] keys = new string[rules.Keys.Count];
        rules.Keys.CopyTo(keys, 0);
        int index = 0;
        foreach (List<string[]> items in rules.Values)
        {

            System.Console.Write($"{keys[index]} -> ");
            index += 1;
            System.Console.Write("[");
            foreach (string[] item in items)
            {
                System.Console.Write("[");
                foreach (string s in item)
                {
                    System.Console.Write(s);
                    System.Console.Write(",");
                }
                System.Console.Write("]");

            }
            System.Console.Write("]\n");
        }
    }
    public bool checkSyntax()
    {
        // System.Console.WriteLine("tokens.Length() == " + tokens.Length());
        if (helper("<START>") && index >= tokens.Count)
        {
            System.Console.WriteLine("INDEX == " + index);
            return true;
        }
        else
        {
            // System.Console.WriteLine("error wala token = " + invalidToken.value + " ,line no =" + invalidToken.line);
            System.Console.WriteLine("INDEX == " + index);
            return false;
        }
    }
    private bool helper(String curNT)
    {
        System.Console.WriteLine(rules[curNT]);

        List<String[]> productionRules = rules[curNT];
        System.Console.WriteLine("------------");

        foreach (String[] pr in productionRules)        
        {
            System.Console.WriteLine("% " + curNT + " -> " );
            int prev = index;
            int j = 0;
            for (; j < pr.Length; j++)
            {

                String element = pr[j];
                
                System.Console.WriteLine("\nElement :" + element + "' { of :" + curNT + "}");
                if (element[0] == '~')
                {
                    ++index;
                    return true;
                }
                if (element[0] == '<')
                {
                    System.Console.WriteLine("into => " + element);

                    if (!helper(element))
                    {
                        System.Console.WriteLine("@backing off");
                        //                        errorLine = tokens.get(index).line;

                        index = prev;
                        break;

                    }
                }
                else if (element.Length == 1 && element[0] == 'E')
                {

                    continue;
                }
                else
                {
                    System.Console.WriteLine("HERE IN TERMINAL");
                    System.Console.WriteLine("tokens.get(index).type =" + tokens[index].class_Part.ToString() + "'");
                    System.Console.WriteLine("tokens.get(index).type =" + tokens[index].word + "'");
                    // string a = la.ht.Contains();
                    
                    if (string.Equals(element,tokens[index].class_Part.ToString(), StringComparison.OrdinalIgnoreCase))
                    {

                        index++;
                        // if (invalidToken == tokens.get(index)) invalidToken = null;

                        System.Console.WriteLine("Matched Terminal =" + element);
                        // System.Console.WriteLine("Matched Terminal value =" + tokens.get(index).value);
                        // parsed.add(tokens.get(index).value);
                        
                        // System.Console.WriteLine("NEXT Terminal value =" + tokens.get(index).value);

                        // System.Console.WriteLine("%% PArsed = " + parsed);

                    }
                    else
                    {

                       
                        break;
                    }
                }
            }
            if (j == pr.Length)
            {
                System.Console.WriteLine("Successfully parsed from here");
                return true;
            }
            else
            {
                index = prev;
            }

        }

        return false;
    }

}