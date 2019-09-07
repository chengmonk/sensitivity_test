using System;
using System.Collections.Generic;
namespace FFT
{



    /// <summary>
        /// 快速傅立叶变换(Fast Fourier Transform)。 
        /// </summary>
    public class TWFFT
    {
        public static int putlen;
        /// <summary>
        /// 填充数据：将不够2的n次方的数据长度填充到2的n次方
        /// 最后一个数据是填充的长度的一半，通过这个长度将填充的数据剔除掉。
        /// </summary>
        public static List<double> DataFill(List<double> res)
        {
            int i = 0;           
            int n = Convert.ToString(res.ToArray().Length, 2).Length;
            putlen = (int)(Math.Pow(2.0, n) - res.ToArray().Length);
            putlen = putlen / 2 + 1;
            double begin = res[1];
            double end = res[res.ToArray().Length - 1];
            for (i = 0; i < putlen; i++)
            {
                res.Insert(0, begin);
            }
            for (i = 0; i < putlen; i++)
            {
                res.Add(end);
            }            
            return res;
        }
        public static List<float> DataFill(List<float> res)
        {
            int i = 0;
            int n = Convert.ToString(res.ToArray().Length, 2).Length;
            putlen = (int)(Math.Pow(2.0, n) - res.ToArray().Length);
            putlen = putlen / 2 + 1;
            float begin = res[1];
            float end = res[res.ToArray().Length - 1];
            for (i = 0; i < putlen; i++)
            {
                res.Insert(0, begin);
            }
            for (i = 0; i < putlen; i++)
            {
                res.Add(end);
            }
            return res;
        }

        public static double[] filterFFT(double[] res, double filter)
        {
            int reslen = res.Length;
            List<double> Y = new List<double>(res);
            Console.WriteLine("ydata:" + res.Length);
            Console.WriteLine("a:" + Y.ToArray().Length);
            Y = TWFFT.DataFill(Y);
            Console.WriteLine("b:" + Y.ToArray().Length);
            float[] y = new float[Y.ToArray().Length];
            y = TWFFT.FFT_filter(Y.ToArray(), filter);//第二个参数可调，调整范围是：(0,1)。为1 的时候没有滤波效果，为0的时候将所有频率都过滤掉。
            int putlen = TWFFT.putlen;//获取填充数据的长度的一半
            Console.WriteLine("putlen:" + putlen);
            Console.WriteLine("a:" + Y.ToArray().Length);
            for (int i = 0; i < reslen; i++)
            {
                res[i] = Math.Round(y[i + putlen], 2);
            }
            return res;
        }
        public static float[] filterFFT(float[] res, double filter)
        {
            int reslen = res.Length;
            List<float> Y = new List<float>(res);
            Console.WriteLine("ydata:" + res.Length);
            Console.WriteLine("a:" + Y.ToArray().Length);
            Y = TWFFT.DataFill(Y);
            Console.WriteLine("b:" + Y.ToArray().Length);
            float[] y = new float[Y.ToArray().Length];
            y = TWFFT.FFT_filter(Y.ToArray(),filter);//第二个参数可调，调整范围是：(0,1)。为1 的时候没有滤波效果，为0的时候将所有频率都过滤掉。
            int putlen = TWFFT.putlen;//获取填充数据的长度的一半
            Console.WriteLine("putlen:" + putlen);
            Console.WriteLine("a:" + Y.ToArray().Length);
            for (int i = 0; i < reslen; i++)
            {
                res[i] = (float)Math.Round(y[i + putlen], 2);
            }
            return res;
        }

        #region//调用该类的例子
        //res是原始的数据        
        //float[] ans = new float[res.ToArray().Length];
        //float[] R = new float[res.ToArray().Length];
        //    for (i = 0; i<res.ToArray().Length; i++)
        //        R[i] = (float) res[i];
        //ans = TWFFT.FFT_filter(R);
        //    for (i = 0; i<TWFFT.length; i++)            
        //        hslCurve1.AddCurveData("滤波数据", (float) ans[i]);

        #endregion
        #region//调用快速傅里叶的代码
        //length是傅里叶变换处理过后的数组大小
        public static int length = 0;
        //数组长度需要为2的n次方
        //filter的大小最大值为1，不能低于0，越接近0，滤波强度越强，也可能会导致失去真实信号，为1时没有滤波效果。
        public static float[] FFT_filter(float[] res, double filter)
        {
            //a是实部、b是虚部

            float[] a = new float[res.Length];
            float[] b = new float[res.Length];
            List<float> ans = new List<float>();
            for (int j = 0; j < res.Length; j++)
            {
                //  Console.WriteLine("I:" + j);
                a[j] = (float)res[j];
                b[j] = 0.0f;
            }
            length = TWFFT.FFT(a, b);
            //length是傅里叶变换处理过后的数组大小
            //在频域上对数据进行过滤 
            int p = (int)(length * filter);
            for (int i = 0; i < length; i++)
            {
                if (p < i && i < length - p)
                {
                    a[i] = 0f;
                    b[i] = 0f;
                }
            }
            length = TWFFT.IFFT(a, b);
            for (int i = 0; i < length; i++)
            {
                //Console.WriteLine("{0}\t{1}\t{2}", i, a[i], b[i]);
                ans.Add((float)Math.Sqrt(a[i] * a[i] + b[i] * b[i]));
            }

            return ans.ToArray();
        }
      
