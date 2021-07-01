using System;
using System.IO;

using Microsoft.Extensions.CommandLineUtils;
using net.sf.mpxj;
using net.sf.mpxj.reader;
using net.sf.mpxj.writer;
using net.sf.mpxj.mspdi;
using net.sf.mpxj.json;
using net.sf.mpxj.mpp;
using net.sf.mpxj.mpx;


namespace mpp2xml
{
    class Converter
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "mpp2xml";
            app.Description = "Microsoft Project MPP File Converter";
            app.ExtendedHelpText = "This is a project related conversion utility used to convert MS project files (MPP or MPX) to MSPDI.";

            app.HelpOption("-h|--help");

            var inputFile = app.Argument("inputFile", "The input file for convert");
            var outputFile = app.Argument("outputFile", "The the conversion output file");

            // actually, option type without value if specified by user, the value stored is string "on".
            var force = app.Option("-f|--force",
                                   "Overwrite output file if already exist",
                                   CommandOptionType.NoValue);

            var password = app.Option("-p|--password",
                                      "only needed if input file format is mpp and password protected",
                                      CommandOptionType.SingleValue);

            Converter conv = new Converter();
            app.OnExecute(() =>
                {
                    if (force.Value() != null)
                    {
                        conv.ForceOverwrite = true;
                    }

                    if (password.Value() != null)
                    {
                        conv.MPPPassword = password.Value();
                    }

                    if (inputFile.Value is null || outputFile.Value is null)
                    {
                        app.ShowHint();
                        app.ShowHelp();
                        return 1;
                    }

                    conv.Convert2MSPDI(inputFile.Value, outputFile.Value);

                    return 0;
                });

            app.Execute(args);
        }

        // if output file already exist, we should not overwrite as default.
        private bool ForceOverwrite = false;

        // I have tested It's not support password decryption for MS
        // project 2010, I don't know if it's supported for older
        // ones?
        private string MPPPassword;


        static bool IsXMLSupported
        {
            get
            {
                return ProjectReaderUtility.getSupportedFileExtensions().contains("XML");
            }
        }

        public static void DumpSupportedFileExtensions()
        {
            var exts = ProjectReaderUtility.getSupportedFileExtensions();

            foreach (var ext in exts.toArray())
            {
                Console.WriteLine(ext);
            }

        }

        /// <summary>
        ///   Get ProjectFile from input file
        /// </summary>
        ProjectFile ProjectFileFromFile(string inputFile)
        {
            if (!File.Exists(inputFile))
            {
                throw new ConversionException("Input file Should exist!");
            }

            if (inputFile.EndsWith(".mpp"))
            {
                return ProjectFileFromMPP(inputFile);
            }
            else if (inputFile.EndsWith(".mpx"))
            {
                return ProjectFileFromMPX(inputFile);
            }
            else
            {
                throw new ConversionException("Unknown input file type!");
            }
        }

        private ProjectFile ProjectFileFromMPP(string inputFile)
        {
            MPPReader reader = new MPPReader();

            if (MPPPassword != null)
            {
                reader.ReadPassword = MPPPassword;
            }

            try
            {
                using (FileStream s = File.Open(inputFile, FileMode.Open))
                {
                    DotNetInputStream ins = new DotNetInputStream(s);
                    return reader.Read(ins);
                }
            }
            catch (MPXJException e)
            {
                throw new ConversionException(e.ToString());
            }
        }

        private ProjectFile ProjectFileFromMPX(string inputFile)
        {
            MPXReader reader = new MPXReader();

            try
            {
                using (FileStream s = File.Open(inputFile, FileMode.Open))
                {
                    DotNetInputStream ins = new DotNetInputStream(s);
                    return reader.Read(ins);
                }
            }
            catch (MPXJException e)
            {
                throw new ConversionException(e.ToString());
            }
        }

        /// <summary>
        ///   Convert MPP to MSPDI file.
        /// </summary>
        private void Convert2MSPDI(string inputFile, string outputFile)
        {
            MSPDIWriter writer = new MSPDIWriter();
            Convert(inputFile, outputFile, writer);
        }

        /// <summary>
        ///   Convert MPP to JSON file.
        ///   OBSOLETE method, mpxj do not support json output.
        /// </summary>
        private void Convert2JSON(string inputFile, string outputFile)
        {
            JsonWriter writer = new JsonWriter();
            Convert(inputFile, outputFile, writer);
        }

        private void Convert(string inputFile, string outputFile, ProjectWriter writer)
        {
            ProjectFile pFile = ProjectFileFromFile(inputFile);

            if (File.Exists(outputFile) && ForceOverwrite is false)
            {
                throw new ConversionException("Output file already exist, but you have not specify overwrite it, use option \"-f\" to allow.");
            }

            try
            {
                using (FileStream s = File.Create(outputFile))
                {
                    DotNetOutputStream os = new DotNetOutputStream(s);
                    writer.write(pFile, os);
                }
            }
            catch (Exception e)
            {
                throw new ConversionException(e.ToString());
            }
        }
    }
}
