public bool Q_(Exp exp)
{
    if (tokens[index].class_Part == TokenType.MDM)
    {
        exp.setOp(tokens[index].word);
        index++;
        Exp ex = new Exp();
        if (F(ex))
        {
            String t3 = se.Compatibility(exp.getT(), ex.getT(), exp.getOp());
            exp.setT(t3);
            if (Q_(exp))
            {
                return true;
            }
        }
    }
    else if (tokens[index].class_Part == TokenType.PM
                || tokens[index].class_Part == TokenType.COMP
                || tokens[index].class_Part == TokenType.COM
                || tokens[index].class_Part == TokenType.CRB
                || tokens[index].class_Part == TokenType.CCB
                || tokens[index].class_Part == TokenType.CSB
                || tokens[index].class_Part == TokenType.SEC)
    {
        return true;
    }

    return false;
}

public bool Q(Exp exp)
{
    if (tokens[index].class_Part == TokenType.SUPER
            || tokens[index].class_Part == TokenType.SELF
            || tokens[index].class_Part == TokenType.ID
            || tokens[index].class_Part == TokenType.IC
            || tokens[index].class_Part == TokenType.FC
            || tokens[index].class_Part == TokenType.SC
            || tokens[index].class_Part == TokenType.CC
            || tokens[index].class_Part == TokenType.ORB
            || tokens[index].class_Part == TokenType.CREATE)
    {
        if (F(exp))
        {
            if (Q_(exp))
            {
                return true;
            }
        }
    }

    return false;
}

public bool E_(Exp exp)
{
    if (tokens[index].class_Part == TokenType.PM)
    {
        exp.setOp(tokens[index].word);
        index++;
        Exp ex = new Exp();
        if (Q(ex))
        {
            String t3 = se.Compatibility(exp.getT(), ex.getT(), exp.getOp());
            exp.setT(t3);
            if (E_(exp))
            {
                return true;
            }
        }
    }
    else if (tokens[index].class_Part == TokenType.COM || tokens[index].class_Part == TokenType.CRB
            || tokens[index].class_Part == TokenType.CCB || tokens[index].class_Part == TokenType.CSB
            || tokens[index].class_Part == TokenType.SEC)
    {
        return true;
    }
    return false;
}

public bool E(Exp exp)
{
    if (tokens[index].class_Part == TokenType.SUPER || tokens[index].class_Part == TokenType.SELF || tokens[index].class_Part == TokenType.ID
           || tokens[index].class_Part == TokenType.IC || tokens[index].class_Part == TokenType.FC || tokens[index].class_Part == TokenType.SC
           || tokens[index].class_Part == TokenType.CC || tokens[index].class_Part == TokenType.ORB
           || tokens[index].class_Part == TokenType.CREATE)
    {
        if (Q(exp))
        {
            if (E_(exp))
            {
                return true;
            }
        }
    }
    return false;
}

