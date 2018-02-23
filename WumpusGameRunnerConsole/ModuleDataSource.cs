using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace net.PhoebeZeitler.WumpusGameRunnerConsole
{
    /**
     * This is the base class for all modules' data source objects.
     */
    public abstract class ModuleDataSourceBase : ISerializable
    {
        public string ModuleIdentifier
        {
            get;
            protected set;
        }
        public ulong HomeChannel
        {
            get;
            set;
        }
        public ulong GameRunnerID
        {
            get;
            set;
        }
        private Dictionary<ulong, ModuleCharacterSheet> _characters;

        protected ModuleDataSourceBase()
        {

        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("modIdent", ModuleIdentifier, typeof(string));
            info.AddValue("channelIdent", HomeChannel, typeof(ulong));
            info.AddValue("gameRunnerIdent", GameRunnerID, typeof(ulong));
            info.AddValue("characters", _characters, typeof(Dictionary<ulong, ModuleCharacterSheet>));
        }

        public ModuleDataSourceBase(SerializationInfo info, StreamingContext context)
        {
            ModuleIdentifier = (string)info.GetValue("modIdent", typeof(string));
            HomeChannel = (ulong)info.GetValue("channelIdent", typeof(ulong));
            GameRunnerID = (ulong)info.GetValue("gameRunnerIdent", typeof(ulong));
            _characters = (Dictionary<ulong, ModuleCharacterSheet>)info.GetValue("characters", typeof(Dictionary<ulong, ModuleCharacterSheet>));
        }
    }

    /**
     * This is the base abstract class for all modules' CharacterSheet objects.
     */
    public abstract class ModuleCharacterSheet : ISerializable
    {
        public string ModuleIdentifier { get; private set; }
        public string CharacterName { get; protected set; }
        public ulong PlayerID { get; protected set; }
        public List<StatisticScore> Statistics { get; protected set; }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("modIdent", ModuleIdentifier, typeof(string));
            info.AddValue("charname", CharacterName, typeof(string));
            info.AddValue("DiscordUser_ID", PlayerID, typeof(ulong));
            info.AddValue("statisticsList", Statistics, typeof(List<StatisticScore>));
        }

        public ModuleCharacterSheet(SerializationInfo info, StreamingContext context)
        {
            ModuleIdentifier = (string)info.GetValue("modIdent", typeof(string));
            CharacterName = (string)info.GetValue("charname", typeof(string));
            PlayerID = (ulong)info.GetValue("DiscordUser_ID", typeof(ulong));
            Statistics = (List<StatisticScore>)info.GetValue("statisticsList", typeof(List<StatisticScore>));
        }
    }

    public class StatisticScore : ISerializable
    {
        public string Name { get; private set; }
        public string Abbreviation { get; private set; }
        public int Value { get; private set; }

        //default constructor
        public StatisticScore()
        {
        }

        public StatisticScore(string name, string abbr, int val)
        {
            Name = name;
            Abbreviation = abbr;
            Value = val;
        }

        new public virtual string ToString
        {
            get { return Name + " (" + Abbreviation + "): " + Value; }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", Name, typeof(string));
            info.AddValue("abbr", Abbreviation, typeof(string));
            info.AddValue("intval", Value, typeof(int));
        }

        public StatisticScore(SerializationInfo info, StreamingContext context)
        {
            Name = (string) info.GetValue("name", typeof(string));
            Abbreviation = (string)info.GetValue("abbr", typeof(string));
            Value = (int)info.GetValue("intval", typeof(int));
        }
    }

    public class StatisticWithModifier : StatisticScore, ISerializable
    {
        public int Modifier { get; private set; }

        public StatisticWithModifier() : base()
        {
        }

        public StatisticWithModifier(string name, string abbr, int val, int mod) : base(name, abbr, val)
        {
            Modifier = mod;
        }

        public override string ToString
        {
            get { return base.ToString + " (" + Modifier + ")"; }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("intmod", Modifier, typeof(int));
        }

        public StatisticWithModifier(SerializationInfo info, StreamingContext context) : base (info, context)
        {
            Modifier = (int)info.GetValue("intmod", typeof(int));
        }
    }
}
