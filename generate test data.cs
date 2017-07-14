using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApplication2
{
    public partial class csvGenerator : Form
    {
        public csvGenerator()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

        }

        private void propertyNums_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void itemNums_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void datasourceNums_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void year_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void itemprefix_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void codeTypeName_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int propertyNum = int.Parse(propertyNums.Text);
            int itemNum = int.Parse(itemNums.Text);
            int datasourceNum = int.Parse(datasourceNums.Text);
            double years = double.Parse(year.Text);
            string item = itemprefix.Text;
            string codeType = codeTypeName.Text;
           
        }
        // Pads Items with the desired number of zeros
        public string Padding (int itemNumber, int itemCount)
        {
                double dbl_CountLength = Math.Floor(Math.Log10(itemNumber) + 1);
                int int_CountLengthConv = Convert.ToInt32(dbl_CountLength);

                string str_finaloutput = itemCount.ToString().PadLeft(int_CountLengthConv, '0');

                return str_finaloutput;
                
        }

        private void Generatecsv_MouseClick(object sender, MouseEventArgs e)
        {
            // Parsing Input variables to int
            int propertyNum = int.Parse(propertyNums.Text);
            int itemNum = int.Parse(itemNums.Text);
            int datasourceNum = int.Parse(datasourceNums.Text);
            double years = double.Parse(year.Text);
            string item = itemprefix.Text;
            string codeType = codeTypeName.Text;
            double new_price = 0;
            double volatility = 0.02;
            double old_price;
            double change_percent = 0;
            double rnd;
            int priceFlux;
            int dateCount = 0;
            Random posneg = new Random();
            Random num = new Random();
            Random initial_price = new Random();
            Random oldp = new Random();
            Random ran_gap = new Random();
            Random ran_flux = new Random();
            Random ran_spike = new Random();
            List<double> DS1;
            bool decision;
            double oldprice;
            double change_amount;

            
            string strItemNum = itemNum.ToString();
            strItemNum.PadLeft(strItemNum.Length -1, '0');

            // Column headers are defined
            List<string> fieldName = new List<string>();
            fieldName.Add("Code");
            fieldName.Add("Code Types");
            fieldName.Add("Property");
            fieldName.Add("Data Sources");
            fieldName.Add("Date");
            fieldName.Add("Value");



           // Lists used to house data are generated
            List<string> filestart = new List<string>();
            List<string> itemName = new List<string>();
            List<string> itemDate = new List<string>();
            List<string> property = new List<string>();
            List<string> datasource = new List<string>();
            List<Int32> init_priceList = new List<Int32>();
            List<bool> decisionList;
            List<List<double>> Code02 = new List<List<double>>();

            // The Items are created based on the itemNum variable input
            int codesuff = 1;
            for (int i = 0; i < itemNum; i++)
            {
        
               string str_finaloutput = Padding (itemNum, codesuff);
               if (itemNum < 10)
               {
                   itemName.Add(item + "_0" + str_finaloutput);
               }
               else
               {
                   itemName.Add(item + "_" + str_finaloutput);
               }
               codesuff++;

            }

            // The properties are created based on the propertyNum input variable
            int datasuff = 1;
            int propertysuff = 1;
            for (int i = 0; i < propertyNum; i++)
            {
                string str_finaloutput = Padding(propertyNum, propertysuff);
                if (propertyNum < 10)
                {
                    property.Add("Property_0" + str_finaloutput);
                }
                else
                {
                    property.Add("Property_" + str_finaloutput);
                }
                
                propertysuff++;


            }



            // The data sources are created based on the datasourcenum input variable
            for (int i = 0; i < datasourceNum; i++)
            {
                
                    string str_finaloutput = Padding(datasourceNum, datasuff);
                    if (datasourceNum < 10)
                    {
                        datasource.Add("DS_0" + str_finaloutput);
                    }
                    else
                    {
                        datasource.Add("DS_" + str_finaloutput);
                    }
                    
                    datasuff++;

            }



            // The start date of the time series is hard coded to start on 01/01/2007
            // and is incremented from that  date forward
            DateTime testDate = new DateTime(2007, 1, 1);
            itemDate.Add(testDate.ToString("dd-MMM-yyyy"));
            for (int i = 1; i < (years * 261.0); i++)
            {


                if (i != 0 && i % 5 == 0)
                {
                    testDate = testDate.AddDays(3);
                }
                else
                {
                    testDate = testDate.AddDays(1);
                }
                itemDate.Add(testDate.ToString("dd-MMM-yyyy"));


            }



            for (int propnum = 0; propnum < propertyNum; propnum++)
            {
                init_priceList.Add(initial_price.Next(90, 110));
            }

            List<double> Code01 = new List<double>();
            for (int y = 0; y < itemNum; y++)
            {
                for (int h = 0; h < propertyNum; h++)
                {

                    //old_price = oldp.Next(10, 60);
                    old_price = init_priceList[h];
                    DS1 = new List<double>();
                    decisionList = new List<bool>();


                    for (int g = 0; g < datasourceNum; g++)
                    {
                        //Re-initializes Code01 which houses the time series data per datasource
                        Code01 = new List<double>();
                        
                        if (g > 0)
                        {

                            Code01.Add(DS1[0]);
                        }
                        else
                        {
                            DS1.Add(old_price);
                        }


                        for (int v = 0; v < (years * 261.0); v++)
                        {

                            change_amount = num.NextDouble() * Math.Abs(0.30);
                            decision = posneg.NextDouble() > 0.5;
                            // Determines whether or not the change amount will be added or subtracted from the old price
                            //string str_decision = Convert.ToString(decision);
                            if (g == 0)
                            {
                                rnd = num.NextDouble();
                                change_percent = 2 * volatility * rnd;
                                decision = change_percent > volatility;
                                if (change_percent > volatility)
                                {
                                    change_percent -= (2 * volatility);
                                }
                                change_amount = old_price * change_percent;
                                new_price = old_price + change_amount;
                                old_price = new_price;
                                decisionList.Add(decision);
                                DS1.Add(new_price);
                            }



                            else
                            {
                                rnd = num.NextDouble();
                                change_percent = 2 * volatility * rnd;
                                if (decisionList[v] == false)
                                {
                                    
                                    change_percent -= (2 * volatility);
                                    change_amount = DS1[v] * change_percent;
                                    Code01.Add(DS1[v] + change_amount);
                                    //old_price = new_price;
                                    

                                }
                                else
                                {
                                    change_amount = DS1[v] * change_percent;
                                    Code01.Add(DS1[v] + change_amount);
                                }

                            }

                            if (g == 0)
                            {
                                Code02.Add(DS1.ToList());

                            }
                            else
                            {
                                Code02.Add(Code01.ToList());
                            }


                        }

                       


                    }


                }

            }

           
            try
            {

                //  The stream writer is created to write the generated data to an ouput file
                Random r = new Random();
                string username = System.Environment.UserName;
                int o = 0;
                using (StreamWriter writer = new StreamWriter(@"C:\csvGen\test.csv", false))
                    if (o == 0)
                    {
                        for (o = 0; o < fieldName.Count; o++)
                        {
                            if (o < fieldName.Count - 1)
                            {
                                writer.Write(fieldName[o] + ",");
                            }
                            else
                            {
                                writer.Write(fieldName[o]);
                            }
                        }
                        writer.Write(writer.NewLine);


                        volatility = 0.03;
                        int pricecount = 0;
                        //double price;
                        for (int p = 0; p < itemName.Count; p++)
                        {
                            // writer.WriteLine("START SECURITY|" + itemName[0] + "|" + fieldName[p] + "|");

                            //string fieldInd = fieldName[p];
                            for (int b = 0; b < property.Count; b++)
                            {

                                oldprice = oldp.Next(10, 200);

                                // int opertr = alt_operator.Next(0, 1);

                                for (int m = 0; m < datasource.Count; m++)
                                {

                                    for (int q = 0; q < itemDate.Count; q++)
                                    {
                                        priceFlux = ran_flux.Next(1, 1000);
                                        //Creates a flat, where the price remains the same as the previous one
                                        if (priceFlux <= 3)
                                        {
                                            if (this.chkbx_Flats.Checked)
                                            {
                                                // The index is checked whether or not it is zero before subtracting one from it 
                                                if (q == 0)
                                                {
                                                    writer.Write(itemName[p] + "," + codeType + "," + property[b] + "," + datasource[m] + "," + itemDate[q] + "," + Code02[pricecount][q]);
                                                    writer.Write(writer.NewLine);
                                                }
                                          
                                            }
                                        }
                                        // Doez not add a pricepoint for that day
                                        else if (priceFlux <= 7)
                                        {
                                            if (this.chkbx_Gaps.Checked)
                                            {
                                                //writer.Write(itemName[p] + "," + CodeType + "," + property[b] + "," + datasource[m]+ "," + "," + Code02[pricecount][q]);
                                                //writer.Write(writer.NewLine);
                                            }
                                            // This creates a spike in the dataseries
                                        }
                                        else  if (priceFlux >= 8 && priceFlux <= 10)
                                        {
                                            if (chkbx_Spikes.Checked)
                                            {
                                                writer.Write(itemName[p] + "," + codeType + "," + property[b] + "," + datasource[m] + "," + itemDate[q] + "," + (Code02[pricecount][q] * (priceFlux / 100)) + Code02[pricecount][q]);
                                                writer.Write(writer.NewLine);
                                            }
                                              //price = generatePrice(volatility,oldprice,nums);
                                              //The loop runs as normal
                                        }
                                        else
                                        {
                                            writer.Write(itemName[p] + "," + codeType + "," + property[b] + "," + datasource[m] + "," + itemDate[q] + "," + Code02[pricecount][q]);
                                            writer.Write(writer.NewLine);
                                        }


                                        if (pricecount < Code02.Count)
                                        {
                                            pricecount++;
                                        }
                                    }

                                }

                            }

                        }
                    }

                string success = "The file was created successfully";
                MessageBox.Show(success);
            }
            catch(IOException)
            {
  
                string error = "An error occured while creating file";
                MessageBox.Show(error);
            }

            // After the file is created, the user is given the option to import the data into a database 
            DialogResult dialogResult = MessageBox.Show("Do you wish to import the data that was generated?", "Import Data", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "C:\\csvGen\\csvImport.bat";
                proc.StartInfo.WorkingDirectory = "C:\\csvGen";
                proc.Start();
            }
            else if (dialogResult == DialogResult.No)
            {
                
                
            }

            
        }

        private void Generatecsv_Click(object sender, EventArgs e)
        {
            int propertyNum = int.Parse(propertyNums.Text);
            int itemNum = int.Parse(itemNums.Text);
            int datasourceNum = int.Parse(datasourceNums.Text);
            double years = double.Parse(year.Text);
            string item = itemprefix.Text;
            string codeType = codeTypeName.Text;
         
         }

        private void chkbx_Gaps_CheckedChanged(object sender, EventArgs e)
        {

        }

            
         }



       


    
}
