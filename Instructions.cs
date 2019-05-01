using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Instructions
    {
        public string Name;
        public char type;
        public string op, funct;
        public  Instructions(string name , char type , string op , string funct)
        {
            this.Name = name;
            this.type = type;
            this.op = op;
            this.funct = funct;
        }
        public void Update(string name, char type, string op, string funct)
        {
            this.Name = name;
            this.type = type;
            this.op = op;
            this.funct = funct;
        }
    }
}
