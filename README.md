# ConfigViewer

## Sitecore configuration viewer

Simple console app that mimics the sitecore admin ShowConfig.aspx tool.

Occasionally we need to see the effective configuration of sitecore and the built in ShowConfig.aspx and SHowConfigLayers.aspx will not suffice, usually because it is from a ContentDelivery server or other.

Download you App_Config folder and place it in the /bin.

In the program.cs set the desired roles and layers to see the effective configuration.

e.g.

```
  var xml = c.Run(
            "Sitecore|Modules|Environment|Custom",
            new Dictionary<string, string>
            {
              { "role", "ContentDelivery"},
              { "search", "Solr"}
            });
```

## TODO
Make the roles and layers configurable via the app.config :)

