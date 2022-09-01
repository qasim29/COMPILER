public enum TokenType
{
    ID,
    DT,             // DATA_TYPE,
    IF,
    ELSE,
    WHILE,
    LK,              // LOOP_KEYWORDS
    PM,              // PLUS-MINUS
    MDM,             // MULTIPLY,DIVIDE,MODULOUS
    COMP,            // COMPARISION OPERATORS
    ASI,            // = ASSIGNMENT OPERATOR
    NOT,
    AND,
    OR,
    FUNC,
    RET,             // RETURN
    SEC,             // SEMI_COLON ;
    COL,             // COLON    :
    COM,             // COMMA ,
    DOT,
    ORB,             // OPEN_ROUND_BRACKET
    CRB,             // ROUND BRACKET ClOSE
    OSB,             // OPEN SQUARE BRACKETS
    CSB,             // SQUARE BRACKET CLOSE
    OCB,             // OPEN CURRLY BRACKET
    CCB,             // CLOSE CURRLY BRACKET
    FC,              // fLOAT CONSTANT
    IC,              // INT CONSTANT
    SC,              // STRING CONSTANT
    CC,              // CHAR CONSTANT

    /*  
        // OOP
    */    
    AM,             // ACCESS MODIFIERS
    C_OF,           // CHILD_OF EXTENDS
    ABS,            // ABSRACT
    STA,            // STATIC
    NEW,            // CREATE -->NEW
    CLA,            // CLASS
    SELF,           // THIS- KEYWORD
    SUP,            // SUPER
    CONST,          // FINAL
    NULL,
    IL,             // INVALID LEXEME
    RETURN
}
