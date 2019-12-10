using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace ConfigView
{
    public class MyRuleBasedConfigReader : RuleBasedConfigReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sitecore.Configuration.RuleBasedConfigReader" /> class.
        /// </summary>
        /// <param name="includeFiles">The include files.</param>
        /// <param name="ruleCollection">The rule collection.</param>
        public MyRuleBasedConfigReader(IEnumerable<string> includeFiles, NameValueCollection ruleCollection)
            : base(includeFiles, ruleCollection)
        {
        }

        public new XmlDocument DoGetConfiguration()
        {
            var configNode = GetConfigNode();

            Assert.IsNotNull((object)configNode, "Could not read Sitecore configuration.");

            var xmlDocument = new XmlDocument();

            xmlDocument.AppendChild(xmlDocument.CreateElement("sitecore"));
            GetConfigPatcher(xmlDocument.DocumentElement).ApplyPatch(configNode);
            ExpandIncludeFiles(xmlDocument.DocumentElement, new Hashtable());
            LoadIncludeFiles(xmlDocument.DocumentElement);
            ReplaceGlobalVariables(xmlDocument.DocumentElement);
            ReplaceEnvironmentVariables(xmlDocument.DocumentElement);

            return xmlDocument;
        }
    }
}