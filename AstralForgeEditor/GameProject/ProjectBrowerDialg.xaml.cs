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
using System.Windows.Shapes;
using Microsoft.Win32; // Correct namespace for OpenFileDialog
using MessageBox = System.Windows.MessageBox; // Disambiguate between Windows.Forms and WPF

namespace AstralForgeEditor.GameProject
{
    /// <summary>
    /// Interaction logic for ProjectBrowerDialg.xaml
    /// </summary>
    public partial class ProjectBrowerDialg : Window
    {
        private NewProject _newProject;

        public ProjectBrowerDialg()
        {
            InitializeComponent();
            _newProject = new NewProject();
            DataContext = _newProject;
        }

        private void CreateNewProjectButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectList.Visibility = Visibility.Collapsed;
            TemplateList.Visibility = Visibility.Visible;
            DetailsTitle.Text = "Template Details";
            DetailsContent.Text = "Select a template to see the details.";
            NewProjectDetails.Visibility = Visibility.Visible; // Show the new project details section
        }

        private void OpenProjectButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectList.Visibility = Visibility.Visible;
            TemplateList.Visibility = Visibility.Collapsed;
            DetailsTitle.Text = "Project Details";
            DetailsContent.Text = "Select a project to see the details.";
            NewProjectDetails.Visibility = Visibility.Collapsed; // Hide the new project details section
        }

        private void ProjectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProjectList.SelectedItem != null)
            {
                DetailsTitle.Text = "Project Details";
                DetailsContent.Text = $"Details of {((ListBoxItem)ProjectList.SelectedItem).Content}";
            }
        }

        private void TemplateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TemplateList.SelectedItem != null)
            {
                var selectedTemplate = (ProjectTemplate)TemplateList.SelectedItem;
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
                // Create the project directory
                Directory.CreateDirectory(projectPath);

                // Create template folders
                foreach (var folder in _newProject.SelectedTemplate.Folders)
                {
                    Directory.CreateDirectory(System.IO.Path.Combine(projectPath, folder));
                }

                // Save the project file
                _newProject.SaveProjectFile(projectPath);

                MessageBox.Show($"Project '{projectName}' created at '{projectPath}'", "Project Created", MessageBoxButton.OK, MessageBoxImage.Information);

                // Optionally, you can close the dialog or switch to the new project here
                this.Close();
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
