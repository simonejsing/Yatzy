using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Yatzy;

namespace YatzyModernApp
{
    public class ViewModel : INotifyPropertyChanged
    {
        private ScoreCard _scoreCard;
        private readonly bool?[] _holdMask;

        private Roll _currentRoll;

        public IList<string> ObjectiveNames => _scoreCard.Objectives.Select(o => o.Identifier).ToList();

        private readonly ObservableCollection<string> _observableScores = new ObservableCollection<string>();
        public ObservableCollection<string> ObjectiveScores => _observableScores;

        private readonly ObservableCollection<string> _observablePotentialScores = new ObservableCollection<string>();
        public ObservableCollection<string> PotentialScores => _observablePotentialScores;

        public string TotalScore => string.Format("Score: {0}", _scoreCard.TotalScore);

        public Roll CurrentRoll
        {
            get
            {
                return _currentRoll;
            }
            set
            {
                _currentRoll = value;
                UpdatePotentialScores(_currentRoll);
                OnPropertyChanged();
            }
        }

        public bool? HoldDie1
        {
            get
            {
                return _holdMask[0];
            }
            set
            {
                _holdMask[0] = value;
                OnPropertyChanged();
            }
        }

        public bool? HoldDie2
        {
            get
            {
                return _holdMask[1];
            }
            set
            {
                _holdMask[1] = value;
                OnPropertyChanged();
            }
        }

        public bool? HoldDie3
        {
            get
            {
                return _holdMask[2];
            }
            set
            {
                _holdMask[2] = value;
                OnPropertyChanged();
            }
        }

        public bool? HoldDie4
        {
            get
            {
                return _holdMask[3];
            }
            set
            {
                _holdMask[3] = value;
                OnPropertyChanged();
            }
        }

        public bool? HoldDie5
        {
            get
            {
                return _holdMask[4];
            }
            set
            {
                _holdMask[4] = value;
                OnPropertyChanged();
            }
        }

        public bool? HoldDie6
        {
            get
            {
                return _holdMask[5];
            }
            set
            {
                _holdMask[5] = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ViewModel(ScoreCard scoreCard)
        {
            _holdMask = new bool?[6];
            _scoreCard = scoreCard;
            InitializeScoreCard(_scoreCard);
            ResetHoldMask();
            UpdateScoreCard(_scoreCard);
        }

        public void ResetHoldMask()
        {
            HoldDie1 = false;
            HoldDie2 = false;
            HoldDie3 = false;
            HoldDie4 = false;
            HoldDie5 = false;
            HoldDie6 = false;
        }

        private void InitializeScoreCard(ScoreCard scoreCard)
        {
            for (int i = 0; i < scoreCard.Scores.Length; i++)
            {
                _observableScores.Add("");
                _observablePotentialScores.Add("");
            }
        }

        public void UpdatePotentialScores(Roll currentRoll)
        {
            int i = 0;
            foreach (var objective in _scoreCard.Objectives)
            {
                if (_scoreCard.Scores[i] != -1)
                {
                    _observablePotentialScores[i] = "-";
                }
                else
                {
                    _observablePotentialScores[i] = FormatScore(objective.Score(currentRoll));
                }
                i++;
            }
        }

        public void UpdateScoreCard(ScoreCard scoreCard)
        {
            _scoreCard = scoreCard;
            var scores = scoreCard.Scores.Select(FormatScore).ToArray();
            for(int i = 0; i < scores.Length; i++)
            {
                _observableScores[i] = scores[i];
            }

            OnPropertyChanged("TotalScore");
        }

        private static string FormatScore(int s)
        {
            return s == -1 ? "" : s.ToString();
        }
    }
}
