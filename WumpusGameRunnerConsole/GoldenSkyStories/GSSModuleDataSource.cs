using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace net.PhoebeZeitler.WumpusGameRunnerConsole.GoldenSkyStoriesModule
{
    public class GSSModuleDataSource : ModuleDataSourceBase
    {
        public GSSModuleDataSource()
        {
            this.ModuleIdentifier = "GSS";
        }

        public GSSModuleDataSource(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.ModuleIdentifier = "GSS";
            //nothing special
        }

    }
}
