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
    bool bracktCheck = true;
    string? exptype;
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

                if (element == "<EXP_F>" || element == "<EXP_F>")
                {
                    Exp e = new Exp();
                    RE(e);
                    index++;
                }

                else if (element[0] == '<') { if (!helper(element)) { index = prev; break; } }

                else if (element.Length == 1 && element[0] == 'E') continue;

                else
                {
                    if (string.Equals(element, tokens[index].class_Part.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        checkScope();

                        if (tokens[index].class_Part == TokenType.OCB && tokens[index - 1].class_Part == TokenType.ASI) bracktCheck = false;

                        if (tokens[index].class_Part != TokenType.CCB || bracktCheck == false) ptokens.Add(tokens[index]);

                        if (tokens[index].class_Part == TokenType.SEC || (tokens[index].class_Part == TokenType.CCB && bracktCheck))
                        {
                            if (!secheck()) return false;

                            ptokens.Clear();
                            bracktCheck = true;
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

    public bool F(Exp exp)
    {
        if (tokens[index].class_Part == TokenType.SUPER
        || tokens[index].class_Part == TokenType.SELF
        || tokens[index].class_Part == TokenType.ID
        || tokens[index].class_Part == TokenType.IC
        || tokens[index].class_Part == TokenType.FC
        || tokens[index].class_Part == TokenType.SC
        || tokens[index].class_Part == TokenType.CC
        || tokens[index].class_Part == TokenType.ORB
        || tokens[index].class_Part == TokenType.CREATE)
        {
            if (tokens[index].class_Part == TokenType.SUPER || tokens[index].class_Part == TokenType.SELF || tokens[index].class_Part == TokenType.ID)
            {
                if (TS(exp))
                {
                    if (tokens[index].class_Part == TokenType.ID)
                    {
                            
                        exp.setN(tokens[index].word);
                        index++;
                        if (O(exp))
                        {
                            return true;
                        }
                    }
                }
            }
            else if (tokens[index].class_Part == TokenType.ORB)
            {
                index++;
                // Exp ex = new Exp();
                if (exp(exp))
                {
                    if (tokens[index].class_Part == TokenType.CRB) {
                        index++;
                        return true;
                    }
                }
            }
            else if (tokens[index].class_Part == TokenType.CREATE)
            {
                if (obj_dec())
                {
                    if (O_(exp))
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (Const(exp))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool Q_(Exp exp)
    {
        if (tokens[index].class_Part == TokenType.MDM)
        {
            exp.setOp(tokens[index].word);
            index++;
            Exp ex = new Exp();
            if (F(ex))
            {
                String
                 t3 = se.compatibility(exp.type, ex.type, exp.getOp());
                exp.setT(t3);
                if (Q_(exp))
                {
                    return true;
                }
            }
        }
        else if (tokens[index].class_Part == TokenType.PM
        || tokens[index].class_Part == TokenType.COMP
        || tokens[index].class_Part == TokenType.COM
        || tokens[index].class_Part == TokenType.CRB
        || tokens[index].class_Part == TokenType.CCB
        || tokens[index].class_Part == TokenType.CSB
        || tokens[index].class_Part == TokenType.SEC)
        {
            return true;
        }

        return false;
    }

    public bool Q(Exp exp)
    {
        if (tokens[index].class_Part == TokenType.SUPER
        || tokens[index].class_Part == TokenType.SELF
        || tokens[index].class_Part == TokenType.ID
        || tokens[index].class_Part == TokenType.IC
        || tokens[index].class_Part == TokenType.FC
        || tokens[index].class_Part == TokenType.SC
        || tokens[index].class_Part == TokenType.CC
        || tokens[index].class_Part == TokenType.ORB
        || tokens[index].class_Part == TokenType.CREATE)
        {
            if (F(exp))
            {
                if (Q_(exp))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool E_(Exp exp)
    {
        if (tokens[index].class_Part == TokenType.PM)
        {
            exp.setOp(tokens[index].word);
            index++;
            Exp ex = new Exp();
            if (Q(ex))
            {
                String t3 = se.compatibility(exp.type, ex.type, exp.getOp());
                exp.setT(t3);
                if (E_(exp))
                {
                    return true;
                }
            }
        }
        else if (tokens[index].class_Part == TokenType.COM
        || tokens[index].class_Part == TokenType.CRB
        || tokens[index].class_Part == TokenType.CCB
        || tokens[index].class_Part == TokenType.CSB
        || tokens[index].class_Part == TokenType.SEC)
        {
            return true;
        }
        return false;
    }

    public bool E(Exp exp)
    {
        if (tokens[index].class_Part == TokenType.SUPER
        || tokens[index].class_Part == TokenType.SELF
        || tokens[index].class_Part == TokenType.ID
        || tokens[index].class_Part == TokenType.IC
        || tokens[index].class_Part == TokenType.FC
        || tokens[index].class_Part == TokenType.SC
        || tokens[index].class_Part == TokenType.CC
        || tokens[index].class_Part == TokenType.ORB
        || tokens[index].class_Part == TokenType.CREATE)
        {
            if (Q(exp))
            {
                if (E_(exp))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool RE_(Exp exp)
    {
        if (tokens[index].class_Part == TokenType.COMP)
        {
            exp.setOp(tokens[index].word);
            index++;
            Exp ex = new Exp();
            if (E(ex))
            {
                exp.setT(se.compatibility(exp.type, ex.type, exp.getOp()));
                if (RE_(exp))
                {
                    return true;
                }
            }
        }
        else if (tokens[index].class_Part == TokenType.COM
        || tokens[index].class_Part == TokenType.CRB
        || tokens[index].class_Part == TokenType.CCB
        || tokens[index].class_Part == TokenType.CSB
        || tokens[index].class_Part == TokenType.SEC)
        {
            return true;
        }

        return false;
    }
    public bool RE(Exp exp)
    {
        if (ptokens[index].class_Part == TokenType.SUPER
        || ptokens[index].class_Part == TokenType.SELF
        || ptokens[index].class_Part == TokenType.ID
        || ptokens[index].class_Part == TokenType.IC
        || ptokens[index].class_Part == TokenType.FC
        || ptokens[index].class_Part == TokenType.SC
        || ptokens[index].class_Part == TokenType.CC
        || ptokens[index].class_Part == TokenType.ORB
        || ptokens[index].class_Part == TokenType.CREATE)
        {

            if (E(exp))
            {
                if (RE_(exp))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool obj_dec() {
        if (tokens[index].class_Part == TokenType.CREATE) {
            index++;
            if (tokens[index].class_Part == TokenType.ID) {
                index++;
                if (tokens[index].class_Part == TokenType.CRB) {
                    index++;
                    if (argu(null)) {
                        if (tokens[index].class_Part == TokenType.ORB) {
                            index++;
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

 public bool O(Exp exp) {
        if (tokens[index].class_Part == TokenType.OSB) {
            index++;
            Exp ex = new Exp();
            if (exp(ex)) {
                if (!ex.type.Equals("int")) {
                    Console.WriteLine("Error at " + tokens[index].lineNo + "  index must be an integer value");
                    Environment.Exit(0);
                }
                if (tokens[index].class_Part == TokenType.CSB) {
                    index++;
                    if (opt(exp)) {
                        if (O_(exp)) {
                            return true;
                        }
                    }
                }
            }
        } else if (tokens[index].class_Part == TokenType.CRB) {
            index++;
            Exp ex = new Exp();
            if (argu(ex)) {
                if (tokens[index].class_Part == TokenType.CCB) {
                    exp.setT(ex.type);
                    index++;
                    if (exp.getR().Equals("") || exp.getR().Equals("self")) {
                        SE_Class_Data_Table? c = se.lookUpDataTable(exp.name, ex.type, se.curr_class_name);
                        if (c != null) {
                            String[] T = c.type.Split("->");
                            exp.setT(T[1]);
                        } else {
                            Console.WriteLine("Error at " + tokens[index].lineNo + " Function not declare");
                            Environment.Exit(0);
                        }

                    } else if (exp.getR().Equals("super")) {
                        SE_Main_Data_Table? m = se.lookUpMainTable(se.curr_class_name);
                        if (m.extends == null || m.extends.Equals("")) {
                            Console.WriteLine("Current Class does not have parent class");
                            Environment.Exit(0);
                        } else {
                            SE_Class_Data_Table? c = se.lookUpDataTable(exp.name, exp.type, m.extends);
                            if (c != null) {
                                String[] T = c.type.Split("->");
                                exp.setT(T[1]);
                            } else {
                                Console.WriteLine("Error at " + tokens[index].lineNo + "Function not declare");
                                Environment.Exit(0);
                            }
                            
                        }
                    } else if (exp.getR().Equals("int") || exp.getR().Equals("float") || exp.getR().Equals("char")
                            || exp.getR().Equals("String") || exp.getR().Equals("bool")) {
                        Console.WriteLine("Error at " + tokens[index].lineNo + "Primitive data Type cant be instantiated");
                        Environment.Exit(0);
                    } else if (exp.getR().Equals("void")) {
                        Console.WriteLine("Error at " + tokens[index].lineNo + "Return type is void");
                        Environment.Exit(0);
                    } else if (exp.getR().Equals("constructor") || exp.getR().Equals("None")) {
                        SE_Class_Data_Table? c = se.lookUpDataTable(exp.name, exp.type, exp.getR());
                        if (c == null) {
                            while (true) {
                                SE_Main_Data_Table? m = se.lookUpMainTable(exp.getR());
                                if (m == null) {
                                    Console.WriteLine("Error at " + tokens[index].lineNo + " :Undeclared");
                                    Environment.Exit(0);
                                } else if (m.extends == null || m.extends.Equals("")) {
                                    Console.WriteLine("Error at " + tokens[index].lineNo + " :Undeclared");
                                    Environment.Exit(0);
                                } else {
                                    c = se.lookUpDataTable(exp.name, exp.type, m.extends);
                                    if (c == null) {
                                        exp.setR(m.extends);
                                    } else if (c.am.Equals("local") && !c.name.Equals(se.curr_class_name)) {
                                        Console.WriteLine("Error at : " + tokens[index].lineNo + "private attribute or function cant be accessed outside the class");
                                        Environment.Exit(0);
                                    } else {
                                        String[] Ty = c.type.Split("->");
                                        exp.setT(Ty[1]);
                                        break;
                                    }

                                }
                            }
                        } else if (c.am.Equals("local") && !c.name.Equals(se.curr_class_name)) {
                            Console.WriteLine("Error at : " + tokens[index].lineNo  + "private attribute or function cant be accessed outside the class");
                            Environment.Exit(0);
                        }
                        String[] T = c.type.Split("->");
                        exp.setT(T[1]);
                    } else {
                        SE_Class_Data_Table? c = se.lookUpDataTable(exp.name, exp.type, exp.getR());
                        if (c == null) {
                            while (true) {
                                SE_Main_Data_Table? m = se.lookUpMainTable(exp.getR());
                                if (m == null) {
                                    Console.WriteLine("Error at " + tokens[index].lineNo + " :Undeclared");
                                    Environment.Exit(0);
                                } else if (m.extends == null || m.extends.Equals("")) {
                                    Console.WriteLine("Error at " + tokens[index].lineNo + " :Undeclared");
                                    Environment.Exit(0);
                                } else {
                                    c = se.lookUpDataTable(exp.name, exp.type, m.extends);
                                    if (c == null) {
                                        exp.setR(m.extends);
                                    } else if (c.am.Equals("local") && !c.name.Equals(se.curr_class_name)) {
                                        Console.WriteLine("Error at : " + tokens[index].lineNo + "private attribute or function cant be accessed outside the class");
                                        Environment.Exit(0);
                                    } else {
                                        String[] T = c.type.Split("->");
                                        exp.setT(T[1]);
                                        break;
                                    }

                                }
                            }
                        } else if (c.am.Equals("local") && !c.name.Equals(se.curr_class_name)) {
                            Console.WriteLine("Error at : " + tokens[index].lineNo + " private attribute or function cant be accessed outside the class");
                            Environment.Exit(0);
                        }
                        String[] T = c.type.Split("->");
                        exp.setT(T[1]);
                    }
                    if (O_(exp)) {
                        return true;
                    }
                }
            }
        } else if (tokens[index].class_Part == TokenType.DOT) {
            if (exp.getR().Equals("")) {
                String t = se.lookUpFuncTable(exp.name);
                if (t != null) {
                    exp.setT(t);
                } else {
                    SE_Main_Data_Table? m = se.lookUpMainTable(exp.name);
                    if (m != null) {
                        if (m.tm == null || !m.tm.Equals("static")) {
                            Console.WriteLine("Error at : " + tokens[index].lineNo + " variable should be static");
                            Environment.Exit(0);
                        }
                        exp.setR(m.name);
                    } else {
                        Console.WriteLine("Error at " + tokens[index].lineNo + ": Variable not declare ");
                        Environment.Exit(0);
                    }
                }

            } else if (exp.getR().Equals("self")) {
                SE_Class_Data_Table? c = se.lookUpDataTable(exp.name, se.curr_class_name);
                if (c != null) {

                    exp.setT(c.type);

                } else {
                    Console.WriteLine("Error at " + tokens[index].lineNo + ": Variable not declare ");
                    Environment.Exit(0);
                }
            } else if (exp.getR().Equals("super")) {
                SE_Main_Data_Table? m = se.lookUpMainTable(se.curr_class_name);
                if (m.extends == null || m.extends.Equals("")) {
                    Console.WriteLine("Current Class does not have parent class");
                    Environment.Exit(0);
                } else {
                    SE_Class_Data_Table? c = se.lookUpDataTable(exp.name, m.extends);
                    if (c != null) {

                        exp.setT(c.type);

                    } else {
                        Console.WriteLine("Error at " + tokens[index].lineNo + ": Variable not declare ");
                        Environment.Exit(0);
                    }
                }
            } else if (exp.getR().Equals("int") || exp.getR().Equals("float") || exp.getR().Equals("char")
                    || exp.getR().Equals("String") || exp.getR().Equals("bool")) {
                Console.WriteLine("Error at " + tokens[index].lineNo + "Primitive data Type cant be instantiated");
                Environment.Exit(0);
            } else if (exp.getR().Equals("void")) {
                Console.WriteLine("Error at " + tokens[index].lineNo + "Return type is void");
                Environment.Exit(0);
            } else {
                SE_Class_Data_Table? c = se.lookUpDataTable(exp.name, exp.getR());
                if (c == null) {
                    while (true) {
                        SE_Main_Data_Table? m = se.lookUpMainTable(exp.getR());
                        if (m == null) {
                            Console.WriteLine("Error at " + tokens[index].lineNo + " :Undeclared");
                            Environment.Exit(0);
                        } else if (m.extends == null || m.extends.Equals("")) {
                            Console.WriteLine("Error at " + tokens[index].lineNo + " :Undeclared");
                            Environment.Exit(0);
                        } else {
                            c = se.lookUpDataTable(exp.name, m.extends);
                            if (c == null) {
                                exp.setR(m.extends);
                            } else if (c.am.Equals("local") && !c.name.Equals(se.curr_class_name)) {
                                Console.WriteLine("Error at : " + tokens[index].lineNo + "private attribute or function cant be accessed outside the class");
                                Environment.Exit(0);
                            } else {
                                exp.setT(c.type);
                                break;
                            }

                        }
                    }
                } else if (c.am.Equals("local") && !c.name.Equals(se.curr_class_name)) {
                    Console.WriteLine("Error at : " + tokens[index].lineNo + "private attribute or function cant be accessed outside the class");
                    Environment.Exit(0);
                } else {
                    exp.setT(c.type);
                }

            }
            if (O_(exp)) {
                return true;
            }
        } 
        else if (
            tokens[index].class_Part == TokenType.MDM 
        || tokens[index].class_Part == TokenType.COM 
        || tokens[index].class_Part == TokenType.COMP 
        || tokens[index].class_Part == TokenType.CRB
        || tokens[index].class_Part == TokenType.CCB 
        || tokens[index].class_Part == TokenType.CSB 
        || tokens[index].class_Part == TokenType.SEC 
        || tokens[index].class_Part == TokenType.PM) 
        {
            if (exp.getR().Equals("")) {
                String t = se.lookUpFuncTable(exp.name);
                if (t != null) {
                    exp.setT(t);
                } else {
                    SE_Main_Data_Table? m = se.lookUpMainTable(exp.name);
                    if (m != null) {
                        if (m.tm == null || !m.tm.Equals("static")) {
                            Console.WriteLine("Error at : " + tokens[index].lineNo + " variable should be static");
                            Environment.Exit(0);
                        }
                        exp.setR(m.name);
                    } else {
                        Console.WriteLine("Error at " + tokens[index].lineNo + ": Variable not declare ");
                        Environment.Exit(0);
                    }
                }

            } else if (exp.getR().Equals("self")) {
                SE_Class_Data_Table? c = se.lookUpDataTable(exp.name, se.curr_class_name);
                if (c != null) {

                    exp.setT(c.type);

                } else {
                    Console.WriteLine("Error at " + tokens[index].lineNo + ": Variable not declare ");
                    Environment.Exit(0);
                }
                
            } else if (exp.getR().Equals("super")) {
                SE_Main_Data_Table? m = se.lookUpMainTable(se.curr_class_name);
                if (m.extends == null || m.extends.Equals("")) {
                    Console.WriteLine("Current Class does not have parent class");
                    Environment.Exit(0);
                } else {
                    SE_Class_Data_Table? c = se.lookUpDataTable(exp.name, m.extends);
                    if (c != null) {

                        exp.setT(c.type);

                    } else {
                        Console.WriteLine("Error at " + tokens[index].lineNo + ": Variable not declare ");
                        Environment.Exit(0);
                    }
                }
            } else if (exp.getR().Equals("int") 
            || exp.getR().Equals("float") 
            || exp.getR().Equals("char")
            || exp.getR().Equals("String") 
            || exp.getR().Equals("bool")) 
            {
                Console.WriteLine("Error at " + tokens[index].lineNo + "Primitive data Type cant be instantiated");
                Environment.Exit(0);
            } else if (exp.getR().Equals("void")) {
                Console.WriteLine("Error at " + tokens[index].lineNo + "Return type is void");
                Environment.Exit(0);
            } else {
                SE_Class_Data_Table? c = se.lookUpDataTable(exp.name, exp.getR());
                if (c == null) {
                    while (true) {
                        SE_Main_Data_Table? m = se.lookUpMainTable(exp.getR());
                        if (m == null) {
                            Console.WriteLine("Error at " + tokens[index].lineNo + " :Undeclared");
                            Environment.Exit(0);
                        } else if (m.extends == null || m.extends.Equals("")) {
                            Console.WriteLine("Error at " + tokens[index].lineNo + " :Undeclared");
                            Environment.Exit(0);
                        } else {
                            c = se.lookUpDataTable(exp.name, m.extends);
                            if (c == null) {
                                exp.setR(m.extends);
                            } else if (c.am.Equals("local") && !c.name.Equals(se.curr_class_name)) {
                                Console.WriteLine("Error at : " + tokens[index].lineNo + "private attribute or function cant be accessed outside the class");
                                Environment.Exit(0);
                            } else {
                                exp.setT(c.type);
                                break;
                            }

                        }
                    }
                } else if (c.am.Equals("local") && !c.name.Equals(se.curr_class_name)) {
                    Console.WriteLine("Error at : " + tokens[index].lineNo + "private attribute or function cant be accessed outside the class");
                    Environment.Exit(0);
                } else {
                    exp.setT(c.type);
                }

            }
            return true;
        }

        return false;
    }


















    bool secheck()
    {
        printPTokens();

        if (ptokens[1].class_Part == TokenType.CLASS || ptokens[2].class_Part == TokenType.CLASS) { return classSE(); }

        else if (ptokens[0].class_Part == TokenType.AM ||
                 ptokens[0].class_Part == TokenType.ID ||
                 ptokens[0].class_Part == TokenType.DT) { return VariableSE(); }

        else if (ptokens[0].class_Part == TokenType.FUNC || ptokens[0].class_Part == TokenType.EXECUTE) { return FuncSE(); }

        // else if (ptokens[0].class_Part == TokenType.RETURN) { return ReturnSE(); } //to be implemented

        // else if (ptokens[0].class_Part == TokenType.IF || ptokens[0].class_Part == TokenType.WHILE) { return If_WhileSE(); } //to be implemented

        else if (ptokens[0].class_Part == TokenType.LK) { return true; }

        // else { return SimpleStatementSE(); } //to be implemented

        return true;

        void printPTokens()
        {
            System.Console.WriteLine("\n\t---------Parsed Tokens List--------");
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
        int i;
        for (i = 0; i < ptokens.Count; i++)
        {
            if (ptokens[i].class_Part == TokenType.ASI) { valueType = getExpType(i + 1, ptokens.Count); break; }

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
        }
        if (!type.Contains(valueType))
        {
            SE_Main_Data_Table? mt = null;
            string? btype = valueType; // child

            while (type != btype)
            {
                mt = se.lookUpMainTable(btype);

                if (mt == null) { System.Console.WriteLine("\nType Mismatch" + " on lineNo: " + ptokens[i].lineNo); Environment.Exit(0); }
                else
                {
                    btype = mt.extends;
                }
            }
        }

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
        return true;
    }

    private string getExpType(int i, int j)
    {
        string type = "", name = "";

        for (; i <= j; i++)
        {
            if (ptokens[i].class_Part == TokenType.CREATE)
            {
                //  create person(a+1 ,' b*c)
                //  arr[a*b ]' ;
                name = ptokens[i + 1].word;
                i = i + 3;
                // in recurssion below for loop code wil be switchecd with a recursive call.
                // now we will face error to get value of j so we make a loop till to get thge value of j
                // modify this when exp is done
                for (; i < j - 2; i++)
                {
                    type += ptokens[i].word;
                }
                SE_Main_Data_Table? mt = se.lookUpMainTable(name);
                if (mt == null) { System.Console.WriteLine("\nNo refference found for: " + name + " on lineNo: " + ptokens[i - 1].lineNo); Environment.Exit(0); }

                else if (!mt.cdt.ContainsKey(name + ":" + type)) { System.Console.WriteLine("\nNo constructor found for: " + name + " on lineNo: " + ptokens[i - 1].lineNo); Environment.Exit(0); }

                return name;
            }


        }

        // if (ptokens[i].class_Part == TokenType.OCB)
        // {          
        //     name = ptokens[i + 1].word;
        //     i = i + 3;
        //     // modify this when exp is done
        //     for (; i < j - 2; i++)
        //     {
        //         type += ptokens[i].word;
        //     }
        //     SE_Main_Data_Table? mt = se.lookUpMainTable(name);
        //     if (mt == null) { System.Console.WriteLine("\nNo refference found for: " + name + " on lineNo: " + ptokens[i - 1].lineNo); Environment.Exit(0); }

        //     else if (!mt.cdt.ContainsKey(name + ":" + type)) { System.Console.WriteLine("\nNo constructor found for: " + name + " on lineNo: " + ptokens[i - 1].lineNo); Environment.Exit(0); }

        //     return name;
        // }




        // String temp = "";
        // for (; i < ptokens.Count - 1; i++)
        // {
        //     temp += ptokens[i].word;
        //     // if (ROP.Contains(ptokens[i].word)) { expROP.Add(ptokens[i].word); }
        // }

        // string? type = se.lookUpFuncTable(bits[0]);
        // if (type == null) { Console.WriteLine("Error at " + ptokens[index].lineNo + ": Variable not declare "); Environment.Exit(0); }
        // if (bits.Length == 1) { return type; }
        // for (int j = 1; j < bits.Length; j++)
        // {
        //     SE_Main_Data_Table? mt = se.lookUpMainTable(type);
        //     if (mt == null) { System.Console.WriteLine("\nNo refference found for: " + type + "  on lineNo: " + ptokens[index].lineNo); Environment.Exit(0); }
        //     if (!mt.cdt.ContainsKey(bits[j])) { System.Console.WriteLine("\nNo refference found for: " + bits[j] + "  on lineNo: " + ptokens[index].lineNo); Environment.Exit(0); }
        //     type = mt.cdt[bits[j]].type;
        // }
        return type;

    }

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

        else if (se.lookUpMainTable(ext) == null && ext != null) { System.Console.WriteLine("\nParent class : " + ext + " isn't Decleared" + " at lineNo: " + tokens[index - 1].lineNo); return false; }

        else if (se.lookUpMainTable(ext) != null)
        {
            if (se.lookUpMainTable(ext)?.tm == "CONST") { System.Console.WriteLine("\nParent class : " + ext + " is Decleared as FINAL class" + " at lineNo: " + tokens[index - 1].lineNo); return false; }
        }
        se.curr_class_name = name;
        return se.insertMainTable(name, type, tm, ext);
    }
    public void checkScope()
    {

        if (tokens[index].class_Part == TokenType.CLASS) { se.scopeStack.Add(0); System.Console.WriteLine("Matched Terminal = (" + tokens[index].class_Part.ToString() + ", " + tokens[index].word + ")"); printScopeStack(); }

        else if (tokens[index].class_Part == TokenType.OCB && tokens[index - 1].class_Part == TokenType.CRB && ptokens[0].class_Part != TokenType.FUNC) { se.createScope(); System.Console.WriteLine("Matched Terminal = (" + tokens[index].class_Part.ToString() + ", " + tokens[index].word + ")"); printScopeStack(); }

        else if (tokens[index].class_Part == TokenType.ORB && (ptokens[0].class_Part == TokenType.FUNC || ptokens[0].class_Part == TokenType.EXECUTE)) { se.createScope(); System.Console.WriteLine("Matched Terminal = (" + tokens[index].class_Part.ToString() + ", " + tokens[index].word + ")"); printScopeStack(); }

        else if (tokens[index].class_Part == TokenType.CCB && bracktCheck) { System.Console.WriteLine("Matched Terminal = (" + tokens[index - 1].class_Part.ToString() + ", " + tokens[index].word + ")"); se.destroyScope(); printScopeStack(); }

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
