using System;
using System.Collections.Generic;
using System.Xml.Linq;
namespace SimpleFhir
{
  class Program
  {
    static void Main(string[] args)
    {
      var MyPatient = new Hl7.Fhir.Model.Patient();

      //Patient's Name
      var PatientName = new Hl7.Fhir.Model.HumanName();
      PatientName.Use = Hl7.Fhir.Model.HumanName.NameUse.Official;
      PatientName.Prefix = new string[] { "Mr" };
      PatientName.Given = new string[] { "Sam" };
      PatientName.Family = "Fhirman";
      MyPatient.Name = new List<Hl7.Fhir.Model.HumanName>();
      MyPatient.Name.Add(PatientName);

      //Patient Identifier 
      var PatientIdentifier = new Hl7.Fhir.Model.Identifier();
      PatientIdentifier.System = "http://ns.electronichealth.net.au/id/hi/ihi/1.0";
      PatientIdentifier.Value = "8003608166690503";
      MyPatient.Identifier = new List<Hl7.Fhir.Model.Identifier>();
      MyPatient.Identifier.Add(PatientIdentifier);

      Console.WriteLine("Press any key to serialise Resource to the console as XML.");
      Console.ReadKey();
      Console.WriteLine("");
      try
      {
        //attempt to serialize the resource
        string xml = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToXml(MyPatient);
        XDocument xDoc = XDocument.Parse(xml);
        Console.Write(xDoc.ToString());
        Console.WriteLine("");
        Console.WriteLine("");
      }
      catch (Exception Exec)
      {
        Console.Write("Error message: " + Exec.Message);
      }
      Console.Write("Press any key to end.");
      Console.ReadKey();
    }
  }
}