using System.Text;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.Samples.Frieght.Nodes
{
    public class SetVariableNode : AstNode, IJavascriptGenerator
    {
        public override void Init(ParsingContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            TreeNode = treeNode;
        }

        private ParseTreeNode TreeNode { get; set; }

        public Token Variable
        {
            get
            {
                return TreeNode.ChildNodes[1].Token;
            }
        }

        private ExpressionNode Expression
        {
            get
            {
                return (ExpressionNode)TreeNode.ChildNodes[3].AstNode;
            }
        }

        public void GenerateScript(StringBuilder builder)
        {
            builder.Append(Variable.Text);
            builder.Append(" = ");
            Expression.GenerateScript(builder);
            builder.AppendLine(";");
        }

    }
}
