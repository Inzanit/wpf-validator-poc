using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using AnnotationValidator.Annotations;

namespace AnnotationValidator
{
    public class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, ICollection<string>>
            _validationErrors = new Dictionary<string, ICollection<string>>();

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _validationErrors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool IsValid()
        {
            _validationErrors.Clear();

            ICollection<ValidationResult> validationResults = new List<ValidationResult>();

            var validationContext = new ValidationContext(this, null, null);

            if (Validator.TryValidateObject(this, validationContext, validationResults, true))
                return true;

            foreach (var validationResult in validationResults)
            {
                var property = validationResult.MemberNames.ElementAt(0);

                if (_validationErrors.ContainsKey(property))
                {
                    _validationErrors[property].Add(validationResult.ErrorMessage);
                }
                else
                {
                    _validationErrors.Add(property, new List<string> { validationResult.ErrorMessage });
                }

                OnPropertyChanged(property);
            }

            return false;
        }
    }
}