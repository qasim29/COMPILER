using System.Collections;
class SE_Semantic_Analyzer
{
    public Dictionary<string, SE_Main_Data_Table> main_table;
    public Dictionary<string, SE_Func_Data_Table> function_table;
    // public int scope;
    // public List<int> scopeStack = new List<int>();
    // public string curr_class_name;
    public SE_Semantic_Analyzer()
    {
        main_table = new Dictionary<string, SE_Main_Data_Table>();
        function_table = new Dictionary<string, SE_Func_Data_Table>();
    }
    bool insertMainTable(string name, string type, string tm, string extends)
    {
        if (main_table.ContainsKey(name)) return false;
        main_table[name] = new SE_Main_Data_Table(name, type, tm, extends);
        return true;

    }
    bool insertClassData(string name, string type, string am, bool sta, bool final, bool abstrac, string curr_class_name)
    {
        string key = (name + ":" + type).Split("->")[0];

        if (main_table[curr_class_name].cdt.ContainsKey(key) || main_table[curr_class_name].cdt.ContainsKey(name)) return false;

        else if (type.Contains("->"))
        {
            main_table[curr_class_name].cdt[key] = new SE_Class_Data_Table(name, type, am, sta, final, abstrac);
            return true;
        }
        else
        {
            main_table[curr_class_name].cdt[name] = new SE_Class_Data_Table(name, type, am, sta, final, abstrac);
            return true;
        }
    }
    bool insertFuncTable(string name, string type, int scope)
    {
        if (function_table.ContainsKey(name)) return false;
        function_table[name + scope.ToString()] = new SE_Func_Data_Table(name, type, scope);
        return true;
    }
    SE_Main_Data_Table? lookUpMainTable(string name)
    {
        if (main_table.ContainsKey(name)) return main_table[name];
        else return null;
    }
    SE_Class_Data_Table? lookUpDataTable(string name, string curr_class_name)
    {
        if (main_table[curr_class_name].cdt.ContainsKey(name)) return main_table[curr_class_name].cdt[name];
        else return null;
    }
    SE_Class_Data_Table? lookUpDataTable(string name, string pl, string curr_class_name)
    {
        if (main_table[curr_class_name].cdt.ContainsKey(name + ":" + pl)) return main_table[curr_class_name].cdt[(name + ":" + pl)];
        else return null;
    }
    string? lookUpFuncTable(string name, List<int> scopeStack, string curr_class_name)
    {
        for (int i = scopeStack.Count - 1; i >= 0; i--)
        {
            if (function_table.ContainsKey(name + scopeStack[i].ToString())) return function_table[(name + scopeStack[i].ToString())].type;
        }
        if (main_table[curr_class_name].cdt.ContainsKey(name)) return main_table[curr_class_name].cdt[name].type;

        return null;
    }
    void createScope(int scope, List<int> scopeStack) { scope += 1; scopeStack.Add(scope); }
    void destroyScope(int scope, List<int> scopeStack) { scopeStack.RemoveAt(scopeStack.Count - 1); }



}