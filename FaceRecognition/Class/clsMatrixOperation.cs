using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PCALib;

namespace FaceRecognition.Class
{
    class clsMatrixOperation
    {
        public static int iDefaultImageCount;
        public static int iDimensionalReduction;

        public double[][] GetMeanVector(ref double[][]MainImageMatrix)
        {
            double[][] MeanVector = new double[MainImageMatrix.Length][];
            for (int iRow = 0; iRow < MainImageMatrix.Length; iRow++)
            {
                MeanVector[iRow] = new double[1];
                double iSum = 0;
                for (int iColumn = 0; iColumn < iDefaultImageCount; iColumn++)
                {
                    iSum += MainImageMatrix[iRow][iColumn];                

                }
                MeanVector[iRow][0] = new double();
                MeanVector[iRow][0] = iSum/iDefaultImageCount;
            }

            return MeanVector;

        }

        public void getMeanAdjustedMatrix(ref double[][] MainImageMatrix, double[][] MeanVector, bool bflag = false)
        {
            if (!bflag) { 
            for (int iRow = 0; iRow < MainImageMatrix.Length; iRow++)
            {
                for (int iColumn = 0; iColumn < iDefaultImageCount; iColumn++)
                {

                    MainImageMatrix[iRow][iColumn] = MainImageMatrix[iRow][iColumn] - MeanVector[iRow][0];
                }

            }
        }
        else{
                for (int iRow = 0; iRow < MainImageMatrix.Length; iRow++)
                {
                    for (int iColumn = 0; iColumn < 1; iColumn++)
                    {

                        MainImageMatrix[iRow][iColumn] = MainImageMatrix[iRow][iColumn] - MeanVector[iRow][0];
                    }

                }
            }

        }

        /// <summary>
        /// For eigen values  we have to make this as a square matrix
        /// </summary>
        /// <param name="MainImageMatrix"> reference of MainImage matrix</param>
        
        public double[][] getCovarianceMatrix( double[][] MainImageMatrix)
        {
            double[][] MainImagesquareMatrix = new double[iDefaultImageCount][];
            for (int iImagePrimary = 0; iImagePrimary < iDefaultImageCount; iImagePrimary++)
            {
                
                for (int iImageSecondry = 0; iImageSecondry < iDefaultImageCount; iImageSecondry++)
                {
                    double iSum = 0;
                    for(int iPara=0;iPara<MainImageMatrix.Length;iPara++)
                    {

                        iSum += MainImageMatrix[iPara][iImagePrimary]* MainImageMatrix[iPara][iImageSecondry];

                    }
                    if(iImageSecondry == 0) MainImagesquareMatrix[iImagePrimary]= new double[iDefaultImageCount];
                    MainImagesquareMatrix[iImagePrimary][iImageSecondry] = new double();
                    MainImagesquareMatrix[iImagePrimary][iImageSecondry] = iSum;
                }

            }
            return MainImagesquareMatrix;

        }

        public void GetTopEigenValues(double [] OriginalEigenVAl,ref double [] EigenValforImage)
        {
            for (int iRow = 0; iRow <iDimensionalReduction; iRow++)
            {

                EigenValforImage[iRow] = OriginalEigenVAl[iRow];


            }
        }

        /// <summary>
        /// Function used to get top FaceRecognition.Properties.Settings.Default.DReductionVal Image vectors
        /// </summary>
        /// <param name="OriginalEigenVector"></param>
        /// <param name="EigenVectorForImage"></param>
        public void GetTopEigenVectors(PCALib.IMatrix OriginalEigenVector, ref double[][] EigenVectorForImage)
        {
            for (int iRow = 0; iRow < OriginalEigenVector.Rows; iRow++)
            {
                for (int iColumn = 0; iColumn < FaceRecognition.Properties.Settings.Default.DReductionVal; iColumn++)
                {
                     if (iColumn == 0)
                    EigenVectorForImage[iRow] = new double[FaceRecognition.Properties.Settings.Default.DReductionVal];
                    EigenVectorForImage[iRow][iColumn] = new double();
                    EigenVectorForImage[iRow][iColumn] = OriginalEigenVector[iRow,iColumn];
                }

            }

        }

