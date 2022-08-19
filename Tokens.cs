

public class Tokens
{
    /*     
        For example, a database field may contain true or false, 
        or it may contain no value at all, that is, NULL. 
        You can use the bool? type in that scenario.
    */  
    private int lineNo ;
    private string? class_Part;
    private string? word;

    public Tokens(string class_Part, int lineNo)
    {
        this.Class_Part = class_Part;
        this.LineNo = lineNo;
    }
    public Tokens(string class_Part, string word, int lineNo)
    {
        this.Class_Part = class_Part;
        this.Word = word;
        this.LineNo = lineNo;
    }
    public int LineNo { get => lineNo; set => lineNo = value; }
    public string? Class_Part { get => class_Part; set => class_Part = value; }
    public string? Word { get => word; set => word = value; }

}


