using LoaderLibrary.CommandLineParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefFilePreparing
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Type> verbs = new List<Type>() 
            {
                typeof(Verbs.Organize)
            };

            Verb v = null;

            try
            {
                PrintHeader();
                v = (Verb)ArgumentParser.Parse(args, verbs);
            }
            catch (ArgumentParserException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Console.WriteLine();

                ArgumentParser.PrintUsage(verbs, Console.Out);
            }

            if (v != null)
            {
                v.Run();
            }
        }


        private static void PrintHeader()
        {
            Console.WriteLine(
            @"Reference genome/chromosome files organizer
            (c) 2014 János Szalai-Gindl szalai@complex.elte.hu, László Dobos dobos@complex.elte.hu
            Department of Physics of Complex Systems, Eötvös Loránd University

            ");
        }
    }
}
