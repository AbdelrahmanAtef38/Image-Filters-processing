using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Linq;


namespace ImageFilters
{
    
    public static class ImageOperations
    {


        /// <summary>
        /// Open an image, convert it to gray scale and load it into 2D array of size (Height x Width)
        /// </summary>
        /// <param name="ImagePath">Image file path</param>
        /// <returns>2D array of gray values</returns>
        public static byte[,] OpenImage(string ImagePath)
        {
            Bitmap original_bm = new Bitmap(ImagePath);
            int Height = original_bm.Height;
            int Width = original_bm.Width;

            byte[,] Buffer = new byte[Height, Width];

            unsafe
            {
                BitmapData bmd = original_bm.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, original_bm.PixelFormat);
                int x, y;
                int nWidth = 0;
                bool Format32 = false;
                bool Format24 = false;
                bool Format8 = false;

                if (original_bm.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    Format24 = true;
                    nWidth = Width * 3;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format32bppArgb || original_bm.PixelFormat == PixelFormat.Format32bppRgb || original_bm.PixelFormat == PixelFormat.Format32bppPArgb)
                {
                    Format32 = true;
                    nWidth = Width * 4;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    Format8 = true;
                    nWidth = Width;
                }
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (y = 0; y < Height; y++)
                {
                    for (x = 0; x < Width; x++)
                    {
                        if (Format8)
                        {
                            Buffer[y, x] = p[0];
                            p++;
                        }
                        else
                        {
                            Buffer[y, x] = (byte)((int)(p[0] + p[1] + p[2]) / 3);
                            if (Format24) p += 3;
                            else if (Format32) p += 4;
                        }
                    }
                    p += nOffset;
                }
                original_bm.UnlockBits(bmd);
            }

            return Buffer;
        }

