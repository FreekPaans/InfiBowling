using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BowlingGame {
	abstract class Frame {
		readonly int _frameNumber;
		readonly int _worpIndex;
		readonly List<int> _worpen;
		protected readonly List<int> _worpenInFrame = new List<int>();

		public Frame(int frameNumber,int worpIndex,List<int> worpen) {
			_frameNumber = frameNumber;
			_worpIndex = worpIndex;
			_worpen = worpen;
		}

		public void AddWorp(int pins) {
			_worpenInFrame.Add(pins);
		}

		internal static Frame New(int frameNumber, int worpIndex, List<int> worpen) {
			if(IsLastFrame(frameNumber)) {
				return new LastFrame(frameNumber,worpIndex,worpen);
			}
			return new NormalFrame(frameNumber,worpIndex,worpen);
		}

		private static bool IsLastFrame(int frameNumber) {
			return frameNumber == 10;
		}

		class NormalFrame : Frame {
			public NormalFrame(int frameNumber, int worpIndex, List<int> worpen) : base(frameNumber,worpIndex,worpen){
				
			}
			public override bool IsKlaar() {
				return _worpenInFrame.Count==2 || IsSpare() || IsStrike();
			}

		
			protected override int GetFramePins() {
				return _worpenInFrame.Sum();
			}
		}

		class LastFrame : Frame {
			public LastFrame(int frameNumber, int worpIndex, List<int> worpen) : base(frameNumber,worpIndex,worpen){
				
			}
			public override bool IsKlaar() {
				if(IsSpare() || IsStrike()) {
					return _worpenInFrame.Count == 3;
				}
				
				return _worpenInFrame.Count == 2;
			}
			

			protected override int GetFramePins() {
				if(IsStrike()) {
					return 10;
				}
				return _worpenInFrame.Take(2).Sum();
			}
		}

		public abstract bool IsKlaar();

		internal int CalculateScore() {
			var frameScore = GetFramePins();

			if(IsStrike()) {
				frameScore+=GetWorpOrZero(_worpIndex+1)+GetWorpOrZero(_worpIndex+2);
			}

			if(IsSpare()) {
				frameScore += GetWorpOrZero(_worpIndex+2);
			}

			return frameScore;
			
		}

		protected abstract int GetFramePins();
		
		private bool IsStrike() {
			if(!_worpenInFrame.Any()) {
				return false;
			}

			return _worpenInFrame.First() ==10;
		}

		private bool IsSpare() {
			if(_worpenInFrame.Count<2) {
				return false;
			}
			return _worpenInFrame.Take(2).Sum()== 10;
		}

		
		private int GetWorpOrZero(int i) {
			if(i>=_worpen.Count) {
				return 0;
			}
			return _worpen[i];
		}
	}
}
