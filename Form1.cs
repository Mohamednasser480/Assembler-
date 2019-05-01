using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        //Dictionary< KeyValuePair< string , string  > >=new Dictionary< KeyValuePair <string , string  >>();
        Dictionary<string,string> MapToBinary = new Dictionary<string , string>();
        Dictionary<string, int> Variable = new Dictionary<string, int>();
        Dictionary<string, Instructions> instructions = new Dictionary<string, Instructions>();
        
        Dictionary<string, int> Label_map = new Dictionary<string, int>();
        string DontCare = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
        public Form1()
        {
            InitializeComponent();
        }
        public void Load()
        {

            Instructions pair_intruction1 = new Instructions("add", 'R', "000000", "100000");
           instructions["add"]= pair_intruction1;

            Instructions pair_intruction2 = new Instructions("and", 'R', "000000", "100100");
           instructions["and"] = pair_intruction2;

            Instructions pair_intruction3 = new Instructions("sub", 'R', "000000", "100010");
           instructions["sub"] = pair_intruction3;

            Instructions pair_intruction4 = new Instructions("nor", 'R', "000000", "100111");
           instructions["nor"] = pair_intruction4;

            Instructions pair_intruction5 = new Instructions("or", 'R', "000000", "100101");
           instructions["or"] = pair_intruction5;

            Instructions pair_intruction6 = new Instructions("slt", 'R', "000000", "101010");
           instructions["slt"] = pair_intruction6;


            Instructions pair_intruction7 = new Instructions("addi", 'I', "001000", "00000000");
           instructions["addi"] = pair_intruction7;


            Instructions pair_intruction8 = new Instructions("lw", 'I', "100011", "00000000");
           instructions["lw"] = pair_intruction8;

            Instructions pair_intruction9 = new Instructions("sw", 'I', "101011", "00000000");
           instructions["sw"] = pair_intruction9;


            Instructions pair_intruction10 = new Instructions("beq", 'I', "000100", "00000000");
           instructions["beq"] = pair_intruction10;

            Instructions pair_intruction11 = new Instructions("bne", 'I', "000101", "00000000");
           instructions["bne"] = pair_intruction11;

            Instructions pair_intruction12 = new Instructions("j", 'J', "000010", "00000000");
           instructions["j"] = pair_intruction12;

            MapToBinary["$zero"] = "00000";
            MapToBinary["$at"] =    "00001";
            MapToBinary["$v0"] =   "00010";
            MapToBinary["$v1"] =   "00011";
            MapToBinary["$a0"] =   "00100";
            MapToBinary["$a1"] =   "00101";
            MapToBinary["$a2"] =   "00110";
            MapToBinary["$a3"] =   "00111";
            MapToBinary["$t0"] =   "01000";
            MapToBinary["$t1"] =   "01001";
            MapToBinary["$t2"] =   "01010";
            MapToBinary["$t3"] =   "01011";
            MapToBinary["$t4"] =   "01100";
            MapToBinary["$t5"] =   "01101";
            MapToBinary["$t6"] =   "01110";
            MapToBinary["$t7"] =   "01111";
            MapToBinary["$s0"] =   "10000";
            MapToBinary["$s1"] =   "10001";
            MapToBinary["$s2"] =   "10010";
            MapToBinary["$s3"] =   "10011";
            MapToBinary["$s4"] =   "10100";
            MapToBinary["$s5"] =   "10101";
            MapToBinary["$s6"] =   "10110";
            MapToBinary["$s7"] =   "10111";
            MapToBinary["$t8"] =   "11000";
            MapToBinary["$t9"] =   "11001";
            MapToBinary["$k0"] =   "11010";
            MapToBinary["$k1"] =   "11011";
            MapToBinary["$gp"] =   "11100";
            MapToBinary["$sp"] =   "11101";
            MapToBinary["$fp"] =   "11110";
            MapToBinary["$ra"] =   "11111";
        }
        public string HandelComments(string Line)
        {
            string temp = "";
            for (int i = 0; i < Line.Length; i++)
            {
                if (Line[i] == '#')
                {
                    for (int j = i; j < Line.Length; j++)
                    {
                        temp += "";
                    }
                    break;
                }
                else
                    temp += Line[i];
            }

            return temp;
        } // Remove All Comments from input string and return clean string 
        public List<string>  RemoveSpaces(List<string> Line )
        {
            for (int k = 0; k < Line.Count; k++)
                if (Line[k] == "")
                {
                    Line.Remove(Line[k]);
                    k--;
                }
            return Line;
        } // Remove All spaces from input string and return clean string 
        public string ConvertToBinary(int num, char c) {
            int NegtiveValue = 0;
            string ans="";
            if (num < 0)
            {
                NegtiveValue = num ;
                num *= -1;
            }
            while (num>0){
               int res = num % 2;
               ans += res.ToString();
               num/=2;
            }
            
            while (ans.Length < 16 && c == 'i')
                ans += "0";
            while (ans.Length < 32 && c == 'd')
                ans += "0";
            if (NegtiveValue < 0)
            {
                bool FindOne = false;
                string TwosComplement = "";
                for (int i = 0; i < ans.Length; i++)
                {
                    if (ans[i] == '1' && FindOne == false)
                    {
                        FindOne = true;
                        TwosComplement += '1';
                    }
                    else if (ans[i] == '1' && FindOne == true)
                        TwosComplement += '0';
                    else if (ans[i] == '0' && FindOne == true)
                        TwosComplement += '1';
                }
                ans = TwosComplement;
            }
            string ans1="";
            for (int i = ans.Length - 1; i >= 0; i--)
                ans1+= ans[i];
            
             return ans1;
        } // Convert decimal number to binary code and if number is negtive will return Twos Complment

        public string ConvertR(List<string> Line)
        {
            string machine =instructions[Line[0]].op;   // op Code of instruction
            machine += MapToBinary[Line[2]];     
            machine += MapToBinary[Line[3]];
            machine += MapToBinary[Line[1]];
            machine += "00000";
            machine +=instructions[Line[0]].funct;
            return machine;
        } // Convert instruction by R type 
        public string ConvertJ(List<string> Line)
        {
            string machine = instructions[Line[0]].op;
            int s = Label_map[Line[1]]/4;
            string s1 = ConvertToBinary(s,'j');
            string s2 = "";
            for (int i = s1.Length; i < 26; i++)
                s2 += '0';
                machine += s2+s1;
            return machine;
        } // Convert instruction by J type 

        public string ConvertI(List<string> Line , int pos)
        {
            string machine =instructions[Line[0]].op;
            if (Line[0] != "bne" && Line[0] != "beq")
            {
                if (Line[0] == "addi")
                {
                    machine += MapToBinary[Line[2]];
                    machine += MapToBinary[Line[1]];
                    machine += ConvertToBinary(Convert.ToInt32 (Line[3]) , 'i');
                }
                else
                {
                    machine += MapToBinary[Line[3]];
                    machine += MapToBinary[Line[1]];
                    if (Variable.ContainsKey(Line[2]))
                        machine += ConvertToBinary(Variable[Line[2]], 'i');
                    else
                        machine += ConvertToBinary(Convert.ToInt32(Line[2]) , 'i'); 
                }
            }
            else{
                machine += MapToBinary[Line[1]];
                machine += MapToBinary[Line[2]];
                int relative_address = (pos * 4);
                relative_address += 4;
                string label_name = Line[Line.Count - 1];
                relative_address = Label_map[label_name] - relative_address;
                
                relative_address /= 4;
                
                machine += ConvertToBinary(relative_address , 'i');
            }
            return machine;
        } // Convert instruction by I type 
        private void button1_Click(object sender, EventArgs e)
        {
            Load();            // Load All Data
            string output = ""; // string will be store the result of each line
            int DataMemory = 0 , TextMemory = 0; // two counter to save position of memory
            List<string> code = new List<string>(Input.Text.Split('\n')); // Split input string to lines separeted by enter
            List<string> data = new List<string>();        // string to store DataSegment 
            for (int i = 0; i < code.Count; i++)    // for loop to filter input string to get DataSegment code only
            {
                string Code = HandelComments (code[i]);      // return string without comments
                List<string> TestForData = new List<string>(Code.Split('.','\r')); // split returned string to loop and get data
                TestForData = RemoveSpaces(TestForData); // remove all spaces if found 
                if (TestForData.Count != 0)      // check Length of list to check it elements
                {
                    if (TestForData[0] == "text")      // if first element is text will be break and finish Data part 
                    {
                        code.Remove(code[i]);
                        break;
                    }
                    if (TestForData[0] != "data")     // store all elements expect data we don't need it 
                        data.Add(Code);
                    code.Remove(code[i]);           // remove each line from code to keep CodeSegement only later 
                    i--;
                }
           }
            Output.Text = Output.Text + "#Translation of Data Segment";
            Output.AppendText(Environment.NewLine); // Add new line

            for (int i = 0; i < data.Count; i++) // Loop of Data to translate it 
            {
                List<string > DataLine =new List<string> (data[i].Split(':', '.',' ')); // Split each line of data to translate each one 
                DataLine = RemoveSpaces(DataLine); // remove all spaces if found 
                Variable[DataLine[0]] = 4 * DataMemory; // multiply DataCounter to 4 because our index 0 4 8 etc..
                if (DataLine[1] == "space") // if Space we will declare n array in our memory 
                    for (int j = 0; j < Convert.ToInt32(DataLine[2]); j++)  // for loop to declare arraies in memory 
                    {
                        Output.Text = Output.Text + "MEMORY(" + DataMemory + ") <= " + '"' + DontCare + '"' + " ;";
                        Output.AppendText(Environment.NewLine); // add new line
                        DataMemory++; 
                    }
                else
                {
                    for (int j = 2; j < DataLine.Count; j++) // loop of all word value  
                    {
                        string result = ConvertToBinary(Convert.ToInt32(DataLine[j]), 'd');
                        Output.Text = Output.Text + "MEMORY(" + DataMemory + ") <= " + '"' + result + '"' + " ;";
                        Output.AppendText(Environment.NewLine);  // add new line 
                        DataMemory++;
                    }
                }
            }
            // store DataSegement to file
            File.WriteAllText("E:\\University\\Third Year\\Second term\\Architecture\\Project\\Final Project\\WindowsFormsApplication1\\DataSegment.txt", Output.Text);

            string FinalOutputInTextBox = Output.Text; // store text of text box to display all of text at the end 
            Output.Text = "";
            Output.Text = Output.Text + "#Translation of Code Segment";
            Output.AppendText(Environment.NewLine); // add new line 
            int pos = 0; // Counter of Code Statment to help us to ignore all lines of comments
            for (int i = 0; i < code.Count; i++) // for loop to store all Lables in input code
            {
                string Code = HandelComments(code[i]); // return string without comments
                List<string> split_labels = new List<string>(Code.Split(':', ' ', ',', '(', ')', '\r')); // split each line to export labels
                split_labels = RemoveSpaces(split_labels); // Remove spaces if found 
                if (split_labels.Count != 0)  // check Length of list to check it elements
                {
                    if (!instructions.ContainsKey(split_labels[0])) // Check if first element in our instruction map then is not lable
                    {
                        Label_map[split_labels[0]] = (4 * pos);
                        split_labels.Remove(split_labels[0]);
                    }
                    pos++; // increment code Segement in this part because in this pices of code not a Comment line 
                }
                
            }

            pos = 0; // return code Segement to 0
            for (int i = 0; i < code.Count; i++) // For loop of each line to translate each insrtuction 
            {
                string Code = HandelComments(code[i]);// return string without comments
                List<string> Line = new List<string>(Code.Split(':',' ',',','(',')','\r'));// split each line to export instructions
                Line = RemoveSpaces(Line);// Remove spaces if found
                if (Line.Count != 0) // check Length of list to check it elements
                {
                    if (!instructions.ContainsKey(Line[0]))// Check if first element in our instruction map then is not lable
                    {
                        Label_map[Line[0]] = (4 * pos);
                        Line.Remove(Line[0]); // Remove the label from our line to handel all postion after this code
                    }
                    if (instructions[Line[0]].type == 'R') // if the instruction R type
                        output = ConvertR(Line);
                    else if (instructions[Line[0]].type == 'I') // if the instruction I type
                        output = ConvertI(Line, pos);
                    else if (instructions[Line[0]].type == 'J')// if the instruction J type
                        output = ConvertJ(Line);
                    Output.Text = Output.Text + "MEMORY(" + TextMemory + ") := " + '"' + output + '"' + " ;";
                    Output.AppendText(Environment.NewLine);
                    TextMemory++;
                    output = "";
                    pos++;// increment code Segement in this part because in this pices of code not a Comment line
                }
           }
            // store CodeSegement to file

            File.WriteAllText("E:\\University\\Third Year\\Second term\\Architecture\\Project\\Final Project\\WindowsFormsApplication1\\CodeSegment.txt", Output.Text);
            Output.Text = FinalOutputInTextBox + Output.Text; // Write All Code in Text Box
        }
        private void Input_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void clear_Click(object sender, EventArgs e)
        {
            Output.Text = ""; // Clear TextBox
            Input.Text = "";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Clipboard.SetText(Output.Text);
            MessageBox.Show("Code Copied Successfuly", "Copy Code ", MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
} 