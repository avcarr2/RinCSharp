using System;
using RDotNet;
using System.Linq;

namespace RinCSharp
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
		}
	}
	public class LearnR
	{
		// initializes engine, an R engine that can run R code
		public REngine engine;
		public LearnR() // Constructor that initializes the REngine
		{
			engine = REngine.GetInstance();
		}
		~LearnR() // Destructor that disposes of the R engine
		{
			engine.Dispose();
		}

        public void HelloRWorld() // R engine prints a string to console. 
        {
            CharacterVector charVec = engine.CreateCharacterVector(new[] { "Hello, R world!, .NET speaking" });
            engine.SetSymbol("greetings", charVec);
            engine.Evaluate("str(greetings)"); // print out in the console
            string[] a = engine.Evaluate("'Hi there .NET, from the R engine'").AsCharacter().ToArray();
            // Evaluate uses the R engine to evaluate the passed expression. Useful for running a line or two or R code
            // Anything beyond ~2 lines or needs a library beyond those loaded in base need to get passed to an R script. 
            Console.WriteLine("R answered: '{0}'", a[0]);
            Console.WriteLine("Press any key to exit the program");
            Console.ReadKey();
        }
        public string MakeFourWithR() // Adds two numbers using the R engine
        {
            IntegerVector FirstTwo = engine.CreateIntegerVector(new[] { 2 });
            IntegerVector SecondTwo = engine.CreateIntegerVector(new[] { 2 });
            engine.SetSymbol("FirstTwo", FirstTwo); // SetSymbol() is the equivalent to <- in R. Required to pass a variable to the R engine
            engine.SetSymbol("SecondTwo", SecondTwo);
            return engine.Evaluate("FirstTwo + SecondTwo").AsInteger().ToArray().Single().ToString();
        }
        public void AddTwoArraysWithR()
        {
            IntegerVector FirstVec = engine.CreateIntegerVector(new[] { 1, 2, 3, 4 });
            IntegerVector SecondVec = engine.CreateIntegerVector(new[] { 1, 2, 3, 4 });
            engine.SetSymbol("FirstVec", FirstVec);
            engine.SetSymbol("SecondVec", SecondVec);
            engine.Evaluate("print(FirstVec + SecondVec)");
        }

        public void AddTwoArraysOriginatingFromCSharp(int[] array1, int[] array2)
        {
            IntegerVector rVector1 = engine.CreateIntegerVector(array1);
            IntegerVector rVector2 = engine.CreateIntegerVector(array2);
            engine.SetSymbol("array1", rVector1);
            engine.SetSymbol("array2", rVector2);
            engine.Evaluate("print(array1 + array2)");
        }

        public int[] AddTwoArraysAndReturnToCSharp(int[] array1, int[] array2)
        {
            IntegerVector rVector1 = engine.CreateIntegerVector(array1);
            IntegerVector rVector2 = engine.CreateIntegerVector(array2);
            engine.SetSymbol("array1", rVector1);
            engine.SetSymbol("array2", rVector2);
            return engine.Evaluate("array1 + array2").AsInteger().ToArray();
        }

        public void AddTwoArraysWithRScript(int[] array1, int[] array2)
        {
            // passes C# variables to the command line and sources an R script to process those variables. 
            // output to console is a string, which isn't particularly useful if you need to pass large amounts of data 
            // better to use temp files to pass variables. 
            // NOTE: Sourced files aren't included in this repository. 
            string array1String = string.Join(" ", array1);
            string array2String = string.Join(" ", array2);
            string rcommand = @"source('C:/Users/Austin Carr/Documents/Visual Studio R Testing/ConsoleAppTesting/R/AddTwoArrays.R')";
            string[] argsR = new string[] { array1String, array2String };

            engine.SetCommandLineArguments(argsR);
            engine.Evaluate(rcommand);

        }

        public void PlotSomethingInR(int[] array1, int[] array2)
        {
            // plots a function using R code 
            string array1String = string.Join(" ", array1);
            string array2String = string.Join(" ", array2);
            string rcommand = @"source('C:/Users/Austin Carr/Documents/Visual Studio R Testing/ConsoleAppTesting/R/SimplePlotter.R')";
            string[] argsR = new string[] { array1String, array2String };

            engine.SetCommandLineArguments(argsR);
            engine.Evaluate(rcommand);
        }

    }
}
