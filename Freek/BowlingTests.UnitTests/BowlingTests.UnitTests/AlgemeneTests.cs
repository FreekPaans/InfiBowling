using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BowlingGame;

namespace BowlingTests.UnitTests {
	[TestClass]
	public class AlgemeneTests {
		[TestMethod]
		public void als_we_een_nieuwe_game_hebben_en_we_vragende_score_op_sdan_is_de_score_0() {
			var game = GetGame();

			Assert.AreEqual(0, game.CurrentScore);
		}

		private static Game GetGame() {
			return new Game();
		}

		[TestMethod]
		public void als_we_een_worp_gegooid_met_5_pins_en_we_vragen_de_score_op_dan_is_de_current_score_5() {
			var game = GetGame();

			game.Gooi(5);

			Assert.AreEqual(5,game.CurrentScore);
		}

		[TestMethod]
		public void als_we_een_worp_gegooid_met_7_pins_en_we_vragen_de_score_op_dan_is_de_current_score_7() {
			var game = GetGame();

			game.Gooi(7);

			Assert.AreEqual(7,game.CurrentScore);
		}

		[TestMethod]
		public void als_we_2_ballen_gooien_dan_is_de_score_de_som_van_die_scores() {
			var game = GetGame();

			game.Gooi(6);
			game.Gooi(2);

			Assert.AreEqual(8,game.CurrentScore);
		}

		[TestMethod]
		public void als_we_4_5_4_gooien_dan_is_de_score_van_frame_1_9() {
			var game = GetGame();
			game.Gooi(4);
			game.Gooi(5);
			game.Gooi(4);
			Assert.AreEqual(9,game.ScoreVoorFrame(1));
		}

		[TestMethod]
		public void als_we_3_2_4_gooien_dan_is_de_score_van_frame_1_5() {
			var game = GetGame();
			game.Gooi(3);
			game.Gooi(2);
			game.Gooi(4);
			Assert.AreEqual(5,game.ScoreVoorFrame(1));
		}

		[TestMethod]
		public void als_we_3_2_4_gooien_dan_is_de_score_van_frame_2_9() {
			var game = GetGame();
			game.Gooi(3);
			game.Gooi(2);
			game.Gooi(4);
			Assert.AreEqual(9,game.ScoreVoorFrame(2));
		}

		[TestMethod]
		public void als_we_een_spare_gooien_en_in_de_frame_daarna_3_dan_is_de_score_voor_frame_1_13() {
			var game = GetGame();
			game.Gooi(3);
			game.Gooi(7);
			game.Gooi(3);
			Assert.AreEqual(13,game.ScoreVoorFrame(1));
		}

		[TestMethod]
		public void als_we_een_spare_gooien_en_in_de_frame_daarna_3_dan_is_de_current_score_16() {
			var game = GetGame();
			game.Gooi(3);
			game.Gooi(7);
			game.Gooi(3);
			Assert.AreEqual(16,game.CurrentScore);
		}

		
		[TestMethod]
		public void als_we_3_7_7_3_5_gooien_dan_is_de_score_voor_frame_2_32() {
			var game = GetGame();
			game.Gooi(3);
			game.Gooi(7);
			game.Gooi(7);
			game.Gooi(3);
			game.Gooi(5);
			Assert.AreEqual(32,game.ScoreVoorFrame(2));
		}

		[TestMethod]
		public void als_we_3_7_7_3_5_gooien_dan_is_de_current_score_37() {
			var game = GetGame();
			game.Gooi(3);
			game.Gooi(7);
			game.Gooi(7);
			game.Gooi(3);
			game.Gooi(5);
			Assert.AreEqual(37,game.CurrentScore);
		}

		[TestMethod]
		public void als_we_een_strike_gooien_en_dan_3_dan_is_de_score_voor_frame_1_13() {
			var game = GetGame();
			game.Gooi(10);
			game.Gooi(3);
			Assert.AreEqual(13,game.ScoreVoorFrame(1));
		}

		[TestMethod]
		public void als_we_een_strike_gooien_en_dan_3_dan_is_de_current_score_16() {
			var game = GetGame();
			game.Gooi(10);
			game.Gooi(3);
			Assert.AreEqual(16,game.CurrentScore);
		}

		[TestMethod]
		public void als_we_een_strike_gooien_en_dan_3_4_dan_is_de_score_voor_frame_1_17() {
			var game = GetGame();
			game.Gooi(10);
			game.Gooi(3);
			game.Gooi(4);
			Assert.AreEqual(17,game.ScoreVoorFrame(1));
		}

