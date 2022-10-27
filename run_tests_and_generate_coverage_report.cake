var target = Argument("target", "Report");

#addin nuget:?package=Cake.Coverlet&version=2.5.4
#tool nuget:?package=ReportGenerator 

/*  Specify the relative paths to your tests projects here. */
var testProjectsRelativePaths = new string[]
{
    "GSG.Tests/GSG.Tests.csproj",
 //  "../tests/CodeCoverageCalculation.Domain.Tests/CodeCoverageCalculation.Domain.Tests.csproj"
};

/*  Change the output artifacts and their configuration here. */
var parentDirectory = Directory(".");
var coverageDirectory = parentDirectory + Directory("code_coverage");
var cuberturaFileName = "results";
var cuberturaFileExtension = ".cobertura.xml";
var reportTypes = "HtmlInline_AzurePipelines"; // Use "Html" value locally for performance and files' size.
var coverageFilePath = coverageDirectory + File(cuberturaFileName + cuberturaFileExtension);
var jsonFilePath = coverageDirectory + File(cuberturaFileName + ".json");

Task("Clean")
    .Does(() =>
{
    if (!DirectoryExists(coverageDirectory))
        CreateDirectory(coverageDirectory);
    else
        CleanDirectory(coverageDirectory);
});

Task("Test")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var testSettings = new DotNetCoreTestSettings
    {
        // 'trx' files will be used to publish the results of tests' execution in an Azure DevOps pipeline.
        ArgumentCustomization = args => args
            .Append($"--logger trx")
            .Append("/p:Threshold=50")
            .Append("/p:ThresholdType=line")
            .Append("/p:ThresholdStat=total")
    };

    var coverletSettings = new CoverletSettings
    {
        CollectCoverage = true,
        CoverletOutputDirectory = coverageDirectory,
        CoverletOutputName = cuberturaFileName
    };

    if (testProjectsRelativePaths.Length == 1)
    {
        coverletSettings.CoverletOutputFormat  = CoverletOutputFormat.cobertura;
        DotNetCoreTest(testProjectsRelativePaths[0], testSettings, coverletSettings);
    }
    else
    {
        DotNetCoreTest(testProjectsRelativePaths[0], testSettings, coverletSettings);

        coverletSettings.MergeWithFile = jsonFilePath;
        for (int i = 1; i < testProjectsRelativePaths.Length; i++)
        {
            if (i == testProjectsRelativePaths.Length - 1)
            {
                coverletSettings.CoverletOutputFormat  = CoverletOutputFormat.cobertura;
            }

            DotNetCoreTest(testProjectsRelativePaths[i], testSettings, coverletSettings);
        }
    }
});

Task("Report")
    .IsDependentOn("Test")
    .Does(() =>
{
    var toolpath = Context.Tools.Resolve("net6.0/ReportGenerator.dll");

     var reportSettings = new ProcessArgumentBuilder();
              reportSettings.Append($"-targetdir:." + coverageDirectory);
              reportSettings.Append($"-reports:" + coverageFilePath);
              reportSettings.Append($"-reportTypes:{reportTypes}");
        
           
    Information($"Tool Path : {toolpath.ToString()}");

    DotNetExecute(toolpath, reportSettings);
});


RunTarget(target);