using System;
using System.IO;
using Installer.Instrumentation;
using Mono.Cecil;

namespace Installer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
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
                new ActivateAttachmentAnankeCore(), 
                new RegisterAnankeCoreOverrides(),
                new UnlockAccess(),
            };
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