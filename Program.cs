﻿namespace TeleprompterConsole;

internal class Program{
  static async Task Main(string[] args){
    await RunTeleprompter();
  }

  static IEnumerable<string> ReadFrom(string file){
    string? line;
    using(var reader = File.OpenText(file)){
      while((line = reader.ReadLine()) != null){
        var words = line.Split(' ');
        var lineLength = 0;

        foreach(var word in words){
          yield return word + " ";
          lineLength += word.Length + 1;
          if(lineLength > 70){
            yield return Environment.NewLine;
            lineLength = 0;
          }
        }
        yield return Environment.NewLine;
      }
    }
  }

  private static async Task ShowTeleprompter(TelePrompterConfig config){
    var words = ReadFrom("sampleQuotes.txt");

    foreach (var word in words){
      System.Console.Write(word);
      if(!string.IsNullOrWhiteSpace(word)){
        await Task.Delay(config.DelayInMilliseconds);
      }
    }
    config.SetDone();
  }

  private static async Task GetInput(TelePrompterConfig config){    
    Action work = () => {
      do{
        var key = Console.ReadKey(true);
        if (key.KeyChar == '>'){
          config.UpdateDelay(-10);
        } else if (key.KeyChar == '<'){
          config.UpdateDelay(10);
        } else if (key.KeyChar == 'X' || key.KeyChar == 'x'){
          config.SetDone();
        }
      }while(!config.Done);
    };

    await Task.Run(work);
  }

  private static async Task RunTeleprompter(){
    var config = new TelePrompterConfig();
    var displayTask = ShowTeleprompter(config);

    var speedTask = GetInput(config);
    await Task.WhenAny(displayTask, speedTask);
  }
}