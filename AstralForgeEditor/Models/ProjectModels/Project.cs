using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

namespace AstralForgeEditor.GameProject
{
    public class Project
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime CreationDate { get; set; }
        public string AstralForgeEditorVersion { get; set; }
        public string AstralForgeEngineVersion { get; set; }
        public string TemplateName { get; set; }
        public List<string> TemplateFolders { get; set; }

        public static Project Load(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Project file not found", path);
            }

            string jsonContent = File.ReadAllText(path);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var project = JsonSerializer.Deserialize<Project>(jsonContent, options);

            // Set the Path property to the directory containing the project file
            project.Path = System.IO.Path.GetDirectoryName(path);

            return project;
        }

        public void Save()
        {
            string projectFilePath = System.IO.Path.Combine(Path, $"{Name}.afproj");
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonContent = JsonSerializer.Serialize(this, options);
            File.WriteAllText(projectFilePath, jsonContent);
        }
    }
}