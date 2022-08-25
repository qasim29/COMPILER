

public class Tokens
{
    /*     
        For example, a database field may contain true or false, 
        or it may contain no value at all, that is, NULL. 
        You can use the bool? type in that scenario.
    */  
    private int lineNo{get;set;}
    private TokenType class_Part{get;set;}
    private string? word{get;set;}

    public Tokens(TokenType class_Part, int lineNo)
    {
        this.class_Part = class_Part;
        this.lineNo = lineNo;
    }
    public Tokens(TokenType class_Part, string word, int lineNo)
    {
        this.class_Part = class_Part;
        this.word = word;
        this.lineNo = lineNo;
    }

}


