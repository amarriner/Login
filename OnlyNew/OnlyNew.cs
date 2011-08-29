using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using Terraria_Server;
using Terraria_Server.Plugin;
using Terraria_Server.Collections;
using Terraria_Server.Commands;
using Terraria_Server.Events;
using Terraria_Server.Logging;

using System.Xml;

namespace OnlyNew
{
    public class OnlyNew : Plugin
    {
        public static OnlyNew plugin;
        public Properties properties;
        public String xmlFilename = "onlynew.xml";
        public String xmlNamespace = "onlyNewPlugin";
        public Terraria_Server.Misc.Color chatColor = new Terraria_Server.Misc.Color(100, 200, 100);
        public XmlDocument onlyNewXML = new XmlDocument();
        public string pluginFolder;

        public override void Load()
        {
            Name = "House";
            Description = "A plugin to allow players to define safe areas";
            Author = "amarriner";
            Version = "0.3.5";
            TDSMBuild = 31;

            plugin = this;

            this.registerHook(Hooks.PLAYER_TILECHANGE);
        }

        public override void Enable()
        {
            pluginFolder = Statics.PluginPath + Path.DirectorySeparatorChar + "OnlyNew";
            CreateDirectory(pluginFolder);

            properties = new Properties(pluginFolder + Path.DirectorySeparatorChar + "onlyNew.properties");
            properties.Load();
            properties.Save();

            // LoadHouseData();
            Program.tConsole.WriteLine(base.Name + " enabled.");
        }

        public override void Disable()
        {
            // SaveHouseData();
            Program.tConsole.WriteLine(base.Name + " disabled.");
        }

        private static void CreateDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
    }
}
