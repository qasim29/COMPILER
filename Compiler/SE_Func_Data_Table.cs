class SE_Func_Data_Table
{
    public SE_Func_Data_Table(string name, string type, int scope)
    {
        this.name = name;
        this.type = type;
        this.scope = scope;
    }

    public string name { get; set; }
    public string type { get; set; }
    public int scope { get; set; }
}
