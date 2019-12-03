using System;
using System.IO;
using System.Linq;
using Installer.Instrumentation;
using Mono.Cecil;

namespace Installer
{
    internal class Program
    {
        public static void Main(string[] args)
        {

            if (args.Length < 1)
            {
                Console.WriteLine("Running in dry mode.");
                Console.WriteLine("You can link with Ananke core with: -ananke");
            }
            
            AnankeContext ananke = new AnankeContext();
            
            DetectSoL detect = new DetectSoL();
            FileInfo mainExecutable = detect.GetSoLMainExecutable();
            if(mainExecutable == null)
                throw new FileNotFoundException("Unable to find SoL main executable!");

            FileInfo targetExecutable = new FileInfo(mainExecutable.FullName.Replace(".exe", " Modded.exe"));
            
            AssemblyDefinition rootDefinition = AssemblyDefinition.ReadAssembly(mainExecutable.FullName);

            AbstractInstrumentation[] instrumentations = {
                new ChangeCoreModuleName(), 
                new ReplaceVersion(),
                new FixupHackySpecialCases(), 
                new AddAttachmentAnankeCore(),
                new UnlockAccess(),
            };

            if (args.Length>=1 && args[0] == "-ananke")
            {
                instrumentations = instrumentations.Concat(new AbstractInstrumentation[]
                {
                    new ActivateAttachmentAnankeCore(), 
                    new RegisterAnankeCoreOverrides()
                }).ToArray();
            }
            
            foreach (var instrumentation in instrumentations)
            {
                Console.WriteLine($"Applying instrumentation: {instrumentation.Name}...");
                instrumentation.Instrument(ananke, rootDefinition);
                Console.WriteLine("...Completed!");
            }
            rootDefinition.Write(targetExecutable.FullName);
            
        }
    }
}