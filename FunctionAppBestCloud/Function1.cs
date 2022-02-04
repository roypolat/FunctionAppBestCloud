using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Net;
namespace FunctionApp2
{
    public static class Function1
    {
        [FunctionName("Function1")]
        
        //http trigger fonskiyonu.sadece get metodu alıyor.
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "temperature")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            // istenilen yeri kullanıcadan almak için city değişkenini kullanıyoruz.
            string city = req.Query["city"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            city = city ?? data?.city;
            //wheather api ile gerekli bilgileri çekiyoruz
            string uri = String.Format("http://api.openweathermap.org/data/2.5/weather?q=" + city + "&mode=xml&appid=4238ec8727d226cd414e33119add4e34");
            
            //api dan gelen xml dosyasını stringe çevirip oluşturacağımız listin içine atıyoruz ve veri kullanıma hazır hale geliyor 
            XmlTextReader reader = new XmlTextReader(uri);
            List<string> mylist = new List<string>();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.



                        while (reader.MoveToNextAttribute()) // Read the attributes.
                            Console.Write(" " + reader.Name + "='" + reader.Value + "'");

                        Console.Write("");
                        Console.WriteLine("");
                        mylist.Add(reader.Name);
                        mylist.Add(reader.Value);
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        Console.WriteLine(reader.Value);



                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        Console.Write("" + reader.Name);


                        Console.WriteLine("");

                        break;
                }

            }
            //isim ve soyad verisini json a çevirme
            string jsonData = @"{'FirstName':'Roy','LastName':'Polat'}";
            

            string responseMessage = string.IsNullOrEmpty(city)
                ? "This HTTP triggered function executed successfully. Pass a city in the query string or in the request body for a personalized response."
                : $" {jsonData}  \xA {city} = {mylist[5]}";




            return new OkObjectResult(responseMessage);
        }
    }
}
