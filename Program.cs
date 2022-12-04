namespace TeleprompterConsole;

internal class Program{
  static void Main(string[] args){
    var lines = ReadFrom("sampleQuotes.txt");
    foreach (var line in lines){
      System.Console.WriteLine(line);
    }
  }

  static IEnumerable<string> ReadFrom(string file){
    string? line;
    using(var reader = File.OpenText(file)){
      while((line = reader.ReadLine()) != null){
        yield return line;
      }
    }
  }
}