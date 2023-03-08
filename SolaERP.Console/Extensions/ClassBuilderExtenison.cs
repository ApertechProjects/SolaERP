using System.CodeDom;
using System.CodeDom.Compiler;
using System.Data;

namespace SolaERP.Console.Extensions
{
    public static class ClassBuilderExtenison
    {
        public static void GenerateClassFromDataTable(this DataTable dt, string className, string outputFilePath)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace ns = new CodeNamespace("GeneratedClasses");
            compileUnit.Namespaces.Add(ns);

            CodeTypeDeclaration classDecl = new CodeTypeDeclaration(className);
            ns.Types.Add(classDecl);

            foreach (DataColumn col in dt.Columns)
            {
                CodeMemberField field = new CodeMemberField(col.DataType, col.ColumnName);
                classDecl.Members.Add(field);

                CodeMemberProperty prop = new CodeMemberProperty();
                prop.Name = col.ColumnName;
                prop.Type = new CodeTypeReference(col.DataType);
                prop.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                prop.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), col.ColumnName)));
                prop.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), col.ColumnName), new CodePropertySetValueReferenceExpression()));
                classDecl.Members.Add(prop);
            }

            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            options.BlankLinesBetweenMembers = true;
            options.VerbatimOrder = true;

            using (StreamWriter writer = new StreamWriter(outputFilePath + @$"/{className}.cs"))
            {
                codeProvider.GenerateCodeFromCompileUnit(compileUnit, writer, options);
            }
        }

    }
}