        /// <summary>
        /// Get the height of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Height</returns>
        public static int GetHeight(byte[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(0);
        }

        /// <summary>
        /// Get the width of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Width</returns>
        public static int GetWidth(byte[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(1);
        }

        /// <summary>
        /// Display the given image on the given PictureBox object
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <param name="PicBox">PictureBox object to display the image on it</param>
        public static void DisplayImage(byte[,] ImageMatrix, PictureBox PicBox)
        {
            // Create Image:
            //==============
            int Height = ImageMatrix.GetLength(0);
            int Width = ImageMatrix.GetLength(1);

            Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            unsafe
            {
                BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat);
                int nWidth = 0;
                nWidth = Width * 3;
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        p[0] = p[1] = p[2] = ImageMatrix[i, j];
                        p += 3;
                    }

                    p += nOffset;
                }
                ImageBMP.UnlockBits(bmd);
            }
            PicBox.Image = ImageBMP;
        }

        public static byte kthLargestElement(byte[,] ImageMatrix,int Wmax)
        {
            int arraylength=0;
            byte[] Arr;
            Arr = new byte[Wmax * Wmax];
            byte max = 0;
            int x=0,y=0;
            for (int i = 0; i < 300; i++)
            {
                    Arr[arraylength] = ImageMatrix[x, y];
                    if (Arr[arraylength] > max)
                    {
                        max = Arr[arraylength];
                        
                    }
                    x++;
                    y++;
                }
            
            return max;
            
            
        }
        public static byte kthLowestElement(byte[,] ImageMatrix, int Wmax)
        {
            int arraylength = 0;
            byte[] Arr;
            Arr = new byte[Wmax * Wmax];
            byte min = 255;
            int x = 0, y = 0;
            for (int i = 0; i < 300; i++)
            {
                Arr[arraylength] = ImageMatrix[x, y];
                if (Arr[arraylength] < min)
                {
                    min = Arr[arraylength];

                }
                x++;
                y++;
            }

            return min;


        }
        public static byte[] COUNTING_SORT(byte[] Array, int ArrayLength, byte Max, byte Min)
        {
            byte[] count = new byte[Max - Min + 1];
            int z = 0;

            for (int i = 0; i < count.Length; i++) { 
                count[i] = 0; 
            }
            for (int i = 0; i < ArrayLength; i++) {
                count[Array[i] - Min]++; 
            }

            for (int i = Min; i <= Max; i++)
            {
                while (count[i - Min]-- > 0)
                {
                    Array[z] = (byte)i;
                    z++;
                }
            }
            return Array;
        }
  
        public static byte[] quickSort(byte[] Array, int low, int high)
        {
            int pivot;
            if (low < high)
            {
                    pivot = Partition(Array, low, high);
                    quickSort(Array, low, pivot - 1);
                    quickSort(Array, pivot + 1, high);
                
            }
            return Array;
        }
        private static int Partition(byte[] Array, int low, int high)
        {
            byte pivot;
            byte Temp;
            int i = (low - 1);
            pivot = Array[high];
            for (int j = low; j <= high - 1; j++)
            {
                if(Array[j]<=pivot)
                {
                    i++;
                    Temp = Array[i];
                    Array[i] = Array[j];
                    Array[j] = Temp;
                }
            }
            Temp = Array[i+1];
            Array[i+1] = Array[high];
            Array[high] = Temp;
            return i+1;
             
            
        }

        public static int LEFT(int i)
        {
            return 2 * i + 1;
        }
        public static int RIGHT(int i)
        {
            return 2 * i + 2;
        }
        public static void MAX_HEAPIFY(byte[] Array, int ArrayLength, int i)
        {
            int Left = LEFT(i);
            int Right = RIGHT(i);
            int Largest;
            if (Left < ArrayLength && Array[Left] > Array[i])
                Largest = Left;
            else
                Largest = i;
            if (Right < ArrayLength && Array[Right] > Array[Largest])
                Largest = Right;
            if (Largest != i)
            {
                byte Temp = Array[i];
                Array[i] = Array[Largest];
                Array[Largest] = Temp;
                MAX_HEAPIFY(Array, ArrayLength, Largest);
            }
        }
        public static void BUILD_MAX_HEAP(byte[] Array, int ArrayLength)
        {
            for (int i = ArrayLength / 2 - 1; i >= 0; i--)
                MAX_HEAPIFY(Array, ArrayLength, i);
        }
        public static byte[] HEAP_SORT(byte[] Array, int ArrayLength)
        {
            int HeapSize = ArrayLength;
            BUILD_MAX_HEAP(Array, ArrayLength);
            for (int i = ArrayLength - 1; i > 0; i--)
            {
                byte Temp = Array[0];
                Array[0] = Array[i];
                Array[i] = Temp;
                HeapSize--;
                MAX_HEAPIFY(Array, HeapSize, 0);
            }
            return Array;
        }


        public static byte[] SELECTION_SORT(byte[] Array, int ArrayLength)
        {
            for (int j = 0; j < ArrayLength - 1; j++)
            {
                int smallest = j;
                for (int i = j + 1; i < ArrayLength; i++)
                    if (Array[i] < Array[smallest])
                        smallest = i;
                if (smallest != j)
                {
                    int Temp = Array[j];
                    Array[j] = Array[smallest];
                    Array[smallest] = (byte)Temp;
                }
            }
            return Array;
        }

     
        public static byte Alphatrimfilter(byte[,] ImageMatrix, int x, int y, int Wmax,int T, int Sort)
        {
          
            byte[] Array;
            int[] Dx, Dy;
          
                Array = new byte[Wmax * Wmax];
                Dx = new int[Wmax * Wmax];
                Dy = new int[Wmax * Wmax];

            int Index = 0;
            for (int _y = -(Wmax / 2); _y <= (Wmax / 2); _y++)
            {
                for (int _x = -(Wmax / 2); _x <= (Wmax / 2); _x++)
                {
                    Dx[Index] = _x;
                    Dy[Index] = _y;
                    Index++;
                }
            }
            byte Max, Min;
            int ArrayLength, Sum, NewY, NewX, Avg;
            Sum = 0;
            Max = 0;
            Min = 255;
            ArrayLength = 0;
            for (int i = 0; i < Wmax * Wmax; i++)
            {
                NewY = y + Dy[i];
                NewX = x + Dx[i];
                if (NewX >= 0 && NewX < GetWidth(ImageMatrix) && NewY >= 0 && NewY < GetHeight(ImageMatrix))
                {
                    Array[ArrayLength] = ImageMatrix[NewY, NewX];
                    if (Array[ArrayLength] > Max)
                    {
                        Max = Array[ArrayLength];
                    }

                    if (Array[ArrayLength] < Min)
                    {
                        Min = Array[ArrayLength];
                    } 
                    
              
                    ArrayLength++;
                }
            }
          
             if (Sort == 1) Array = COUNTING_SORT(Array, ArrayLength, Max, Min);
             else if (Sort == 2) Array = quickSort(Array, 0, ArrayLength - 1);
             else if (Sort == 3) Array = HEAP_SORT(Array, ArrayLength);
             else if (Sort == 4) Array = SELECTION_SORT(Array, ArrayLength);
       
             for (int i = T; i <Array.Length-T; i++)
             {
                 Sum = Sum + Array[i];
                
             }
            Avg = Sum / Array.Length-(T*2);
            return (byte)Avg;
            
        }
        public static byte Adaptivemedianfilter(byte[,] ImageMatrix, int x, int y,int W, int Wmax, int Sort)
        {
           
            byte[] Array = new byte[Wmax * Wmax];
            int[] Dx = new int[Wmax * Wmax];
            int[] Dy = new int[Wmax * Wmax];
            int Index = 0;
            for (int _y = -(Wmax / 2); _y <= (Wmax / 2); _y++)
            {
                for (int _x = -(Wmax / 2); _x <= (Wmax / 2); _x++)
                {
                    Dx[Index] = _x;
                    Dy[Index] = _y;
                    Index++;
                }
            }
            
            byte Max, Min, Med, Z;
            int A1, A2, B1, B2, ArrayLength, NewY, NewX;
            Max = 0;
            Min = 255;
            ArrayLength = 0;
            Z = ImageMatrix[y, x];
            for (int i = 0; i < Wmax * Wmax; i++)
            {
                NewY = y + Dy[i];
                NewX = x + Dx[i];
                if (NewX >= 0 && NewX < GetWidth(ImageMatrix) && NewY >= 0 && NewY < GetHeight(ImageMatrix))
                {
                    Array[ArrayLength] = ImageMatrix[NewY, NewX];
                    if (Array[ArrayLength] > Max)
                        Max = Array[ArrayLength];
                    if (Array[ArrayLength] < Min)
                        Min = Array[ArrayLength];
                    ArrayLength++;
                }
            }
            if (Sort == 1) Array = COUNTING_SORT(Array, ArrayLength, Max, Min);
            else if (Sort == 2) Array = quickSort(Array, 0, ArrayLength-1);
            else if (Sort == 3) Array = HEAP_SORT(Array, ArrayLength);
            else if (Sort == 4) Array = SELECTION_SORT(Array, ArrayLength);
         

            Min = Array[0];
            Med = Array[ArrayLength / 2];
            A1 = Med - Min;
            A2 = Max - Med;
            if (A1 > 0 && A2 > 0)
            {
                B1 = Z - Min;
                B2 = Max - Z;
                if (B1 > 0 && B2 > 0)
                    return Z;

                else
                    return Med;
            }
            else
            {
                if (W + 2 <= Wmax)
                    return Adaptivemedianfilter(ImageMatrix, x, y, W + 2, Wmax, Sort);

                else
                    return Med;
            }
            
        }

        public static byte[,] ImageFilter(byte[,] ImageMatrix, int Max_Size, int Sort, int T ,int filter)
        {
            byte[,] ImageMatrix2 = ImageMatrix;
            
            for (int y = 0; y < GetHeight(ImageMatrix); y++)
            {
                for (int x = 0; x < GetWidth(ImageMatrix); x++)
                {
                    if (filter == 1)
                         ImageMatrix2[y, x] = Alphatrimfilter(ImageMatrix, x, y,Max_Size,T, Sort);
                    else if (filter == 2)
                        ImageMatrix2[y, x] = Adaptivemedianfilter(ImageMatrix, x, y, 3, Max_Size, Sort);
                }
            }

            return ImageMatrix2;
        }

      

      
        

       
 
    


     
    }  
}


