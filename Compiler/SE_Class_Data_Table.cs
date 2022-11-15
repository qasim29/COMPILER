class SE_Class_Data_Table
{
    public SE_Class_Data_Table(string name, string type, string am, bool sta, bool final, bool abstrac)
    {
        this.name = name;
        this.type = type;
        this.am = am;
        this.sta = sta;
        this.final = final;
        this.abstrac = abstrac;
    }
    public string name { get; set; }
    public string type { get; set; }
    public string am { get; set; }
    public bool sta { get; set; }
    public bool final { get; set; }
    public bool abstrac { get; set; }

    public override string? ToString()
    {
        return "{"+name+", ("+type+"), "+am+", "+"static: "+sta.ToString()+", "+"final: "+final.ToString()+", "+"abstract: "+abstrac.ToString()+"}";
    }

}