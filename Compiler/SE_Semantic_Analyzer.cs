using System.Collections;
class SE_Semantic_Analyzer
{
    public Dictionary<string, SE_Main_Data_Table> main_table;
    public Dictionary<string, SE_Main_Data_Table> function_table;
    public SE_Semantic_Analyzer()
    {
        main_table = new Dictionary<string, SE_Main_Data_Table>();
        function_table = new Dictionary<string, SE_Main_Data_Table>();
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

    SE_Main_Data_Table? lookUpMainTable(string name) { return main_table[name]; }
    SE_Class_Data_Table? lookUpDataTable(string name, string curr_class_name) { return main_table[curr_class_name].cdt[name]; }
    SE_Class_Data_Table? lookUpDataTable(string name, string type, string curr_class_name) { return main_table[curr_class_name].cdt[(name + ":" + type).Split("->")[0]]; }

}