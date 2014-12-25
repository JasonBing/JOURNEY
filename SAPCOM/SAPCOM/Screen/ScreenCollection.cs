using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SAPCOM
{
  	public class ScreenCollection : CollectionBase
	{
		//private ArrayList Screens = new ArrayList();
		public ScreenBase this[int index]
		{
			get
			{
				return (ScreenBase)base.List[index];
			}
		}
        //private string (string , int )
        //{
        //    return  + "-" + .ToString();
        //}
		public int Add(ScreenBase scr)
		{
			return base.List.Add(scr);
		}
		public ScreenBase NewScreen(string screenname, int screennumber)
		{
			return new ScreenBase(screenname, screennumber);
		}
	}
}
