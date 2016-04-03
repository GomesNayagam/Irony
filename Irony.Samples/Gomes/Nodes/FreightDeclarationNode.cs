using System.Text;
using Irony.Ast;
using Irony.Parsing;

namespace Irony.Samples.Frieght.Nodes
{
    public class FreightDeclarationNode : AstNode, IJavascriptGenerator
    {
        public override void Init(ParsingContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            TreeNode = treeNode;
        }

        private ParseTreeNode TreeNode { get; set; }

        private ExpressionNode Expression
        {
            get
            {
                return (ExpressionNode)TreeNode.ChildNodes[3].AstNode;
            }
        }

        public void GenerateScript(StringBuilder builder)
        {
            builder.Append("return ");
            Expression.GenerateScript(builder);
            builder.AppendLine(";");
        }

    }
}
