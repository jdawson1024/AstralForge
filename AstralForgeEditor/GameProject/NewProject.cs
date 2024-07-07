using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralForgeEditor.GameProject
{
    public class NewProject : ViewModelBase
    {
        private string _name = "NewProject";
        public string Name { get { return _name; } set { if (_name != value) { _name = value; OnPropertyChangd(nameof(Name)); } } }
        private string _path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\AstralForgeProjects\";
        public string Path { get { return _path; } set { if (_path != value) { _path = value; OnPropertyChangd(nameof(Path)); } } }

    }
}
