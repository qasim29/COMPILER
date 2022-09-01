

public class Tokens
{
    /*     
        For example, a database field may contain true or false, 
        or it may contain no value at all, that is, NULL. 
        You can use the bool? type in that scenario.
    */
    private string lineNo { get; set; }
    private TokenType? class_Part { get; set; }
    private string word { get; set; }

    public Tokens(string lineNo, TokenType? class_Part, string word)
    {
        this.lineNo = lineNo;
        this.class_Part = class_Part;
        this.word = word;
    }

    public override string ToString()
    {
        return "[ " + class_Part.ToString() + ", " + word.ToString() + ", " + lineNo + " ]";
    }
}