        /// <summary>
        /// Function used for matrix transpose
        /// </summary>
        /// <param name="data"></param>
        /// <param name="MainImageMatrix"></param>
		public void Transpose(double[][] data, ref double[][] MainImageMatrix,int column,int ImageNo=0)
        {
            if (ImageNo == 0)
            {
                MainImageMatrix = new double[column][];
                for (int iRow = 0; iRow < column; iRow++)
                {
                    for (int iColumn = 0; iColumn < data.Length; iColumn++)
                    {
                        if (iColumn == 0)
                            MainImageMatrix[iRow] = new double[data.Length];
                        MainImageMatrix[iRow][iColumn] = new double();
                        MainImageMatrix[iRow][iColumn] = data[iColumn][iRow];
                    }
                }
            }
            else {
                MainImageMatrix = new double[column][];
                
                    for (int iColumn = 0; iColumn < data.Length; iColumn++)
                    {
                        if (iColumn == 0)
                            MainImageMatrix[0] = new double[data.Length];
                        MainImageMatrix[0][iColumn] = new double();
                        MainImageMatrix[0][iColumn] = data[iColumn][ImageNo-1];
                    }
                
            }
           
        }

        public void CopyMatrix(double[][] data, ref double[][] MainImageMatrix)
        {
            MainImageMatrix = new double[data.Length][];
            for (int iRow = 0; iRow < data.Length; iRow++)
            {
                for (int iColumn = 0; iColumn < iDefaultImageCount; iColumn++)
                {
                    if (iColumn == 0)
                        MainImageMatrix[iRow] = new double[iDefaultImageCount];

                    MainImageMatrix[iRow][iColumn] = new double();
                    MainImageMatrix[iRow][iColumn] = data[iRow][iColumn];
                }
            }

        }


        /// <summary>
        /// Function gives product of two matrix
        /// </summary>
        /// <param name="TransposeMatrix"></param>
        /// <param name="EigenVectorMatrix"></param>
        /// <param name="ProductMaterix"></param>
        public void Multiply(double[][] TransposeMatrix,  double[][] EigenVectorMatrix, ref double[][] ProductMaterix)
        {
            ProductMaterix = new double[TransposeMatrix.Length][];
            for (int iRow = 0; iRow < TransposeMatrix.Length; iRow++)
            {
                
                for (int iColumn = 0; iColumn < FaceRecognition.Properties.Settings.Default.DReductionVal; iColumn++)
                {
                   
                    double s = 0;
                    for (int k = 0; k < 200; k++)
                    {
                        s += TransposeMatrix[iRow][k] * EigenVectorMatrix[k][iColumn]; 
                    }
                    if (iColumn == 0)
                        ProductMaterix[iRow] = new double[FaceRecognition.Properties.Settings.Default.DReductionVal];
                    ProductMaterix[iRow][iColumn] = new double();
                    ProductMaterix[iRow][iColumn] = s;
                }
            }

           
        }

