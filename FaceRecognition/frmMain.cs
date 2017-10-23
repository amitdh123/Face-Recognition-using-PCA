//johndoe1591@gmail.com
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using FaceRecognition.Class;
using PCALib;
namespace FaceRecognition
{
    public partial class frmMain : Form
    {
        clsMatrixOperation _objMatrix = new clsMatrixOperation();
        double[][] EigenFaceImage = null;
        double[][] BaseMatrix = null;
        double[][] CopyImageMatrix_forallImages = null;
        double[][] EigenVectorforImage=null;
        double[][] MeanVector = null;
        double[][] MainImageMatrix_forAllImages = null;
        public frmMain()
        {
            InitializeComponent();
        }

        public void GetImageMatrix()
        {
            //Code for Intializing Matrix
             MainImageMatrix_forAllImages = new double[1][];
            double[][] MainImageMatrix_Covariance = new double[1][];
             CopyImageMatrix_forallImages = new double[1][];
            //InitializeMatrix_1(ref MainImageMatrix);
           // Call function to fetch all image in a matrix format
              _objMatrix.InitializeMatrix(ref MainImageMatrix_forAllImages);
            _objMatrix.CopyMatrix(MainImageMatrix_forAllImages, ref CopyImageMatrix_forallImages);
            //Code for finding means vector from the MainImage Matrix
            MeanVector = new double[1][];
            MeanVector = _objMatrix.GetMeanVector(ref MainImageMatrix_forAllImages);



            //Performing Step 2 of Algorithm

            _objMatrix.getMeanAdjustedMatrix(ref MainImageMatrix_forAllImages, MeanVector);

            //Compute Covariance matrix

            MainImageMatrix_Covariance = _objMatrix.getCovarianceMatrix(MainImageMatrix_forAllImages);

            //Computing Eigen values and Eigen vecotors by suding MaPack library
            PCALib.Matrix obj = new Matrix(MainImageMatrix_Covariance);
            IEigenvalueDecomposition EigenVal;
            EigenVal = obj.GetEigenvalueDecomposition();

            // Take only top 10 eigen values
            double[] EigenValforImage = new double[FaceRecognition.Properties.Settings.Default.DReductionVal];
            double[][] TransposevalImage = new double[FaceRecognition.Properties.Settings.Default.DReductionVal][];
             EigenFaceImage = new double[FaceRecognition.Properties.Settings.Default.DReductionVal][];
             EigenVectorforImage = new double[FaceRecognition.Properties.Settings.Default.DefaultNumberOfImages][];
            // double[][] PixelmatrixforImage = new double[FaceRecognition.Properties.Settings.Default.DefaultNumberOfImages][];
            _objMatrix.GetTopEigenValues(EigenVal.RealEigenvalues, ref EigenValforImage);

            // Taking top 10 Eigen vectors
            PCALib.IMatrix EigenVector = EigenVal.EigenvectorMatrix;

            _objMatrix.GetTopEigenVectors(EigenVector, ref EigenVectorforImage);
            // _objMatrix.Transpose(CopyImageMatrix, ref TransposevalImage);
            _objMatrix.Multiply(MainImageMatrix_forAllImages, EigenVectorforImage, ref EigenFaceImage);

            // Transpose the Eigen Face images so that we can use inbuilt max and min function in array.
             _objMatrix.Transpose(EigenFaceImage, ref TransposevalImage, FaceRecognition.Properties.Settings.Default.DReductionVal);
            double[][] PixelmatrixforImage = new double[FaceRecognition.Properties.Settings.Default.DReductionVal][];
            _objMatrix.ConvertinPixelScale(TransposevalImage, ref PixelmatrixforImage, EigenFaceImage.Length);
            //  _objMatrix.Transpose(PixelmatrixforImage, ref TransposevalImage,EigenFaceImage.Length);

            int iControl = 0;
           /* foreach (Control cobj in gbxImages.Controls)
            {
                if (cobj is PictureBox)
                {
                    cobj.BackgroundImage = _objMatrix.DrawFaceValue(PixelmatrixforImage, iControl);
                    iControl++;
                }

            }*/

            BaseMatrix = new double[clsMatrixOperation.iDefaultImageCount][];
            double[][] ProductMatrix = null;
            double[][] Transponse_EachImage = new double[FaceRecognition.Properties.Settings.Default.DReductionVal][];
            for (int irow = 0; irow < clsMatrixOperation.iDefaultImageCount; irow++)
            {
            Transponse_EachImage = new double[EigenFaceImage.Length][];
                _objMatrix.Transpose(MainImageMatrix_forAllImages, ref Transponse_EachImage,1,irow+1);
                _objMatrix.Multiply(Transponse_EachImage, EigenFaceImage, ref ProductMatrix);
                for (int iColumn = 0; iColumn < clsMatrixOperation.iDimensionalReduction; iColumn++)
                {
                    if (iColumn == 0)
                        BaseMatrix[irow] = new double[clsMatrixOperation.iDimensionalReduction];
                    BaseMatrix[irow][iColumn] = new double();
                    BaseMatrix[irow][iColumn] = ProductMatrix[0][iColumn];
                }
            }
          //  _objMatrix.Multiply(CopyImageMatrix, EigenFaceImage, ref BaseMatrix);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            clsMatrixOperation.iDefaultImageCount= FaceRecognition.Properties.Settings.Default.DefaultNumberOfImages;
            clsMatrixOperation.iDimensionalReduction = FaceRecognition.Properties.Settings.Default.DReductionVal;
            GetImageMatrix();
        }

      

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "jpeg files (*.jpg)|*.jpg|(*.gif)|gif||";
            if (DialogResult.OK == dialog.ShowDialog())
            {
                this.pbTest.Image = new Bitmap(dialog.FileName);
                FileInfo finfo = new FileInfo(dialog.FileName);
               
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            GetImageMatrixforTest();
        }

        public void GetImageMatrixforTest()
        {
            clsMatrixOperation. iDefaultImageCount = 1;
            //Code for Intializing Matrix
            double[][] MainImageMatrix = new double[1][];
            double[][] CopyImageMatrix = new double[1][];
            //InitializeMatrix_1(ref MainImageMatrix);
            // Call function to fetch all image in a matrix format
            _objMatrix.InitializeMatrix_ForTest(ref MainImageMatrix,(Bitmap)pbTest.Image);
            _objMatrix.CopyMatrix(MainImageMatrix, ref CopyImageMatrix);
            

             // Take only top 10 eigen values
               double[] ResultMatrix = new double[1];
               double[][] TransposevalImage = new double[1][];
               double[][] EigenFaceImage_test = new double[1][];
              double[][] OutputMatrix = new double[1][];
            
            _objMatrix.getMeanAdjustedMatrix(ref MainImageMatrix, MeanVector,true);
            _objMatrix.Transpose(MainImageMatrix, ref TransposevalImage, 1,1);
            _objMatrix.Multiply(TransposevalImage, EigenFaceImage, ref EigenFaceImage_test);
            _objMatrix.GetEucledianDistance(EigenFaceImage_test, BaseMatrix, ref ResultMatrix);
            _objMatrix.GetTopfiveGuess(ResultMatrix, CopyImageMatrix_forallImages,ref OutputMatrix);
            _objMatrix.Transpose(OutputMatrix, ref TransposevalImage, FaceRecognition.Properties.Settings.Default.DReductionVal);
            int iControl = 9;
            foreach (Control cobj in gbxTest .Controls)
            {
                if (cobj is PictureBox)
                {
                    cobj.BackgroundImage = _objMatrix.DrawFaceValue(TransposevalImage, iControl);
                    iControl--;
                }

            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }
    }
}
