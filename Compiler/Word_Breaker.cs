using System.Collections;

class Word_Breaker
{
    private char[] breakers = { '(',')','[',']','{','}',';',':',',',        // punctuators  
                                '+','-','*','/','%','<','>','=','!',       // operators
                                ' ','\'','"' };
    private string word = "";
    private ArrayList words;
    private string[] lines;
    private char ch;
    private int i;
    private int lineNo;
    // COUNSTRUCTOR
    public Word_Breaker()
    {
        words = new ArrayList();

        lines = System.IO.File.ReadAllLines(@"E:\GITHUB\Language_Compiler\res\SOURCE_CODE.txt");
    }
    public Word_Breaker(string path)
    {
        words = new ArrayList();

        lines = System.IO.File.ReadAllLines(@path);
    }
    // GETTERS
    public ArrayList GetWords()
    {
        BreakIntoWords();

        return words;
    }


    private void BreakIntoWords()
    {
        bool flag = false;
        foreach (string line in lines)
        {
            lineNo+=1;
            string l = line + " ";
            for (i = 0; i <= l.Length - 1; i++)
            {                                                        
                if (l[i] == '#') break;                              // This condition is for the SINGLE LINE comment
                                                                   
                if (l[i] == '$' || flag == true)                     // This condition is for the MULTI LINE comment
                {                                                                       
                    flag = Check_Comment_Status(l, flag);           // flag maintains the status for Multi-line comment
                    if (flag) break;
                }

                if (l[i] == '.')
                {
                    bool isNumeric;
                    if (!WordIsEmpty()) isNumeric = int.TryParse(word, out _);
                    else isNumeric = true;

                    if (isNumeric)
                    {
                        AddCharacter(l[i]);
                        isNumeric = int.TryParse(l[i].ToString(), out _);
                        if (isNumeric)
                        {
                            while (!breakers.Contains(l[i]) && l[i] != '.')
                            {
                                AddCharacter(l[i]);
                            }
                            createWord(word);
                            i--;
                            continue;
                        }
                        else
                        {
                            createWord(word);
                            AddCharacter(l[i]);
                            i--;
                            continue;
                        }
                    }
                    else
                    {
                        createWord(word);
                        isNumeric = int.TryParse(l[i + 1].ToString(), out _);
                        if (isNumeric)
                        {
                            AddCharacter(l[i]);
                            AddCharacter(l[i + 1]);
                            continue;
                        }
                        createWord(l[i].ToString());
                        continue;
                    }
                }

                if (breakers.Contains(l[i]))
                {
                    if (l[i] == '"')
                    {
                        if (!WordIsEmpty()) { createWord(word); AddCharacter(l[i]); }

                        else { AddCharacter(l[i]); }

                        ch = ' ';

                        while (ch != '"' && i < l.Length - 1)
                        {
                            if (l[i] == '\\')
                            {
                                AddCharacter(l[i]);
                                if (i == l.Length - 1) break;     //Case: {"\ }

                                AddCharacter(l[i]);
                                ch = ' ';
                                continue;                        //Added continue for this case { "\" }
                            }
                            AddCharacter(l[i]);
                        }
                        createWord(word);
                        i--;
                        continue;
                    }
                    
                    if (l[i] == '\'')
                    {
                        int count = 0;
                        if (!WordIsEmpty()) { createWord(word); AddCharacter(l[i]); }

                        else { AddCharacter(l[i]); }
                        count += 1;
                        ch = ' ';
                        while (ch != '\'' && i < l.Length - 1 && count != 3)
                        {
                            if (l[i] == '\\')
                            {
                                AddCharacter(l[i]);
                                count += 1;
                                if (i == l.Length - 1) break;     //Case: {"\ }

                                AddCharacter(l[i]);
                                ch = ' ';
                                continue;                        //Added continue for this case { "\" }
                            }
                            AddCharacter(l[i]);
                            count += 1;
                        }
                        createWord(word);
                        i--;
                        continue;
                    }

                    if (!WordIsEmpty()) createWord(word);

                    if (l[i] == ' ') continue;

                    if (Check_RO(l)) continue;

                    createWord(l[i].ToString());
                    continue;
                }
 
                word = word + l[i];
            }
        }

    }
    private void AddCharacter(char character)
    {
        ch = character;
        word += character;
        i++;
    }
    private void createWord(string w)
    {
        words.Add(new string[]{w,lineNo.ToString()});
        word = "";
    }
    private bool WordIsEmpty()
    {
        if (word == "") return true;
        return false;
    }
    private bool Check_Comment_Status(string l, bool flag)
    {
        int index;
        if (flag == true) index = l.IndexOf('$', i);    // cases that avoids skipping of '$' 
        else index = l.IndexOf('$', i + 1);

        if (index == -1) return true;                   // flag is true if current line dosen't conatin '$'
                                                        // Break to get to the next line
        i = index + 1;
        return false;
    }
    private bool Check_RO(string l)
    {
        if ((l[i] == '>' || l[i] == '<' || l[i] == '=' || l[i] == '!') && l[i + 1] == '=')
        {
            createWord(l[i].ToString() + l[i + 1].ToString());
            i++;
            return true;
        }
        else return false;
    }
}
