using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace net.PhoebeZeitler.WumpusGameRunnerConsole
{

    /**
     * This will hold the data for all games and sessions.
     * 
     * TODO: It does not yet support serialization/persistence.
     **/
    public class MasterDataSingleton
    {
        #region Singleton Support
        private static MasterDataSingleton _instance;
        public static MasterDataSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MasterDataSingleton();
                }
                return _instance;
            }
        }
        #endregion

        private List<SupportedGameIdentifier> _supportedGames;

        // Channel ID -> Module Data
        private Dictionary<DiscordChannel, ModuleDataSource> _masterDataSource;

        /**
         * Default constructor
         **/
        private MasterDataSingleton()
        {
            _masterDataSource = new Dictionary<DiscordChannel, ModuleDataSource>();
            _supportedGames = new List<SupportedGameIdentifier>();
        }

        public ModuleDataSource GetDataSource(DiscordChannel channel)
        {
            ModuleDataSource retval = null;
            if (_masterDataSource.ContainsKey(channel))
            {
                retval = _masterDataSource[channel];
            }
            //TODO: better exception handling
            /*
            else
            {
                throw new Exception("No game set for channel " + channel.Name);
            }
            */

            return retval;
        }

        #region Setting/Updating Data Source
        private bool DoesDataExistForChannel(DiscordChannel channel)
        {
            return (_masterDataSource.ContainsKey(channel));           
        }
        private void SetNewDataSource(DiscordChannel channel, ModuleDataSource dataSource)
        {
            _masterDataSource.Add(channel, dataSource);
        }
        private void ReplaceDataSource(DiscordChannel channel, ModuleDataSource dataSource)
        {
            _masterDataSource[channel] = dataSource;
        }

        public void SetDataSourceForChannel(DiscordChannel channel, ModuleDataSource dataSource, bool safeMode = true)
        {
            if (!DoesDataExistForChannel(channel))
            {
                SetNewDataSource(channel, dataSource);
            } else
            {
                if(!safeMode)
                {
                    ReplaceDataSource(channel, dataSource);
                } 
                //TODO: better exception handling
                /*
                else
                {
                    throw new Exception("Data already exists for " + channel.Name + " and safeMode was enabled");
                }
                */
            }
        }
        #endregion

        #region Game Support Listing
        public void RegisterSupportedGameType(SupportedGameIdentifier identifier)
        {
            _supportedGames.Add(identifier);
        }

        public List<SupportedGameIdentifier> GetSupportedGames()
        {
            List<SupportedGameIdentifier> retval = this._supportedGames;
            return retval;
        }
        #endregion
    }

    public class SupportedGameIdentifier
    {
        public List<string> Identifiers { get; private set; }
        public string Name { get; private set; }
        public string ShortName { get; private set; }

        public string CreatorName { get; private set; }
        public string CreatorAddress { get; private set; }

        public string ImplementorName { get; private set; }
        public string ImplementorAddress { get; private set; }

        public SupportedGameIdentifier(string name, string shortName, string creatorName, string creatorAddress, string implementorName, string implementorAddress, string[] identifiers)
        {
            this.Name = name;
            this.ShortName = shortName;
            this.CreatorName = creatorName;
            this.CreatorAddress = creatorAddress;
            this.ImplementorName = implementorName;
            this.ImplementorAddress = implementorAddress;

            this.Identifiers = new List<string>(identifiers);
          
        }

        public bool Contains(string identifier)
        {
            bool retval = false;
            string findThis = identifier.ToLower();
            foreach (string candidate in this.Identifiers)
            {
                if (candidate.ToLower().Equals(findThis))
                {
                    retval = true;
                    break;
                }
            }
            return retval;
        }
    }
}
