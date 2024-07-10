using AstralForgeEditor.Models.ProjectModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using MessageBox = System.Windows.MessageBox;

namespace AstralForgeEditor.GameProject
{
    public partial class ProjectBrowerDialg : Window
    {
        private NewProject _newProject;

        public ProjectBrowerDialg()
        {
            InitializeComponent();
            _newProject = new NewProject();
            DataContext = _newProject;

            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.ResizeMode = ResizeMode.NoResize;
            this.ShowInTaskbar = false;
        }

        private void CreateNewProjectButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectList.Visibility = Visibility.Collapsed;
            TemplateList.Visibility = Visibility.Visible;
            DetailsTitle.Text = "Template Details";
            DetailsContent.Text = "Select a template to see the details.";
            NewProjectDetails.Visibility = Visibility.Visible;
            OpenSelectedProjectButton.Visibility = Visibility.Collapsed;
        }

        private void OpenExistingProjectButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "AstralForge Project Files (*.afproj)|*.afproj",
                Title = "Open AstralForge Project"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string projectPath = openFileDialog.FileName;
                // Implement logic to open the selected project file
                _newProject.AddRecentProject(System.IO.Path.GetFileNameWithoutExtension(projectPath), projectPath);
                MessageBox.Show($"Opening project: {projectPath}", "Open Project", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
        }

        private void OpenSelectedProjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectList.SelectedItem is RecentProject selectedProject)
            {
                _newProject.AddRecentProject(selectedProject.Name, selectedProject.Path);
                MessageBox.Show($"Opening project: {selectedProject.Name}", "Open Project", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please select a project to open.", "No Project Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ProjectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProjectList.SelectedItem is RecentProject selectedProject)
            {
                DetailsTitle.Text = "Project Details";
                DetailsContent.Text = $"Name: {selectedProject.Name}\nPath: {selectedProject.Path}\nLast Opened: {selectedProject.LastOpened:g}";
            }
        }

        private void TemplateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TemplateList.SelectedItem is ProjectTemplate selectedTemplate)
            {
                _newProject.SelectedTemplate = selectedTemplate;
                DetailsTitle.Text = "Template Details";
                DetailsContent.Text = $"Details of {selectedTemplate.ProjectType}\nFolders: {string.Join(", ", selectedTemplate.Folders)}";
            }
        }

        private void CreateProjectButton_Click(object sender, RoutedEventArgs e)
        {
            string projectName = _newProject.Name;
            string projectPath = System.IO.Path.Combine(_newProject.ProjectPath, projectName);

            if (!_newProject.IsValid)
            {
                MessageBox.Show($"The specified path '{projectPath}' is not a valid project path.", "Invalid Path", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_newProject.SelectedTemplate == null)
            {
                MessageBox.Show("Please select a project template.", "No Template Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                System.IO.Directory.CreateDirectory(projectPath);

                foreach (var folder in _newProject.SelectedTemplate.Folders)
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(projectPath, folder));
                }

                _newProject.SaveProjectFile(projectPath);
                _newProject.AddRecentProject(projectName, projectPath);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void BackToRecentProjectsButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectList.Visibility = Visibility.Visible;
            TemplateList.Visibility = Visibility.Collapsed;
            DetailsTitle.Text = "Project Details";
            DetailsContent.Text = "Select a project to see the details.";
            NewProjectDetails.Visibility = Visibility.Collapsed;
            OpenSelectedProjectButton.Visibility = Visibility.Visible;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}