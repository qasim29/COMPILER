using System.Collections;
using System.Text.RegularExpressions;

class Lexical_Analyzer
{
    List<Token> tokens;
    public Dictionary<string,TokenType> ht;
    Regex? regex;
    public Lexical_Analyzer()
    {
        tokens = new List<Token>();
        ht = new Dictionary<string,TokenType>()
        {
            // ARITHEMATIC & LOGICAL OPERATORS
            {"+",TokenType.PM},         {"-",TokenType.PM},    
            {"/",TokenType.MDM},        {"%",TokenType.MDM},
            {"*",TokenType.MDM},        
            // {"!",TokenType.NOT},   language doesn't support this keyword
            //  COMPARISION OPERATORS
            {">",TokenType.COMP},       {"<",TokenType.COMP},
            {"<=",TokenType.COMP},      {">=",TokenType.COMP},
            {"==",TokenType.COMP},      {"!=",TokenType.COMP},
            {"=",TokenType.ASI},        {"return",TokenType.RETURN},         
            // {"and",TokenType.AND},{"or",TokenType.OR},   language doesn't support this keyword     
            
            // BRACKETS
            {"(",TokenType.ORB},        {")",TokenType.CRB},
            {"{",TokenType.OCB},        {"}",TokenType.CCB},
            {"[",TokenType.OSB},        {"]",TokenType.CSB},
            // CONDITIONAL STATEMENT
            {"if",TokenType.IF},        {"else",TokenType.ELSE},
            // LOOP             
            {"while",TokenType.WHILE},  {"break",TokenType.LK},
            {"continue",TokenType.LK}, 
            // DATA TYPES
            {"int",TokenType.DT},       {"float",TokenType.DT},
            {"string",TokenType.DT},    {"char",TokenType.DT},
            // PUNCTUATORS
            {";",TokenType.SEC},        {":", TokenType.COL},
            {",",TokenType.COM},        {".",TokenType.DOT},  //added dot in case if bugs occurs in future
            // OOP
            {"null",TokenType.NULL},    {"abstract",TokenType.ABSTRACT},  
            {"static",TokenType.STATIC},   {"create",TokenType.CREATE},
            {"class",TokenType.CLASS},    {"self",TokenType.SELF},
            {"super",TokenType.SUPER},    {"const",TokenType.CONST},
            {"child_of",TokenType.CHILDOF}, 
            //
            {"func",TokenType.FUNC},    {"void",TokenType.VOID},
            {"execute",TokenType.EXECUTE},

            // ACCESS MODIFIERS
            {"local",TokenType.AM},    {"global",TokenType.AM},
            {"protected",TokenType.AM}  //since we will work on single code file I've dropped access modifiers
        };
    

    }


    public List<Token> GetTokens(List<string[]> words)
    {
        foreach (string[] w in words)
        {
            if (ht.ContainsKey(w[0])) { tokens.Add(new Token(w[1], ht[w[0]], w[0])); continue; }

            else if (Char.IsNumber(w[0][0]))
            {
                regex = new Regex(@"^[0-9]+$");
                if (regex.IsMatch(w[0])) { tokens.Add(new Token(w[1], TokenType.IC, w[0])); continue; }

                regex = new Regex(@"^[0-9]*[.][0-9]+$");
                if (regex.IsMatch(w[0])) { tokens.Add(new Token(w[1], TokenType.FC, w[0])); continue; }

                else { tokens.Add(new Token(w[1], TokenType.IL, w[0])); continue; }
            }
            else if (w[0][0] == '.')
            {
                if (w[0].Length == 1) { tokens.Add(new Token(w[1], TokenType.DOT, w[0])); continue; }

                regex = new Regex(@"^[0-9]*[.][0-9]+$");
                if (regex.IsMatch(w[0])) { tokens.Add(new Token(w[1], TokenType.FC, w[0])); continue; }

                else { tokens.Add(new Token(w[1], TokenType.IL, w[0])); continue; }

            }
            else if (w[0][0] == '"')
            {
                regex = new Regex("^[\"]([\\\\][abfnrtv0\"\'\\\\]|[^(\"\'\\\\)]|[()])*[\"]$");
                if (regex.IsMatch(w[0])) { tokens.Add(new Token(w[1], TokenType.SC, w[0].Trim('"'))); continue; }

                else { tokens.Add(new Token(w[1], TokenType.IL, w[0])); continue; }
            }
            else if (w[0][0] == '\'')
            {
                regex = new Regex("^[\']([\\\\][abfnrtv0\"\'\\\\]|[^(\"\'\\\\)]|[()])[\']$");
                if (regex.IsMatch(w[0])) { tokens.Add(new Token(w[1], TokenType.CC, w[0].Trim('\''))); continue; }

                else { tokens.Add(new Token(w[1], TokenType.IL, w[0])); continue; }

            }
            else
            {
                regex = new Regex("^([a-zA-Z_$][a-zA-Z\\d_$]*)$");
                if (regex.IsMatch(w[0])) { tokens.Add(new Token(w[1], TokenType.ID, w[0])); continue; }

                else { tokens.Add(new Token(w[1], TokenType.IL, w[0])); continue; }
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