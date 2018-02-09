using System;
using System.Reflection;

using Xamarin.Interactive;
using Xamarin.Interactive.Serialization;
using Xamarin.Interactive.Logging;
using Xamarin.Interactive.Representations;
using System.Collections.Generic;

[assembly: AgentIntegration(typeof(SymbolicsRenderer.SymbolicsIntegration))]

namespace SymbolicsRenderer
{
    public class SymbolicsIntegration : RepresentationProvider, IAgentIntegration
    {
        const string Tag = nameof(SymbolicsIntegration);
        const string ExpressionTypeName = "MathNet.Symbolics.Expression, MathNet.Symbolics, Culture=neutral, PublicKeyToken=null";
        static Type SymbolicsExpressionType;

        // Use the FQTN to load the type. The renderer uses reflection because MathNet.Symbolics
        // is a .NET 4.0-targeting library, so we can't get a direct reference into a .NET Standard library.
        static SymbolicsIntegration() =>
            SymbolicsExpressionType = Type.GetType(ExpressionTypeName);
        
        public void IntegrateWith(IAgent agent)
        {
            if (SymbolicsExpressionType == null) {
                Log.Warning(
                    Tag,
                    "Cannot initialize MathNet.Symbolics representation provider! " +
                    "Could not load MathNet.Symbolics.Expression type."
                );
                return;
            }

            Log.Debug(Tag, "Registering MathNet.Symbolics representation provider!");
            agent.RepresentationManager.AddProvider(this);
        }

        public override IEnumerable<object> ProvideRepresentations(object obj)
        {
            var objectType = obj.GetType();
            if (SymbolicsExpressionType.IsAssignableFrom(objectType))
                yield return new SymbolicExpressionWrapper(obj);
        }
    }
}
