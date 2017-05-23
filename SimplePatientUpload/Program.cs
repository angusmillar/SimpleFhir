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
      string ServiceRootUrl = "http://pyrohealth.net/test/stu3/fhir";

      //Create a patient resource instance
      var MyPatient = new Hl7.Fhir.Model.Patient();

      //Patient's Name
      var PatientName = new Hl7.Fhir.Model.HumanName();
      PatientName.Use = Hl7.Fhir.Model.HumanName.NameUse.Official;
      PatientName.Prefix = new string[] { "Mr" };
      PatientName.Given = new string[] { "Sam" };
      PatientName.Family = "Fhirman" ;
      MyPatient.Name = new List<Hl7.Fhir.Model.HumanName>();
      MyPatient.Name.Add(PatientName);

      //Patient Identifier 
      var PatientIdentifier = new Hl7.Fhir.Model.Identifier();
      PatientIdentifier.System = "http://ns.electronichealth.net.au/id/hi/ihi/1.0";
      PatientIdentifier.Value = "8003608166690503";
      MyPatient.Identifier = new List<Hl7.Fhir.Model.Identifier>();
      MyPatient.Identifier.Add(PatientIdentifier);

      //Create a client to send to the server at a given endpoint.
      var FhirClient = new Hl7.Fhir.Rest.FhirClient(ServiceRootUrl);

      // increase timeouts since the server might be powered down
      FhirClient.Timeout = (60 * 1000);

      Console.WriteLine("Press any key to send to server: " + ServiceRootUrl);
      Console.ReadKey();
      try
      {
        //Attempt to send the resource to the server endpoint
        Hl7.Fhir.Model.Patient ReturnedPatient = FhirClient.Create<Hl7.Fhir.Model.Patient>(MyPatient);
        Console.WriteLine(string.Format("Resource is available at: {0}", ReturnedPatient.Id));
        Console.WriteLine();
        Console.WriteLine("This is what we sent up: ");
        Console.WriteLine();
        string xml = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToXml(MyPatient);
        XDocument xDoc = XDocument.Parse(xml);
        Console.WriteLine(xDoc.ToString());
        Console.WriteLine();
        Console.WriteLine("This is what we received back: ");
        Console.WriteLine();
        xml = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToXml(ReturnedPatient);
        xDoc = XDocument.Parse(xml);
        Console.WriteLine(xDoc.ToString());
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