using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingGame {
	public class Game {
		readonly List<int> _worpen;

		public int CurrentScore {
			get { return GetScorer().CurrentScore; }
		}


		public Game() {
			_worpen = new List<int>();
		}

		public void Gooi(int aantalPins) {
			if(GetScorer().IsKlaar) {
				throw new InvalidOperationException("Game al klaar");
			}

			_worpen.Add(aantalPins);
		}

		public int ScoreVoorFrame(int frame) {
			return GetScorer().ScoreVoorFrame(frame);
		}

		private Scorer GetScorer() {
			return new Scorer(_worpen);
		}

	}
}
