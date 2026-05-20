using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOLib
{
    public class MsgBoxFactory
    {
        private string bodyText = string.Empty;
        private string title = string.Empty;
        private int buttonWidth;
        private const int buttonHeight = 29;
        private const int buttonTop = 106;

        private string[] buttonCaptions;        
        private int[] buttonLefts;        

        private GEnum.MsgBoxButton btnReturn;
        private GEnum.MsgBoxDefaultButton btnDefault;

        private System.Data.SqlClient.SqlConnection cn = null;

        #region +++ Properties +++
        
        public string BodyText
        {
            get
            {
                return bodyText;
            }
        }

        public string Title
        {
            get;
            set;
        }

        public int ButtonWidth
        {
            get
            {
                return buttonWidth;
            }

        }

        public int ButtonHeight
        {
            get
            {
                return buttonHeight;
            }

        }

        public int ButtonTop
        {
            get
            {
                return buttonTop;
            }

        }

        public string[] ButtonCaptions
        {
            get
            {
                return buttonCaptions;
            }
        }

        public int[] ButtonLefts
        {
            get
            {
                return buttonLefts;
            }
        }
        public System.Data.SqlClient.SqlConnection  Connection
        {
            set
            {
                cn=value ;
            }
        }  
        public GEnum.MsgBoxButton[] buttonNos
        {
            get;
            set;
        }

        public int FormWidth
        {
            get;
            set;
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for this Factory.
        /// </summary>
        public MsgBoxFactory()
        {
           
        }

        #endregion // Constructors

        #region Expose Methods to UI

        public bool Initialisation( GEnum.InstanceMode? instanceMode, string msgID, GEnum.MsgBoxButton[] btns)
        {
            bool isInitialisation = true;
            string TitleText = string.Empty;

            if (msgID.Contains(" ") && (!msgID.Contains("%")))
            {
                this.bodyText = msgID;
            }
                else
            {
                if (cn != null)
                {
                    this.bodyText = SysMessageUtility.Get(cn, msgID,ref TitleText);
                }
                else
                {
                    this.bodyText = SysMessageUtility.Get(msgID, ref TitleText);
                }

            }
            this.buttonCaptions = new string[btns.Length];
            this.buttonNos = new GEnum.MsgBoxButton[btns.Length];
            this.buttonNos = btns;
            this.Title = TitleText;
            this.buttonLefts = new int[btns.Length];           
            return isInitialisation;
        }

        public bool PrepareData()
        {            
            for (int i = 0; i < buttonNos.Length; i++)
            {
                buttonCaptions[i] = GetBtnCaption(buttonNos[i]);                
                if(buttonCaptions[i].Length * 7 > buttonWidth) buttonWidth = buttonCaptions[i].Length * 7;
            }
            if (buttonNos[0] == GEnum.MsgBoxButton.Append_Job)
                buttonWidth = 70;
            else if (buttonWidth < 50) buttonWidth = 50;

            
            if ((buttonWidth * 2) + (20 * 2) > this.FormWidth) this.FormWidth = (buttonWidth * 2) + (20 * 2);
            //if(this.bodyText.Length*8 >this.FormWidth)
            //    this.FormWidth= this.bodyText.Length*8;

            switch(buttonNos.Length)
            {
                //case 1: this.buttonLefts[0] = this.FormWidth / 2 - this.buttonWidth / 2; break;
                //case 2: this.buttonLefts[0] = this.FormWidth / 2 - this.buttonWidth - 10; break;
                //case 3: this.buttonLefts[0] = this.FormWidth / 3 - this.buttonWidth - 10; break;
                //case 4: this.buttonLefts[0] = this.FormWidth / 4 - this.buttonWidth - 15; break;

                case 1:                
                        this.buttonLefts[0] = this.FormWidth / 2 - this.buttonWidth / 2; break;
                case 2: 
                        this.buttonLefts[0] = this.FormWidth / 2 - this.buttonWidth-10; break;
                case 3:
                        if (buttonWidth * 3 + 60 > this.FormWidth) this.FormWidth = buttonWidth * 3 + 60;
                        this.buttonLefts[0] = this.FormWidth / 3 - this.buttonWidth; break;
                case 4:
                        if (buttonWidth * 4 + 70 > this.FormWidth) this.FormWidth = buttonWidth * 4 + 70;
                        this.buttonLefts[0] = this.FormWidth / 4 - this.buttonWidth; break;
            }

            for (int i = 1; i < buttonNos.Length; i++)
            {
                buttonLefts[i] += buttonLefts[i - 1] + buttonWidth + 10;
            }
            
            return true;
        }

        #endregion

        #region private functions

        private string GetBtnCaption(GEnum.MsgBoxButton BtnKey)
        {
            if (BtnKey == GEnum.MsgBoxButton.Save_Send_Email)
                return "Save && Send Email";
            else if(BtnKey == GEnum.MsgBoxButton.Save_Only)
                return "Save Changes";
            else if(BtnKey==GEnum.MsgBoxButton.Append_Job)
                return "Append";
            else if (BtnKey == GEnum.MsgBoxButton.Replace_Job)
                return "Replace";

            SYSMsgList objSYSMsgList;
            if (cn != null)
            {
                objSYSMsgList = SYSMsgList.Get(cn,(int?)GEnum.SYSMsgList.MsgButton, (int?)BtnKey);
            }
            else
            {
                objSYSMsgList = SYSMsgList.Get((int?)GEnum.SYSMsgList.MsgButton, (int?)BtnKey);
            }
           
            if (!GFunc.IsNE(SysOptionUtility.LanguageUsed))           

                switch (SysOptionUtility.LanguageUsed.ToString())
                {
                    case "1": return objSYSMsgList.LangText1;
                    case "2": return objSYSMsgList.LangText2;
                    case "3": return objSYSMsgList.LangText3;
                    case "4": return objSYSMsgList.LangText4;
                    case "5": return objSYSMsgList.LangText5;
                    case "6": return objSYSMsgList.LangText6;
                    case "7": return objSYSMsgList.LangText7;
                    case "8": return objSYSMsgList.LangText8;
                    case "9": return objSYSMsgList.LangText9;
                    case "10": return objSYSMsgList.LangText10;
                }
            return objSYSMsgList.DataDes ;
        }      
        #endregion
    }
}
