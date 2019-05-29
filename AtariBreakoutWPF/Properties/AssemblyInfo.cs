using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Breakout Game")]
[assembly: AssemblyDescription("This is a simple clone of Breakout made as my first year coursework for software engineering degree at Kyiv Polytechnic Institute." +
                               " Original game was made by Steven Wozniak for Atari game consoles in 1976." +
                               "\n\rThe goal of the game is to destroy all the bricks an top of the game field with ball that bounces off obstacles" +
                               "by hitting it with paddle at the the bottom of the screen. Be careful, if the ball moves by your paddle out of the lower" +
                               "bound of game field, you lose it. Overall you have 1 ball at start of the game and 5 in reserve. Good luck and have fun!")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Kyiv Polytechnic Institute")]
[assembly: AssemblyProduct("Breakout")]
[assembly: AssemblyCopyright("Copyright © Alexey Litvinov 2019")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

//In order to begin building localizable applications, set
//<UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file
//inside a <PropertyGroup>.  For example, if you are using US english
//in your source files, set the <UICulture> to en-US.  Then uncomment
//the NeutralResourceLanguage attribute below.  Update the "en-US" in
//the line below to match the UICulture setting in the project file.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
    //(used if a resource is not found in the page,
    // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
    //(used if a resource is not found in the page,
    // app, or any theme specific resource dictionaries)
)]


// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]