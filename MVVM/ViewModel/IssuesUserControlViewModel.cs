using ProjectTracker.MVVM.Core;

namespace ProjectTracker.MVVM.ViewModel
{
    public class IssuesUserControlViewModel : ViewModelBase
    {
        public IssuesUserControlViewModel() { }

        private string _text = "Button...";
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        private RelayCommand _button;
        public RelayCommand Button
        {
            get
            {
                return _button ??
                    (_button = new RelayCommand(obj =>
                    {
                        Text = "Button is pressed!";
                    }));
            }
        }
    }
}
