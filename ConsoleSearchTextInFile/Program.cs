static List<string> SearchFilesByExtension(string path, string extension)
{
    var dir = new DirectoryInfo(path);
    var dirs = dir.GetDirectories();
    var files = dir.GetFiles();

    var foundFiles = (from file in files where file.Extension.Equals(extension) select file.FullName).ToList();

    foreach (var dirItem in dirs)
    {
        foundFiles.AddRange(SearchFilesByExtension(dirItem.FullName, extension));
    }

    return foundFiles;
}

static List<string> GetFileContent(string path)
{
    var fileContent = new List<string>();

    using var reader = new StreamReader(path);
    
    while (!reader.EndOfStream)
    {
        fileContent.Add(reader.ReadLine() ?? string.Empty);
    }

    return fileContent;
}

static List<string> FilterFileContent(string word, List<string> fileContent)
{
    return fileContent.Where(textLine => textLine.Contains(word, StringComparison.CurrentCultureIgnoreCase)).ToList();
}

static void SearchFilesByFileExtensionAndText(string path, string extension, string text)
{
    var files = SearchFilesByExtension(path, extension);

    var filesWithSearchText = (from file in files let fileContent = GetFileContent(file) let filteredFileLines = FilterFileContent(text, fileContent) where filteredFileLines.Count > 0 select file).ToList();

    Console.WriteLine(string.Join("\n", filesWithSearchText));
}

var searchDirectory = Directory
    .GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory())?.ToString() ?? string.Empty)
        ?.ToString() ?? string.Empty)?.ToString();

if (searchDirectory != null) SearchFilesByFileExtensionAndText(searchDirectory, ".cs", "Console.WriteLine");