        public static float[] FFT_filter(double[] res, double filter)
        {
            //a是实部、b是虚部

            float[] a = new float[res.Length];
            float[] b = new float[res.Length];
            List<float> ans = new List<float>();
            for (int j = 0; j < res.Length; j++)
            {
                //  Console.WriteLine("I:" + j);
                a[j] = (float)res[j];
                b[j] = 0.0f;
            }
            length = TWFFT.FFT(a, b);
            //length是傅里叶变换处理过后的数组大小
            //在频域上对数据进行过滤 
            int p = (int)(length * filter);
            for (int i = 0; i < length; i++)
            {
                if (p < i && i < length - p)
                {
                    a[i] = 0f;
                    b[i] = 0f;
                }
            }
            length = TWFFT.IFFT(a, b);
            for (int i = 0; i < length; i++)
            {
                //Console.WriteLine("{0}\t{1}\t{2}", i, a[i], b[i]);
                ans.Add((float)Math.Sqrt(a[i] * a[i] + b[i] * b[i]));
            }

            return ans.ToArray();
        }
        #endregion

        #region//快速傅里叶变化的相关代码
        private static void bitrp(float[] xreal, float[] ximag, int n)
        {
            // 位反转置换 Bit-reversal Permutation
            int i, j, a, b, p;
            for (i = 1, p = 0; i < n; i *= 2)
            {
                p++;
            }
            for (i = 0; i < n; i++)
            {
                a = i;
                b = 0;
                for (j = 0; j < p; j++)
                {
                    b = b * 2 + a % 2;
                    a = a / 2;
                }
                if (b > i)
                {
                    float t = xreal[i];
                    xreal[i] = xreal[b];
                    xreal[b] = t;
                    t = ximag[i];
                    ximag[i] = ximag[b];
                    ximag[b] = t;
                }
            }
        }

        public static int FFT(float[] xreal, float[] ximag)
        {
            //n值为2的N次方
            int n = 2;
            while (n <= xreal.Length)
            {
                n *= 2;
            }
            n /= 2;
            // 快速傅立叶变换，将复数 x 变换后仍保存在 x 中，xreal, ximag 分别是 x 的实部和虚部
            float[] wreal = new float[n / 2];
            float[] wimag = new float[n / 2];
            float treal, timag, ureal, uimag, arg;
            int m, k, j, t, index1, index2;
            bitrp(xreal, ximag, n);
            // 计算 1 的前 n / 2 个 n 次方根的共轭复数 W'j = wreal [j] + i * wimag [j] , j = 0, 1, ... , n / 2 - 1
            arg = (float)(-2 * Math.PI / n);
            treal = (float)Math.Cos(arg);
            timag = (float)Math.Sin(arg);
            wreal[0] = 1.0f;
            wimag[0] = 0.0f;
            for (j = 1; j < n / 2; j++)
            {
                wreal[j] = wreal[j - 1] * treal - wimag[j - 1] * timag;
                wimag[j] = wreal[j - 1] * timag + wimag[j - 1] * treal;
            }
            for (m = 2; m <= n; m *= 2)
            {
                for (k = 0; k < n; k += m)
                {
                    for (j = 0; j < m / 2; j++)
                    {
                        index1 = k + j;
                        index2 = index1 + m / 2;
                        t = n * j / m;    // 旋转因子 w 的实部在 wreal [] 中的下标为 t
                        treal = wreal[t] * xreal[index2] - wimag[t] * ximag[index2];
                        timag = wreal[t] * ximag[index2] + wimag[t] * xreal[index2];
                        ureal = xreal[index1];
                        uimag = ximag[index1];
                        xreal[index1] = ureal + treal;
                        ximag[index1] = uimag + timag;
                        xreal[index2] = ureal - treal;
                        ximag[index2] = uimag - timag;
                    }
                }
            }
            return n;
        }
        public static int IFFT(float[] xreal, float[] ximag)
        {
            //n值为2的N次方
            int n = 2;
            while (n <= xreal.Length)
            {
                n *= 2;
            }
            n /= 2;
            // 快速傅立叶逆变换
            float[] wreal = new float[n / 2];
            float[] wimag = new float[n / 2];
            float treal, timag, ureal, uimag, arg;
            int m, k, j, t, index1, index2;
            bitrp(xreal, ximag, n);
            // 计算 1 的前 n / 2 个 n 次方根 Wj = wreal [j] + i * wimag [j] , j = 0, 1, ... , n / 2 - 1
            arg = (float)(2 * Math.PI / n);
            treal = (float)(Math.Cos(arg));
            timag = (float)(Math.Sin(arg));
            wreal[0] = 1.0f;
            wimag[0] = 0.0f;
            for (j = 1; j < n / 2; j++)
            {
                wreal[j] = wreal[j - 1] * treal - wimag[j - 1] * timag;
                wimag[j] = wreal[j - 1] * timag + wimag[j - 1] * treal;
            }
            for (m = 2; m <= n; m *= 2)
            {
                for (k = 0; k < n; k += m)
                {
                    for (j = 0; j < m / 2; j++)
                    {
                        index1 = k + j;
                        index2 = index1 + m / 2;
                        t = n * j / m;    // 旋转因子 w 的实部在 wreal [] 中的下标为 t
                        treal = wreal[t] * xreal[index2] - wimag[t] * ximag[index2];
                        timag = wreal[t] * ximag[index2] + wimag[t] * xreal[index2];
                        ureal = xreal[index1];
                        uimag = ximag[index1];
                        xreal[index1] = ureal + treal;
                        ximag[index1] = uimag + timag;
                        xreal[index2] = ureal - treal;
                        ximag[index2] = uimag - timag;
                    }
                }
            }
            for (j = 0; j < n; j++)
            {
                xreal[j] /= n;
                ximag[j] /= n;
            }
            return n;
        }
        #endregion
        private TWFFT()
        {

        }
    }
}
