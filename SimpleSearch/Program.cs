using System;
using System.Collections.Generic;
using System.Xml.Linq;
namespace SimpleFhir
{
  class Program
  {
    static void Main(string[] args)
    {
      //The fhir server end point address      
      string ServiceRootUrl = "http://PyroHealth.net/test/stu3/fhir";
      //Create a client to send to the server at a given endpoint.
      var FhirClient = new Hl7.Fhir.Rest.FhirClient(ServiceRootUrl);
      // increase timeouts since the server might be powered down
      FhirClient.Timeout = (60 * 1000);

      Console.WriteLine("Press any key to send to server: " + ServiceRootUrl);
      Console.WriteLine();
      Console.ReadKey();
      try
      {
        //Attempt to send the resource to the server endpoint
        Hl7.Fhir.Model.Bundle ReturnedSearchBundle = FhirClient.Search<Hl7.Fhir.Model.Patient>(new string[] { "family=Fhirman" });
        Console.WriteLine(string.Format("Found: {0} Fhirman patients.", ReturnedSearchBundle.Total.ToString()));
        Console.WriteLine("Their logical IDs are:");
        foreach (var Entry in ReturnedSearchBundle.Entry)
        {
          Console.WriteLine("ID: " + Entry.Resource.Id);
        }
        Console.WriteLine();
      }
      catch (Hl7.Fhir.Rest.FhirOperationException FhirOpExec)
      {
        //Process any Fhir Errors returned as OperationOutcome resource
        Console.WriteLine();
        Console.WriteLine("An error message: " + FhirOpExec.Message);
        Console.WriteLine();
        string xml = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToXml(FhirOpExec.Outcome);
        XDocument xDoc = XDocument.Parse(xml);
        Console.WriteLine(xDoc.ToString());
      }
      catch (Exception GeneralException)
      {
        Console.WriteLine();
        Console.WriteLine("An error message: " + GeneralException.Message);
        Console.WriteLine();
      }
      Console.WriteLine("Press any key to end.");
      Console.ReadKey();
    }
  }
}