using System.Collections;
class SE_Main_Data_Table
{
    public SE_Main_Data_Table(string name, string type, string? tm, string? extends)
    {
        this.name = name;
        this.type = type;
        this.tm = tm;
        this.extends = extends;
        // cd = new List<SE_Class_Data_Table>();
        cdt = new Dictionary<string, SE_Class_Data_Table>();
    }

    public string name { get; set; }
    public string type { get; set; }
    public string? tm { get; set; }
    public string? extends { get; set; }
    public Dictionary<string, SE_Class_Data_Table> cdt { get; set; }

    // public List<SE_Class_Data_Table> cd { get; set; }

    
    public override string? ToString()
    {
        return "{"+name +","+type +","+tm +","+extends+"}" ;
    }
}