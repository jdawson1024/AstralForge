using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralForgeEditor.GameProject
{
    public class ViewModelBase : INotifyPropertyChanged 
    {
        public event PropertyChangedEventHandler ?PropertyChanged;

        protected void OnPropertyChangd(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
