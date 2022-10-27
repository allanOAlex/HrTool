namespace  GSG.CodeGen;
public class ApplicationSettings
{
    public string ValidationDir { get; set; }
    public string OutputDir { get; set; }
    public string RootNamespace { get; set; }
    public string ModelNamespace { get; set; }
    public string ContextNamespace { get; set; }
    public string ContextName { get; set; }
    public string ContextDir { get; set; }
    public string ConnectionString { get; set; }
    public string ValidationNamespace { get; set; }
    public string[] ValidationUsings { get; set; }
}