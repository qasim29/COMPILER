class SE_Global_Data_Table
{
    public SE_Global_Data_Table(string name, string type, bool sta, bool final)
    {
        this.name = name;
        this.type = type;
        this.sta = sta;
        this.final = final;
    }

    public string name { get; set; }
    public string type { get; set; }
    public bool sta { get; set; }
    public bool final { get; set; }
}