using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.PhoebeZeitler.WumpusGameRunnerConsole.GoldenSkyStoriesModule
{
    public struct GSSConfigJson
    {
        [JsonProperty("listMentions")]
        public Boolean ListMentions { get; private set; }

        /**
        [JsonProperty("prefix")]
        public string CommandPrefix { get; private set; }

        [JsonProperty("appIdentifier")]
        public string AppIdentifier { get; private set; }
        **/
    }

    class GSSModuleConfig
    {
        private static GSSConfigJson _config;

        static GSSModuleConfig()
        {
            var json = "";
            using (var fs = File.OpenRead("gssconfig.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = sr.ReadToEnd();

            // next, let's load the values from that file
            // to our client's configuration
            _config = JsonConvert.DeserializeObject<GSSConfigJson>(json);
        }

        public static Boolean ListMentions
        {
            get
            {
                return _config.ListMentions;
            }
        }

    }
}
