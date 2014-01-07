using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReportFormater
{
    class Program
    {

        static void Main(string[] args)
        {
            string assignedRoot;
            string assignedOutput;
            //If no commandline arguments are provided, ask user for path to the Directory
            if (args.Length == 0)
            {
                System.Console.WriteLine("Please enter the Path to the Directory containing the Software Reports:");
                assignedRoot = System.Console.ReadLine();
            }
            else
            {
                assignedRoot = args[0];
            }
            //Exit with error if directory is invalid
            while (!System.IO.Directory.Exists(assignedRoot))
            {
                System.Console.WriteLine("Directory does not exist!");
                System.Console.WriteLine("Please enter the Path to the Directory containing the Software Reports:");
                assignedRoot = System.Console.ReadLine();
            }

            System.Console.WriteLine("Please enter the file with full path that should be used to store the master:");
            assignedOutput = System.Console.ReadLine();
            /*
            System.IO.File.Create(assignedOutput);
            while (!System.IO.File.Exists(assignedOutput))
            {
                System.Console.WriteLine("Invalid File Path!");
                System.Console.WriteLine("Please enter the file with full path that should be used to store the master:");
                assignedRoot = System.Console.ReadLine();
            }*/
            using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(assignedOutput))
            {
                //There should be one directory for each application, named after the application
                foreach (string directory in System.IO.Directory.EnumerateDirectories(assignedRoot))
                {
                    string applicationName = directory.Split('\\').Last();
                    //There should be a file for each month of collected data, if there was any data to collect
                    foreach(string file in System.IO.Directory.EnumerateFiles(directory, "*.csv")){
                        try { 
                            using (System.IO.StreamReader inputFile = new System.IO.StreamReader(file))
                            {
                                inputFile.ReadLine();
                                inputFile.ReadLine();
                                inputFile.ReadLine();
                                inputFile.ReadLine();
                                //The First 4 lines of each file are useless garbage included by system center
                                string line = inputFile.ReadLine();
                                while(line != null && !line.Equals("")){
                                    string[] parsedLine = line.Split(',');
                                    //Write the name of the application (directory name) as the fist column
                                    outputFile.Write(applicationName);
                                    //For some reason the column headers are instead inserted before the values, skip them
                                    for (int i = (parsedLine.Length / 2); i < parsedLine.Length; i++)
                                    {
                                        outputFile.Write(",");
                                        outputFile.Write(parsedLine[i]);
                                    }
                                    outputFile.WriteLine();
                                    line = inputFile.ReadLine();   
                                }
                            }
                        } //Error Happened
                        catch (Exception e){}
                    }
                }
            }
            

        }
    }
}