public bool RE_(Exp exp)
{
    if (tokens[index].class_Part == TokenType.ROP)
    {
        exp.setOp(tokens[index].word);
        index++;
        Exp ex = new Exp();
        if (E(ex))
        {
            exp.setT(se.Compatibility(exp.getT(), ex.getT(), exp.getOp()));
            if (RE_(exp))
            {
                return true;
            }
        }
    }
    else if (tokens[index].class_Part == TokenType.COM || tokens[index].class_Part == TokenType.CRB
            || tokens[index].class_Part == TokenType.CCB || tokens[index].class_Part == TokenType.CSB
            || tokens[index].class_Part == TokenType.SEC)
    {
        return true;
    }

    return false;
}






 public static boolean F(Exp exp) {
        if (tokens[index].class_Part == TokenType.SUPER 
        || tokens[index].class_Part == TokenType.SELF 
        || tokens[index].class_Part == TokenType.ID
           || tokens[index].class_Part == TokenType.IC 
           || tokens[index].class_Part == TokenType.FC 
           || tokens[index].class_Part == TokenType.SC
           || tokens[index].class_Part == TokenType.CC 
           || tokens[index].class_Part == TokenType.ORB
           || tokens[index].class_Part == TokenType.CREATE)
    {
            if (tokens[index].class_Part == TokenType.SUPER || tokens[index].class_Part == TokenType.SELF || tokens[index].class_Part == TokenType.ID) {
                if (TS(exp)) {
                    if (tokens[index].class_Part == TokenType.ID) {

                        exp.setN(tokens[index].word);
                        index++;
                        if (O(exp)) {
                            return true;
                        }
                    }
                }
            } else if (tokens[index].class_Part == TokenType.ORB) {
                index++;
                // Exp ex = new Exp();
                if (exp(exp)) {
                    if (tokens[index].class_Part == TokenType.CRB {
                        index++;
                        return true;
                    }
                }
            } 
            else if (tokens[index].class_Part == TokenType.CREATE) {
                if (obj_dec()) {
                    if (O_(exp)) {
                        return true;
                    }
                }
            } else {
                if (Const(exp)) {
                    return true;
                }
            }
        }
        return false;
        
    }

 public bool O(Exp exp) {
        if (tokens[index].class_Part == TokenType.OSB) {
            index++;
            Exp ex = new Exp();
            if (exp(ex)) {
                if (!ex.getT().equals("int")) {
                    Console.WriteLine("Error at " + tokens[index].lineNo + "  index must be an integer value");
                    Environment.Exit(0);
                }
                if (tokens[index].class_Part == TokenType.CSB) {
                    index++;
                    if (opt(exp)) {
                        if (O_(exp)) {
                            return true;
                        }
                    }
                }
            }
        } else if (tokens[index].class_Part == TokenType.CRB) {
            index++;
            Exp ex = new Exp();
            if (argu(ex)) {
                if (tokens[index].class_Part == TokenType.CCB) {
                    exp.setT(ex.getT());
                    index++;
                    if (exp.getR().equals("") || exp.getR().equals("self")) {
                        SE_Class_Data_Table? c = se.lookupDT(exp.getN(), ex.getT(), se.curr_class_name);
                        if (c != null) {
                            String[] T = c.getT().split("->");
                            exp.setT(T[1]);
                        } else {
                            Console.WriteLine("Error at " + tokens[index].lineNo + " Function not declare");
                            Environment.Exit(0);
                        }

                    } else if (exp.getR().equals("super")) {
                        SE_Main_Data_Table? m = se.lookupMT(se.curr_class_name);
                        if (m.getExt() == null || m.getExt().equals("")) {
                            Console.WriteLine("Current Class does not have parent class");
                            Environment.Exit(0);
                        } else {
                            SE_Class_Data_Table? c = se.lookupDT(exp.getN(), exp.getT(), m.getExt());
                            if (c != null) {
                                String[] T = c.getT().split("->");
                                exp.setT(T[1]);
                            } else {
                                Console.WriteLine("Error at " + tokens[index].lineNo + "Function not declare");
                                Environment.Exit(0);
                            }
                            
                        }
                    } else if (exp.getR().equals("int") || exp.getR().equals("float") || exp.getR().equals("char")
                            || exp.getR().equals("String") || exp.getR().equals("bool")) {
                        Console.WriteLine("Error at " + tokens[index].lineNo + "Primitive data Type cant be instantiated");
                        Environment.Exit(0);
                    } else if (exp.getR().equals("void")) {
                        Console.WriteLine("Error at " + tokens[index].lineNo + "Return type is void");
                        Environment.Exit(0);
                    } else if (exp.getR().equals("constructor") || exp.getR().equals("None")) {
                        SE_Class_Data_Table? c = se.lookupDT(exp.getN(), exp.getT(), exp.getR());
                        if (c == null) {
                            while (true) {
                                SE_Main_Data_Table? m = se.lookupMT(exp.getR());
                                if (m == null) {
                                    Console.WriteLine("Error at " + tokens[index].lineNo + " :Undeclared");
                                    Environment.Exit(0);
                                } else if (m.getExt() == null || m.getExt().equals("")) {
                                    Console.WriteLine("Error at " + tokens[index].lineNo + " :Undeclared");
                                    Environment.Exit(0);
                                } else {
                                    c = se.lookupDT(exp.getN(), exp.getT(), m.getExt());
                                    if (c == null) {
                                        exp.setR(m.getExt());
                                    } else if (c.getAM().equals("local") && !c.getN().equals(se.curr_class_name)) {
                                        Console.WriteLine("Error at : " + tokens[index].lineNo + "private attribute or function cant be accessed outside the class");
                                        Environment.Exit(0);
                                    } else {
                                        String[] T = c.getT().split("->");
                                        exp.setT(T[1]);
                                        break;
                                    }

                                }
                            }
                        } else if (c.getAM().equals("local") && !c.getN().equals(se.curr_class_name)) {
                            Console.WriteLine("Error at : " + tokens[index].lineNo  + "private attribute or function cant be accessed outside the class");
                            Environment.Exit(0);
                        }
                        String[] T = c.getT().split("->");
                        exp.setT(T[1]);
                    } else {
                        SE_Class_Data_Table? c = se.lookupDT(exp.getN(), exp.getT(), exp.getR());
                        if (c == null) {
                            while (true) {
                                SE_Main_Data_Table? m = se.lookupMT(exp.getR());
                                if (m == null) {
                                    Console.WriteLine("Error at " + tokens[index].lineNo + " :Undeclared");
                                    Environment.Exit(0);
                                } else if (m.getExt() == null || m.getExt().equals("")) {
                                    Console.WriteLine("Error at " + tokens[index].lineNo + " :Undeclared");
                                    Environment.Exit(0);
                                } else {
                                    c = se.lookupDT(exp.getN(), exp.getT(), m.getExt());
                                    if (c == null) {
                                        exp.setR(m.getExt());
                                    } else if (c.getAM().equals("local") && !c.getN().equals(se.curr_class_name)) {
                                        Console.WriteLine("Error at : " + tokens[index].lineNo + "private attribute or function cant be accessed outside the class");
                                        Environment.Exit(0);
                                    } else {
                                        String[] T = c.getT().split("->");
                                        exp.setT(T[1]);
                                        break;
                                    }

                                }
                            }
                        } else if (c.getAM().equals("local") && !c.getN().equals(se.curr_class_name)) {
                            Console.WriteLine("Error at : " + tokens[index].lineNo + " private attribute or function cant be accessed outside the class");
                            Environment.Exit(0);
                        }
                        String[] T = c.getT().split("->");
                        exp.setT(T[1]);
                    }
                    if (O_(exp)) {
                        return true;
                    }
                }
            }
        } else if (tokens[index].class_Part == TokenType.DOT) {
            if (exp.getR().equals("")) {
                String t = se.lookupFT(exp.getN(), se.stack);
                if (t != null) {
                    exp.setT(t);
                } else {
                    SE_Main_Data_Table? m = se.lookupMT(exp.getN());
                    if (m != null) {
                        if (m.getTM() == null || !m.getTM().equals("static")) {
                            Console.WriteLine("Error at : " + tokens[index].lineNo + " variable should be static");
                            Environment.Exit(0);
                        }
                        exp.setR(m.getN());
                    } else {
                        Console.WriteLine("Error at " + tokens[index].lineNo + ": Variable not declare ");
                        Environment.Exit(0);
                    }
                }

            } else if (exp.getR().equals("self")) {
                SE_Class_Data_Table? c = se.lookupDT(exp.getN(), se.curr_class_name);
                if (c != null) {

                    exp.setT(c.getT());

                } else {
                    Console.WriteLine("Error at " + tokens[index].lineNo + ": Variable not declare ");
                    Environment.Exit(0);
                }
            } else if (exp.getR().equals("super")) {
                SE_Main_Data_Table? m = se.lookupMT(se.curr_class_name);
                if (m.getExt() == null || m.getExt().equals("")) {
                    Console.WriteLine("Current Class does not have parent class");
                    Environment.Exit(0);
                } else {
                    SE_Class_Data_Table? c = se.lookupDT(exp.getN(), m.getExt());
                    if (c != null) {

                        exp.setT(c.getT());

                    } else {
                        Console.WriteLine("Error at " + tokens[index].lineNo + ": Variable not declare ");
                        Environment.Exit(0);
                    }
                }
            } else if (exp.getR().equals("int") || exp.getR().equals("float") || exp.getR().equals("char")
                    || exp.getR().equals("String") || exp.getR().equals("bool")) {
                Console.WriteLine("Error at " + tokens[index].lineNo + "Primitive data Type cant be instantiated");
                Environment.Exit(0);
            } else if (exp.getR().equals("void")) {
                Console.WriteLine("Error at " + tokens[index].lineNo + "Return type is void");
                Environment.Exit(0);
            } else {
                SE_Class_Data_Table? c = se.lookupDT(exp.getN(), exp.getR());
                if (c == null) {
                    while (true) {
                        SE_Main_Data_Table? m = se.lookupMT(exp.getR());
                        if (m == null) {
                            Console.WriteLine("Error at " + tokens[index].lineNo + " :Undeclared");
                            Environment.Exit(0);
                        } else if (m.getExt() == null || m.getExt().equals("")) {
                            Console.WriteLine("Error at " + tokens[index].lineNo + " :Undeclared");
                            Environment.Exit(0);
                        } else {
                            c = se.lookupDT(exp.getN(), m.getExt());
                            if (c == null) {
                                exp.setR(m.getExt());
                            } else if (c.getAM().equals("local") && !c.getN().equals(se.curr_class_name)) {
                                Console.WriteLine("Error at : " + tokens[index].lineNo + "private attribute or function cant be accessed outside the class");
                                Environment.Exit(0);
                            } else {
                                exp.setT(c.getT());
                                break;
                            }

                        }
                    }
                } else if (c.getAM().equals("local") && !c.getN().equals(se.curr_class_name)) {
                    Console.WriteLine("Error at : " + tokens[index].lineNo + "private attribute or function cant be accessed outside the class");
                    Environment.Exit(0);
                } else {
                    exp.setT(c.getT());
                }

            }
            if (O_(exp)) {
                return true;
            }
        } 
        else if (
            tokens[index].class_Part == TokenType.MDM 
        || tokens[index].class_Part == TokenType.COM 
        || tokens[index].class_Part == TokenType.COMP 
        || tokens[index].class_Part == TokenType.CRB
        || tokens[index].class_Part == TokenType.CCB 
        || tokens[index].class_Part == TokenType.CSB 
        || tokens[index].class_Part == TokenType.SEC 
        || tokens[index].class_Part == TokenType.PM) 
        {
            if (exp.getR().equals("")) {
                String t = se.lookupFT(exp.getN(), se.stack);
                if (t != null) {
                    exp.setT(t);
                } else {
                    SE_Main_Data_Table? m = se.lookupMT(exp.getN());
                    if (m != null) {
                        if (m.getTM() == null || !m.getTM().equals("static")) {
                            Console.WriteLine("Error at : " + tokens[index].lineNo + " variable should be static");
                            Environment.Exit(0);
                        }
                        exp.setR(m.getN());
                    } else {
                        Console.WriteLine("Error at " + tokens[index].lineNo + ": Variable not declare ");
                        Environment.Exit(0);
                    }
                }

            } else if (exp.getR().equals("self")) {
                SE_Class_Data_Table? c = se.lookupDT(exp.getN(), se.curr_class_name);
                if (c != null) {

                    exp.setT(c.getT());

                } else {
                    Console.WriteLine("Error at " + tokens[index].lineNo + ": Variable not declare ");
                    Environment.Exit(0);
                }
            } else if (exp.getR().equals("super") {
                SE_Main_Data_Table? m = se.lookupMT(se.curr_class_name);
                if (m.getExt() == null || m.getExt().equals("")) {
                    Console.WriteLine("Current Class does not have parent class");
                    Environment.Exit(0);
                } else {
                    SE_Class_Data_Table? c = se.lookupDT(exp.getN(), m.getExt());
                    if (c != null) {

                        exp.setT(c.getT());

                    } else {
                        Console.WriteLine("Error at " + tokens[index].lineNo + ": Variable not declare ");
                        Environment.Exit(0);
                    }
                }
            } else if (exp.getR().equals("int") 
            || exp.getR().equals("float") 
            || exp.getR().equals("char")
            || exp.getR().equals("String") 
            || exp.getR().equals("bool")) 
            {
                Console.WriteLine("Error at " + tokens[index].lineNo + "Primitive data Type cant be instantiated");
                Environment.Exit(0);
            } else if (exp.getR().equals("void")) {
                Console.WriteLine("Error at " + tokens[index].lineNo + "Return type is void");
                Environment.Exit(0);
            } else {
                SE_Class_Data_Table? c = se.lookupDT(exp.getN(), exp.getR());
                if (c == null) {
                    while (true) {
                        SE_Main_Data_Table? m = se.lookupMT(exp.getR());
                        if (m == null) {
                            Console.WriteLine("Error at " + tokens[index].lineNo + " :Undeclared");
                            Environment.Exit(0);
                        } else if (m.getExt() == null || m.getExt().equals("") {
                            Console.WriteLine("Error at " + tokens[index].lineNo + " :Undeclared");
                            Environment.Exit(0);
                        } else {
                            c = se.lookupDT(exp.getN(), m.getExt());
                            if (c == null) {
                                exp.setR(m.getExt());
                            } else if (c.getAM().equals("local") && !c.getN().equals(se.curr_class_name)) {
                                Console.WriteLine("Error at : " + tokens[index].lineNo + "private attribute or function cant be accessed outside the class");
                                Environment.Exit(0);
                            } else {
                                exp.setT(c.getT());
                                break;
                            }

                        }
                    }
                } else if (c.getAM().equals("local") && !c.getN().equals(se.curr_class_name)) {
                    Console.WriteLine("Error at : " + tokens[index].lineNo + "private attribute or function cant be accessed outside the class");
                    Environment.Exit(0);
                } else {
                    exp.setT(c.getT());
                }

            }
            return true;
        }

        return false;
    }