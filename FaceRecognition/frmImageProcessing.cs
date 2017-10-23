using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FaceRecognition.Class;
using PCALib;
namespace FaceRecognition
{
    public partial class frmImageProcessing : Form
    {
        clsMatrixOperation _objMatrix = new clsMatrixOperation();
        public frmImageProcessing()
        {
            InitializeComponent();
        }

        private void frmImageProcessing_Load(object sender, EventArgs e)
        {

        }
        public void GetImageMatrix()
        {
            Matrix MainImageMatrix = null ;
            //_objMatrix.InitializeMatrix_New(ref MainImageMatrix);
        }
    }
}
