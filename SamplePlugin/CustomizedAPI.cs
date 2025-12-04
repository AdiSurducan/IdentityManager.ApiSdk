using QBM.CompositionApi.Crud;
using QBM.CompositionApi.DataSources;
using QBM.CompositionApi.Definition;
using QBM.CompositionApi.Handling;
using QER.CompositionApi.Portal;
using System;
using System.Globalization;

namespace Api
{
    public class CustomizedAPI : IApiProviderFor<PortalApiProject>
    {
        public void Build(IApiBuilder builder)
        {
            builder.ModifyQueryMethod(​"/portal/itshop/requests",​ method => {​
	            method.WithCalculatedProperties(new CalculatedProperty<string>(​"InternalNameUpper"​ context => context.Entity.GetValue("InternalName").String.ToUpperInvariant()​)​
            }​
            );
        }

    }
}
