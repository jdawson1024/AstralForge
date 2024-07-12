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
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms.Integration;
using System.Windows.Forms;

namespace AstralForgeEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("EngineWrapperDLL.dll")]
        private static extern IntPtr CreateEngine();

        [DllImport("EngineWrapperDLL.dll")]
        private static extern void DestroyEngine(IntPtr engine);

        [DllImport("EngineWrapperDLL.dll")]
        private static extern bool InitializeEngine(IntPtr engine, IntPtr windowHandle, int width, int height);

        [DllImport("EngineWrapperDLL.dll")]
        private static extern void RenderEngine(IntPtr engine);

        private IntPtr enginePtr;
        private System.Windows.Forms.Timer renderTimer;

        public MainWindow()
        {
            InitializeComponent();

            enginePtr = CreateEngine();

            Loaded += OnMainWindowLoaded; 
            Closing += OnMainWindowClosing;
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnMainWindowLoaded;
            OpenProjectBrowserDialog();
            
            var handle = GLPanel.Handle;
            if(!InitializeEngine(enginePtr, handle, (int)GLHost.ActualWidth, (int)GLHost.ActualHeight))
            {
                System.Windows.MessageBox.Show("Failed to initialize engine.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            renderTimer = new System.Windows.Forms.Timer();
            renderTimer.Interval = 16; // 60 FPS
            renderTimer.Tick += (s, a) => RenderEngine(enginePtr);
            renderTimer.Start();
        }

        private void OnMainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (renderTimer != null)
            {
                renderTimer.Stop();
                renderTimer.Dispose();
            }

            if(enginePtr != IntPtr.Zero)
            {
                DestroyEngine(enginePtr);
            }
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