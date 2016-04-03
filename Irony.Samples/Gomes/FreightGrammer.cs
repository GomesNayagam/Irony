using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Samples.Frieght.Nodes;

//Set pricePerKG to 2.5;
//If region is "asia" [
//  set pricePerKG to 2.2;
//]
//Set totalWeight to 0;
//loop through order [
//  Set totalWeight to totalWeight + (quantity * weight);
//]
//Set freightCost to totalWeight * pricePerKG;
//if customer is "VIP" [
//  set freightCost to freightCost * 0.7;
//]
//Freight cost is freightCost;

namespace Irony.Samples.Frieght
{
    class FreightGrammer : Grammar
    {
        public FreightGrammer():base(false)
        {
            #region BNF
            /*
            <Program> ::= <StatementList> <FreightDeclaration>
            <StatementList> ::= <Statement>*
            <Statement> ::= <SetVariable> ";" | <IfStatement> | <OrderLoop> | <Expression> ";"
            <SetVariable> ::= "set" <variable> "to" <Expression>
            <IfStatement> ::= "if" <Expression> "[" <StatementList> "]"
            <OrderLoop> ::= "loop" "through" "order" "[" <StatementList> "]"
            <FreightDeclaration> ::= "freight" "cost" "is" <Expression> ";"
            <Expression> ::= <number> | <variable> | <string> | 
                             <Expression> <BinaryOperator> <Expression> | "(" <Expression> ")"
            <BinaryOperator> ::= "+" | "-" | "*" | "/" | "<" | ">" | "<=" | ">=" | "is"
             * */
            #endregion

            // define all the non-terminals
            var program = new NonTerminal("program", typeof(ProgramNode));
            var statementList = new NonTerminal("statementList", typeof(StatementListNode));
            var statement = new NonTerminal("statement", typeof(StatementNode));
            var ifStatement = new NonTerminal("ifStatement", typeof(IfStatementNode));
            var freightDeclaration = new NonTerminal("freightDeclaration", typeof(FreightDeclarationNode));
            var orderLoop = new NonTerminal("orderLoop", typeof(OrderLoopNode));
            var expression = new NonTerminal("expression", typeof(ExpressionNode));
            var setVariable = new NonTerminal("setVariable", typeof(SetVariableNode));
            var binaryOperator = new NonTerminal("binaryOperator", typeof(BinaryOperatorNode));


            //define all terminals

            var keyword = new IdentifierTerminal("keyword");
            var number = new NumberLiteral("number");
            var stringLiteral = new StringLiteral("string", "\"", StringOptions.None );

            // etc…

            this.Root = program;

            // define the grammar

            //<Program> ::= <StatementList> <FreightDeclaration>
            program.Rule = statementList + freightDeclaration;

            //<StatementList> ::= <Statement>*
            statementList.Rule = MakeStarRule(statementList, null, statement);

            //<Statement> ::= <SetVariable> ";" | <IfStatement>
            //                      | <OrderLoop> | <Expression> ";"
            statement.Rule = setVariable + ";" | ifStatement | orderLoop | expression + ";";

            //<SetVariable> ::= "set" <variable> "to" <Expression>
            setVariable.Rule = ToTerm("set") + keyword + "to" + expression;

            //<IfStatement> ::= "if" <Expression> "[" <StatementList> "]"
            ifStatement.Rule = ToTerm("if") + expression + "[" + statementList + "]";

            //<OrderLoop> ::= "loop" "through" "order" "[" <StatementList> "]"
            orderLoop.Rule = ToTerm("loop") + "through" + "order" + "[" + statementList + "]";

            // <FreightDeclaration> ::= "freight" "cost" "is" <Expression> ";"
            freightDeclaration.Rule = ToTerm("freight") + "cost" + "is" + expression + ";";


            // <Expression> ::= <number> | <variable> | <string> | 
            //                  <Expression> <BinaryOperator> <Expression> | "(" <Expression> ")"
            expression.Rule = number | keyword | stringLiteral
                | expression + binaryOperator + expression
                | "(" + expression + ")";


            //<BinaryOperator> ::= "+" | "-" | "*" | "/" | "<" | ">" | "<=" | ">=" | "is"
            binaryOperator.Rule = ToTerm("+") | "-" | "*" | "/" | "<" | ">" | "<=" | ">=" | "is" ;

            RegisterOperators(10, "is", "+", "-");
            RegisterOperators(20, "*", "/");
            RegisterOperators(30, Associativity.Right, "**");
            RegisterOperators(40, "<", ">", "<=", ">=");


            this.LanguageFlags = LanguageFlags.CreateAst | LanguageFlags.CanRunSample;

            this.MarkReservedWords("set", "to", "if", "freight", "cost", "is", "loop", "through", "order");
            this.MarkPunctuation(";", "[", "]", "(", ")");


        }

        public override string RunSample(ParseTree parsedSample)
        {
            IJavascriptGenerator program = (IJavascriptGenerator)parsedSample.Root.AstNode;
            StringBuilder js = new StringBuilder();
            program.GenerateScript(js);
            return js.ToString();
        }
    }
}
