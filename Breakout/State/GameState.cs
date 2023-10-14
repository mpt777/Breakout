using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.State
{
    public class GameState
    {
        public GameState() { }

        /// <summary>
        /// Overloaded constructor used to create an object for long term storage
        /// </summary>
        /// <param name="score"></param>
        /// <param name="level"></param>
        public GameState(uint score, ushort level)
        {
            this.Score = score;
            this.Level = level;
            this.TimeStamp = DateTime.Now;
        }

        public uint Score { get; set; }
        public ushort Level { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}

