using QBM.CompositionApi.Crud;
using QBM.CompositionApi.DataSources;
using QBM.CompositionApi.Definition;
using QBM.CompositionApi.Handling;
using QER.CompositionApi.Portal;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Api
{
    public class CustomizedAPI : IApiProviderFor<PortalApiProject>
    {
        public void Build(IApiBuilder builder)
        {
            builder.ModifyQueryMethod(​
            "portal/itshop/requests",​
             method =>​
            {​
                method.WithCalculatedProperties(new CalculatedPropertyBulk<string>(​
                    "RequestAge"​
                    
                    context =>
                    {
                        context.Entity.GetValue("")
                    }
                )​
            }​
            );​
​
        

}

    }
}
