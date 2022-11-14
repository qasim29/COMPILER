using System.Collections;
using System.Collections.Generic;
using System;
public class Syntax_Analyzer
{
    SE_Semantic_Analyzer se = new SE_Semantic_Analyzer();
    Dictionary<string, List<string[]>> rules;
    List<Token> tokens;
    List<Token> ptokens;
    int index = 0;
    // bool expmode = false;
    public Syntax_Analyzer(List<Token> tokens)
    {
        this.rules = new Dictionary<string, List<string[]>>();
        this.ptokens = new List<Token>();
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
        if (helper("<START>") && index >= tokens.Count) { System.Console.WriteLine("INDEX == " + index); return true; }

        else { System.Console.WriteLine("INDEX == " + index); return false; }
    }
    private bool helper(String curNT)
    {
        List<String[]> productionRules = rules[curNT];
        foreach (String[] pr in productionRules)
        {
            int prev = index;
            int j = 0;
            for (; j < pr.Length; j++)
            {
                String element = pr[j];
                if (element[0] == '~') { ++index; return true; }

                else if (element[0] == '<') { if (!helper(element)) { index = prev; break; } }

                else if (element.Length == 1 && element[0] == 'E') continue;

                else
                {
                    if (string.Equals(element, tokens[index].class_Part.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        // if (updateExpMode()){}
                        checkScope();
                        if (tokens[index].class_Part != TokenType.CCB) ptokens.Add(tokens[index]);

                        if (tokens[index].class_Part == TokenType.SEC || tokens[index].class_Part == TokenType.OCB)
                        {
                            if (!secheck()) return false;

                            ptokens.Clear();
                        }
                        index++;
                    }
                    else break;
                }
            }
            if (j == pr.Length) return true;
        }
        return false;
    }










    private bool secheck()
    {
        printPTokens();

        if (ptokens[1].class_Part == TokenType.CLASS || ptokens[2].class_Part == TokenType.CLASS) { return classSE(); }

        else if (ptokens[0].class_Part == TokenType.AM
                || ptokens[0].class_Part == TokenType.ID
                || ptokens[0].class_Part == TokenType.DT) { return VariableSE(); }

        else if (ptokens[0].class_Part == TokenType.FUNC || ptokens[0].class_Part == TokenType.EXECUTE) { return FuncSE(); }

        // else if (ptokens[0].class_Part == TokenType.RETURN) { return ReturnSE(); } //to be implemented

        // else if (ptokens[0].class_Part == TokenType.IF || ptokens[0].class_Part == TokenType.WHILE) { return If_WhileSE(); } //to be implemented

        else if (ptokens[0].class_Part == TokenType.LK) { return true; }

        // else { return SimpleStatementSE(); } //to be implemented
        
        return true;
        
        void printPTokens()
        {
            System.Console.WriteLine("\n\t^^^^-Parsed Tokens List-^^^^");
            ptokens.ForEach(Console.Write); System.Console.WriteLine("");
        }
    }

    private bool SimpleStatementSE()
    {
        throw new NotImplementedException();
    }

    private bool If_WhileSE()
    {
        throw new NotImplementedException();
    }

    private bool ReturnSE()
    {
        throw new NotImplementedException();
    }

    private bool VariableSE()
    {
        string name = "", type = "", am = "", valueType = "";

        bool sta = false, final = false;

        for (int i = 0; i < ptokens.Count; i++)
        {
            if (ptokens[i].class_Part == TokenType.ASI) {} //valueType = getExpType(i + 1); }

            else if (ptokens[i].class_Part == TokenType.AM) { am = ptokens[i].word; }

            else if (ptokens[i].class_Part == TokenType.STATIC) { sta = true; }

            else if (ptokens[i].class_Part == TokenType.CONST) { final = true; }

            else if (ptokens[i].class_Part == TokenType.OSB || ptokens[i].class_Part == TokenType.CSB) { type += ptokens[i].word; }

            else if (ptokens[i].class_Part == TokenType.DT) { type = ptokens[i].word; name = ptokens[i + 1].word; i++; }

            else if (ptokens[i].class_Part == TokenType.ID && ptokens[i + 1].class_Part == TokenType.ID)
            {
                SE_Main_Data_Table? mt = se.lookUpMainTable(ptokens[i].word);

                if (mt == null) { System.Console.WriteLine("\nNo refference found for: " + ptokens[i].word + " on lineNo: " + ptokens[i].lineNo); Environment.Exit(0); }

                if (mt.tm == "ABSTRACT") { System.Console.WriteLine("\nCan't make objects for abstract class: " + ptokens[i].word + "on lineNo: " + ptokens[i].lineNo); Environment.Exit(0); }

                type = ptokens[i].word;
                name = ptokens[i + 1].word;
                i++;
            }
            else if (ptokens[i].class_Part == TokenType.SEC)
            {
                // comentted until expmethod is not writtten
                // if (!type.Contains(valueType))
                // {
                //     System.Console.WriteLine("\nType Mismatch"+ " on lineNo: " + ptokens[i].lineNo);                    
                // }
                if (se.scopeStack.Count == 0)
                {
                    if (!se.insertGlobalData(name, type, sta, final)) { System.Console.WriteLine("\nariable RE-deleared on lineNo: " + ptokens[i].lineNo); Environment.Exit(0); }
                }
                else if (se.curr_class_name != null && ptokens[0].class_Part == TokenType.AM)
                {
                    if (!se.insertClassData(name, type, am, sta, final, false)) { System.Console.WriteLine("\nariable RE-deleared on lineNo: " + ptokens[i].lineNo); Environment.Exit(0); }
                }
                else
                {
                    if (!se.insertFuncTable(name, type)) { System.Console.WriteLine("\nariable RE-deleared on lineNo: " + ptokens[i].lineNo); Environment.Exit(0); }
                }
            }
        }
        return true;
    }

    // private string getExpType(int i)
    // {
    //     String temp = "";
    //     for (; i < ptokens.Count - 1; i++)
    //     {
    //         temp += ptokens[i].word;    
    //         // if (ROP.Contains(ptokens[i].word)) { expROP.Add(ptokens[i].word); }
    //     }

    //     string? type = se.lookUpFuncTable(bits[0]);
    //     if (type == null) { Console.WriteLine("Error at " + ptokens[index].lineNo + ": Variable not declare "); Environment.Exit(0); }
    //     if (bits.Length == 1) { return type; }
    //     for (int j = 1; j < bits.Length; j++)
    //     {
    //         SE_Main_Data_Table? mt = se.lookUpMainTable(type);
    //         if (mt == null) { System.Console.WriteLine("\nNo refference found for: " + type + "  on lineNo: " + ptokens[index].lineNo); Environment.Exit(0); }
    //         if (!mt.cdt.ContainsKey(bits[j])) { System.Console.WriteLine("\nNo refference found for: " + bits[j] + "  on lineNo: " + ptokens[index].lineNo); Environment.Exit(0); }
    //         type = mt.cdt[bits[j]].type;
    //     }
    //     return type;

    // }

    // private string getExpType(int i)
    // {
    //     string[] ROP = { "==", "<=", ">=", "<", ">", "!=" };
    //     char[] operators = { '+', '-', '/', '*', '%' };
    //     List<string> expROP = new List<string>();  // done                      { "<=" }
    //     List<List<char>> expOperators = new List<List<char>>();//done           { {'+'  , '/'}  , {'/'} }
    //     List<string[]> ids = new List<string[]>();  // done                  [ {"a" , "b.fn()" , "c"}  , { "K.fn()" , "c" }  ]
    //     List<List<string>> idsOutput = new List<List<string>>();
    //     string[] ropOutput; // done                                         [ "a + b.fn() / c" , "K.fn() / c" ]
    //     string temp = "";
    //     for (; i < ptokens.Count-1; i++)
    //     {
    //         temp += ptokens[i].word;
    //         if (ROP.Contains(ptokens[i].word)) { expROP.Add(ptokens[i].word); }

    //     }
    //     ropOutput = temp.Split(new[] { "==", "<=", ">=", "<", ">", "!=" }, StringSplitOptions.None);

    //     foreach (string str in ropOutput)
    //     {
    //         List<char> oplist = new List<char>();
    //         foreach (char chr in str)
    //         {
    //             if (operators.Contains(chr)) { oplist.Add(chr); }
    //         }
    //         expOperators.Add(oplist);
    //     }
    //     foreach (string item in ropOutput)
    //     {
    //         ids.Add(temp.Split('+', '-', '/', '*', '%'));
    //     }
    //     foreach (string[] vals in ids)
    //     {
    //         List<string> typelist = new List<string>();
    //         foreach (string val in vals)
    //         {
    //             typelist.Add(getType(val));
    //         }
    //         idsOutput.Add(typelist);
    //     }
    //     foreach (string item in ropOutput) { Console.Write(item + ","); }

    //     return "";
    // }

    // string getType(string exp)
    // {
    //     string[] bits = exp.Split('.');

    //     if (bits[0] == "super")
    //     {
    //         SE_Main_Data_Table? mt; 
    //         mt= se.lookUpMainTable(se.curr_class_name);
    //         if (mt == null) { Console.WriteLine("Error at " + ptokens[index].lineNo + ": Variable not declare "); Environment.Exit(0); }
    //         string? type=mt.extends;
    //         mt= se.lookUpMainTable(type);
    //         if (type == null) { Console.WriteLine("Error at " + ptokens[index].lineNo + ": Variable not declare "); Environment.Exit(0); }
    //         if (bits.Length == 1) { return type; }
    //         for (int j = 1; j < bits.Length; j++)
    //         {
    //             mt = se.lookUpMainTable(type);
    //             if (mt == null) { System.Console.WriteLine("\nNo refference found for: " + type + "  on lineNo: " + ptokens[index].lineNo); Environment.Exit(0); }
    //             if (!mt.cdt.ContainsKey(bits[j])) { System.Console.WriteLine("\nNo refference found for: " + bits[j] + "  on lineNo: " + ptokens[index].lineNo); Environment.Exit(0); }
    //             type = mt.cdt[bits[j]].type;
    //         }
    //         return type;

    //     }

    //     else if (bits[0] == "self") { }

    //     else
    //     {
    //         string? type = se.lookUpFuncTable(bits[0]);
    //         if (type == null) { Console.WriteLine("Error at " + ptokens[index].lineNo + ": Variable not declare "); Environment.Exit(0); }
    //         if (bits.Length == 1) { return type; }
    //         for (int j = 1; j < bits.Length; j++)
    //         {
    //             SE_Main_Data_Table? mt = se.lookUpMainTable(type);
    //                 if (mt == null) { System.Console.WriteLine("\nNo refference found for: " + type + "  on lineNo: " + ptokens[index].lineNo); Environment.Exit(0); }
    //             if (!mt.cdt.ContainsKey(bits[j])) { System.Console.WriteLine("\nNo refference found for: " + bits[j] + "  on lineNo: " + ptokens[index].lineNo); Environment.Exit(0); }
    //             type = mt.cdt[bits[j]].type;
    //         }
    //         return type;
    //     }


    //     return "";
    // }







    private bool FuncSE()
    {
        string name = "", type = "", stype = "", sname = "", am = "";

        bool sta = false, final = false, abstrac = false;

        for (int i = 0; i < ptokens.Count; i++)
        {
            // if((ptokens[i].class_Part == TokenType.ID || ptokens[i].class_Part == TokenType.EXECUTE) && ptokens[i+1].class_Part == TokenType.ORB ){
            //     name = ptokens[i].word;
            // }
            // uppar alternate check hai
            if (ptokens[i].class_Part == TokenType.ORB)
            {
                name = ptokens[i - 1].word;
            }
            else if (ptokens[i].class_Part == TokenType.ID && ptokens[i + 1].class_Part != TokenType.ORB)
            {
                sname = ptokens[i].word;
            }
            else if (ptokens[i].class_Part == TokenType.COL)
            {
                type += "->" + ptokens[i + 1].word;
                i++;
            }
            else if (ptokens[i].class_Part == TokenType.OSB || ptokens[i].class_Part == TokenType.CSB)
            {
                type += ptokens[i].word;
                stype += ptokens[i].word;
            }
            else if (ptokens[i].class_Part == TokenType.AM)
            {
                am = ptokens[i].word;
            }
            else if (ptokens[i].class_Part == TokenType.STATIC)
            {
                sta = true;
            }
            else if (ptokens[i].class_Part == TokenType.ABSTRACT)
            {
                abstrac = true;
            }
            else if (ptokens[i].class_Part == TokenType.CONST)
            {
                final = true;
            }

            else if (ptokens[i].class_Part == TokenType.COM || ptokens[i].class_Part == TokenType.CRB)
            {
                if (ptokens[i].class_Part == TokenType.COM) type += ",";
                if (sname != "")
                {
                    if (!se.insertFuncTable(sname, stype)) return false;
                    sname = "";
                    stype = "";
                }
            }
            else if ((ptokens[i].class_Part == TokenType.DT || ptokens[i].class_Part == TokenType.ID) && ptokens[i + 1].class_Part == TokenType.ID)
            {
                if (ptokens[i].class_Part == TokenType.ID)
                {
                    SE_Main_Data_Table? mt = se.lookUpMainTable(ptokens[i].word);

                    if (mt == null) { System.Console.WriteLine("\nNo refference found for: " + ptokens[i].word + " on lineNo: " + ptokens[i].lineNo); Environment.Exit(0); }

                    if (mt.tm == "ABSTRACT") { System.Console.WriteLine("\nCan't make objects for abstract class: " + ptokens[i].word + "on lineNo: " + ptokens[i].lineNo); Environment.Exit(0); }
                }
                type += ptokens[i].word;
                stype = ptokens[i].word;
            }
            else if (ptokens[i].class_Part == TokenType.OCB || ptokens[i].class_Part == TokenType.SEC)
            {
                if (ptokens[1].class_Part != TokenType.AM)
                {
                    if (!se.insertGlobalData(name, type, sta, final)) { System.Console.WriteLine("\nFunction RE-deleared on lineNo: " + ptokens[i].lineNo); Environment.Exit(0); }
                }
                else
                {
                    if (!type.Contains("->") && se.curr_class_name != name) { System.Console.WriteLine("\nInValid Constructor Declaration on lineNo: " + ptokens[i].lineNo); Environment.Exit(0); }

                    if (!type.Contains("->")) type += "->";

                    if (!se.insertClassData(name, type, am, sta, final, abstrac)) { System.Console.WriteLine("\nFunction RE-deleared on lineNo: " + ptokens[i].lineNo); Environment.Exit(0); }
                }
            }
        }
        return true;
    }
    private bool classSE()
    {
        string name = "";
        string type = "";
        string? tm = null;
        string? ext = null;

        for (int i = 0; i < ptokens.Count; i++)
        {
            if (ptokens[i].class_Part == TokenType.CLASS)
            {
                i++;
                type = "CLASS";
                name = ptokens[i].word;
            }

            else if (ptokens[i].class_Part == TokenType.CONST || ptokens[i].class_Part == TokenType.ABSTRACT) tm = ptokens[i].class_Part.ToString();

            else if (ptokens[i].class_Part == TokenType.CHILDOF)
            {
                i++;
                for (; i < ptokens.Count; i++)
                {
                    if (ptokens[i].class_Part == TokenType.OCB) continue;

                    else if (ptokens[i].class_Part == TokenType.COM) ext += ",";

                    else ext += ptokens[i].word.ToString();
                }
            }
        }

        if (se.lookUpMainTable(name) != null) { System.Console.WriteLine("\nRe-Decleared class:" + name + " at lineNo: " + tokens[index - 1].lineNo); return false; }

        else if (se.lookUpMainTable(ext) == null && ext != null) { System.Console.WriteLine("\nParent class : " + ext + " isn't Decleared"); return false; }

        else if (se.lookUpMainTable(ext) != null)
        {
            if (se.lookUpMainTable(ext)?.tm == "CONST") { System.Console.WriteLine("\nParent class : " + ext + " is Decleared as FINAL class"); return false; }
        }
        se.curr_class_name = name;
        return se.insertMainTable(name, type, tm, ext);
    }
    // private bool updateExpMode()
    // {
    //     if (tokens[index].class_Part.ToString() == "ORB" || tokens[index].class_Part.ToString() == "ASI" || tokens[index].class_Part.ToString() == "OSB") { expmode = true; }
    //     else if (tokens[index].class_Part.ToString() == "CRB" || tokens[index].class_Part.ToString() == "SC" || tokens[index].class_Part.ToString() == "CSB") { expmode = false; getType(expression); }
    //     return expmode;
    // }
    public void checkScope()
    {
        // System.Console.WriteLine("Matched Terminal = (" + tokens[index].class_Part.ToString() + ", " + tokens[index].word + ")");

        if (tokens[index].class_Part == TokenType.CLASS) { se.scopeStack.Add(0); System.Console.WriteLine("Matched Terminal = (" + tokens[index].class_Part.ToString() + ", " + tokens[index].word + ")"); printScopeStack(); }

        else if (tokens[index].class_Part == TokenType.ORB && (tokens[index - 1].class_Part == TokenType.ID || tokens[index - 1].class_Part == TokenType.EXECUTE)) { se.createScope(); System.Console.WriteLine("Matched Terminal = (" + tokens[index].class_Part.ToString() + ", " + tokens[index].word + ")"); printScopeStack(); }

        else if (tokens[index].class_Part == TokenType.OCB && tokens[index - 1].class_Part == TokenType.CRB && ptokens[0].class_Part != TokenType.FUNC) { se.createScope(); System.Console.WriteLine("Matched Terminal = (" + tokens[index].class_Part.ToString() + ", " + tokens[index].word + ")"); printScopeStack(); }

        else if (tokens[index].class_Part == TokenType.CCB) { se.destroyScope(); System.Console.WriteLine("Matched Terminal = (" + tokens[index].class_Part.ToString() + ", " + tokens[index].word + ")"); printScopeStack(); }
    }
    private void printScopeStack()
    {
        System.Console.WriteLine("*********************");
        System.Console.WriteLine("Scope Count: " + se.scope);
        System.Console.WriteLine("--scope stack--");
        System.Console.Write("[ ");
        foreach (int val in se.scopeStack) System.Console.Write(val + ",");
        System.Console.Write(" ]");
        System.Console.WriteLine("\n*********************");
    }

}
