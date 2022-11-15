using System.Collections;
class SE_Semantic_Analyzer
{
    public Dictionary<string, SE_Func_Data_Table> function_table;
    public Dictionary<string, SE_Global_Data_Table> global_table;
    public Dictionary<string, SE_Main_Data_Table> main_table;
    public List<int> scopeStack = new List<int>();
    public string? curr_class_name = null;
    public int scope;
    public SE_Semantic_Analyzer()
    {
        this.global_table = new Dictionary<string, SE_Global_Data_Table>();
        this.main_table = new Dictionary<string, SE_Main_Data_Table>();
        this.function_table = new Dictionary<string, SE_Func_Data_Table>();
    }
    public bool insertGlobalData(string name, string type, bool sta, bool final)
    {
        if (global_table.ContainsKey(name)) return false;
        global_table[name] = new SE_Global_Data_Table(name, type, sta, final);
        printGTable();
        return true;
    }
    public bool insertMainTable(string name, string type, string? tm, string? extends)
    {
        if (main_table.ContainsKey(name)) return false;
        main_table[name] = new SE_Main_Data_Table(name, type, tm, extends);
        printMainTable();
        return true;
    }
    public bool insertClassData(string name, string type, string am, bool sta, bool final, bool abstrac)
    {
        string key = (name + ":" + type).Split("->")[0];

        if (curr_class_name != null)
        {

            if (main_table[curr_class_name].cdt.ContainsKey(key) || main_table[curr_class_name].cdt.ContainsKey(name)) return false;

            else if (type.Contains("->"))
            {
                main_table[curr_class_name].cdt[key] = new SE_Class_Data_Table(name, type, am, sta, final, abstrac);
                printCTable();
                return true;
            }
            else
            {
                main_table[curr_class_name].cdt[name] = new SE_Class_Data_Table(name, type, am, sta, final, abstrac);
                printCTable();
                return true;
            }   
        }
        return false;
    }
    public bool insertFuncTable(string name, string type)
    {
        if (function_table.ContainsKey(name)) { System.Console.WriteLine("Variable already decleared in Scope"); return false; }
        function_table[name + scope.ToString()] = new SE_Func_Data_Table(name, type, scope);
        printFTable();
        return true;
    }
    /*     
        look Ups 
    */
    public SE_Main_Data_Table? lookUpMainTable(string? name)
    {
        if (name != null)
        {
            if (main_table.ContainsKey(name)) return main_table[name];
        }
        return null;
    }
    public SE_Class_Data_Table? lookUpDataTable(string name)
    {
        if (curr_class_name != null)
        {
            if (main_table[curr_class_name].cdt.ContainsKey(name)) return main_table[curr_class_name].cdt[name];
        }
        return null;
    }
    public SE_Class_Data_Table? lookUpDataTable(string name, string pl)
    {
        if (curr_class_name != null)
        {
            if (main_table[curr_class_name].cdt.ContainsKey(name + ":" + pl)) return main_table[curr_class_name].cdt[(name + ":" + pl)];
        }
        return null;
    }
    public string? lookUpFuncTable(string name)
    {
        for (int i = scopeStack.Count - 1; i >= 0; i--)
        {
            if (scopeStack[i] != 0)
            {
                if (function_table.ContainsKey(name + scopeStack[i].ToString()))
                {
                    return function_table[(name + scopeStack[i].ToString())].type;
                }
            }
        }
        if (curr_class_name != null)
        {
            if (main_table[curr_class_name].cdt.ContainsKey(name))
            {
                return main_table[curr_class_name].cdt[name].type;
            }
        }

        if (global_table.ContainsKey(name)) return global_table[name].type;

        return null;
    }
    public string compatibility(string left, string right, string op)
    {
        if (left == "string" && right == "string")
        {
            if (op == "+") { return "string"; }
            else if (op == "==" || op == "!=") { return "boolean"; }
        }
        else if (left != "string" && right != "string")  // else if ((left == "float" || left == "int" || left == "char") && (right == "float" || right == "int" || right == "char"))
        {
            if (op == "<" || op == ">" || op == "==" || op == "<=" || op == ">=" || op == "!=") { return "boolean"; }
            else if (left == "int" && right == "int") { return "int"; }
            else if (right != "char" && left != "char") { return "float"; }
        }
        return "";
    }
    public void createScope() { scope += 1; scopeStack.Add(scope); }
    public void destroyScope()
    {
        scopeStack.RemoveAt(scopeStack.Count - 1);
        if (scopeStack.Count == 0) curr_class_name = "";
    }

    private void printMainTable()
    {
        System.Console.WriteLine("\n\t^^^^SE_Main_Data_Table^^^^");
        foreach (KeyValuePair<string, SE_Main_Data_Table> kvp in main_table)
        {
            System.Console.WriteLine(string.Format("classID = {0}, Value = {1}", kvp.Key, kvp.Value.ToString()));
        }
        System.Console.WriteLine("\n");
    }
    private void printGTable()
    {
        System.Console.WriteLine("\n\t^^^^SE_Global_Data_Table^^^^");
        foreach (KeyValuePair<string, SE_Global_Data_Table> kvp in global_table)
        {
            System.Console.WriteLine(string.Format("ID = {0}, Value = {1}", kvp.Key, kvp.Value.ToString()));
        }
        System.Console.WriteLine("\n");
    }    
    private void printCTable()
    {
        System.Console.WriteLine("\n\t^^^^SE_Class_Data_Table^^^^");

        foreach (KeyValuePair<string, SE_Class_Data_Table> kvp in main_table[curr_class_name].cdt)
        {
            System.Console.WriteLine(string.Format("classID = {0}, Value = {1}", kvp.Key, kvp.Value.ToString()));
        }
        System.Console.WriteLine("\n");
    }
    private void printFTable()
    {
        System.Console.WriteLine("\n\t^^^^SE_Function_Data_Table^^^^");
        
        // System.Console.WriteLine(function_table.Count);
        foreach (KeyValuePair<string, SE_Func_Data_Table> kvp in function_table)
        {
            System.Console.WriteLine(string.Format("classID = {0}, Value = {1}", kvp.Key, kvp.Value.ToString()));
        }
        System.Console.WriteLine("\n");
    }
}