/*   public static int PARTITION(byte[] Array, int p, int r)
     {
         byte pivot = Array[r];
         byte Temp;
         int i = p;
         for (int j = p; j < r; j++)
         {
             if (Array[j] <= pivot)
             {
                 Temp = Array[j];
                 Array[j] = Array[i];
                 Array[i++] = Temp;
             }
         }
         Temp = Array[i];
         Array[i] = Array[r];
         Array[r] = Temp;
         return i;
     }
   */
/*  public static byte[] QUICK_SORT(byte[] Array, int p, int r)
  {
      if (p < r)
      {
          int q = PARTITION(Array, p, r);
          QUICK_SORT(Array, p, q - 1);
          QUICK_SORT(Array, q + 1, r);
      }
      return Array;
  }
 */


/*heapsort
     
        public static int LEFT(int i)
        {
            return 2 * i + 1;
        }
        public static int RIGHT(int i)
        {
            return 2 * i + 2;
        }
        public static void MAX_HEAPIFY(byte[] Array, int ArrayLength, int i)
        {
            int Left = LEFT(i);
            int Right = RIGHT(i);
            int Largest;
            if (Left < ArrayLength && Array[Left] > Array[i])
                Largest = Left;
            else
                Largest = i;
            if (Right < ArrayLength && Array[Right] > Array[Largest])
                Largest = Right;
            if (Largest != i)
            {
                byte Temp = Array[i];
                Array[i] = Array[Largest];
                Array[Largest] = Temp;
                MAX_HEAPIFY(Array, ArrayLength, Largest);
            }
        }
        public static void BUILD_MAX_HEAP(byte[] Array, int ArrayLength)
        {
            for (int i = ArrayLength / 2 - 1; i >= 0; i--)
                MAX_HEAPIFY(Array, ArrayLength, i);
        }
        public static byte[] HEAP_SORT(byte[] Array, int ArrayLength)
        {
            int HeapSize = ArrayLength;
            BUILD_MAX_HEAP(Array, ArrayLength);
            for (int i = ArrayLength - 1; i > 0; i--)
            {
                byte Temp = Array[0];
                Array[0] = Array[i];
                Array[i] = Temp;
                HeapSize--;
                MAX_HEAPIFY(Array, HeapSize, 0);
            }
            return Array;
        }
        // sorting 
 */

