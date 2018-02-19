using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace net.PhoebeZeitler.WumpusGameRunnerConsole
{
    /**
     * This is the base class for all modules' data source objects.
     */
    public abstract class ModuleDataSource
    {
        private DiscordUser _gameRunner;
        private Dictionary<DiscordUser, ModuleCharacterSheet> _characters;
        

    }

    /**
     * This is the base abstract class for all modules' CharacterSheet objects.
     */
    public abstract class ModuleCharacterSheet
    {
        public string CharacterName { get; protected set; }
        public DiscordUser PlayerID { get; protected set; }
        public List<StatisticScore> Statistics { get; protected set; }
    }

    public class StatisticScore
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
    }

    public class StatisticWithModifier : StatisticScore
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
    }
}
