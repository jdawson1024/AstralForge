using AstralForgeEditor.GameProject;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AstralForgeEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnMainWindowLoaded; 
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnMainWindowLoaded;
            OpenProjectBrowserDialog();
        }

        private void OpenProjectBrowserDialog()
        {
            var projectBrowser = new ProjectBrowserDialog();
            projectBrowser.Owner = this;
            projectBrowser.ShowDialog();
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            // Open the ProjectBrowerDialg for creating a new project
            var projectBrowser = new GameProject.ProjectBrowserDialog();
            projectBrowser.Owner = this;
            var result = projectBrowser.ShowDialog();

            if (result == true)
            {
                // Project was created, you might want to load it or update the UI
                System.Windows.MessageBox.Show("New project created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            // Open the ProjectBrowerDialg for opening an existing project
            var projectBrowser = new GameProject.ProjectBrowserDialog();
            projectBrowser.Owner = this;
            var result = projectBrowser.ShowDialog();

            if (result == true)
            {
                // Project was opened, you might want to load it or update the UI
                System.Windows.MessageBox.Show("Project opened successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Implement save functionality
            System.Windows.MessageBox.Show("Save functionality not yet implemented.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            // Implement any cleanup or save prompts before closing
            if (System.Windows.MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        // You can add more methods here for other functionality, such as:

        private void InitializeViewport()
        {
            // Initialize your 3D viewport
        }

        private void UpdateProjectHierarchy()
        {
            // Update the project hierarchy tree view
        }

        private void UpdatePropertiesPanel()
        {
            // Update the properties panel based on selection
        }

        // Example of how you might handle the Play button click
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement play functionality
            System.Windows.MessageBox.Show("Play functionality not yet implemented.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}