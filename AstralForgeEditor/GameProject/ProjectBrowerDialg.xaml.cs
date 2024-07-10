using AstralForgeEditor.Models.ProjectModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AstralForgeEditor.GameProject
{
    public partial class ProjectBrowserDialog : Window
    {
        private NewProject _newProject;

        public ProjectBrowserDialog()
        {
            InitializeComponent();
            _newProject = new NewProject();
            DataContext = _newProject;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
            this.ShowInTaskbar = false;

            // Select the first project by default if there are any
            if (_newProject.RecentProjects.Any())
            {
                ProjectList.SelectedIndex = 0;
            }
        }

        private void ProjectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProjectList.SelectedItem is RecentProject selectedProject)
            {
                ProjectDetailsTitle.Text = $"Project: {selectedProject.Name}";
                ProjectDetailsContent.Text = $"Path: {selectedProject.Path}\nLast Opened: {selectedProject.LastOpened:g}";
            }
        }

        private void TemplateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TemplateList.SelectedItem is ProjectTemplate selectedTemplate)
            {
                _newProject.SelectedTemplate = selectedTemplate;
            }
        }

        private void OpenSelectedProjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectList.SelectedItem is RecentProject selectedProject)
            {
                try
                {
                    string projectFilePath = Path.Combine(selectedProject.Path, $"{selectedProject.Name}.afproj");
                    Project project = Project.Load(projectFilePath);
                    _newProject.AddRecentProject(project.Name, project.Path);
                    OpenProject(project);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Error opening project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                    System.Windows.MessageBox.Show("Please select a project to open.", "No Project Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CreateProjectButton_Click(object sender, RoutedEventArgs e)
        {
            string projectName = _newProject.Name;
            string projectPath = Path.Combine(_newProject.ProjectPath, projectName);

            if (!_newProject.IsValid)
            {
                System.Windows.MessageBox.Show($"The specified path '{projectPath}' is not a valid project path.", "Invalid Path", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_newProject.SelectedTemplate == null)
            {
                System.Windows.MessageBox.Show("Please select a project template.", "No Template Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Directory.CreateDirectory(projectPath);

                foreach (var folder in _newProject.SelectedTemplate.Folders)
                {
                    Directory.CreateDirectory(Path.Combine(projectPath, folder));
                }

                _newProject.SaveProjectFile(projectPath);
                _newProject.AddRecentProject(projectName, projectPath);

                Project newProject = new Project
                {
                    Name = projectName,
                    Path = projectPath,
                    CreationDate = DateTime.Now,
                    AstralForgeEditorVersion = "0.0.1", // Update this with your actual version
                    AstralForgeEngineVersion = "0.0.1", // Update this with your actual version
                    TemplateName = _newProject.SelectedTemplate.ProjectType,
                    TemplateFolders = _newProject.SelectedTemplate.Folders
                };

                OpenProject(newProject);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error creating project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                _newProject.ProjectPath = dialog.SelectedPath;
            }
        }

        private void OpenProject(Project project)
        {
            // TODO: Implement logic to open the project in the main editor
            System.Windows.MessageBox.Show($"Opening project: {project.Name}\n" +
                            $"Path: {project.Path}\n" +
                            $"Created: {project.CreationDate}\n" +
                            $"Editor Version: {project.AstralForgeEditorVersion}\n" +
                            $"Engine Version: {project.AstralForgeEngineVersion}\n" +
                            $"Template: {project.TemplateName}",
                            "Project Opened", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}