using System.Collections.Generic;
using System.Diagnostics;

namespace ConfigView
{
    class Program
    {
        static void Main(string[] args)
        {
            // app config folder is located in the bin folder
            // the app.config setting must be a relative physical path
            // e.g. <sitecore configSource="App_Config\Sitecore.config" />

            using (new FakeHttpContext.FakeHttpContext())
            {
                var c = new Configurator();

                // you can add whatever roles you like, and whatever features, e.g. we had a myFeature:define 
                // role values are | seperated, e.g. ContentManagement|Processing

                var xml = c.Run("Sitecore|Modules|Environment|Custom",
                    new Dictionary<string, string>
                  {
                { "role","ContentDelivery"},
                { "search","Solr"}
                  });

                System.IO.File.WriteAllText("config.xml", xml.OuterXml);
            }

            Process.Start("config.xml");
        }
    }
}
