using System.Collections.Generic;
using System.Linq;

namespace BestBowlingGame
{
    public class Frame
    {
        private List<int> Throws = new List<int>();

        public void AddThrow(int aantalPins)
        {
            Throws.Add(aantalPins);
        }

        public bool IsDone()
        {
            return Throws.Count == 2 || ScoreInFrame() == 10;
        }

        public bool IsStrike()
        {
            return Throws.Count == 1 && ScoreInFrame() == 10;
        }

        public bool IsSpare()
        {
            return Throws.Count == 2 && ScoreInFrame() == 10;
        }

        public int ScoreInFrame()
        {
            return Throws.Sum();
        }

        public int FirstThrowScore()
        {
            return Throws[0];
        }
    }
}
