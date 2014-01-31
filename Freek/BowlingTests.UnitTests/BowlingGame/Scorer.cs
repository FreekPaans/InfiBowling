using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BowlingGame {
	class Scorer {
		readonly List<int> _worpen;

		public Scorer(ICollection<int> worpen) {
			_worpen = worpen.ToList();
		}

		internal int ScoreVoorFrame(int frameNumber) {
			var sum =0;

			var frames = BuildFrames();

			for(var i=0;i<Math.Min(frameNumber,frames.Count);i++) {
				sum+= frames[i].CalculateScore();
			}
			
			return sum;
		}

		private List<Frame> BuildFrames() {
			var frameIterator = 1;

			var frames = new List<Frame>();

			var frame = GetNextFrame(frameIterator,0);

			frames.Add(frame);

			for(var i=0;i<_worpen.Count;i++) {
				if(frame.IsKlaar()) {
					frameIterator++;
					frame = GetNextFrame(frameIterator,i);
					frames.Add(frame);
				}

				frame.AddWorp(_worpen[i]);

			}
			return frames;
		}

		private Frame GetNextFrame(int frameNumber, int worpIndex) {
			return Frame.New(frameNumber, worpIndex,_worpen);
		}

		private int GetCurrentFrame() {
			var frames = BuildFrames();

			if(frames.Last().IsKlaar()) {
				return frames.Count+1;
			}

			return frames.Count;
		}

	
		public int CurrentScore {
			get {
				return ScoreVoorFrame(GetCurrentFrame());
			}
		}

		public bool IsKlaar {
			get {	
				var currentFrame = GetCurrentFrame();

				if(currentFrame<=10) {
					return false;
				}
				return true;
			}
		}
	}
}