        public void GetBAseMatrix(double[][] OriginalMAtrix, double[][] EigenFaceMatrix, ref double[][] ProductMaterix)
        {
            ProductMaterix = new double[OriginalMAtrix.Length][];
            for (int iRow = 0; iRow < OriginalMAtrix.Length; iRow++)
            {

                for (int iColumn = 0; iColumn < 200; iColumn++)
                {

                    double s = 0;
                    for (int k = 0; k < FaceRecognition.Properties.Settings.Default.DReductionVal; k++)
                    {
                        s += OriginalMAtrix[iRow][k] * EigenFaceMatrix[k][iColumn];
                    }
                    if (iColumn == 0)
                        ProductMaterix[iRow] = new double[FaceRecognition.Properties.Settings.Default.DReductionVal];
                    ProductMaterix[iRow][iColumn] = new double();
                    ProductMaterix[iRow][iColumn] = s;
                }
            }


        }
        public void GetEucledianDistance(double[][] OriginalMAtrix, double[][] BaseMatrix, ref double[] ResultMaterix)
        {
            ResultMaterix = new double[BaseMatrix.Length];
            for (int iRow = 0; iRow < BaseMatrix.Length; iRow++)
            {
                double s = 0;
                for (int iColumn = 0; iColumn < clsMatrixOperation.iDimensionalReduction ; iColumn++)
                {

                    s = s +  (  (BaseMatrix[iRow][iColumn] - OriginalMAtrix[0][iColumn] )* (BaseMatrix[iRow][iColumn]- OriginalMAtrix[0][iColumn]));
                   
                
                        
                }
             //   ResultMaterix[iRow] = new double[1];
              //  ResultMaterix[iRow][0] = new double();
                ResultMaterix[iRow] = Math.Sqrt(s) ;
            }
            

        }

        public void GetTopfiveGuess(  double[] ResultMaterix, double[][] OriginalMAtrix, ref double[][] OutputMAtrix)
        {
           Int32[] IndexMatrix = new Int32 []{ -1,-1,-1,-1,-1,-1,-1,-1,-1,-1};
            for (int iRow = 0; iRow <10; iRow++)
            {
                int iIndex = 0;
               double temp = ResultMaterix[0];
                for (int iColumn = 0; iColumn < ResultMaterix.Length; iColumn++)
                {

                    if (temp >= ResultMaterix[iColumn])
                    {
                        if (iColumn != IndexMatrix[0] && iColumn != IndexMatrix[1] && iColumn != IndexMatrix[2] && iColumn != IndexMatrix[3] && iColumn != IndexMatrix[4] && iColumn != IndexMatrix[5] && iColumn != IndexMatrix[6] && iColumn != IndexMatrix[7] && iColumn != IndexMatrix[8] && iColumn != IndexMatrix[9])
                        {
                            temp = ResultMaterix[iColumn];
                            iIndex = iColumn;
                        }
                    }



                }
                //   ResultMaterix[iRow] = new double[1];
                //  ResultMaterix[iRow][0] = new double();
                IndexMatrix[iRow] =iIndex  ;
            }
            OutputMAtrix = new double[OriginalMAtrix.Length][];
            for (int iRow = 0; iRow < OriginalMAtrix.Length; iRow++)
            {

                for (int iColumn = 0; iColumn < 10; iColumn++)
                {

                   
                    if (iColumn == 0)
                        OutputMAtrix[iRow] = new double[FaceRecognition.Properties.Settings.Default.DReductionVal];
                    OutputMAtrix[iRow][iColumn] = new double();
                    OutputMAtrix[iRow][iColumn] = OriginalMAtrix[iRow ][IndexMatrix[iColumn]];
                }
            }
        }
        public void ConvertinPixelScale(double[][] TransposeMatrix,  ref double[][] PixelMaterix, int length=0)
        {
           
            double min=0, max=0;
           
            for (int iRow = 0; iRow < TransposeMatrix.Length; iRow++)
            {
                min = TransposeMatrix[iRow].Min();
                max = TransposeMatrix[iRow].Max();

                for (int iColumn = 0; iColumn < length; iColumn++)
                {

                    double s = 0;
                    s = ((TransposeMatrix[iRow][iColumn] - min) / (max - min)) * 255;
                    if (iColumn == 0)
                        PixelMaterix[iRow] = new double[length];
                    PixelMaterix[iRow][iColumn] = new double();
                    PixelMaterix[iRow][iColumn] = s;
                }
            }


        }
        public void ConvertinPixelScale_New(double[][] TransposeMatrix, ref double[][] PixelMaterix, int length = 0)
        {

            double min = 0, max = 0;

            for (int iRow = 0; iRow < TransposeMatrix.Length; iRow++)
            {
                min = TransposeMatrix[iRow].Min();
                max = TransposeMatrix[iRow].Max();

                for (int iColumn = 0; iColumn < length; iColumn++)
                {

                    double s = 0;
                    s = ((TransposeMatrix[iRow][iColumn] - min) / (max - min)) * 255;
                    if (iColumn == 0)
                        PixelMaterix[iRow] = new double[length];
                    PixelMaterix[iRow][iColumn] = new double();
                    PixelMaterix[iRow][iColumn] = s;
                }
            }


        }
        public Bitmap DrawFaceValue( double[][] MainImageMatrix,int iControl)
        {


            string[] bitmapFilePath = Directory.GetFiles(@"C:\Users\amit\Downloads\AttDataSet\ATTDataSet\Training", "*.jpg");

            Bitmap b1 = new Bitmap(bitmapFilePath[0]);
            Int32 height = b1.Height;
            Int32 width = b1.Width;

           
           
            int bpp = 24;

            Bitmap bmp = new Bitmap(width, height);
            for (int iRow = 0; iRow <= iControl; iRow++)
            {
                if (iControl != iRow) continue;
                int iColumn = 0;
               
                    for (int y = 0; y < width; y++)
                    {
                        for (int x = 0; x < height; x++)
                        {
                           
                            if (bpp == 24) // in this case you have 3 color values (red, green, blue)
                            {
                                // first byte will be red, because you are writing it as first value
                                byte r = (byte)MainImageMatrix[iRow][iColumn];
                                byte g = (byte)MainImageMatrix[iRow][iColumn];
                                byte b = (byte)MainImageMatrix[iRow][iColumn];
                                Color color = Color.FromArgb(r, g, b);
                                bmp.SetPixel(y, x, color);
                            }
                        iColumn++;
                        }
                    
                }
            }
            return bmp;
        }

