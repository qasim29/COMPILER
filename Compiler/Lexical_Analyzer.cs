using System.Collections;
using System.Text.RegularExpressions;

class Lexical_Analyzer
{
    ArrayList tokens;
    Hashtable ht;
    Regex? regex;
    public Lexical_Analyzer()
    {
        tokens = new ArrayList();
        ht = new Hashtable()
        {
            // ARITHEMATIC & LOGICAL OPERATORS
            {"+",TokenType.PM},         {"-",TokenType.PM},    
            {"/",TokenType.MDM},        {"%",TokenType.MDM},
            {"*",TokenType.MDM},        {"!",TokenType.NOT},
            //  COMPARISION OPERATORS
            {">",TokenType.COMP},       {"<",TokenType.COMP},
            {"<=",TokenType.COMP},      {">=",TokenType.COMP},
            {"==",TokenType.COMP},      {"!=",TokenType.COMP},
            {"=",TokenType.ASI},        {"and",TokenType.AND},     
            {"or",TokenType.OR},        {"return",TokenType.RETURN},
            // BRACKETS
            {"(",TokenType.ORB},        {")",TokenType.CRB},
            {"{",TokenType.OCB},        {"}",TokenType.CCB},
            {"[",TokenType.OSB},        {"]",TokenType.CSB},
            // CONDITIONAL STATEMENT
            {"if",TokenType.IF},        {"else",TokenType.ELSE},
            // LOOP             
            {"while",TokenType.WHILE},  {"break",TokenType.LK},
            {"skip",TokenType.LK}, 
            // DATA TYPES
            {"int",TokenType.DT},       {"float",TokenType.DT},
            {"string",TokenType.DT},    {"char",TokenType.DT},
            // PUNCTUATORS
            {";",TokenType.SEC},        {":", TokenType.COL},
            {",",TokenType.COM}, 
            // OOP
            {"null",TokenType.NULL},    {"abstract",TokenType.ABS},  
            {"static",TokenType.STA},   {"create",TokenType.NEW},
            {"class",TokenType.CLA},    {"self",TokenType.SELF},
            {"super",TokenType.SUP},    {"const",TokenType.CONST},
            {"child_of",TokenType.C_OF},
            // ACCESS MODIFIERS
            {"local",TokenType.CLA},    {"global",TokenType.SELF},
            {"protected",TokenType.SUP}
        };
    

    }

    public ArrayList GetTokens(ArrayList words)
    {
        foreach (string[] w in words)
        {
            if (ht.ContainsKey(w[0])) { tokens.Add(new Tokens(w[1], (TokenType?)ht[w[0]], w[0])); continue; }

            else if (Char.IsNumber(w[0][0]))
            {
                regex = new Regex(@"^[0-9]+$");
                if (regex.IsMatch(w[0])) { tokens.Add(new Tokens(w[1], TokenType.IC, w[0])); continue; }

                regex = new Regex(@"^[0-9]*[.][0-9]+$");
                if (regex.IsMatch(w[0])) { tokens.Add(new Tokens(w[1], TokenType.FC, w[0])); continue; }

                else { tokens.Add(new Tokens(w[1], TokenType.IL, w[0])); continue; }
            }
            else if (w[0][0] == '.')
            {
                if (w[0].Length == 1) { tokens.Add(new Tokens(w[1], TokenType.DOT, w[0])); continue; }

                regex = new Regex(@"^[0-9]*[.][0-9]+$");
                if (regex.IsMatch(w[0])) { tokens.Add(new Tokens(w[1], TokenType.FC, w[0])); continue; }

                else { tokens.Add(new Tokens(w[1], TokenType.IL, w[0])); continue; }

            }
            else if (w[0][0] == '"')
            {
                regex = new Regex("^[\"]([\\\\][abfnrtv0\"\'\\\\]|[^(\"\'\\\\)]|[()])*[\"]$");
                if (regex.IsMatch(w[0])) { tokens.Add(new Tokens(w[1], TokenType.SC, w[0])); continue; }

                else { tokens.Add(new Tokens(w[1], TokenType.IL, w[0])); continue; }
            }
            else if (w[0][0] == '\'')
            {
                regex = new Regex("^[\']([\\\\][abfnrtv0\"\'\\\\]|[^(\"\'\\\\)]|[()])[\']$");
                if (regex.IsMatch(w[0])) { tokens.Add(new Tokens(w[1], TokenType.CC, w[0])); continue; }

                else { tokens.Add(new Tokens(w[1], TokenType.IL, w[0])); continue; }

            }
            else
            {
                regex = new Regex("^([a-zA-Z_$][a-zA-Z\\d_$]*)$");
                if (regex.IsMatch(w[0])) { tokens.Add(new Tokens(w[1], TokenType.ID, w[0])); continue; }

                else { tokens.Add(new Tokens(w[1], TokenType.IL, w[0])); continue; }
            }
        }
        return tokens;
    }

    // private bool isNum(char ){

    // }

    // private TokenType checkType(string word)
    // {
    //     if(ht.ContainsKey(word)) 
    //     char? ch = word?[0];
    //         return;
    // }

}