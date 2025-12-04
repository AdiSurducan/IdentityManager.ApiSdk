using QBM.CompositionApi.Crud;
using QBM.CompositionApi.DataSources;
using QBM.CompositionApi.Definition;
using QBM.CompositionApi.Handling;
using QER.CompositionApi.Portal;
using System;
using System.Globalization;

namespace Api
{
    public class CRUDApi : IApiProviderFor<PortalApiProject>
    {
        public void Build(IApiBuilder builder)
        {
            builder.AddMethod(Method.Define("exercise1")
                .FromTable("Person")
                .EnableRead()
                .WithResultColumns("Firstname", "LastName")
            );

            builder.AddMethod(Method.Define("exercise2")
                .FromTable("Person")
                .EnableCreate()
                .WithWritableAllColumns()
            );

            builder.AddMethod(Method.Define("exercise3")
               .FromTable("Person")
               .EnableRead()
               .WithResultColumns("EntryDate", "FirstName", "LastName")
               .WithParameter("remarks")
               .WithWhereClause((request) =>
               {
                   var remarksValue = request.Parameters.Get<string>("remarks");
                   return String.Format("Remarks = '{0}'", remarksValue);

               })
               .WithCalculatedProperties(new CalculatedProperty<string>("EntryDateWeekday",

                   context =>
                   {
                       var dt = context.Entity
                           .GetValue("EntryDate")
                           .ToDateTime(CultureInfo.InvariantCulture);

                       return dt.ToString("dddd", CultureInfo.InvariantCulture);
                   }
               ))
           );


            builder.AddMethod(Method.Define("exercise4")
               .FromTable("Person")
               .EnableUpdate()
               .WithWritableAllColumns()
           );

            builder.AddMethod(Method.Define("exercise5")
               .FromTable("PersonHasESet")
               .EnableCreate()
               .WithWritableAllColumns()
           );


            builder.AddMethod(Method.Define("exercise8")
               .FromTable("Person")
               .EnableRead()
               .WithParameter("email")
               .WithWhereClause((request) =>
               {
                   var emailValue = request.Parameters.Get<string>("email");
                   return String.Format("DefaultEmailAddress = '{0}'", emailValue);

               })
               .WithResultColumns("JPegPhoto")



             );

        }

    }
}
