using AstralForgeEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;


namespace AstralForgeEditor.Models.ProjectModels
{
    public class NewProject : ViewModelBase
    {
        private readonly string _templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\AstralForgeEditor\ProjectTemplates");
        private string _name = "NewProject";
        public string Name { get { return _name; } set { if (_name != value) { _name = value; ValidateProjectPath(); OnPropertyChanged(nameof(Name)); } } }
        private string _projectPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AstralForgeProjects");
        public string ProjectPath { get { return _projectPath; } set { if (_projectPath != value) { _projectPath = value; ValidateProjectPath(); OnPropertyChanged(nameof(ProjectPath)); } } }
        private bool _isValid;
        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }
        private string _errorMsg;
        public string ErrorMsg
        {
            get => _errorMsg;
            set
            {
                if (_errorMsg != value)
                {
                    _errorMsg = value;
                    OnPropertyChanged(nameof(ErrorMsg));
                }
            }
        }
        private ObservableCollection<ProjectTemplate> _projectTemplates = new ObservableCollection<ProjectTemplate>();
        public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates { get; }

        private bool ValidateProjectPath()
        {
            var path = ProjectPath;
            if (!Path.EndsInDirectorySeparator(path)) path += @"\";
            path += $@"{Name}\";

            IsValid = false;
            if (string.IsNullOrEmpty(Name.Trim()))
            {
                ErrorMsg = "Type in a project name.";
            }
            else if (Name.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                ErrorMsg = "Invalid character(s) used in project name.";
            }
            else if (string.IsNullOrEmpty(ProjectPath.Trim()))
            {
                ErrorMsg = "Type in a project path.";
            }
            else if (ProjectPath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                ErrorMsg = "Invalid character(s) used in project path.";
            }
            else
            {
                try
                {
                    // Check if the path is a valid path format
                    if (!Path.IsPathRooted(ProjectPath))
                    {
                        ErrorMsg = "The specified path is not a valid absolute path.";
                        return false;
                    }

                    // Check if the directory exists
                    if (!Directory.Exists(ProjectPath))
                    {
                        ErrorMsg = "The specified path does not exist.";
                        return false;
                    }

                    // Check if the path is writable without creating files
                    var directoryInfo = new DirectoryInfo(ProjectPath);
                    if ((directoryInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        ErrorMsg = "The specified directory is read-only.";
                        return false;
                    }

                    // Check if a directory with the project name already exists
                    if (Directory.Exists(path))
                    {
                        ErrorMsg = "A project with this name already exists in the specified path.";
                        return false;
                    }

                    // Try to get directory contents to check for access
                    directoryInfo.GetDirectories();

                    ErrorMsg = string.Empty;
                    IsValid = true;
                }
                catch (UnauthorizedAccessException)
                {
                    ErrorMsg = "Access to the path is denied.";
                }
                catch (IOException)
                {
                    ErrorMsg = "An I/O error occurred while checking the path.";
                }
                catch (Exception)
                {
                    ErrorMsg = "Project path is not valid or not writable.";
                }
            }

            return IsValid;
        }

        public NewProject()
        {
            ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(_projectTemplates);
            try
            {
                var templates = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
                if (!templates.Any())
                {
                    CreateDefaultTemplates();
                    templates = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
                }

                foreach (var file in templates)
                {
                    var template = Serializer.FromFile<ProjectTemplate>(file);
                    _projectTemplates.Add(template);
                }
                ValidateProjectPath();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //TODO: Log errors 
            }
        }

        private void CreateDefaultTemplates()
        {
            try
            {
                Directory.CreateDirectory(_templatePath);

                var defaultTemplates = new List<ProjectTemplate>
                {
                    new ProjectTemplate
                    {
                        ProjectType = "Empty Project",
                        ProjectFile = "project.astralforge",
                        Folders = new List<string> { ".AstralForge", "Content", "GameCode" }
                    },
                    new ProjectTemplate
                    {
                        ProjectType = "First Person Project",
                        ProjectFile = "project.astralforge",
                        Folders = new List<string> { ".AstralForge", "Content", "GameCode", "FirstPerson" }
                    },
                    new ProjectTemplate
                    {
                        ProjectType = "Third Person Project",
                        ProjectFile = "project.astralforge",
                        Folders = new List<string> { ".AstralForge", "Content", "GameCode", "ThirdPerson" }
                    },
                    new ProjectTemplate
                    {
                        ProjectType = "Top Down Project",
                        ProjectFile = "project.astralforge",
                        Folders = new List<string> { ".AstralForge", "Content", "GameCode", "TopDown" }
                    }
                };

                foreach (var template in defaultTemplates)
                {
                    var templateDir = Path.Combine(_templatePath, template.ProjectType.Replace(" ", string.Empty));
                    Directory.CreateDirectory(templateDir);
                    var templateFile = Path.Combine(templateDir, "template.xml");
                    Serializer.ToFile(template, templateFile);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //TODO: Log errors 
            }
        }
    }
}
