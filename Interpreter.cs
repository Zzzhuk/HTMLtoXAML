using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTMLtoXAML
{
    class Interpreter
    {
		public List<string> elements, parsElements;
		Dictionary<string, string> tags = new Dictionary<string, string>
			{
				{"button", "Button"},
				{"text", "TextBox"},
				{"number", "TextBox"},
				{"password", "PasswordBox"},
				{"textarea", "TextBox"},
				{"ul", "ListBox"},
				{"span", "Label"},
				{"label", "Label"},
				{"p", "TextBlock"},
				{"div", "StackPanel"},
				{"li", "StackPanel"},
			};
		Dictionary<string, string> styles = new Dictionary<string, string>
			{
				{"width", "Width"},
				{"height", "Height"},
				{"border-color", "BorderBrush"},
				{"background-color", "Background"},
				{"color", "Foreground"},
				{"font-size", "FontSize"},
				{"font-weight", "FontWeight"},
				{"margin", "Margin"},
				{"padding", "Padding"}
			};
		public Interpreter()
		{

		}
		public void ReadAndWrite(string path)
		{
			bool canRead = false;
			string line;
			elements = new List<string>();
			parsElements = new List<string>();
			using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
			{
				while ((line = sr.ReadLine()) != null)
				{
					//elements.Add(@"<Window xmlns = ""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns: x = ""http://schemas.microsoft.com/winfx/2006/xaml"" xmlns: d = ""http://schemas.microsoft.com/expression/blend/2008"" xmlns: mc = ""http://schemas.openxmlformats.org/markup-compatibility/2006"" Title = "" " + line.Replace("<title>", "").Replace("</title>", "") + @" "" Height = ""700"" Width = ""500"">");
					if(line == "<body>")
					{
						canRead = true;
						continue;
					} else if(line == "</body>")
					{
						break;
					}
					if (canRead || line.Contains("<title>")) {
						elements.Add(line.Replace("\t",""));
					}
				}
			}
			Parser();
			//elements.Add("</Window>");
			string resDir = Environment.CurrentDirectory + "/res";
			Directory.CreateDirectory(resDir);

			using (StreamWriter sw = new StreamWriter(resDir + "/" + Path.GetFileNameWithoutExtension(path) + ".xaml", false))
			{
				sw.Flush();
				foreach (var el in parsElements)	
				{
					sw.WriteLine(el);
				}
				sw.Close();
			}

		}
		//string Parser(string line)
		void Parser()
		{
			

			Regex patternBegin = new Regex(@"<\w*>"),
					patternEnd = new Regex(@"</\w*>"),
					patternSingle = new Regex(@"<\w*/>"),
					patternFull = new Regex(@"<\w*>\w*</\w*>"),
				  patternStyle = new Regex(@"style=""(\w*:\w*;)*""");
			foreach (var el in elements)
			{
				string resLine = "", styleAttr = "";
				if (el.Contains(@"style="""))
				{
					string stl = @"style=""", tempStl = "";
					var i = el.IndexOf(stl);
					int v = i + stl.Length;
					while (el[v] != '"')
					{
						tempStl += el[v];
						v++;
					}
					styleAttr = StyleParser(tempStl);
				}
				if (patternFull.IsMatch(@el))
				{
					if (el.Contains("<title>"))
					{
						parsElements.Add(@"<Window xmlns = ""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns: x = ""http://schemas.microsoft.com/winfx/2006/xaml"" xmlns: d = ""http://schemas.microsoft.com/expression/blend/2008"" xmlns: mc = ""http://schemas.openxmlformats.org/markup-compatibility/2006"" Title = """ + el.Replace("<title>", "").Replace("</title>", "") + @""" Height = ""700"" Width = ""500"">");
						parsElements.Add(@"<StackPanel>");
						continue;
					}
					
					try
					{
						string tag = patternBegin.Match(@el).Value.Replace("<", "").Replace(">", "").Split(' ')[0];
						resLine = @"<" + tags[tag];
						switch (tag)
						{
							case "button":
								resLine += @" Content=""" + el.Replace(patternBegin.Match(el).Value, "").Replace(patternEnd.Match(el).Value, "") + @"""";
								break;
							case "p":
								resLine += @" TextWrapping=""Wrap"" Text=""" + el.Replace(patternBegin.Match(el).Value, "").Replace(patternEnd.Match(el).Value, "") + @"""";
								break;
							case "label":
								resLine += @" Content=""" + el.Replace(patternBegin.Match(el).Value, "").Replace(patternEnd.Match(el).Value, "") + @"""";
								break;
							case "span":
								resLine += @" Content=""" + el.Replace(patternBegin.Match(el).Value, "").Replace(patternEnd.Match(el).Value, "") + @"""";
								break;
						}
						resLine += styleAttr + "/>";
					} catch { }
					
				} else if (!patternEnd.IsMatch(@el))
				{
					if(new Regex(@"<img\w*>").IsMatch(@el))
					{
						
					} else if(el.Contains("<input"))
					{
						string type = new Regex(@"type=""\w*""").Match(@el).Value.Replace(@"type=""", "").Replace(@"""", "");
						resLine = @"<" + tags[type];
						if(new Regex(@"value=""").IsMatch(el))
						{
							try
							{
								string val = new Regex(@"value=""\w*""").Match(el).Value.Replace(@"value=""", "").Replace(@"""", "");
								resLine += @" Content=""" + val + @"""";
							}
							catch { }
						}
						resLine += styleAttr + "/>";
					} else if(patternBegin.IsMatch(@el))
					{
						try
						{
							resLine += @"<" + tags[el.Replace("<", "").Replace(">", "").Split(' ')[0]] + styleAttr + ">";
						}
						catch { }
					}
				} else if (patternEnd.IsMatch(@el))
				{
					try
					{
						resLine += @"</" + tags[el.Replace("</", "").Replace(">", "")] + ">";
					}
					catch { }
				} else
				{
					resLine += @"<Label Content=""" + el + @""" />";
				}
				if(resLine != "")
				parsElements.Add(resLine);
			}
			parsElements.Add("</StackPanel>");
			parsElements.Add("</Window>");
		
		}
		string StyleParser(string styleLine)
		{
			Dictionary<string, string> tempArg = new Dictionary<string, string>();
			string tempStr = "", result = "";
			foreach (char c in styleLine)
			{
				if (c == ' ')
					continue;
				else if (c == ';')
				{
					string[] tempStrArr = tempStr.Split(':');
					tempArg.Add(tempStrArr[0], tempStrArr[1]);
					tempStr = "";
				} else
				{
					tempStr += c;
				}
			}
			foreach (var el in tempArg)
			{
				if (el.Key == "width" || el.Key == "height")
					result += " " + styles[el.Key] + @"=""" + el.Value.Replace("px", "") + @"""";
				else if (el.Key == "margin" || el.Key == "padding")
				{
					string[] value_px = el.Value.Replace("px", ";").Split(';');
					Array.Resize(ref value_px, value_px.Length - 1);
					result += " " + styles[el.Key] + @"=""" + String.Join(",", value_px) + @"""";
				} else
				{
					result += " " + styles[el.Key] + @"=""" + el.Value.Replace("px","") + @"""";
				}
			}
			return result;
		}
    }
}
