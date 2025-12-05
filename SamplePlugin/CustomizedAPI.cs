using QBM.CompositionApi.Crud;
using QBM.CompositionApi.DataSources;
using QBM.CompositionApi.Definition;
using QBM.CompositionApi.Dto;
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
            var timeNow = DateTime.Today.ToString();
            builder.ModifyQueryMethod(
                "itshop/requests",
                method =>
                {
                    method.OptionalClauseProviders.Add(new TechFilter());
                    method.WithCalculatedProperties(new CalculatedProperty<int>("RequestAge", context => {


                        var orderDateObj = context.Entity.GetValue("OrderDate");

                        if (orderDateObj == null)
                            return 0; 

                        DateTime orderDate;
                        try
                        {
                            orderDate = orderDateObj.ToDateTime(CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            return 0; 
                        }
                        orderDate = DateTime.Parse(orderDateObj.ToString());
                        
            
                        return (DateTime.Today - orderDate.Date).Days;


                    }));
                }

            );
        }

        private class TechFilter : IPredefinedFilter
        {
            public string UrlParameterName => "Admin Identities";

            public string Description => "Only Admin Identities";

            // This is the translatable display for the filter in the UI
            public MultiLanguageStringData Title { get; }
                = MultiLanguageStringData.FromWebTranslations("Admin Identities");

            // Define a filter with three options: denied, pending and granted.
            public IDictionary<string, IPredefinedFilterOption> Options { get; }
                = new Dictionary<string, IPredefinedFilterOption>
                {
            {
                "Admin Identities", new PredefinedFilterOption
                {
                    // Each parameter value defines a SQL clause that is applied when the filter is selected
                    ClauseProvider = new ClauseProvider("UID_PersonOrdered in (select UID_Person from Person where IdentityType = 'Admin')"),
                    DisplayName = MultiLanguageStringData.FromWebTranslations("Admin Identities")
                }
            },
            
            };
        }

    }
}
