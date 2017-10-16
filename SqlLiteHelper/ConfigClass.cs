using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLiteHelper
{
    public class ConfigClass
    {
        private string id;
        private string name;
        private string formalValue;
        private string testValue;
        private string remark;

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string FormalValue
        {
            get
            {
                return formalValue;
            }

            set
            {
                formalValue = value;
            }
        }

        public string TestValue
        {
            get
            {
                return testValue;
            }

            set
            {
                testValue = value;
            }
        }

        public string Remark
        {
            get
            {
                return remark;
            }

            set
            {
                remark = value;
            }
        }
    }
}