//arthimetic mean filter

/*  public static Bitmap ArithmeticMean(byte[,] ImageMatrix,PictureBox PicBox2)
        {
            int w = ImageMatrix.GetLength(1); 
            int h = ImageMatrix.GetLength(0);
            Bitmap  imagee = new Bitmap(w, h, PixelFormat.Format24bppRgb);

            BitmapData image_data = imagee.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, imagee.PixelFormat);
            
            int bytes = image_data.Stride * image_data.Height;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
            imagee.UnlockBits(image_data);
            
            int r = 1;
            int wres = w - 2 * r;
            int hres = h - 2 * r;
            Bitmap result_image = new Bitmap(wres, hres, PixelFormat.Format24bppRgb);
            BitmapData result_data = result_image.LockBits(new Rectangle(0, 0, wres, hres), ImageLockMode.ReadWrite, result_image.PixelFormat);
            int res_bytes = result_data.Stride * result_data.Height;
            byte[] result = new byte[res_bytes];

            for (int x = r; x < w - r; x++)
            {
                for (int y = r; y < h - r; y++)
                {
                    int pixel_location = x * 3 + y * image_data.Stride;
                    int res_pixel_loc = (x - r) * 3 + (y - r) * result_data.Stride;
                    double[] mean = new double[3];

                    for (int kx = -r; kx <= r; kx++)
                    {
                        for (int ky = -r; ky <= r; ky++)
                        {
                            int kernel_pixel = pixel_location + kx * 3 + ky * image_data.Stride;

                            for (int c = 0; c < 3; c++)
                            {
                                mean[c] += buffer[kernel_pixel + c] / Math.Pow(2 * r + 1, 2);
                            }
                        }
                    }

                    for (int c = 0; c < 3; c++)
                    {
                        result[res_pixel_loc + c] = (byte)mean[c];
                    }
                }
            }

            Marshal.Copy(result, 0, result_data.Scan0, res_bytes);
            result_image.UnlockBits(result_data);
            PicBox2.Image = result_image;
            return result_image;
            
        }
 */
/*   public static byte Alphaaa(byte[,] ImageMatrix, int x, int y, int Wmax, int T, int Sort)
        {

            byte[] Array = new byte[Wmax * Wmax];
            int w = GetWidth(ImageMatrix);
            int h = GetHeight(ImageMatrix);

            int top = x - (Wmax / 2);
            int left = y - (Wmax / 2);
            int down = x + (Wmax / 2);
            int right = y + (Wmax / 2);
            int k = 0;

            for (int row = top; row <= down; row++)
            {
                for (int column = left; column <= right; column++)
                {

                    if (row >= 0 && row < h && column >= 0 && column < w)
                    {
                        Array[k] = ImageMatrix[row, column];
                        k++;
                    }
                }
            }
            byte Max, Min;
            int  Sum, Avg;
            Sum = 0;
            Max = 255;
            Min = 0;
           
         

            if (Sort == 1) Array = COUNTING_SORT(Array, Array.Length, Max, Min);
            else if (Sort == 2) Array = quickSort(Array, 0, Array.Length - 1);
            else if (Sort == 3) Array = HEAP_SORT(Array, Array.Length);
            else if (Sort == 4) Array = SELECTION_SORT(Array, Array.Length);

            for (int i = T; i < Array.Length - T; i++)
            {
                Sum = Sum + Array[i];

            }
            Avg = Sum / Array.Length - (T * 2);
            return (byte)Avg;

        }
      */