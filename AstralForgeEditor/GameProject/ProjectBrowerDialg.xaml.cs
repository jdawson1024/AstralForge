using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AstralForgeEditor.GameProject
{
    /// <summary>
    /// Interaction logic for ProjectBrowerDialg.xaml
    /// </summary>
    public partial class ProjectBrowerDialg : Window
    {
        public ProjectBrowerDialg()
        {
            InitializeComponent();
        }

        private void CreateNewProjectButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectList.Visibility = Visibility.Collapsed;
            TemplateList.Visibility = Visibility.Visible;
            DetailsTitle.Text = "Template Details";
            DetailsContent.Text = "Select a template to see the details.";
        }

        private void OpenProjectButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectList.Visibility = Visibility.Visible;
            TemplateList.Visibility = Visibility.Collapsed;
            DetailsTitle.Text = "Project Details";
            DetailsContent.Text = "Select a project to see the details.";
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
                DetailsTitle.Text = "Template Details";
                DetailsContent.Text = $"Details of {((ListBoxItem)TemplateList.SelectedItem).Content}";
            }
        }
    }
}
