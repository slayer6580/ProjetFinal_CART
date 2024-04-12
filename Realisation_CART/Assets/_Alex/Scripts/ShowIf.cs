using System;
using UnityEngine;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
     AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]

    // Tool who create an attributes, other script in 'editor' folder
    public class ShowIfAttribute : PropertyAttribute
    {
        // name of the bool
        public string m_BoolName = "";
        public string m_SecondBoolName = "";
        public string m_ThirdBoolName = "";
        public bool m_HideInInspector = false;

        public bool m_NeedOneCHeck = false;
        public bool m_NeedTwoCheck = false;
        public bool m_NeedThreeCheck = false;

        public bool m_NeedToBe = false;
        public bool m_SecondNeedToBe = false;
        public bool m_ThirdNeedToBe = false;

        public ShowIfAttribute(string boolName, bool needTobe)
        {
            // true = hideInInspector / false = disabled
            this.m_HideInInspector = true;

            this.m_NeedOneCHeck = true;
            this.m_NeedTwoCheck = false;
            this.m_NeedThreeCheck = false;

            this.m_BoolName = boolName;
            this.m_NeedToBe = needTobe;
        }

        public ShowIfAttribute(string boolName, bool needTobe, string secondBoolName, bool secondNeedToBe)
        {
            this.m_HideInInspector = true;

            this.m_NeedOneCHeck = false;
            this.m_NeedTwoCheck = true;
            this.m_NeedThreeCheck = false;

            this.m_BoolName = boolName;
            this.m_NeedToBe = needTobe;

            this.m_SecondBoolName = secondBoolName;
            this.m_SecondNeedToBe = secondNeedToBe;
        }

        public ShowIfAttribute(string boolName, bool needTobe, string secondBoolName, bool secondNeedToBe, string thirdBoolName, bool thirdNeedToBe)
        {
            this.m_HideInInspector = true;

            this.m_NeedOneCHeck = false;
            this.m_NeedTwoCheck = false;
            this.m_NeedThreeCheck = true;

            this.m_BoolName = boolName;
            this.m_NeedToBe = needTobe;

            this.m_SecondBoolName = secondBoolName;
            this.m_SecondNeedToBe = secondNeedToBe;

            this.m_ThirdBoolName = thirdBoolName;
            this.m_ThirdNeedToBe = thirdNeedToBe;
        }





    }

