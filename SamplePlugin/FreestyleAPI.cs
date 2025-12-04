using QBM.CompositionApi.Crud;
using QBM.CompositionApi.DataSources;
using QBM.CompositionApi.DataSources.SqlWizard;
using QBM.CompositionApi.Definition;
using QBM.CompositionApi.Handling;
using QER.CompositionApi.Portal;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using VI.DB;
using VI.DB.Entities;
using VI.DB.Sync;

namespace Api
{
    public class FreestyleAPI : IApiProviderFor<PortalApiProject>
    {
        public void Build(IApiBuilder builder)
        {
            builder.AddMethod(Method.Define("freestyleEx1")
                .HandleGet(async request =>
                {
                    var collection = await request.Session.Source().GetCollectionAsync(Query.From("Person").Take(5).OrderBy("FirstName").Select("FirstName", "LastName")).ConfigureAwait(false);
                   
                    Identity[] names = [];

                    foreach (var Identity in collection)
                    {
                        Identity returnedPerson = new Identity();
                        returnedPerson.FirstName = Identity.GetValue("FirstName").ToString();
                        returnedPerson.LastName = Identity.GetValue("LastName").ToString();

                        names = names.Append(returnedPerson).ToArray();
                     

                    }

                    return names;

                })
            );

            builder.AddMethod(Method.Define("freestyleEx2")
               .Handle<Identity, Identity>("POST", (person, qr) =>
               {
                   IEntity createPerson = qr.Session.Source().CreateNew("Person");
                   IEntity assignEset = qr.Session.Source().CreateNew("PersonHasEset");

          
                   createPerson.PutValue("FirstName", person.FirstName);
                   createPerson.PutValue("LastName", person.LastName);
                   createPerson.PutValue("Remarks", person.Remarks);
                   createPerson.Save(qr.Session);

                   assignEset.PutValue("UID_Person", createPerson.GetValue("UID_Person"));


                   string UID_Eset = qr.Session.Source().GetSingleValue<string>(tablename:"ESet", columnname:"UID_ESet", whereclause:$"Ident_Eset='{person.ESet}'");

                   assignEset.PutValue("UID_Eset", UID_Eset);

                   assignEset.Save(qr.Session);


                   var uow = qr.Session.StartUnitOfWork();
                   System.Threading.Thread.Sleep(5000);
                   uow.Generate(assignEset, "CREATEITSHOPORDER");
                  
                   uow.Commit();


                   Identity returnedPerson = new Identity();
                   returnedPerson.FirstName = person.FirstName;
                   returnedPerson.LastName = person.LastName;
                   returnedPerson.Remarks = person.Remarks;
                   returnedPerson.ESet = person.ESet;

                   return returnedPerson;
               })

           
           );

            builder.AddMethod(Method.Define("freestyleEx3")
               .HandleGet(async request =>
               {
                   var collection = await request.Session.Source().GetCollectionAsync(
                       Query.From("Person")
                       .Select("FirstName", "LastName","Remarks","EntryDate")
                       .Where("Remarks='API Development Training'")).ConfigureAwait(false);

                   Identity[] names = [];
               

                   foreach (var Identity in collection)
                   {
                       Identity returnedPerson = new Identity();
                       returnedPerson.FirstName = Identity.GetValue("FirstName").ToString();
                       returnedPerson.LastName = Identity.GetValue("LastName").ToString();
                       returnedPerson.Remarks = Identity.GetValue("Remarks").ToString();




                       returnedPerson.EntryDateWeekday = Identity.GetValue("EntryDate").ToDateTime(CultureInfo.InvariantCulture).ToString("dddd", CultureInfo.InvariantCulture);

                       names = names.Append(returnedPerson).ToArray();


                   }

                   return names;

               })
           );


            builder.AddMethod(Method.Define("freestyleEx4")
             .Handle<Identity, Identity>("PUT", (person, qr) =>
             {
                 IEntity existingPerson = qr.Session.Source().Get("Person", person.UID_Person);

                 existingPerson.PutValue("FirstName", person.NewName);
                 
                 

                 existingPerson.Save(qr.Session);


                 Identity returnedPerson = new Identity();
                 returnedPerson.FirstName = existingPerson.GetValue("FirstName");
                 returnedPerson.LastName = existingPerson.GetValue("LastName");
                 returnedPerson.Remarks = existingPerson.GetValue("Remarks");
                 returnedPerson.UID_Person = existingPerson.GetValue("UID_Person");

                 return returnedPerson;
             })


         );



        }

    }

    public class Identity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Remarks { get; set; }
        public string UID_Person { get; set; }
        public string NewName { get; set; }
        public string ESet { get; set; }
        public string EntryDateWeekday { get; set; }
    }
}
