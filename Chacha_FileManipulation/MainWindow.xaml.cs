
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.IO.IsolatedStorage;
using System.IO.Compression;

namespace Chacha_FileManipulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static string[] inputFileGlobal = {};

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a file";
            openFileDialog.Multiselect = true; // Allow multiple files to be selected
            openFileDialog.Filter = "All Files (*.*)|*.*"; // Set the file filter to show all files

            // Show the OpenFileDialog and get the result
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                // Get the selected file path
                inputFileGlobal = openFileDialog.FileNames;

                // Use the file path as needed, e.g. read its contents or display its name
                foreach (string inputFile in inputFileGlobal)
                    SelectedFile.Text += System.IO.Path.GetFileName(inputFile) + "\n";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] inputFiles = inputFileGlobal;
            foreach(string inputFile in inputFiles)
                if (File.Exists(inputFile))
                {
                    DateTime today = DateTime.Today;
                    string directoryPath = new TextRange(rtbOutput.Document.ContentStart, rtbOutput.Document.ContentEnd).Text;
                    directoryPath = directoryPath.Trim();
                    directoryPath += @"\" + today.ToString("D");
                    string fileName = System.IO.Path.GetFileName(inputFile);
                    SeparateAndWrite(inputFile, directoryPath);
                }
                else
                {
                    MessageBox.Show("File Does not Exist!!");
                }
            SelectedFile.Text = "";
            inputFileGlobal = null;
            
            MessageBox.Show("Process Completed...");
        }

        private static void SeparateAndWrite(string inputFile , string directoryPath)
        {
            using (StreamReader reader = new StreamReader(inputFile))
            {
                // Read and display each line of the file
                string line;
                string fileName = "";
                string temp = "";
                //string directoryPath = new TextRange(rtbOutput.Document.ContentStart, rtbOutput.Document.ContentEnd).Text;
                directoryPath = directoryPath.Trim();
                while ((line = reader.ReadLine()) != null)
                {

                    if (line.Contains("SHANTI MANAGEMENT [ARB]") && fileName.Length > 0)
                    {
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        fileName = fileName.Trim();
                        var filenames = fileName.Split();
                        string res = "";
                        for (int i = 2; i < filenames.Length; i++)
                        {
                            if (filenames[i] == "Interest")
                                break;
                            else
                            {
                                if (filenames[i] == " " && filenames[i + 1] == " ")
                                    continue;
                                res += filenames[i];
                                res += " ";
                            }
                        }
                        fileName = res.Trim();
                        fileName = fileName.Replace(" ", "_");
                        fileName = fileName.Replace("/", "&");
                        string filePath;
                        if (System.IO.Path.GetFileName(inputFile).StartsWith("INT"))
                        {
                        string check = fileName.Substring(0, 5);
                        check += "*";
                        string[] dirFiles = Directory.GetFiles(directoryPath , check);
                        foreach(string dirFile in dirFiles)
                        {
                            fileName = System.IO.Path.GetFileName(dirFile);
                            //fileName = fileName.Replace(".txt","");
                        }
                        filePath = System.IO.Path.Combine(directoryPath, fileName);
                        }
                        else
                        {
                            filePath = System.IO.Path.Combine(directoryPath, fileName + ".txt");
                        }
                        if (File.Exists(filePath))
                        {
                            // If the file exists, append the text to it
                            using (StreamWriter writer = File.AppendText(filePath))
                            {
                                temp = temp.Replace("~", "_");
                                temp = temp.Replace("\u000e", "");
                                writer.WriteLine(temp);
                            }
                        }
                        else
                        {
                            // If the file doesn't exist, create it and write the text to it
                            using (StreamWriter writer = File.CreateText(filePath))
                            {
                                temp = temp.Replace("~", "_");
                                temp = temp.Replace("\u000e", "");
                                writer.WriteLine(temp);
                            }
                        }
                        fileName = "";
                        temp = "";
                    }
                    if (line.StartsWith("Client"))
                    {
                        fileName = line;
                    }
                    temp += line + "\n";
                }

            }
        }

       
    }
}
