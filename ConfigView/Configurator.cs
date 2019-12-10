using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml;
using Sitecore.Configuration;

namespace ConfigView
{
    public class Configurator
    {
        public XmlDocument Run( string layers, Dictionary<string, string> roles)
        {
            var nameValueCollection = new NameValueCollection();

            foreach (var role in roles)
            {
                nameValueCollection.Add($"{role.Key}:{RuleBasedConfigReader.RuleDefineSuffix}", role.Value);
            }
            
            return (nameValueCollection.Keys.Count == 0 && layers == string.Empty)
                ? Factory.GetConfiguration() 
                : GetRuleBasedConfiguration(nameValueCollection, layers ?? "");
        }
        
        /// <summary>
        /// Gets the configuration for specific configuration.
        /// </summary>
        /// <param name="ruleCollection">The rules collection.</param>
        /// <param name="layers">The layers.</param>
        /// <returns>
        /// Xml document containing the entire Sitecore configuration for specific configuration.
        /// </returns>
        protected virtual XmlDocument GetRuleBasedConfiguration(NameValueCollection ruleCollection, string layers)
        {
            var layers2 = layers.Split(new[]{'|'}, StringSplitOptions.RemoveEmptyEntries);
            var includeFiles = GetIncludeFiles(layers2);
            var ruleBasedConfigReader = new MyRuleBasedConfigReader(includeFiles, ruleCollection);

            return ruleBasedConfigReader.DoGetConfiguration();
        }

        /// <summary>
        /// Returns a collection of include files for specific layers.
        /// </summary>
        /// <param name="layers">The layers array.</param>
        /// <returns>The list of include files.</returns>
        protected IEnumerable<string> GetIncludeFiles(string[] layers)
        {
            var layeredConfiguration = GetLayeredConfiguration();
            var source = layeredConfiguration.ConfigurationLayerProviders.SelectMany(x => x.GetLayers());

            if (layers.Length == 0)
            {
                return source.SelectMany(x => x.GetConfigurationFiles()).Distinct(StringComparer.OrdinalIgnoreCase);
            }

            return (from x in source
                where layers.Contains(x.Name)
                select x).SelectMany(x => x.GetConfigurationFiles()).Distinct(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the layered configuration.
        /// </summary>
        /// <returns>An instance of the <see cref="T:Sitecore.Configuration.LayeredConfigurationFiles" /> class.</returns>
        protected LayeredConfigurationFiles GetLayeredConfiguration()
        {
            return new LayeredConfigurationFiles();
        }
    }
}