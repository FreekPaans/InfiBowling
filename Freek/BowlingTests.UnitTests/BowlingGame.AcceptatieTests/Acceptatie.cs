using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BowlingGame.AcceptatieTests {
	[TestClass]
	public class Acceptatie {
		[TestMethod]
		public void gegeven_een_perfect_game_dan_is_de_score_300() {
			var game = GooiPerfectGame();

			Assert.AreEqual(300, game.CurrentScore);
		}

		

		[TestMethod]
		public void gegeven_een_perfect_game_dan_is_de_score_per_frame_30_60_etc() {
			var game = GooiPerfectGame();

			for(var i=1;i<=10;i++) {
				Assert.AreEqual(i*30, game.ScoreVoorFrame(i));
			}
		}

		[TestMethod]
		public void gegeven_het_voorbeeld_spel_dan_is_de_score_133() {
			var game = GooiVoorbeeldSpel();

			Assert.AreEqual(133,game.CurrentScore);
		}

		


		[TestMethod]
		public void gegeven_het_voorbeeld_spel_dan_klopt_de_score_per_frame() {
			var game = GooiVoorbeeldSpel();

			for(var i=1;i<=10;i++) {
				Assert.AreEqual(ScorePerFrame[i],game.ScoreVoorFrame(i));
			}
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void gegeven_het_spel_is_afgelopen_dan_kunnen_we_niet_meer_gooien() {
			var game = GetAfgelopenSpel();

			game.Gooi(10);
		}

		private Game GetAfgelopenSpel() {
			return GooiPerfectGame();
		}



		readonly static int[] VoorbeeldSpelScores=  new int[] { 1,4,4,5,6,4,5,5,10,0,1,7,3,6,4,10,2,8,6 };
		readonly static Dictionary<int,int> ScorePerFrame = new Dictionary<int,int> {
			{1,5},
			{2,14},
			{3,29},
			{4,49},
			{5,60},
			{6,61},
			{7,77},
			{8,97},
			{9,117},
			{10,133}
		};

		private Game GooiVoorbeeldSpel() {
			var game = new Game();

			foreach(var score in VoorbeeldSpelScores) {
				game.Gooi(score);
			}

			return game;
		}

		private Game GooiPerfectGame() {
			var game = new Game();
			for(var i=0; i<12;i++) {
				game.Gooi(10);
			}

			return game;
		}
	}
}