        public void InitializeMatrix(ref double[][] MainImageMatrix)
        {

            
            string[] bitmapFilePath = Directory.GetFiles(@"C:\Users\amit\Downloads\AttDataSet\ATTDataSet\Training", "*.jpg");

            Bitmap b1 = new Bitmap(bitmapFilePath[0]);
            Int32 height = b1.Height;
            Int32 width = b1.Width;

             MainImageMatrix = new double[width * height][];
            for (int ImageCount = 0; ImageCount < iDefaultImageCount; ImageCount++)
            {
                b1 = new Bitmap(bitmapFilePath[ImageCount]);
                int iRow = 0;
                for (int i = 0; i < width; i++)
                {

                    for (int j = 0; j < height; j++)
                    {
                        if (ImageCount == 0)
                            MainImageMatrix[iRow] = new double[iDefaultImageCount];

                        MainImageMatrix[iRow][ImageCount] = new double();
                        MainImageMatrix[iRow][ImageCount] = b1.GetPixel(i, j).R; 
                        
                        iRow++;
                    }
                }


            }
           
        }
        public void InitializeMatrix_ForTest(ref double[][] MainImageMatrix, Bitmap b1)
        {


            //string[] bitmapFilePath = Directory.GetFiles(@"AttDataSet\ATTDataSet\Training", "*.jpg");

           
            Int32 height = b1.Height;
            Int32 width = b1.Width;

            MainImageMatrix = new double[width * height][];
            for (int ImageCount = 0; ImageCount < iDefaultImageCount; ImageCount++)
            {
               
                int iRow = 0;
                for (int i = 0; i < width; i++)
                {

                    for (int j = 0; j < height; j++)
                    {
                        if (ImageCount == 0)
                            MainImageMatrix[iRow] = new double[1];

                        MainImageMatrix[iRow][ImageCount] = new double();
                        MainImageMatrix[iRow][ImageCount] = b1.GetPixel(i, j).R;

                        iRow++;
                    }
                }


            }

        }
    }
}
