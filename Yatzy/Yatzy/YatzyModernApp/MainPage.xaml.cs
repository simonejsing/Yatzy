using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Yatzy;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace YatzyModernApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly YatzyGameEngine game;

        public ViewModel ViewModel { get; private set; }

        public MainPage()
        {
            game = YatzyGameEngine.GetInstance();

            ViewModel = new ViewModel(game.ScoreCard);
            ViewModel.CurrentRoll = new Roll(new byte[] { 0, 0, 0, 0, 0, 0 });

            this.InitializeComponent();
            this.DataContext = ViewModel;
        }

        private async void RollDice_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.CurrentRoll = game.RollDice();
                ViewModel.ResetHoldMask();
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "Error");
                await dialog.ShowAsync();
            }
        }

        private readonly List<int> _holdList = new List<int>();

        private async void RerollDice_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Parse hold mask
                _holdList.Clear();
                if (ViewModel.HoldDie1 ?? false)
                    _holdList.Add(0);
                if (ViewModel.HoldDie2 ?? false)
                    _holdList.Add(1);
                if (ViewModel.HoldDie3 ?? false)
                    _holdList.Add(2);
                if (ViewModel.HoldDie4 ?? false)
                    _holdList.Add(3);
                if (ViewModel.HoldDie5 ?? false)
                    _holdList.Add(4);
                if (ViewModel.HoldDie6 ?? false)
                    _holdList.Add(5);

                ViewModel.CurrentRoll = game.Reroll(_holdList.ToArray());
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "Error");
                await dialog.ShowAsync();
            }
        }

        private async void ScoreRoll_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                game.Score(ObjectiveList.SelectedIndex);
                var scoreCard = game.ScoreCard;
                ViewModel.UpdateScoreCard(scoreCard);

                if (scoreCard.Completed)
                {
                    var finalScore = scoreCard.TotalScore;
                    var greeting = finalScore < 200 ? "Oh my God that was terrible..." : "Congratulations you finished the game.";
                    var message = string.Format("{0}\r\nFinal score: {1}", greeting, finalScore);
                    var dialog = new MessageDialog(message, "Finished");
                    await dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "Error");
                await dialog.ShowAsync();
            }
        }
    }
}
