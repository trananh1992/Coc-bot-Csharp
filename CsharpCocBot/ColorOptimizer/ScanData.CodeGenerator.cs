﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColorOptimizer
{
	public partial class ScanData
	{
		public void ExportFile(string path, string className, List<DataPerCategory> mainList)
		{
			if (File.Exists(path))
				if (MessageBox.Show("The script file already exists, do you want to overwrite it ?", path, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
					return;
				else
					File.Delete(path);
			List<string> script = new List<string>();
			script.Add(string.Format("// This file was generated by a tool. Any change may be lost. "));
			script.Add(string.Format("// Generated by ColorOptimizer {0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString()));
			script.Add(string.Format("// Original tool written by FastFrench"));
			script.Add("");

			#region Writes stats in comments
			foreach (DataPerCategory category in mainList)
			{
				script.Add(string.Format("// {0} : {1} colors (start {2} => all good {3} => not in bad {4})", category.Label, category.SelectedColorsSoFar.Count, category.ColorsLeftAfterFirstGood, category.ColorsLeftAfterLastGood, category.ColorsLeftAfterAllBad));
				if (!string.IsNullOrEmpty(category.KilledBy))
					script.Add(string.Format("//    => the last colors for {0} were killed by {1}", category.Label, category.KilledBy));
			}
			#endregion Writes stats in comments

			script.Add("");
			script.Add("namespace CoC.Bot.Data.ColorList {");
			script.Add("");
			script.Add("\tpublic class " + className + " : ColorListBase {");

			#region Enum
			script.Add("\t\tpublic enum Item {");
			foreach (DataPerCategory category in mainList)
				script.Add(string.Format("\t\t\t{0},", category.Label));
			script.Add("\t\t}");
			script.Add("");
			#endregion Enum

			#region GenericName
			script.Add("\t\tpublic override string GenericName");
			script.Add("\t\t{");
			script.Add("\t\t\tget { return \"" + className + "\"; }");
			script.Add("\t\t}");
			script.Add("");
			#endregion GenericName

			#region Extract data from each category
			script.Add("\t\tstatic private ColorListItem[] _items = {");
			foreach (DataPerCategory category in mainList)
			{
				script.Add(string.Format("\t\t// {0} has {1} colors (computed from {2} good files and {3} bad files) ", category.Label, category.SelectedColorsSoFar.Count(), category.GoodSnaps.Count, category.BadSnaps.Count));
					script.Add("\t\t\tnew ColorListItem(");
					script.Add("\t\t\t\t\"" + category.Label + "\",");
					script.Add("\t\t\t\tnew int[] {");
					int targetCount = 0;
					foreach (var pair in category.SelectedColorsSoFar.OrderByDescending(kp => kp.Value).Take(10))
					{
						script.Add(string.Format("\t\t\t\t\t0x{0}, // weight: {1} ", pair.Key.ToString("X8"), pair.Value));
						targetCount += pair.Value;
					}
					script.Add("\t\t\t\t},");
					script.Add(string.Format("\t\t\t\t{0} // Minimum number of pixels from that colorList that should be found on each building of this kind", targetCount));
				script.Add("\t\t\t),"); 
				script.Add("");
			}
			script.Add("\t\t};");
			script.Add("");			
			#endregion Extract data from each category
			script.Add("\tpublic override ColorListItem[] Items { get { return _items; }}");
			

			script.Add("\t}");
			script.Add("}");

			script.Add("");



			File.WriteAllLines(path, script);
		}
	}
}