		[TestMethod]
		public void als_we_een_strike_gooien_en_dan_3_4_dan_is_de_current_score_24() {
			var game = GetGame();
			game.Gooi(10);
			game.Gooi(3);
			game.Gooi(4);
			Assert.AreEqual(24,game.CurrentScore);
		}

		[TestMethod]
		public void als_we_2_strikes_gooien_dan_is_de_score_voor_frame_1_20() {
			var game = GetGame();
			game.Gooi(10);
			game.Gooi(10);
			Assert.AreEqual(20,game.ScoreVoorFrame(1));
		}

	

		[TestMethod]
		public void als_we_3_strikes_gooien_dan_is_de_score_voor_frame_1_30() {
			var game = GetGame();
			game.Gooi(10);
			game.Gooi(10);
			game.Gooi(10);
			Assert.AreEqual(30,game.ScoreVoorFrame(1));
		}

		[TestMethod]
		public void als_we_2_strikes_gooien_dan_is_de_current_score_30() {
			var game = GetGame();
			game.Gooi(10);
			game.Gooi(10);
			Assert.AreEqual(30,game.CurrentScore);
		}

		[TestMethod]
		public void als_we_2_strikes_gooien_dan_is_de_score_voor_frame_2_30() {
			var game = GetGame();
			game.Gooi(10);
			game.Gooi(10);
			Assert.AreEqual(30,game.ScoreVoorFrame(2));
		}

		[TestMethod]
		public void als_we_een_strike_gooien_in_de_laatste_beurt_en_dan_gooien_we_nog_2_strikes_dan_is_de_current_score_30() {
			var game = GetGame();
			Gooi9Frames0(game);
			game.Gooi(10);
			game.Gooi(10);
			game.Gooi(10);
			Assert.AreEqual(30,game.CurrentScore);
		}

		[TestMethod]
		public void als_we_een_spare_in_de_laatste_frame_gooien_en_dan_3_dan_is_de_current_score_13() {
			var game =GetGame();
			Gooi9Frames0(game);
			game.Gooi(3);
			game.Gooi(7);
			game.Gooi(3);
			Assert.AreEqual(13,game.CurrentScore);
		}

		[TestMethod]
		public void als_we_een_spare_in_de_laatste_frame_gooien_en_dan_3_dan_is_de_score_voor_frame_10_13() {
			var game =GetGame();
			Gooi9Frames0(game);
			game.Gooi(3);
			game.Gooi(7);
			game.Gooi(3);
			Assert.AreEqual(13,game.ScoreVoorFrame(10));
		}

		[TestMethod]
		public void als_we_een_spare_in_de_laatste_frame_gooien_en_dan_een_strike_dan_is_de_current_score_20() {
			var game =GetGame();
			Gooi9Frames0(game);
			game.Gooi(3);
			game.Gooi(7);
			game.Gooi(10);
			Assert.AreEqual(20,game.CurrentScore);
		}

		[TestMethod]
		public void als_we_een_8_frames_nul_gooien_dan_10_10_10_10_dan_is_de_current_score_60() {
			var game =GetGame();
			GooiXFrames0(game,8);
			game.Gooi(10);
			game.Gooi(10);
			game.Gooi(10);
			game.Gooi(10);
			Assert.AreEqual(60,game.CurrentScore);
		}

		[TestMethod]
		public void als_we_een_8_frames_nul_gooien_dan_10_10_10_10_dan_is_de_score_voor_frame_9_30_en_voor_frame_10_60() {
			var game =GetGame();
			GooiXFrames0(game,8);
			game.Gooi(10);
			game.Gooi(10);
			game.Gooi(10);
			game.Gooi(10);
			Assert.AreEqual(30,game.ScoreVoorFrame(9));
			Assert.AreEqual(60,game.ScoreVoorFrame(10));
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void als_we_in_een_afgesloten_game_gooien_dan_krijgen_we_een_invalid_operation_exception() {
			var game = GetGame();
			GooiXFrames0(game,10);
			game.Gooi(5);
		}

		private void Gooi9Frames0(Game game) {
			GooiXFrames0(game,9);
		}

		private void GooiXFrames0(Game game, int x) {
			for(var i=0;i<(x*2);i++) {
				game.Gooi(0);
			}
		}
	}
}
