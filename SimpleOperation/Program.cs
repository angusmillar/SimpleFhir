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
      string ServiceRootUrl = "http://sqlonfhir-stu3.azurewebsites.net/fhir";
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
        UriBuilder UriBuilderx = new UriBuilder(ServiceRootUrl);
        UriBuilderx.Path = "Patient/MY_PATIENT_ID";
        Hl7.Fhir.Model.Resource ReturnedResource = FhirClient.InstanceOperation(UriBuilderx.Uri, "everything");

        if (ReturnedResource is Hl7.Fhir.Model.Bundle)
        {
          Hl7.Fhir.Model.Bundle ReturnedBundle = ReturnedResource as Hl7.Fhir.Model.Bundle;
          Console.WriteLine("Received: " + ReturnedBundle.Total + " results, the resources are: ");
          foreach (var Entry in ReturnedBundle.Entry)
          {
            Console.WriteLine(string.Format("{0}/{1}", Entry.Resource.TypeName, Entry.Resource.Id));
          }
        }
        else
        {
          throw new Exception("Operation call must return a bundle resource");
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