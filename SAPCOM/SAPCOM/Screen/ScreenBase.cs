using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SAPCOM
{
    public class ScreenBase : Component
    {
        private string screenName;
        [Browsable(true), Category("Layout")]
        public string ScreenName
        {
            get
            {
                return this.screenName;
            }
            set
            {
                this.screenName = value;
            }
        }
        private int screenNumber;
        [Browsable(true), Category("Layout")]
        public int ScreenNumber
        {
            get { return screenNumber; }
            set { screenNumber = value; }
        }
        private string cursor;
        [Browsable(true), Category("Action"), DefaultValue("")]
        public string Cursor
        {
            get { return cursor; }
            set { cursor = value; }
        }
        private string subscr;

        public string Subscr
        {
            get { return subscr; }
            set { subscr = value; }
        }
        private string okCode;

        public string OkCode
        {
            get { return okCode; }
            set { okCode = value; }
        }
        private ScreenItemCollection ScreenCollection;
        private ArrayList subscrs = new ArrayList();
        [Browsable(true), Category("Action"), DefaultValue("")]
        public ArrayList Subscrs
        {
            get { return subscrs; }
            set { subscrs = value; }
        }
        [Browsable(false)]
        public ScreenItemCollection Fields
        {
            get
            {
                return this.ScreenCollection;
            }
        }
        public string this[string key]
        {
            get
            {
                object itemValue = this.Fields.FromKey(key).ItemValue;
                if (itemValue == null)
                {
                    return "";
                }
                return itemValue.ToString();
            }
            set
            {
                if (this.Fields.Contains(key))
                {
                    this.Fields.FromKey(key).ItemValue = value;
                    return;
                }
                ScreenItem screenItem = new ScreenItem(key, key);
                screenItem.ItemValue = value;
                this.Fields.Add(screenItem);
            }
        }
        public ScreenBase()
        {
            this.cursor = "";
            this.subscr = "";
            this.okCode = "";
            this.ScreenCollection = new ScreenItemCollection();
        }
        public ScreenBase(string screenName, int screenNumber)
        {
            this.screenName = screenName;
            this.screenNumber = screenNumber;
            this.cursor = "";
            this.subscr = "";
            this.okCode = "";
            this.ScreenCollection = new ScreenItemCollection();
        }
    }
}
