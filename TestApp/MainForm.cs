using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DevelopmentSimplyPut.CommonUtilities;
using System.Globalization;

namespace TestApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            rtxtOutput.Text = string.Empty;

            string[] colors = new string[2] { "White", "Black" };
            string[] brands = new string[2] { "Nokia", "Samsung" };
            string[] os = new string[2] { "Symbian", "Android" };

            Int64[] attributesSizes = new Int64[3];
            attributesSizes[0] = colors.Length;
            attributesSizes[1] = brands.Length;
            attributesSizes[2] = os.Length;

            PossibilitiesCube container = new PossibilitiesCube(2, attributesSizes);
            container.AttributesCombinationValidator = new Func<Int64[], bool>
                    (
                        delegate(Int64[] attributesCombination)
                        {
                            bool result = true;
                            //filter out if the brand is "Samsung" and the os is "Symbian"
                            if (attributesCombination[1] == 1 && attributesCombination[2] == 0)
                            {
                                result = false;
                            }
                            return result;
                        }
                    );

            container.InstancesCombinationValidator = new Func<Int64[], bool>
                    (
                        delegate(Int64[] instanceCombination)
                        {
                            bool result = true;
                            //filter out if both mobile phones are identical
                            if (instanceCombination[0] == instanceCombination[1])
                            {
                                result = false;
                            }
                            return result;
                        }
                    );

            container.BuildPossibilitiesMatrix();

            for (Int64 i = 0; i <= container.InstancesCombinationsMaxRowIndex; i++)
            {
                if (container.CombinationsExist)
                {
                    for (Int64 k = 0; k <= container.InstancesCombinationsMaxColumnIndex; k++)
                    {
                        string color1 = colors[container[i, k, 0]];
                        string brand1 = brands[container[i, k, 1]];
                        string os1 = os[container[i, k, 2]];
                        rtxtOutput.Text += string.Format(CultureInfo.InvariantCulture, "[{0},{1},{2}]", color1, brand1, os1) + ((k != container.InstancesCombinationsMaxColumnIndex) ? "\t" : string.Empty);
                    }

                    rtxtOutput.Text += Environment.NewLine;
                }   
            }

            MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "{0} prize combinations are found.", container.InstancesCombinationsMaxRowIndex + 1));
        }
    }
}
