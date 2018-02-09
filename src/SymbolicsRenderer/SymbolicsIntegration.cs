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
        
        public void IntegrateWith(IAgent agent)
        {
            Log.Debug(Tag, "Registering MathNet.Symbolics representation provider!");
            agent.RepresentationManager.AddProvider(this);
        }

        public override IEnumerable<object> ProvideRepresentations(object obj)
        {
            if (SymbolicsExpressionType == null) {
                try {
                    SymbolicsExpressionType = Type.GetType(ExpressionTypeName, throwOnError: true);
                } catch (Exception e) {
                    Log.Warning(Tag, e, "Could not load MathNet.Symbolics.Expression type.");
                    yield break;
                }
            }

            var objectType = obj.GetType();
            if (SymbolicsExpressionType.IsAssignableFrom(objectType))
                yield return new SymbolicExpressionWrapper(obj);
        }
    }
}
