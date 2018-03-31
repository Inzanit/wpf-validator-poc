using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;

namespace AnnotationValidator
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _name;
        private int _age;

        public MainWindowViewModel()
        {
            ValidateCommand = new RelayCommand(Validate);
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A name is required for the person")]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "An age is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid age")]
        public int Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        public ICommand ValidateCommand { get; }

        private void Validate()
        {
            if (!IsValid())
                return;

            MessageBox.Show("Yay!");
        }
    }
}
