using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Net;
using NetConfigProfile.Serialize;
using System.Management;
using System.Collections.ObjectModel;
using System.IO;

namespace NetConfigProfile.Cmdlet
{
    [Cmdlet(VerbsDiagnostic.Test, "Process")]
    public class TestProcess : PSCmdlet
    {
        [Parameter]
        public string Name { get; set; }



        protected override void ProcessRecord()
        {

            string ifName = "イーサネット";
            string pnpid = "";

            


            foreach (ManagementObject mo in new ManagementClass("Win32_NetworkAdapter").GetInstances())
            {
                if ((bool)mo["PhysicalAdapter"])
                {
                    Console.WriteLine(mo["NetConnectionID"]);
                }
            }

            /*
            Console.WriteLine(pnpid);

            foreach (ManagementObject mo in
                new ManagementClass(@"root\wmi", "MSPower_DeviceEnable", new ObjectGetOptions()).
                GetInstances())
            {
                if(mo["InstanceName"].ToString().StartsWith(pnpid, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(ifName);
                    Console.WriteLine(mo["InstanceName"]);

                    mo["Enable"] = true;
                    mo.Put();
                    
                }
                
            }
            */



        }
    }

    public class DynamicParam2
    {
        [Parameter]
        public string AAAA { get; set; } = "aaaa";

        [Parameter]
        public string BBBB { get; set; } = "bbbb";
    }
}




/*
public object GetDynamicParameters()
{
    RuntimeDefinedParameterDictionary runtimeDefinedParameterDictionary = new RuntimeDefinedParameterDictionary();

    Collection<Attribute> attribute = new Collection<Attribute>()
    {
        new ParameterAttribute(),
        new ValidateSetAttribute(Directory.GetFiles(@"C:\Users\tq\Downloads\sample\sample3")),
    };

    RuntimeDefinedParameter runtimeDefinedparameter = new RuntimeDefinedParameter("filename", typeof(string), attribute);
    runtimeDefinedParameterDictionary.Add("fileName", runtimeDefinedparameter);

    _staticStorage = runtimeDefinedParameterDictionary;

    return runtimeDefinedParameterDictionary;
}
*/


/*
             NetworkProfile profile = new NetworkProfile()
            {
                Name = "Settei1",
                Interface = new InterfaceConfig[]
                {
                    new InterfaceConfig(){
                        Name = "イーサネット",
                        Enabled = true,
                        NetworkAddress = new AddressConfig[1]{new AddressConfig("192.168.151.54/24")},
                        DefaultGateway = "192.168.151.1",
                        DNSServer = new string[2]{"192.168.151.249", "192.168.151.250"},
                    }
                },
            };


            ManagementObject netAdapter = new ManagementClass("Win32_NetworkAdapter").
                GetInstances().
                OfType<ManagementObject>().
                FirstOrDefault(x =>
                    profile.Interface.Any(y =>
                        y.Name.Equals(x["NetConnectionID"] as string, StringComparison.OrdinalIgnoreCase)));
            ManagementObject netConfig = new ManagementClass("Win32_NetworkAdapterConfiguration").
                GetInstances().
                OfType<ManagementObject>().
                FirstOrDefault(x => (x["SettingID"] as string).Equals(netAdapter["GUID"] as string));



            netConfig.InvokeMethod("EnableStatic", new object[] {
                profile.Interface[0].GetIPAddresses(),
                profile.Interface[0].GetSubnetMasks()});

*/


//private static RuntimeDefinedParameterDictionary _staticStorage;

/*
public object GetDynamicParameters()
{
 RuntimeDefinedParameterDictionary dictionary = new RuntimeDefinedParameterDictionary();

 Collection<Attribute> attributes = new Collection<Attribute>()
 {
     new ParameterAttribute(){ Position = 0 },
     new ValidateSetAttribute(Directory.GetFiles(@"C:\Users\tq\Downloads\sample\san3")),
 };
 RuntimeDefinedParameter rdParam = new RuntimeDefinedParameter("FileName", typeof(string), attributes);
 dictionary.Add("FileName", rdParam);

 return dictionary;
}
*/
