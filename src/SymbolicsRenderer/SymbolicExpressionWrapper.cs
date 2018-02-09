using System;
using System.Reflection;

using Xamarin.Interactive.Serialization;

namespace SymbolicsRenderer
{
    public class SymbolicExpressionWrapper : ISerializableObject
    {
        const string LaTeXTypeName = "MathNet.Symbolics.LaTeX, MathNet.Symbolics, Culture=neutral, PublicKeyToken=null";
        const string InfixTypeName = "MathNet.Symbolics.Infix, MathNet.Symbolics, Culture=neutral, PublicKeyToken=null";

        static readonly Type SymbolicsLaTeXType;
        static readonly MethodInfo SymbolicsLaTeXFormatMethod;
        static readonly Type SymbolicsInfixType;
        static readonly MethodInfo SymbolicsInfixFormatMethod;

        object symbolicsExpression;

        static SymbolicExpressionWrapper()
        {
            SymbolicsLaTeXType = Type.GetType(LaTeXTypeName);
            SymbolicsInfixType = Type.GetType(InfixTypeName);
            SymbolicsLaTeXFormatMethod = SymbolicsLaTeXType.GetMethod("Format");
            SymbolicsInfixFormatMethod = SymbolicsInfixType.GetMethod("Format");
        }

        public SymbolicExpressionWrapper(object symbolicsExpression) =>
            this.symbolicsExpression = symbolicsExpression;

        public void Serialize(ObjectSerializer serializer)
        {
            var args =  new object[] { symbolicsExpression };
            var latex = (string)SymbolicsLaTeXFormatMethod.Invoke(null, args);
            var infix = (string)SymbolicsInfixFormatMethod.Invoke(null, args);

            serializer.Property("Latex", latex);
            serializer.Property("Infix", infix);
        }
    }
}